# Prosjektbeskrivelse – IT-utviklingsprosjekt (2IMI)
## SmashBros
________________________________________
Deltakere
Matej Mikic – Backend / PythonFlask <br>
Eduard Mannino – Frontend / c# unity
________________________________________
## Prosjektidé og problemstilling
Beskrivelse
Prosjektet er en blanding av vårt tidligere SmashBros prosjekt koblet sammen med nettvarks oppgaven. Det vi Ønsker å oppnå med prosjektet er å laget et funskionerene spill, med loggin, registering og håndtering av spillerens stats og achivments ved bruk av c# som frontend og mariadb og python som beckend på rassbarry pi. Målgruppen er barn/ungdommer som liker spill som brawlhalla eller smashbros.
________________________________________

## Funskioner
1.	Registrering av bruker
2.	Incryptering av passord
3.	Innlogging med session
4.	håndtering av Stats
5.	funksjonærene Achivments
6.	tilkobling fra Unity

________________________________________



## Teknologivalg
Vi bruker python, flask som beckend, sammen med C# på unity som vår frontend, databasen er på rassbarry pi og er en mariadb. Vi også bruker Github og Github Kanban og github desktop for å pushe filer.
________________________________________
## Datamodell
### Tabell for Accounts
| Field    | Type         | Null | Key | Default | Extra          |
| -------- | ------------ | ---- | --- | ------- | -------------- |
| id       | int(11)      | NO   | PRI | NULL    | auto_increment |
| Username | varchar(255) | NO   | UNI | NULL    |                |
| Password | varchar(255) | NO   |     | NULL    |                |


### Tabell for Stats
| Field      | Type     | Null | Key | Default | Extra          |
| ---------- | -------- | ---- | --- | ------- | -------------- |
| id         | int(11)  | NO   | PRI | NULL    | auto_increment |
| account_id | int(11)  | NO   | MUL | NULL    |                |
| Kills      | int(11)  | YES  |     | 0       |                |
| Deaths     | int(11)  | YES  |     | 0       |                |
| Dmg        | float    | YES  |     | 0       |                |
| DateC      | datetime | NO   |     | NULL    |                |


### Tabell for Achivments
| Field      | Type       | Null | Key | Default | Extra          |
| ---------- | ---------- | ---- | --- | ------- | -------------- |
| id         | int(11)    | NO   | PRI | NULL    | auto_increment |
| account_id | int(11)    | NO   | UNI | NULL    |                |
| AcKills1   | tinyint(1) | YES  |     | 0       |                |
| AcKills10  | tinyint(1) | YES  |     | 0       |                |
| AcKills67  | tinyint(1) | YES  |     | 0       |                |
| AcDeath1   | tinyint(1) | YES  |     | 0       |                |
| AcDeath10  | tinyint(1) | YES  |     | 0       |                |
| AcDeath67  | tinyint(1) | YES  |     | 0       |                |
| AcDmg1000  | tinyint(1) | YES  |     | 0       |                |
| AcDmg5000  | tinyint(1) | YES  |     | 0       |                |
| AcDmg10000 | tinyint(1) | YES  |     | 0       |                |
>


Vi bruker en trigger fordi det er lettere å lage og håndtere enn en hel fuksjion innen python

DELIMITER // <br>
CREATE TRIGGER UpdateAc <br>

AFTER INSERT ON Stats <br>
FOR EACH ROW <br>

BEGIN <br>
  INSERT INTO Achievements (account_id) <br>
  VALUES (NEW.account_id) <br>
  ON DUPLICATE KEY UPDATE account_id = account_id; <br>

  SELECT  <br>
      COALESCE(SUM(Kills), 0), <br>
      COALESCE(SUM(Deaths), 0), <br>
      COALESCE(SUM(Dmg), 0) <br>
  INTO  <br>
      @totalKills, <br>
      @totalDeaths, <br>
      @totalDmg <br>
  FROM Stats <br>
  WHERE account_id = NEW.account_id; <br>

  IF @totalKills >= 1 THEN <br>
      UPDATE Achievements SET AcKills1 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>

  IF @totalKills >= 10 THEN <br>
      UPDATE Achievements SET AcKills10 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>

  IF @totalKills >= 67 THEN <br>
      UPDATE Achievements SET AcKills67 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>

  IF @totalDeaths >= 1 THEN <br>
      UPDATE Achievements SET AcDeath1 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>

  IF @totalDeaths >= 10 THEN <br>
      UPDATE Achievements SET AcDeath10 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>

  IF @totalDeaths >= 67 THEN <br>
      UPDATE Achievements SET AcDeath67 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>

  IF @totalDmg >= 1000 THEN <br>
      UPDATE Achievements SET AcDmg1000 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>

  IF @totalDmg >= 5000 THEN <br>
      UPDATE Achievements SET AcDmg5000 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>

  IF @totalDmg >= 10000 THEN <br>
      UPDATE Achievements SET AcDmg10000 = TRUE WHERE account_id = NEW.account_id; <br>
  END IF; <br>
END//

DELIMITER ;


