# Voorraadleenbeheersysteem

Dit systeem zorgt ervoor dat ROC-Nijmegen veilig producten kan uitlenen aan beide studenten en docenten.
Door het gebruik van een NFC-scanner kunnen uitleners gemakkelijk leners helpen.

## Dev-info

### Dependencies:

Alles gemarkeerd met een sterretje is een optionele dependency voor in development. De rest is nodig voor het programma.

- .NET SDK 10.0.x / ASP.NET Core Runtime 10.0.x
    - MacOS/Windows: Installeer deze
      op [de downloadpagina van .NET](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
    - Linux: Installeer deze met je gebruikelijke package manager
- PCSC-drivers:
    - Dit zijn de drivers die je nodig hebt om te communiceren met je NFC-scanner.
    - Linux:
        - Voor Linux moet je PCSC Lite installeren. Dit geeft in de meeste gevallen een systemctl service toe die moet
          draaien om dit programma te gebruiken. Deze kun je aanzetten met ```sudo systemctl start pcscd```
        - Arch en gerelateerd:
            - De drivers zijn te installeren met ```sudo pacman -S pcsclite pcsc-tools ccid```
        - Debian en gerelateerd:
            - De drivers zijn te installeren met ```sudo apt install pcscd pcsc-tools libccid```
        - Fedora en gerelateerd:
            - De drivers zijn te installeren met ```sudo dnf install pcsc-lite pcsc-lite-ccid pcsc-tools```
    - MacOS:
        - MacOS gebruikt zijn eigen smartcard libraries.
    - Windows:
        - Afhankelijk aan het type NFC-scanner die je gebruikt, maar ook Windows heeft een eigen smartcard library. Vaak
          is het zo dat deze niet werkt, installeer dan de PCSC-drivers van de fabrikant van je NFC-scanner.
- Virtual smart card*
    - VSmartCard is een kaart-emulator die het testen makkelijker maakt.
      In [de documentatie](https://frankmorgner.github.io/vsmartcard/index.html) zie je hoe je dit kunt gebruiken om
      virtuele kaarten te maken voor gebruik in het programma.
    - Linux:
        - Arch en gerelateerd:
            - Beschikbaar via de AUR onder de naam ```aur/vsmartcard```, deze is te installeren op de manier van voorkeur. Wil je de AUR niet gebruiken, kun je het altijd handmatig compileren volgens [de instructies van vsmartcard](https://frankmorgner.github.io/vsmartcard/virtualsmartcard/README.html#installation-on-linux-unix-and-similar).
        - Debian en gerelateerd:
            - Direct beschikbaar in de officiële base repositories. Installatie via
              ```sudo apt install vsmartcard-vpcd vsmartcard-vpicc```.
        - Fedora en gerelateerd:
            - Beschikbaar via de officiële Copr-repository (jjelen/vsmartcard). Schakel in en installeer met:
              ```sudo dnf copr enable jjelen/vsmartcard && sudo dnf install vsmartcard-vpcd vsmartcard-vpicc```
    - MacOS:
        - Te installeren via Homebrew
          of [handmatige compilatie van vsmartcard](https://frankmorgner.github.io/vsmartcard/virtualsmartcard/README.html#building-and-installing-vpcd-on-mac-os-x).
          Via Homebrew: ```brew install pcsc-tools```
    - Windows:
        - Te installeren via handmatige compilatie of klaargemaakte bestanden zonder certificaten.
          Volg [de instructies van vsmartcard voor Windows](https://frankmorgner.github.io/vsmartcard/virtualsmartcard/README.html#building-vpcd-on-windows).

### Code cleanup

Voor het formatteren maken we gebruik van JetBrains Code-Cleanup. Mocht je Rider gebruiken, neemt het automatisch de
opties over van de configuratiebestanden. Je kunt het dan ook direct uitvoeren in Rider zelf (ctrl + shift + alt + L).
Gebruik dan Code-Cleanup over de hele solution voor het maken een PR, het is een benodigdheid om te kunnen mergen.

Als je iets anders gebruikt, installeer dan de CLI-versie van de tool met:
```shell
dotnet tool install -g JetBrains.ReSharper.GlobalTools
```
Om het uit te voeren zoals de Github-Action dat doet:
```shell
jb cleanupcode StorageBorrowManagement.sln --profile="Built-in: Full Cleanup" --verbosity=WARN
```

### Packages

Deze solution gebruikt centraal pakketbeheer (CPM). Informatie hierover lees je op [de officiële documentatiepagina](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management).
Als je pakketten wilt toevoegen, probeer dan je IDE te gebruiken omdat deze het kan versimpelen. Mocht dit niet kunnen, gebruik dan de commandline in de directory van het project waarin je een pakket wilt toevoegen:
```shell
dotnet package add <package_name>
```
Ga niet handmatig sleutelen aan de projectbestanden hiervoor, omdat CPM anders verstoort kan worden.

Installeer de NuGet pakketten in de solution met deze commando:
```shell
dotnet restore
```

### Testen

In deze solution hebben we twee testprojecten (Backend.Tests en Frontend.Tests). Hierin wordt [NUnit](https://docs.nunit.org/) gebruikt voor de testen.
Alle testen moet slagen voor je een PR kan mergen. Voer elke test in het project uit met:
```shell
dotnet test --no-restore
```
