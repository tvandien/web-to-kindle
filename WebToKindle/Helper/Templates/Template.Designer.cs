﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebToKindle.Helper.Templates {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Template {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Template() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebToKindle.Helper.Templates.Template", typeof(Template).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot; ?&gt;
        ///&lt;container version=&quot;1.0&quot; xmlns=&quot;urn:oasis:names:tc:opendocument:xmlns:container&quot;&gt;
        ///  &lt;rootfiles&gt;
        ///    &lt;rootfile full-path=&quot;content.opf&quot; media-type=&quot;application/oebps-package+xml&quot;/&gt;
        ///  &lt;/rootfiles&gt;
        ///&lt;/container&gt;
        ///.
        /// </summary>
        public static string Container {
            get {
                return ResourceManager.GetString("Container", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;package xmlns=&quot;http://www.idpf.org/2007/opf&quot; version=&quot;2.0&quot; unique-identifier=&quot;BookID&quot;&gt;
        ///  &lt;metadata xmlns:dc=&quot;http://purl.org/dc/elements/1.1/&quot; xmlns:opf=&quot;http://www.idpf.org/2007/opf&quot;&gt;
        ///    &lt;!--
        ///      &lt;dc:title&gt;My title&lt;/dc:title&gt;
        ///      &lt;dc:date opf:event=&quot;publication&quot;&gt;2011-09-21&lt;/dc:date&gt;
        ///      &lt;dc:creator opf:role=&quot;aut&quot; opf:file-as=&quot;Jackie, Jackie&quot;&gt;Jackie&lt;/dc:creator&gt;
        ///      &lt;dc:language&gt;en&lt;/dc:language&gt;
        ///      &lt;dc:publisher&gt;ePub Bud (www.epubbud.com)&lt;/dc:publisher&gt;
        ///   [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Content {
            get {
                return ResourceManager.GetString("Content", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;!DOCTYPE ncx PUBLIC &quot;-//NISO//DTD ncx 2005-1//EN&quot;
        ///   &quot;http://www.daisy.org/z3986/2005/ncx-2005-1.dtd&quot;&gt;
        ///&lt;ncx xml:lang=&quot;en&quot; xmlns=&quot;http://www.daisy.org/z3986/2005/ncx/&quot; version=&quot;2005-1&quot;&gt;
        ///  &lt;head&gt;
        ///    &lt;meta name=&quot;dtb:uid&quot; content=&quot;TODO&quot;/&gt;
        ///    &lt;meta name=&quot;dtb:depth&quot; content=&quot;1&quot;/&gt;
        ///    &lt;meta name=&quot;dtb:totalPageCount&quot; content=&quot;0&quot;/&gt;
        ///    &lt;meta name=&quot;dtb:maxPageNumber&quot; content=&quot;0&quot;/&gt;
        ///  &lt;/head&gt;
        ///  &lt;docTitle&gt;
        ///    &lt;text&gt;TODO&lt;/text&gt;
        ///  &lt;/docTitle&gt;
        ///  &lt;docAuthor&gt;
        ///    &lt;text&gt;TODO&lt;/text&gt;
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Toc {
            get {
                return ResourceManager.GetString("Toc", resourceCulture);
            }
        }
    }
}
