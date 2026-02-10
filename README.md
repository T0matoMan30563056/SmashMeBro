# SmashMeBro

## Projektbeskrivelse
### Formål med applikasjonen:
Det vi Ønsker å oppnå med prosjektet er å laget et funskionerene spill, med loggin, registering og håndtering av spillerens stats og achivments ved bruk av c# som frontend og mariadb og python som beckend på rassbarry pi.

### Brukerflyt:
Brukeren kan lage en account som blir lagret i vårt Account tabell sammen med passord som er incryptert og dagen accounten er lagret. Etter dette kan spilleren spille spillet vårt som gjør at du kan få stats som kills, deaths og dmg som blir lagret i vårt Stats tabell med en forreig key i account_id, etter du har fått nokk kills så for du achivments for disse kills, deaths eller dmg som blir håndert av en trigger funskion inni mariadb som skjøres hver gang vi oppdaterer stats, det gjør sånn at du kan få achivments eller so og so mange kills.

### Teknologier brukt:
Vi bruker python, flask som beckend på vår rassbarry pi, med mariadb database og c# unity som frontend
