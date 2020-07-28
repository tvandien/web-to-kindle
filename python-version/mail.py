import requests
import database
import json

def get_mail_connection():
  try:
    with open("settings.json") as settings:
      details = json.load(settings)
  except Exception as e:
    print("Unable to open settings.json, please configure your mail connection")
    print(e)
    exit()
  return (details['mailgun_api_key'], details['mailgun_domain'], details['mail_from'])

def send_ebook_to_kindle(kindle_address, mail_subject, mail_content, mail_attachment):
  (mailgun_api_key, mailgun_domain, mail_from) = get_mail_connection()
  url = 'https://api.mailgun.net/v3/{0}/messages'.format(mailgun_domain)
  auth = ('api', mailgun_api_key)
  data = {
    'from': mail_from,
    'to': kindle_address,
    'subject': mail_subject,
    'text': mail_content
  }
  files = [("attachment", open(mail_attachment, "rb"))]
  response = requests.post(url, auth=auth, data=data, files=files)
  response.raise_for_status()

def send_mail(kindle_address, chapter_id, name):
  print("Sending chapter " + str(chapter_id) + " to " + kindle_address + " (" + name + ")")
  send_ebook_to_kindle(kindle_address, "Mother of Learning chapter " + str(chapter_id), 
    "Hello " + name + ", this is chapter " + str(chapter_id) + " of Mother of Learning",
    "output/mol-" + str(chapter_id) + ".mobi")

def send_mails():
  print("Sending out mails...")
  db = database.get_db_connection()
  with db.cursor() as cursor:
    sql = ("SELECT name, kindle_address, mail_address, chapter_id "
           "FROM targets JOIN chapters ON chapters.chapter_id > targets.last_chapter")
    cursor.execute(sql)
    result = cursor.fetchone()
    while result:
      send_mail(result[1], result[3], result[0])
      result = cursor.fetchone()
  with db.cursor() as cursor:
    sql = ("UPDATE targets SET last_chapter=(SELECT max(chapter_id) FROM chapters)")
    cursor.execute(sql)
  db.commit()

