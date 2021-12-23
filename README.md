# Torpedó játék

> .NET-es grafikus (WPF keretrendszerben írt) játék elkészítése.

## Játék leírása

[Magyar](https://hu.wikipedia.org/wiki/Torped%C3%B3_(j%C3%A1t%C3%A9k))

## Követelmények

### Egymás ellen lehessen játszani ugyanazon a gépen, ugyanabban az alkalmazásban

- Két játékos nevet kér be
- A soron kövezkező játékos csak a saját játékmezőjét látja (életszerű helyzet szimulálása)

### AI ellen lehessen játszani

- Egy játékos nevet kér be
- Kezdő játékos véletlenszerűen választva
- AI elemezési sorrend
    1. Random találgat
    2. Ha van találat akkor már a mellette lévő mezőket lövi
         - UNIT tesztet írni a logikához (randomhoz nem muszáj)

### Játékmenet

- Belépéskor kérjen nevet, majd ahhoz mentse az eredményeket
    - Név nem lehet: üres, whitespace, különleges karakterek szűrése pl !?_-:;#
- Eredményjelző
    - Körök száma
    - Saját találatok
    - Ellenfél találatai
    - Milyen hajók vannak még és melyek lettek elsüllyesztve
    - Billentyűkombinációra mutassa meg az AI hajóit (Csak AI ellen működjön)

### Játék vége

- Tárolja le az eredményeket
    - Adatok tárolása: JSON, XML vagy adatbázis(Entity FWK)
- Mindenkori eredménylista beolvasva tárolt adatokból
    - Listázza az egyes korábbi menetek adatai táblázatban (játékosok, körök száma, játékosok találatai, nyertes)

### Egyéb követelmények

- Git repo
    - Rendszeres commit minden csapattagtól
- Egy solution használata
- Egy haladó technológia használata a felsoroltak közül (Entity FWK, Blazor, Async, MVVM)
- Konvenciók alkalmazása: [NI C# Style Guide](https://github.com/ni/csharp-styleguide)
    - Java, Python stílusú elnvezéseket ne használjatok
