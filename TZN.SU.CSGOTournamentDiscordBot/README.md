# TZN.SU.CSGOTournamentDiscordBot

Bot używany do automatycznego przypisywania ról graczom na serwerze Discorda dla turnieju gier.

## Sposób użycia
1. Skompiluj (pisano na .NET 7).
2. Uzupełnij dane w pliku `appsettings.json` w sekcji `BotOptions` (token bota na discordzie - do wygenerowania w konsoli deweloperskiej Discorda i lokalizacja pliku CSV z drużynami - domyślnie `AppData/data.csv`. Dokumentacja również w pliku `BotOptions.cs`.
3. Wrzuć plik `data.csv` z danymi drużyn. Pierwsze pole to nazwa drużyny, kolejne to tag na Discordzie kapitana, kolejne 4 to członkowie drużyny. Plik taki można łatwo wygenerować np. z formularza w Wordpressie, usuwając następnie niepotrzebne pola. Pierwszy wiersz z pliku zostanie pominięty, bo jest to wiersz nagłówka.
4. Całość wgraj na jakiś serwer, uruchom (np. jako demona linuxowego).
5. Gotowe - użytkownicy będą automatycznie dodawani do ról z nazwą taką, jak nazwa ich drużyny. Jeśli rola nie będzie istnieć, zostanie utworzona. Nazwy ról dłuższe niż 100 znaków zostaną przycięte. Użytkownicy bez przypisanej drużyny w pliku CSV zostaną pominięci.
6. Użytkownicy będący kapitanami drużyn będą dodawani do roli `Kapitanowie` (do zmiany w pliku konfiguracyjnym). Rola zostanie utworzona automatycznie.
7. Użytkownicy będący kapitanami lub członkami drużyn będą dodawani do roli `Zawodnicy` (do zmiany w pliku konfiguracyjnym). Rola zostanie utworzina automatycznie.

**Copyright Dominik Orlikowski 2023**