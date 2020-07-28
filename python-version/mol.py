import urllib.request
import re
import pypandoc
import html
import os
import database
import mail
import json

"""
    - update_ebook
      - find_number_of_chapters
        - regex (type: chapter_count, string: 'Ch 1 of <a href=\'/s/2961893/[0-9]+/\'>([0-9]+)</a' )
      - get_page
        - get_chapter_from_page
          - regex (type: chapter_content, string: '(<strong>Chapter.+)</div></div>' )
        - get_chapter_title
          -regex (type: chapter_title, string: '<strong>(.+?)</strong>' )
"""


def get_output_directory():
  try:
    with open("settings.json") as settings:
      details = json.load(settings)
  except: 
    print("Unable to open settings.json, please configure your settings")
    exit()
  return details["output_directory"]

def init():
  output_directory = get_output_directory()
  try:
    os.stat(output_directory)
  except:
    os.mkdir(output_directory)

def get_page(url):
  print("downloading: " + url)
  opener = urllib.request.FancyURLopener({})
  f = opener.open(url)
  return (f.read()).decode('utf-8')
  
def find_number_of_chapters(url):
  page = get_page(url.format(1))
  match = re.findall('Ch 1 of <a href=\'/s/2961893/[0-9]+/\'>([0-9]+)</a', page)
  print('Found ' + match[0] + ' chapters')
  return int(match[0])

def update_ebook(url):
  number_of_chapters = find_number_of_chapters(url)
  db = database.get_db_connection()
  chapters = []
  changes = False
  
  for i in range(number_of_chapters):
    if database.chapter_uptodate((i+1), db):
      chapter = (database.get_chapter_info_from_db((i+1), db))
    else:
      chapter = (get_chapter_info(get_chapter_from_page(get_page(url.format(i + 1)))))
      database.add_chapter_to_db(chapter[0], chapter[1], chapter[2], db)
      create_ebook([chapter], 'mol-' + str(i+1), ' - Chapter ' + (str(i+1)))
      changes = True
    chapters.append(chapter)
  
  if changes:
    create_ebook(chapters, 'mol-full', '')

def get_chapter_from_page(page):
  match = re.search('(<strong>Chapter.+)</div></div>', page)
  return match.group(1)  

def get_chapter_info(chapter):
  match = re.findall('<strong>(.+?)</strong>', chapter)
  chapter_id = int(match[0].replace('Chapter ', ''))
  chapter_title = match[1]
  match = re.findall('<p(?: style=\'text-align:center;\'|)>(.+?)</p>', chapter)
  for paragraph in range(len(match)):
    if match[paragraph] == '- break -':
      match[paragraph] = '<center><pre>* * *</pre></center>'
    else:
      match[paragraph] = fix_encoding_issues(match[paragraph])
  return (chapter_id, chapter_title, '  \n&nbsp;&nbsp;&nbsp;&nbsp;'.join(match[1:len(match)]))

def fix_encoding_issues(text):
  ret = html.escape(text).encode('ascii', 'xmlcharrefreplace').decode()
  ret = ret.replace('&lt;', '<')
  ret = ret.replace('&gt;', '>')
  return ret
  
def generate_markdown_full_ebook(chapters, subtitle):
  markdown = '% Mother of Learning' + subtitle + '\n'
  markdown = markdown + '% Domagoj Kurmaic\n'
  
  for (chapter_id, chapter_title, chapter_content) in chapters:
    markdown = markdown + '# ' + chapter_id + ' <br />' + fix_encoding_issues(chapter_title) + '\n\n'
    markdown = markdown + chapter_content + '\n\n'
  
  return markdown
  
def convert_markdown_to_epub(markdown, epub):
  pypandoc.convert_text(markdown, 'epub', format='md', outputfile=epub)

def convert_epub_to_mobi(epub, mobi):
  if os.name == 'nt':
    os.system('kindlegen.exe ' + epub + ' -o ' + mobi)
  else:
    os.system('./kindlegen ' + epub + ' -o ' + mobi)
  
def create_ebook(chapters, name, subtitle):
  print("Creating " + name + " ebook")
  output_directory = get_output_directory()
  markdown = generate_markdown_full_ebook(chapters, subtitle)
  convert_markdown_to_epub(markdown, output_directory + '/' + name + '.epub')
  convert_epub_to_mobi(output_directory + '/' + name + '.epub', name + '.mobi')

init()
update_ebook("https://m.fictionpress.com/s/2961893/{0}/Mother-of-Learning")
mail.send_mails()
