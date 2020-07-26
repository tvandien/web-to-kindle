import pymysql
import json

def get_db_connection():
  try:
    with open("settings.json") as settings:
      details = json.load(settings)
  except: 
    print("Unable to open settings.json, please configure your database connection")
    exit()
  conn = pymysql.connect(host=details['server'], 
    user=details['username'], 
    password=details['password'], 
    db=details['database']) 
  return conn

def check_chapter(url, id, db):
  if chapter_uptodate(id, db):
    print("exists, loading from cache")
  else:
    print("chapter must be refreshed")

def chapter_uptodate(id, db):
  with db.cursor() as cursor:
    # Read a single record
    sql = ("SELECT count(*) FROM chapters WHERE chapter_id=%s "
           "AND last_updated > (CURRENT_DATE() - INTERVAL 1 MONTH)")
    cursor.execute(sql, (id))
    result = cursor.fetchone()
    return result[0] != 0

def get_chapter_info_from_db(id, db):
  with db.cursor() as cursor:
    sql = ("SELECT C1.chapter_id, C1.chapter_title, C1.chapter_content FROM chapters C1 "
           "WHERE NOT EXISTS(SELECT * FROM chapters C2 WHERE C2.version > C1.version AND C2.chapter_id=%s) "
           "AND C1.chapter_id = %s")
    cursor.execute(sql, (id, id))
    result = cursor.fetchone()
    return ('Chapter ' + str(result[0]), result[1], result[2].decode('utf-8'))

def add_chapter_to_db(chapter_id, chapter_title, chapter_content, db):
  with db.cursor() as cursor:
    sql = ("INSERT INTO chapters (chapter_id, chapter_title, chapter_content, version, last_updated) "
           "VALUES (%s, %s, %s, (SELECT version FROM "
           "(SELECT IFNULL(max(version), 0) AS version "
           "FROM chapters WHERE chapter_id=%s) AS version_table) + 1, CURRENT_DATE())")
    cursor.execute(sql, (chapter_id, chapter_title, chapter_content, chapter_id))
  db.commit()

