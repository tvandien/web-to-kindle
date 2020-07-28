using WebToKindle.Helper;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text;
using WebToKindle.Helper.Templates;

namespace WebToKindle.Helper
{
    public class EPubWriter : IDisposable
    {
        #region Inner Types
        private class ChapterInfo
        {
            public string FileName { get; set; }
            public string Title { get; set; }
        }

        private class ResourceInfo
        {
            public string FileName { get; set; }
            public string MimeType { get; set; }
        }
        #endregion

        #region Fields
        private const string MIME_TYPE = "mimetype";
        private const string CONTAINER = "META-INF/container.xml";
        private const string CONTENT = "content.opf";
        private const string TOC = "toc.ncx";
        private static readonly string[] RESERVED_PATH = new[] { MIME_TYPE, CONTAINER, CONTENT, TOC };
        private static readonly byte[] STARTING_POINT = FetchStartingPoint();
        private static readonly XNamespace OPF = "http://www.idpf.org/2007/opf";
        private static readonly XNamespace DC = "http://purl.org/dc/elements/1.1/";
        private static readonly XNamespace XSI = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace NCX = "http://www.daisy.org/z3986/2005/ncx/";

        private readonly string _title;
        private readonly string _identifier;
        private readonly string _author;
        private readonly string _language;
        private readonly List<ChapterInfo> _chapters = new List<ChapterInfo>();
        private readonly List<ResourceInfo> _resources = new List<ResourceInfo>();
        private ZipArchive _archive;
        private bool _isTocWritten = false;
        #endregion

        #region Constructors
        /// <summary>Creates a writer connected to a <see cref="Stream"/>.</summary>
        /// <param name="stream">
        /// Output stream ; must be at position zero and have zero length in order for the resulting epub
        /// to make sense.
        /// </param>
        /// <param name="leaveOpen">
        /// <c>true</c> to leave the stream open after the object is disposed; otherwise, <c>false</c>.
        /// </param>
        /// <param name="title">Title of the package.</param>
        /// <param name="author">Author of the package.</param>
        /// <param name="identifier">Unique identifier of the package (could be a uri).</param>
        /// <param name="culture">Culture of the package ; default is <c>en</c>.</param>
        /// <returns>Asynchronously created writer.</returns>
        public static async Task<EPubWriter> CreateWriterAsync(
            Stream stream,
            string title,
            string author,
            string identifier,
            CultureInfo culture = null,
            bool leaveOpen = false)
        {
            var writer = new EPubWriter(
                title,
                author,
                identifier,
                culture == null ? "en" : culture.TwoLetterISOLanguageName);

            await writer.InitWriterAsync(stream, leaveOpen).ConfigureAwait(false);

            return writer;
        }

        private EPubWriter(
            string title,
            string author,
            string identifier,
            string language)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException("title");
            }
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentNullException("author");
            }
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentNullException("identifier");
            }
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException("language");
            }

            _title = title;
            _author = author;
            _identifier = identifier;
            _language = language;
        }

        private async Task InitWriterAsync(Stream stream, bool leaveOpen)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            //  Start with a starting point
            //  Reason for that is that ZipArchive is unable to create an archive with the proper
            //  Compression in order to satisfy ePub specs
            //  So here we start with a correct ePub archive (containing the mimetype file)
            //await stream.WriteAsync(STARTING_POINT, 0, STARTING_POINT.Length);

            _archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen);

            var s = _archive.CreateEntry("File.txt");
            using (var writer = new StreamWriter(s.Open()))
            {
                writer.WriteLine("This is a file!");
            }

            await CreateEntry(CONTAINER, Template.Container, CompressionLevel.Optimal).ConfigureAwait(false);

        }
        #endregion

        #region Optional Package Information
        /// <summary>Publisher of the package.</summary>
        /// <remarks>Optional.</remarks>
        public string Publisher { get; set; }

        /// <summary>Date of creation of the package.</summary>
        /// <remarks>Optional ; if not specified, current date is used.</remarks>
        public DateTime? CreationDate { get; set; }
        #endregion

        #region Main API
        /// <summary>
        /// Releases the resources used by the current instance of the <see cref="EPubWriter"/> class.
        /// </summary>
        public void Dispose()
        {
            _archive.Dispose();
        }

        /// <summary>
        /// Writes the TOC of the package.
        /// Must be called and have completed before calling <see cref="Dispose"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the method has already been called.</exception>
        /// <returns>Asynchronous completion.</returns>
        public async Task WriteEndOfPackageAsync()
        {
            if (_isTocWritten)
            {
                throw new InvalidOperationException("WriteEndOfPackageAsync has already been called");
            }
            _isTocWritten = true;

            await WriteContentAsync().ConfigureAwait(false);
            await WriteTocAsync().ConfigureAwait(false);
        }

        /// <summary>Add a chapter to the e-pub package.</summary>
        /// <param name="fileName">File name of the chapter.</param>
        /// <param name="title">Title of the chapter.</param>
        /// <param name="content">Html content of the chapter.</param>
        /// <returns>Asynchronous completion.</returns>
        public async Task AddChapterAsync(string fileName, string title, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            var entry = CreateNewChapter(fileName, title);

            using (var streamWriter = new StreamWriter(entry.Open()))
            {
                await streamWriter.WriteAsync(content).ConfigureAwait(false);
                streamWriter.Flush();
            }

        }

        /// <summary>Adds a chapter to the e-pub package and return its writable stream.</summary>
        /// <param name="fileName">File name of the chapter.</param>
        /// <param name="title">Title of the chapter.</param>
        /// <returns>Chapter stream ; must be closed or disposed.</returns>
        public Stream GetChapterStream(string fileName, string title)
        {
            var entry = CreateNewChapter(fileName, title);

            return entry.Open();
        }

        /// <summary>Adds a resource (e.g. an image) to the e-pub package.</summary>
        /// <param name="fileName">File name of the resource.</param>
        /// <param name="mimeType">Mime type of the resource.</param>
        /// <param name="content">Byte-array representation of the resource.</param>
        /// <returns>Asynchronous completion.</returns>
        public async Task AddResourceAsync(string fileName, string mimeType, byte[] content)
        {
            var entry = CreateNewResource(fileName, mimeType);

            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            using (var stream = entry.Open())
            {
                await stream.WriteAsync(content, 0, content.Length);
            }
        }

        /// <summary>Adds a resource (e.g. an image) to the e-pub package and return its writable stream.</summary>
        /// <param name="fileName">File name of the resource.</param>
        /// <param name="mimeType">Mime type of the resource.</param>
        /// <returns>Resource stream ; must be closed or disposed.</returns>
        public Stream GetResourceStream(string fileName, string mimeType)
        {
            var entry = CreateNewResource(fileName, mimeType);

            return entry.Open();
        }

        private ZipArchiveEntry CreateNewResource(string fileName, string mimeType)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (string.IsNullOrWhiteSpace(mimeType))
            {
                throw new ArgumentNullException("mimeType");
            }
            if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(fileName)))
            {
                throw new ArgumentException("Must be a pure file name (i.e. without folder)", "fileName");
            }
            if (RESERVED_PATH.Contains(fileName))
            {
                throw new ArgumentException(
                    string.Format("'{0}' is a reserved file name", fileName),
                    "fileName");
            }

            _resources.Add(new ResourceInfo
            {
                FileName = fileName,
                MimeType = mimeType
            });

            var entry = _archive.CreateEntry(fileName);

            return entry;
        }

        private ZipArchiveEntry CreateNewChapter(string fileName, string title)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException("title");
            }
            if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(fileName)))
            {
                throw new ArgumentException("Must be a pure file name (i.e. without folder)", "fileName");
            }
            if (RESERVED_PATH.Contains(fileName))
            {
                throw new ArgumentException(
                    string.Format("'{0}' is a reserved file name", fileName),
                    "fileName");
            }

            _chapters.Add(new ChapterInfo
            {
                FileName = fileName,
                Title = title
            });

            var entry = _archive.CreateEntry(fileName);

            return entry;
        }
        #endregion

        #region Write Content
        private async Task WriteContentAsync()
        {
            var content = XDocument.Parse(Template.Content);
            var package = content.Element(OPF + "package");
            var metaData = package.Element(OPF + "metadata");
            var manifest = package.Element(OPF + "manifest");
            var spine = package.Element(OPF + "spine");

            metaData.RemoveNodes();
            FillMetaData(metaData);

            manifest.RemoveNodes();
            FillManifest(manifest);

            spine.RemoveNodes();
            FillSpine(spine);

            await CreateEntry(CONTENT, content.ToString(), CompressionLevel.Optimal).ConfigureAwait(false);
        }

        private void FillMetaData(XElement metaData)
        {
            metaData.Add(new XElement(DC + "title", _title));
            metaData.Add(new XElement(
                DC + "date",
                new XAttribute(OPF + "event", "publication"),
                (CreationDate ?? DateTime.Now).ToString("yyyy-MM-dd")));
            metaData.Add(new XElement(
                DC + "creator",
                new XAttribute(OPF + "file-as", _author),
                new XAttribute(OPF + "role", "aut"),
                _author));
            metaData.Add(new XElement(DC + "language", _language));
            if (!string.IsNullOrWhiteSpace(Publisher))
            {
                metaData.Add(new XElement(DC + "publisher", Publisher));
            }
            metaData.Add(new XElement(
                DC + "identifier",
                new XAttribute("id", "BookID"),
                new XAttribute(OPF + "scheme", "ePubBud"),
                _identifier));
        }

        private void FillManifest(XElement manifest)
        {
            var chaptersItem = from c in _chapters.Zip(
                                   Enumerable.Range(1, _chapters.Count),
                                   (c, i) => new { Chapter = c, Index = i })
                               select new XElement(
                                   OPF + "item",
                                   new XAttribute("id", "chapter" + c.Index),
                                   new XAttribute("href", c.Chapter.FileName),
                                   new XAttribute("media-type", "application/xhtml+xml"));
            var resourcesItem = from r in _resources.Zip(
                                   Enumerable.Range(1, _resources.Count),
                                   (r, i) => new { Resource = r, Index = i })
                                select new XElement(
                                    OPF + "item",
                                    new XAttribute("id", "resource" + r.Index),
                                    new XAttribute("href", r.Resource.FileName),
                                    new XAttribute("media-type", r.Resource.MimeType));

            foreach (var item in chaptersItem.Concat(resourcesItem))
            {
                manifest.Add(item);
            }

            //  Add TOC
            manifest.Add(new XElement(
                OPF + "item",
                new XAttribute("id", "ncx"),
                new XAttribute("href", "toc.ncx"),
                new XAttribute("media-type", "application/x-dtbncx+xml")));
        }

        private void FillSpine(XElement spine)
        {
            var spineItem = from c in _chapters.Zip(
                                   Enumerable.Range(1, _chapters.Count),
                                   (c, i) => new { Chapter = c, Index = i })
                            select new XElement(
                                OPF + "itemref",
                                new XAttribute("idref", "chapter" + c.Index));

            foreach (var item in spineItem)
            {
                spine.Add(item);
            }
        }
        #endregion

        #region Write TOC
        private async Task WriteTocAsync()
        {
            var content = XDocument.Parse(Template.Toc);
            var ncx = content.Element(NCX + "ncx");
            var identifier = ncx.Element(NCX + "head").Element(NCX + "meta").Attribute("content");
            var title = ncx.Element(NCX + "docTitle").Element(NCX + "text");
            var author = ncx.Element(NCX + "docAuthor").Element(NCX + "text");
            var navMap = ncx.Element(NCX + "navMap");

            identifier.Value = _identifier;
            title.Value = _title;
            author.Value = _author;

            navMap.RemoveNodes();
            FillNavPoint(navMap);

            await CreateEntry(TOC, content.ToString(), CompressionLevel.Optimal).ConfigureAwait(false);
        }

        private void FillNavPoint(XElement navMap)
        {
            var navItem = from c in _chapters.Zip(
                                   Enumerable.Range(1, _chapters.Count),
                                   (c, i) => new { Chapter = c, Index = i })
                          select new XElement(
                              NCX + "navPoint",
                              new XAttribute("id", "navPoint-" + c.Index),
                              new XAttribute("playOrder", c.Index),
                              new XElement(
                                  NCX + "navLabel",
                                  new XElement(NCX + "text", c.Chapter.Title)),
                              new XElement(
                                  NCX + "content",
                                  new XAttribute("src", c.Chapter.FileName)));

            foreach (var item in navItem)
            {
                navMap.Add(item);
            }
        }
        #endregion

        private async Task CreateEntry(string path, string content, CompressionLevel level)
        {
            var entry = _archive.CreateEntry(path, level);

            using (var streamWriter = new StreamWriter(entry.Open()))
            {
                await streamWriter.WriteAsync(content).ConfigureAwait(false);
            }
        }

        private static byte[] FetchStartingPoint()
        {
            var currentAssembly = typeof(EPubWriter).GetTypeInfo().Assembly;

            var names = currentAssembly.GetManifestResourceNames();

            using (var stream =
                currentAssembly.GetManifestResourceStream("WebToKindle.Helper.Templates.StartingPoint.zip"))
            {
                var buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }
    }

}
