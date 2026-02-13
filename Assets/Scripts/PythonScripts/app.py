from flask import Flask, render_template, request, redirect, url_for, session, flash, jsonify
from werkzeug.security import generate_password_hash, check_password_hash
import os
import mysql.connector
from dotenv import find_dotenv, load_dotenv
from datetime import datetime, timezone
app = Flask(__name__)


app.secret_key = "SuperSecratKey"
dotenv_path = find_dotenv()
load_dotenv(dotenv_path)
env_User = os.getenv("DB_USER")
env_Host = os.getenv("DB_HOST")
env_Password = os.getenv("DB_PASSWORD")
env_Database = os.getenv("DB_DATABASE")


def get_db():
    return mysql.connector.connect(
        host=env_Host,
        user=env_User,
        password=env_Password,
        database=env_Database
    )


@app.route("/ping", methods=["GET"])
def ping():
    return jsonify({"status": "ok"})


@app.route("/SignUp", methods=["POST", "GET"])
def SignUp():
    db = get_db()
    cursor = db.cursor()
    data = request.get_json()
    Username = data.get("username")
    Password = generate_password_hash(data.get("password"))

    try:
        cursor.execute("INSERT INTO Accounts (Username, Password) VALUES (%s, %s)", (Username, Password))
        db.commit()

        account_id = cursor.lastrowid
        cursor.execute("INSERT INTO Stats(account_id, DateC) VALUES (%s, NOW())", (account_id,))
        db.commit()
        session["Username"] = data.get("username")
        return jsonify({
            "SessionUsername": data.get("username"),
            "Success": True
        })

    except Exception as e:
        print(e)
        return jsonify({
            "Error occured": "Account may already be in use",
            "success": False
        })
@app.route("/login", methods=["POST"])
def login():
    data = request.get_json()
    Username = data.get("username")
    Password = data.get("password")
    db = get_db()
    cursor = db.cursor(dictionary=True)

    try:
        cursor.execute("SELECT * FROM Accounts WHERE Username=%s", (Username,))
        user = cursor.fetchone()

        if user and check_password_hash(user["Password"], Password):
            session['username'] = Username

            return jsonify({
                  "SessionUsername": Username,
                  "Success": True
            })

        cursor.close()
        db.close()
    except Exception as e:
        print(e)
        return jsonify({
            "Error": "Error occured",
            "Success": False
        })
@app.route("/GetStats", methods=["POST"])
def GetStats():
    db = get_db()
    cursor = db.cursor(dictionary = True)
    try:
        username = session.get('username')
        cursor.execute("SELECT id FROM Accounts WHERE Username = %s", (username,))
        account = cursor.fetchone()
        account_id = account["id"]

        cursor.execute("SELECT * FROM Stats WHERE account_id = %s", (account_id,))
        JSONinfo = cursor.fetchone()

        cursor.close()
        db.close()

        return jsonify({
             "Kills": JSONinfo['Kills'],
             "Deaths": JSONinfo['Deaths'],
             "Dmg": JSONinfo['Dmg'],
             "CreationDate": JSONinfo['DateC']
        })

    except Exception as e:
        return jsonify({
            "Error": "Error occured",
            "Success": False
        })

@app.route("/StatsUpdater", methods=["POST"])
def StatsData():
    data = request.get_json()
    username = session.get('username')
    Kills = data.get("Kills")
    Deaths = data.get("Deaths")
    Dmg = data.get("Dmg")

    db = get_db()
    cursor = db.cursor(dictionary=True)


    try:
        cursor.execute("SELECT id FROM Accounts WHERE Username=%s", (username,))
        account = cursor.fetchone()

        account_id = account["id"]

        if account_id != None:
            cursor.execute("SELECT Kills, Deaths, Dmg FROM Stats WHERE account_id = %s", (account_id,))
            stats = cursor.fetchone()

            if stats:
                new_Kills = stats["Kills"] + Kills
                new_Deaths = stats["Deaths"] + Deaths
                new_Dmg = stats["Dmg"] + Dmg

                cursor.execute("UPDATE Stats SET Kills = %s, Deaths = %s, Dmg = %s WHERE account_id = %s",
                (new_Kills, new_Deaths, new_Dmg, account_id))
                db.commit()

                cursor.execute("SELECT Kills, Deaths, Dmg, DateC FROM Stats WHERE account_id = %s", (account_id,))
                JSONstats = cursor.fetchone()

                cursor.close()
                db.close()
                return jsonify({
                    "Kills" : JSONstats["Kills"],
                    "Deaths" : JSONstats["Deaths"],
                    "Dmg" : JSONstats["Dmg"],
                    "CreationDate" : JSONstats["DateC"]
                })
            else:
                cursor.close()
                db.close()
                return jsonify({
                    "success": False,
                    "error": "Stats record not found"
                })

        else:
            cursor.close()
            db.close()
            return jsonify({
                "success": False,
                "error": "User not found"
            })

    except Exception as e:
        print(e)

        cursor.close()
        db.close()
        return jsonify({
            "success": False,
            "error": "Error saving stats"
        })


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)