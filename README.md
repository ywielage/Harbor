# Harbor
## Uitbreidingen en aanpassingen Maartje

Een samenvatting van de verbeteringen op deze branch:

* Het multithreaded aanmaken van schepen en dockingstations.
* Het verwijderen van de timer (deze zijn vervangen door thread.delays), nu wacht het programma ook op de UI thread en kan het programma dus niet achterlopen als het updaten van de UI te lang duurt. 
* Enkele loops code vervangen door (p)linq queries voor betere performance.
* Creëer tijd van de harbor onderdelen zichtbaar in de eerste 2 seconden na het aanmaken van een harbor (threaded en non threaded).
* Threadsave maken van het ophalen en setten van een availabledockingstation.
* Het verwijderen van de threadpool (dit wordt nu met een Task[] gedaan).
* Het verwijderen van deadcode en optimaliseren van (te grote) bestaande methodes
