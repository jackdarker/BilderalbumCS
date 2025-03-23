# BilderalbumCS
a directory based picture-browser written in C#/.net/WindowsForms  

# Todo:
- suche mit bildvorschau
- eine Datenbank für alle Bilder ist ungünstig, es müsste eine pro Verzeichnis sein?
- bildumschaltung im Viewer führt nicht zu neuladen der Datenbankinfo
- helligkeit ändern
- Verzeichnis löschen
- Verzeichnis umbenennen
- shortkeys für navigation
- gif-animation



# Solved:
- drag'n'drop
- schriftgröße TreeView änderbar
- Unterstützung für png, jpeg
- Viewer-Fenster in gültigen Koordinaten öffnen
- zusätzlicher Datei löschen -Button im Datei-verschieben Dialog
- manchmal Fehler wenn Datei gelöscht werden soll weil Datei noch Thumbnail-Generator geöffnet
- beim Auswählen eines Verzeichnisses wird manchmal nicht die Bildliste aktualisiert
- beim löschen /verschieben wird manchml nicht die Bildliste aktualisiert
- Thumbnails werden manchmal nicht alle erzeugt
- vorhandene Thumbnails bei Seitenaktualisierung wiederverwenden
- Zugriffs-Fehler beim Bild verschieben/löschen
- Browser öffnen auf rechtsgeklickten Ordner
- wechseln auf Ordner nach Neuerstellung
- Seitenzähler korrigieren wenn Bilder gelöscht /hinzugefügt werden
- Exception bei ungültigem Drive, Zugriffsfehler abfangen
- bild liste wird nicht aktualisiert wenn Verzeichnis mit gleicher Bildanzahl gewählt wird
- wiederherstellen der letzten Sitzung