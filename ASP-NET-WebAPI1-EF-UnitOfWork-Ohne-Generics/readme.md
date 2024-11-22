# ASP.NET WebAPI 1 mit EF, Repository und UnitOfWork (Ohne Generics)

Dieses Web-API-Projekt nutzt Entity Framework (EF) zusammen mit den Designmustern Repository und Unit of Work, um einen modularen und verwaltbaren Datenzugriff zu ermöglichen.

----

# Inhaltsverzeichnis
    
- [ASP.NET WebAPI 1 mit EF, Repository und UnitOfWork (Ohne Generics)](#aspnet-webapi-1-mit-ef-repository-und-unitofwork-ohne-generics)
- [Inhaltsverzeichnis](#inhaltsverzeichnis)
  - [Designmuster](#designmuster)
    - [Repository](#repository)
    - [Unit of Work](#unit-of-work)
    - [Dependency Injection](#dependency-injection)
  - [Schichten der Architektur](#schichten-der-architektur)
    - [Präsentationsschicht (Controllers)](#präsentationsschicht-controllers)
    - [Servicelayer (Services)](#servicelayer-services)
    - [Datenzugriffsschicht (Repositories)](#datenzugriffsschicht-repositories)
    - [Datenmodell-Schicht (Data)](#datenmodell-schicht-data)
    - [Infrastrukturkomponenten](#infrastrukturkomponenten)
- [Einfache ASP.NET MVC-Erstellung in VS Code](#einfache-aspnet-mvc-erstellung-in-vs-code)
  - [Datenbankstruktur - MS SQL Server](#datenbankstruktur---ms-sql-server)
    - [Erstellen einer MS SQL Server-Datenbanktabelle](#erstellen-einer-ms-sql-server-datenbanktabelle)
    - [Beispiel-Daten zur Person-Tabelle hinzufügen](#beispiel-daten-zur-person-tabelle-hinzufügen)
    - [Abfrage der Beispiel-Daten in der Person-Tabelle](#abfrage-der-beispiel-daten-in-der-person-tabelle)
  - [Ordnerstruktur](#ordnerstruktur)
  - [Abhängigkeiten](#abhängigkeiten)
  - [1. Neues Projekt erstellen](#1-neues-projekt-erstellen)
  - [2. Port-Einstellung vornehmen](#2-port-einstellung-vornehmen)
  - [3. Installation des mit .NET 6.0 kompatiblen Pakets Microsoft.AspNetCore.OpenApi](#3-installation-des-mit-net-60-kompatiblen-pakets-microsoftaspnetcoreopenapi)
  - [4. Entity Framework-Installation und DB First-Einstellungen](#4-entity-framework-installation-und-db-first-einstellungen)
    - [Installation des Entity Framework-Pakets](#installation-des-entity-framework-pakets)
    - [Hinzufügen der Verbindungszeichenfolge zur Datei `appsettings.json`](#hinzufügen-der-verbindungszeichenfolge-zur-datei-appsettingsjson)
    - [Erstellung von Model- und DbContext-Klasse mit DB First](#erstellung-von-model--und-dbcontext-klasse-mit-db-first)
      - [Weitere Optionen](#weitere-optionen)
    - [Projekt Starten](#projekt-starten)
  - [Repositories / IUnitOfWork.cs](#repositories--iunitofworkcs)
  - [Repositories / UnitOfWork.cs](#repositories--unitofworkcs)
  - [Repositories / IPersonRepository.cs](#repositories--ipersonrepositorycs)
  - [Repositories / PersonRepository.cs](#repositories--personrepositorycs)
  - [Services / IPersonService.cs](#services--ipersonservicecs)
  - [Services / PersonService.cs](#services--personservicecs)
  - [Controllers / PersonController.cs](#controllers--personcontrollercs)
  - [Anwendung dieser Klassen](#anwendung-dieser-klassen)
  - [Program.cs](#programcs)
  - [Projekt Starten und Testen](#projekt-starten-und-testen)
    - [Umleiten der Haupt-URL zur Swagger-Oberfläche](#umleiten-der-haupt-url-zur-swagger-oberfläche)
  - [7. CORS-Konfiguration durch Bearbeiten der Datei `Program.cs`](#7-cors-konfiguration-durch-bearbeiten-der-datei-programcs)
  - [8. Projekt kompilieren und testen](#8-projekt-kompilieren-und-testen)
    - [NICHT BENÖTIGTE DATEIEN LÖSCHEN](#nicht-benötigte-dateien-löschen)

Dieses Inhaltsverzeichnis bietet Ihnen eine Übersicht über die Struktur und die Elemente des Projekts, damit Sie gezielt Informationen finden können.

----

## Designmuster

### Repository
Das Repository-Muster kapselt alle CRUD-Operationen (z. B. `AddAsync`, `GetAllAsync`, `Remove`) für die `Person`-Entität in einer strukturierten Form. Dies verringert die Code-Wiederholung und erhöht die Wartbarkeit der Datenzugriffsschicht. EF Core sorgt dafür, dass Änderungen effizient gehandhabt werden und die Datenkonsistenz beibehalten wird.

### Unit of Work
Das Unit of Work-Muster ermöglicht es, mehrere Datenbankoperationen als eine transaktionale Einheit zu verwalten, wodurch atomare Transaktionen sichergestellt werden. Änderungen werden mit den Methoden `Commit` oder `CommitAsync` in die Datenbank übernommen.

### Dependency Injection
Dependency Injection (DI) ist ein Designprinzip, das die Entkopplung von Abhängigkeiten durch den DI-Container ermöglicht. In diesem Projekt wird DI verwendet, um Instanzen von Repositories und Services bereitzustellen. Durch die Registrierung von `IPersonRepository`, `IUnitOfWork` und `IPersonService` in der Startkonfiguration ermöglicht DI eine flexible und testbare Architektur.


## Schichten der Architektur

### Präsentationsschicht (Controllers)
Die Präsentationsschicht umfasst den `PersonController`, der eingehende HTTP-Anfragen verarbeitet. Der `PersonController.cs` definiert die API-Endpunkte für CRUD-Operationen auf `Person`-Entitäten und ruft entsprechende Services auf, um die Geschäftslogik auszuführen.

### Servicelayer (Services)
Dieser Layer besteht aus den Dateien `IPersonService.cs` und `PersonService.cs`, die die Geschäftslogik für die Verwaltung von `Person`-Daten implementieren. Der Service-Layer nimmt Anfragen vom Controller entgegen und nutzt das Unit of Work-Muster, um auf die Repositories zuzugreifen und Transaktionen zu handhaben.

### Datenzugriffsschicht (Repositories)
In diesem Layer finden sich `IPersonRepository.cs` und `PersonRepository.cs`. Sie kapseln den direkten Datenbankzugriff und führen CRUD-Operationen auf `Person`-Entitäten durch. Zudem verwalten `IUnitOfWork.cs` und `UnitOfWork.cs` das Transaktionsmanagement, indem sie mehrere Repository-Operationen in einer Einheit bündeln.

### Datenmodell-Schicht (Data)
Diese Schicht wird durch die Datei `Person.cs` repräsentiert, die die Definition der `Person`-Entität mitsamt ihren Eigenschaften enthält. Sie beschreibt die Datenstruktur, die in der Datenbank gespeichert wird.

### Infrastrukturkomponenten
Eine wichtige Komponente der Infrastruktur ist `PersonDbContext.cs`. Sie stellt den notwendigen Datenbankkontext bereit, der die Kommunikation zwischen der Anwendung und der Datenbank über Entity Framework Core ermöglicht.

Diese Schichten sorgen für eine klare Trennung von Verantwortlichkeiten und erleichtern das Testen, die Wartung und Erweiterung der Anwendung.


# Einfache ASP.NET MVC-Erstellung in VS Code

In diesem Leitfaden erkläre ich Ihnen Schritt für Schritt, wie Sie mit Visual Studio Code und .NET 6.0 eine einfache ASP.NET WebAPI erstellen und diese über Docker bereitstellen können. Sie können das Projekt abschließen, indem Sie die Schritte befolgen.


## Datenbankstruktur - MS SQL Server


### Erstellen einer MS SQL Server-Datenbanktabelle

Verwenden Sie das folgende Skript, um die Datenbanktabelle auf MS SQL Server zu erstellen. Dieses Skript erstellt die Tabelle Person:

MS SQL Server-Verbindungsinformationen

```` text

Server: .\sqlexpress
Server: 192.168.0.150\\SQLEXPRESS

Benutzername: sa
Passwort: www

Database: DbPerson

````

```` sql
CREATE TABLE [dbo].[Person](
    [PersonId] [int] IDENTITY(1,1) NOT NULL,
      NULL,
       CONSTRAY KEY CLUSTERED
    (
        [PersonId] ASC
    )
) ON [PRIMARY]
````

Dieses Skript erstellt die Tabelle Person mit den Feldern PersonId, Vorname, Nachname, Email und GebDatum. PersonId ist als automatisch inkrementierende Identitätsspalte definiert.


### Beispiel-Daten zur Person-Tabelle hinzufügen

Wir können der in MS SQL Server erstellten Person-Tabelle fünf Beispiel-Datensätze zu Testzwecken hinzufügen. Diese Daten gehören zu realistischen, fiktiven Personen, die wir beim Testen oder Entwickeln unserer API verwenden können. Fügen Sie diese Daten mit der folgenden INSERT-Abfrage in die Tabelle ein:

```` sql
INSERT INTO [dbo].[Person] (Vorname, Nachname, Email, GebDatum)
VALUES
('Max', 'Mustermann', 'max.mustermann@example.com', '1992-03-15'),
('Erika', 'Musterfrau', 'erika.musterfrau@example.com', '1989-06-20'),
('Felix', 'Mustersohn', 'felix.mustersohn@example.com', '1995-09-12'),
('Anna', 'Musterfrau', 'anna.musterfrau@example.com', '1990-01-05'),
('Lukas', 'Mustermann', 'lukas.mustermann@example.com', '1987-11-30');
````

Diese INSERT-Abfrage fügt der Person-Tabelle fünf neue Personen hinzu. Mit den Feldern Vorname, Nachname, Email und GebDatum werden der Vorname, Nachname, die E-Mail-Adresse und das Geburtsdatum jeder Person eingegeben.


### Abfrage der Beispiel-Daten in der Person-Tabelle

```` sql
SELECT * FROM Person
````

Ergebnis:

```` sql
1	Max	Mustermann	max.mustermann@example.com	1992-03-15
2	Erika	Musterfrau	erika.musterfrau@example.com	1989-06-20
3	Felix	Mustersohn	felix.mustersohn@example.com	1995-09-12
4	Anna	Musterfrau	anna.musterfrau@example.com	1990-01-05
5	Lukas	Mustermann	lukas.mustermann@example.com	1987-11-30
````

Diese Daten werden verwendet, um während der Entwicklung und des Testens der API zu verstehen und zu testen, wie CRUD-Operationen (Create, Read, Update, Delete) auf die Daten in der Datenbank angewendet werden.

Auf diese Weise können wir realistische Testszenarien erstellen und die Funktionalität der von uns entwickelten API überprüfen, während wir Datenbankverbindungen und Datenoperationen durchführen.


## Ordnerstruktur
Ihre Projektordnerstruktur sollte wie folgt aussehen:

``` bash
Projek/
├── PersonApi/
│   ├── Controllers/
│   │   └── PersonController.cs
│   ├── Data/
│   │   └── Person.cs
│   ├── Repositories/
│   │   ├── IPersonRepository.cs
│   │   └── IUnitOfWork.cs
│   ├── Services/
│   │   └── PersonService.cs
│   ├── Properties/
│   │   ├── appsettings.Development.json
│   │   └── appsettings.json
│   ├── Program.cs
│   └── PersonApi.csproj
└── PersonApi.sln
```

- appsettings.json: Datei mit Konfigurationseinstellungen einschließlich der Datenbank-Verbindungszeichenfolge und Logging.
- appsettings.Development.json: Entwicklungsumgebungsspezifische Konfiguration, die die allgemeinen Einstellungen von appsettings.json überschreibt oder ergänzt.
- Controllers/: Ordner, der die API-Controller enthält, wobei PersonController.cs die Anfragen für die 'Person'-Ressource verwaltet.
- Data/: Ordner, der das Person-Modell (Person.cs) beherbergt, welches die Datenstruktur für Personen beschreibt.
- Repositories/: Ordner für Repository-Interfaces und -Implementierungen, enthält IPersonRepository.cs und IUnitOfWork.cs, die CRUD-Operationen und Transaktionsmanagement definieren.
- Services/: Ordner für Geschäftslogik, der die Service-Klasse PersonService.cs für Personenoperationen enthält.
- Program.cs: Hauptprogrammdatei, die die Anwendungskonfiguration, Dienstregistrierung und Middleware-Pipeline steuert.
- PersonApi.csproj: Projektdatei, die das Ziel-Framework und die Paketabhängigkeiten definiert.
- PersonApi.sln: Visual Studio Lösungsdatei, die das Projekt organisiert und verwaltet.


## Abhängigkeiten

```` bash
PersonApi/
├── Data/
│   └── Person.cs: Datenmodell für Person, genutzt von Repositories, Services und Controllern.
├── Repositories/
│   ├── IPersonRepository.cs: Definiert Schnittstelle für Personendatenzugriff.
│   └── IUnitOfWork.cs: Definiert Schnittstelle für Transaktionsverwaltung.
├── Services/
│   └── PersonService.cs
│       ├── Abhängig von: IUnitOfWork für Geschäftslogikoperationen.
│       ├── Verwendet: Person.cs für Datenmodell.
├── Controllers/
│   └── PersonController.cs
│       ├── Abhängig von: IUnitOfWork für Datenoperationen.
│       ├── Nutzt: Person.cs für Datenmodell.
├── Program.cs
│   ├── Verwendet: appsettings.json, appsettings.Development.json für Konfigurationseinstellungen.
│   ├── Nutzt: IPersonRepository und IUnitOfWork für Datenzugriff und Services.
├── appsettings.json: Allgemeine Konfiguration für die Anwendung (wird von Program.cs verwendet).
├── appsettings.Development.json: Entwicklungspezifische Konfiguration (ergänzt/überschreibt appsettings.json).
├── PersonApi.csproj: Konfigurationsdatei für Projektabhängigkeiten und Target Framework.
└── PersonApi.sln: Lösung, die das Projekt PersonApi.csproj enthält.
```` 

In dieser hierarchischen Darstellung stehen fundamentale Modelle und Schnittstellen an oberster Stelle, da sie die Basisstruktur für Repositories, Services und Controller bilden.


## 1. Neues Projekt erstellen
Öffnen Sie das Terminal, um ein neues Projekt zu erstellen und auf .NET 6.0 abzuzielen:

```bash
dotnet new webapi -o PersonApi --framework net6.0
```

Wechseln Sie in das Projektverzeichnis:

```bash
cd PersonApi
```

Öffnen Sie das Projekt in Visual Studio Code:

```bash
code .
```

## 2. Port-Einstellung vornehmen
Öffnen Sie die Datei `launchSettings.json` und nehmen Sie die folgende Einstellung vor, damit das Projekt auf Port 5001 läuft:

```json

  "profiles": {
    "PersonApi": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },

```

Diese Einstellung sorgt dafür, dass das Projekt auf Port 6001 läuft und die Swagger-Oberfläche in der Entwicklungsumgebung automatisch geöffnet wird.


## 3. Installation des mit .NET 6.0 kompatiblen Pakets Microsoft.AspNetCore.OpenApi
Installieren Sie das Paket Swashbuckle.AspNetCore für Swagger/OpenAPI-Unterstützung, die mit .NET 6.0 kompatibel ist:

```bash
dotnet add package Swashbuckle.AspNetCore --version 6.2.3
```

Hinweis: Wenn später eine andere Version von Microsoft.AspNetCore.OpenApi installiert wurde oder das vorhandene Paket auf eine inkompatible Version aktualisiert wurde, können Sie das Paket mit folgendem Befehl entfernen:

```bash
dotnet remove package Microsoft.AspNetCore.OpenApi
```

----




## 4. Entity Framework-Installation und DB First-Einstellungen

Wir werden die notwendigen Schritte befolgen, um die Datenbank im Projekt mit Entity Framework zu verwalten.


### Installation des Entity Framework-Pakets

Installieren Sie das Entity Framework Core-Paket über NuGet:

```` bash

dotnet new tool-manifest
dotnet tool install dotnet-ef --version 6.0.35

dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.35
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.35
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.35

dotnet add package Microsoft.Extensions.Configuration.Json --version 6.0.0

````

Diese Pakete bieten die erforderlichen Tools für Migrationen und Konfigurationen im Projekt mit Entity Framework Core, das mit MS SQL Server kompatibel ist. Durch das Hinzufügen dieser Pakete können Sie eine vollständige EF Core-Installation im Projekt durchführen.

### Hinzufügen der Verbindungszeichenfolge zur Datei `appsettings.json`

Öffnen Sie die Datei `appsettings.json` und fügen Sie die Datenbankverbindungseinstellungen hinzu.

```` json
{

  "ConnectionStrings": {
    "DefaultConnection": "Server=192.168.0.150\\SQLEXPRESS;Database=DbPerson;User Id=sa;Password=www;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;"
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
````

Diese Einstellung ermöglicht die Verbindung zur Datenbank `DbPerson`.

----

### Erstellung von Model- und DbContext-Klasse mit DB First

Führen Sie den folgenden Befehl aus, um basierend auf der in der Datenbank vorhandenen Person-Tabelle automatisch das Model und die DbContext-Klasse zu erstellen:


Zugriff auf die Datenbank über appsettings.json:

```` bash
dotnet ef dbcontext scaffold Name=DefaultConnection Microsoft.EntityFrameworkCore.SqlServer -o Data -c PersonDbContext --force
````


Der `scaffold`-Befehl erstellt die Modelklassen und die `DbContext`-Klasse aus der vorhandenen Datenbank.

 - -o Data: Sorgt dafür, dass die Models und der DbContext im Ordner Data gespeichert werden.
 - -c PersonDbContext: Der Name der DbContext-Klasse wird auf PersonDbContext festgelegt.
 - --force: Aktualisiert vorhandene Dateien, indem sie neu erstellt werden.


Wenn dieser Befehl ausgeführt wird, werden das auf der Person-Tabelle basierende Person-Model und die PersonDbContext-Klasse automatisch im Ordner Data erstellt. Diese Strukturen werden entsprechend der Tabellenstruktur in der Datenbank erstellt und können für CRUD-Operationen verwendet werden.


#### Weitere Optionen

Update (falls später erforderlich):

```` bash
dotnet tool update dotnet-ef --local --version 6.0.35
````

Zugriff auf die Datenbank über eine feste Verbindungszeichenfolge:

```` bash
dotnet ef dbcontext scaffold "Server=.\\sqlexpress;Database=PersonDb;User Id=sa;Password=www;" Microsoft.EntityFrameworkCore.SqlServer -o Data -c PersonDbContext --force
````

### Projekt Starten

- Abhängigkeiten wiederherstellen:

    ```` bash
    dotnet restore
    ````

- Kompilierungsdateien entfernen

    ```` bash
    dotnet clean
    ````

- Projekt bauen:

    ```` bash
    dotnet build
    ````

- Projekt ausführen:

    ```` bash
    dotnet run
    ````

- URL:

    ````bash
    https://localhost:5001/swagger/index.html
    ````

----


## Repositories / IUnitOfWork.cs


Unit of Work: Definiert Schnittstelle für Transaktionsverwaltung.

Erstelle das Interface der Unit of Work.

```` csharp
using System;
using System.Threading.Tasks;

namespace PersonApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Ermöglicht den Zugriff auf das PersonRepository.
        /// </summary>
        IPersonRepository PersonRepository { get; }

        /// <summary>
        /// Speichert Änderungen.
        /// </summary>
        void Commit();

        /// <summary>
        /// Speichert Änderungen asynchron.
        /// </summary>
        Task CommitAsync();
    }
}

````

## Repositories / UnitOfWork.cs

Erstelle die Implementierung der Unit of Work.

```` csharp
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;

namespace PersonApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PersonDbContext _context;

        private IPersonRepository _personRepository;

        public UnitOfWork(PersonDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ruft das PersonRepository ab oder erstellt es.
        /// </summary>
        public IPersonRepository PersonRepository
        {
            get
            {
                return _personRepository ??= new PersonRepository(_context);
            }
        }

        /// <summary>
        /// Speichert Änderungen synchron.
        /// </summary>
        public void Commit()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Speichert Änderungen asynchron.
        /// </summary>
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
````


## Repositories / IPersonRepository.cs


```` csharp
using PersonApi.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonApi.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();

        Task<Person> GetByIdAsync(int id);

        Task AddAsync(Person person);

        void Update(Person person);

        void Delete(Person person);

        Task<IEnumerable<Person>> SearchAsync(string searchTerm);
    }
}

````

**IPersonRepository**: Definiert die Schnittstelle für den Zugriff auf Personendaten. 
Die Schnittstelle legt die Signaturen für CRUD-Operationen (Create, Read, Update, Delete) 
innerhalb des Repositories fest und bietet eine Methode zum Durchführen von Suchabfragen. 

Sie spezifiziert Methoden für:
- das Abrufen aller Personen (`GetAllAsync`),
- das Finden einer Person nach ID (`GetByIdAsync`),
- das Hinzufügen einer Person (`AddAsync`),
- das Aktualisieren von Personendaten (`Update`),
- das Löschen von Personendaten (`Delete`) sowie
- das Suchen von Personen basierend auf Suchbegriffen (`SearchAsync`).

Diese Struktur dient als Vertragsvorlage für die Implementierung der Zugriffsschicht, 
was die Kapselung und Trennung der Datenlogik von der Geschäftslogik fördert und 
den Code leicht erweiterbar und testbar macht.

## Repositories / PersonRepository.cs

Erstelle die Implementierung für Personendatenzugriff.


```` csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;

namespace PersonApi.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDbContext _context;

        public PersonRepository(PersonDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _context.People.FindAsync(id);
        }

        public async Task AddAsync(Person person)
        {
            await _context.People.AddAsync(person);
        }

        public void Update(Person person)
        {
            _context.People.Update(person);
        }

        public void Delete(Person person)
        {
            _context.People.Remove(person);
        }

        public async Task<IEnumerable<Person>> SearchAsync(string searchTerm)
        {
            return await _context.People
                .Where(p => p.Vorname.Contains(searchTerm) ||
                            p.Nachname.Contains(searchTerm) ||
                            p.Email.Contains(searchTerm))
                .ToListAsync();
        }
    }
}

````

PersonRepository implementieren, der die Zugriffsschicht für Entitäten darstellt. 
Der Code nutzt das Repository-Pattern, um CRUD-Operationen (Create, Read, Update, Delete) zu kapseln 
und die Geschäftslogik von der Datenbankinteraktion zu trennen. 

Mithilfe von Entity Framework Core wird der PersonDbContext verwendet, um asynchrone Operationen wie 
 - das Abrufen aller Personen (GetAllAsync), 
 - das Finden einer Person nach ID (GetByIdAsync), 
 - das Hinzufügen (AddAsync), das Aktualisieren (Update) und 
 - das Löschen (Delete) von Personendaten sowie 
 - die Suche nach Personen basierend auf Suchbegriffen (SearchAsync) 

zu ermöglichen. Diese Struktur sorgt für modularen, testbaren und leicht erweiterbaren Code.

----


## Services / IPersonService.cs

```` csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using PersonApi.Data;

namespace PersonApi.Services
{

    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPeopleAsync();
        Task<IEnumerable<Person>> SearchPeopleAsync(string searchTerm); // Neue Methode für die Suche


        Task<Person> GetPersonByIdAsync(int id);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(int id);
        
    }

}
````

**IPersonService**: Diese Schnittstelle definiert den Servicevertrag 
für die Verwaltung von `Person`-Entitäten innerhalb der Anwendung. 

Sie bietet asynchrone Methoden an für:
- das Abrufen aller Personen (`GetPeopleAsync`),
- die Suche nach Personen basierend auf einem Suchbegriff (`SearchPeopleAsync`), 
- das Finden einer Person nach ihrer ID (`GetPersonByIdAsync`),
- das Hinzufügen einer neuen Person (`AddPersonAsync`),
- das Aktualisieren bestehender Personendaten (`UpdatePersonAsync`), und
- das Löschen einer Person anhand ihrer ID (`DeletePersonAsync`).

Die Definition von `IPersonService` fördert eine klare Trennung 
zwischen der Geschäftslogik und der Datenzugriffsschicht, was 
die Entwicklung von modularen und wartbaren Code erleichtert.


## Services / PersonService.cs

```` csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonApi.Data;
using PersonApi.Repositories;

namespace PersonApi.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gibt alle Personen zurück.
        /// </summary>
        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            return await _unitOfWork.PersonRepository.GetAllAsync();
        }

        /// <summary>
        /// Sucht Personen basierend auf Kriterien.
        /// </summary>
        public async Task<IEnumerable<Person>> SearchPeopleAsync(string searchTerm)
        {
            var allPeople = await _unitOfWork.PersonRepository.GetAllAsync();

            return allPeople.Where(p =>
                (p.Vorname != null && p.Vorname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (p.Nachname != null && p.Nachname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (p.Email != null && p.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Gibt eine Person anhand der ID zurück.
        /// </summary>
        public async Task<Person> GetPersonByIdAsync(int id)
        {
            return await _unitOfWork.PersonRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Fügt eine neue Person hinzu.
        /// </summary>
        public async Task AddPersonAsync(Person person)
        {
            await _unitOfWork.PersonRepository.AddAsync(person);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Aktualisiert eine bestehende Person.
        /// </summary>
        public async Task UpdatePersonAsync(Person person)
        {
            _unitOfWork.PersonRepository.Update(person);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Löscht eine Person anhand der ID.
        /// </summary>
        public async Task DeletePersonAsync(int id)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
            if (person != null)
            {
                _unitOfWork.PersonRepository.Delete(person);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}

````

**PersonService**: Diese Klasse implementiert die Geschäftslogik 
für die Verwaltung von `Person`-Entitäten und erfüllt die Anforderungen 
der `IPersonService`-Schnittstelle. Sie nutzt das `UnitOfWork`-Pattern, 
um Datenbankoperationen effizient zu koordinieren und Transaktionen sicherzustellen. 

Der `PersonService` bietet Funktionen an für:
- das Abrufen aller Personen (`GetPeopleAsync`),
- die Suche nach Personen anhand eines Suchbegriffs (`SearchPeopleAsync`),
- das Finden einer Person nach ID (`GetPersonByIdAsync`),
- das Hinzufügen einer neuen Person (`AddPersonAsync`),
- die Aktualisierung bestehender Personendaten (`UpdatePersonAsync`), und
- das Löschen einer Person nach ID (`DeletePersonAsync`).

Durch die zentralisierte Verwaltung der Geschäftslogik wird 
eine klare Trennung von der Datenzugriffsschicht gewährleistet, 
was zu einem modularen und wartbaren Code führt.


----

## Controllers / PersonController.cs

```` csharp

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Repositories;

namespace PersonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gibt alle Personen zurück.
        /// </summary>
        /// <returns>Liste von Personen</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            var people = await _unitOfWork.PersonRepository.GetAllAsync();
            return Ok(people);
        }

        /// <summary>
        /// Gibt eine bestimmte Person anhand ihrer ID zurück.
        /// </summary>
        /// <param name="id">ID der Person</param>
        /// <returns>Person oder NotFound</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        /// <summary>
        /// Sucht Personen anhand eines Suchbegriffs.
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Person>>> SearchPeople([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Suchbegriff darf nicht leer sein.");
            }

            var people = await _unitOfWork.PersonRepository.SearchAsync(searchTerm);
            return Ok(people);
        }

        /// <summary>
        /// Erstellt eine neue Person.
        /// </summary>
        /// <param name="person">Zu erstellende Person</param>
        /// <returns>Erstellte Person</returns>
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            await _unitOfWork.PersonRepository.AddAsync(person);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = person.PersonId }, person);
        }

        /// <summary>
        /// Aktualisiert eine bestehende Person.
        /// </summary>
        /// <param name="id">ID der zu aktualisierenden Person</param>
        /// <param name="person">Aktualisierte Personendaten</param>
        /// <returns>NoContent oder BadRequest/NotFound</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            _unitOfWork.PersonRepository.Update(person);

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _unitOfWork.PersonRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Löscht eine Person anhand ihrer ID.
        /// </summary>
        /// <param name="id">ID der zu löschenden Person</param>
        /// <returns>NoContent oder NotFound</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _unitOfWork.PersonRepository.Delete(person);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}

````


**PersonController**: Diese Controllerklasse definiert die Endpunkte 
für die Verwaltung von `Person`-Entitäten innerhalb der API. 

Sie verwenden das `IUnitOfWork`-Pattern, um die Datenaufgaben zu handhaben, und 
implementieren eine RESTful API mit den folgenden Endpunkten:

- `GET /api/person`: Ruft alle Personen ab und gibt eine Liste zurück (`GetPeople`).
- `GET /api/person/{id}`: Ruft eine bestimmte Person anhand ihrer ID ab (`GetPerson`).
- `GET /api/person/search?searchTerm=`: Sucht nach Personen basierend auf einem Suchbegriff und gibt die Ergebnisse zurück (`SearchPeople`).
- `POST /api/person`: Erstellt eine neue Person und fügt sie der Datenbank hinzu (`PostPerson`).
- `PUT /api/person/{id}`: Aktualisiert die Informationen einer bestehenden Person (`PutPerson`).
- `DELETE /api/person/{id}`: Löscht eine Person aus der Datenbank anhand ihrer ID (`DeletePerson`).

Diese API-Controllerstruktur ermöglicht eine flexible und vollständige Datenverwaltung, 
indem sie die wesentlichen CRUD-Operationen bereitstellt, 
die von modernen Webanwendungen benötigt werden.

---- 

## Anwendung dieser Klassen

Um die oben genannten Klassen in Ihrer ASP.NET Core-Anwendung zu verwenden, 
müssen Sie den `PersonDbContext`, das generische Repository und 
die Unit of Work in Ihrer `Program.cs` (oder `Startup.cs`, 
je nach ASP.NET-Version) registrieren. 

Diese Registrierung ermöglicht die Nutzung von Dependency Injection, 
um die Komponenten modular und organisiert zu verwalten.

````csharp
builder.Services.AddDbContext<PersonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonRepository, PersonRepository>(); // PersonRepository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // UnitOfWork
builder.Services.AddScoped<IPersonService, PersonService>(); // PersonService
```` 

Durch diese Implementierungen erhält Ihr Projekt eine modulare Struktur und 
ermöglicht eine organisierte Handhabung von Datenbankoperationen.


---- 


## Program.cs


```` csharp
using Microsoft.EntityFrameworkCore;  // Sicherstellen, dass dies eingefügt ist.
using PersonApi.Data;  // Sicherstellen, dass dies eingefügt ist.
using PersonApi.Repositories;
using PersonApi.Services;  // Sicherstellen, dass dies eingefügt ist.

var builder = WebApplication.CreateBuilder(args);

// EINTRAGEN - CORS-Konfiguration hinzufügen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
                // erlaubte Domains:
                .AllowAnyOrigin() // Erlaubt alle Quellen (Domains). // Dies bedeutet, dass auf die API von jeder Domain aus zugegriffen werden kann.
                //.WithOrigins("https://www.ornek1.com", "https://www.ornek2.com") // Erlaubt nur die angegebenen Domains

                // erlaubte Methoden:
                .AllowAnyMethod() // Erlaubt alle HTTP-Methoden (z. B. GET, POST, PUT, DELETE). // Dies bedeutet, dass alle HTTP-Anfragen an die API akzeptiert werden.
                //.WithMethods("GET", "POST") // Erlaubt nur die Methoden GET und POST

                // erlaubte Header:
                .AllowAnyHeader() // Erlaubt alle HTTP-Anfrage-Header. // Diese Einstellung ermöglicht es, jeder Anfrage an die API beliebige Header hinzuzufügen.
                //.WithHeaders("Content-Type", "Authorization"); // Erlaubt nur die angegebenen Header
                ;

    });
});


// EINTRAGEN: Die Verbindungszeichenfolge aus appsettings.json hinzufügen und DbContext einfügen
builder.Services.AddDbContext<PersonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// EINTRAGEN: Add services to the container.

// Repositories und Services registrieren
builder.Services.AddScoped<IPersonRepository, PersonRepository>(); // PersonRepository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // UnitOfWork
builder.Services.AddScoped<IPersonService, PersonService>(); // PersonService


// Beispiel für die Service-Registrierung
//builder.Services.AddScoped<IServiceGeneric<Person, PersonDto>, ServiceGeneric<Person, PersonDto>>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// EINTRAGEN: Verwenden der CORS-Richtlinie - CORS aktivieren
app.UseCors("AllowAll"); // Erlaubt alle Domains, Methoden und Header
//app.UseCors("AllowAllOrigins"); ??
//app.UseCors("AllowSpecificMethods"); // Wenn GET, POST, ... angegeben sind.
//app.UseCors("AllowSpecificOrigins"); // Erlaubt nur bestimmte Domains
//app.UseCors("AllowSpecificMethodsAndHeaders"); // Erlaubt bestimmte Domains, bestimmte Header und bestimmte HTTP-Methoden



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// EINTRAGEN - Haupt-URL auf die Swagger-Oberfläche umleiten
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

````

In der `Program.cs`-Datei wird der Eintrittspunkt der Anwendung definiert und 
die grundlegende Konfiguration vorgenommen. Hier sind die wichtigsten Abschnitte, die erklärt werden:

- **Namespaces einbinden**: Zu Beginn werden essentielle Namespaces eingebunden, darunter `Microsoft.EntityFrameworkCore` für die Datenbankinteraktion sowie die Namespaces aus Ihrem Projekt `PersonApi.Data`, `PersonApi.Repositories`, und `PersonApi.Services`.

- **CORS-Konfiguration**: 
  - `AddCors`: Eine CORS-Policy namens "AllowAllOrigins" wird hinzugefügt, die es allen Ursprüngen, Methoden und Headern erlaubt, Anfragen zu senden. 
  - Dies ist nützlich für die Entwicklung, sollte aber in Produktionsumgebungen eingeschränkt werden.

- **Dienstregistrierungen**:
  - `AddDbContext`: Registriert den `PersonDbContext` für die Verwendung mit Dependency Injection, unter Nutzung der Verbindungskette "DefaultConnection".
  - `AddScoped`: Registriert die Repositories und Services wie `PersonRepository`, `UnitOfWork` und `PersonService`, um diese durch den DI-Container bereitzustellen.

- **Middleware-Konfiguration**:
  - `UseCors`: Aktiviert die definierte CORS-Policy.
  - `UseSwagger` und `UseSwaggerUI`: Diese dienen zur Aktivierung von Swagger während der Entwicklungsphase, um die API-Dokumentation und -Erkundung zu unterstützen.
  - `UseHttpsRedirection` und `UseAuthorization`: Konfigurieren die HTTPS-Umleitung und die Autorisierung für die Anwendung.

Diese Einstellungen gewährleisten, dass Ihre Anwendung die benötigten Dienste bereitstellt, 
Datenbankoperationen verwaltet und HTTP-Anfragen korrekt verarbeitet.


----

## Projekt Starten und Testen

```bash
dotnet run
```

Testen Sie die API, indem Sie zu `http://localhost:5001/swagger/index.html` navigieren. 
Wenn Sie jetzt `http://localhost:5001/` aufrufen, werden Sie automatisch auf die Swagger-Seite weitergeleitet.

----


### Umleiten der Haupt-URL zur Swagger-Oberfläche

Der folgende Middleware-Eintrag in Ihrer Anwendung sorgt dafür, dass alle Anfragen, die an die Root-URL (`http://localhost:5001/`) gesendet werden, automatisch auf die Swagger-Benutzeroberfläche umgeleitet werden:

```` csharp
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});
```` 

Erklärung:

 - Middleware-Einrichtung: Die definierte Middleware fängt jede Anfrage ab, bevor sie an die nachfolgenden Middleware-Komponenten weitergeleitet wird.
 - URL-Prüfung: Wenn der Anforderungspfad / ist, bedeutet das, dass der Benutzer die Hauptseite aufruft.
 - Umleitung: In diesem Fall wird die Anfrage auf die Swagger-Benutzeroberfläche (/swagger/index.html) umgeleitet, wodurch der Benutzer das API-Dokumentations- und Test-UI von Swagger sehen kann.
 - Weiterleitung zu nächster Middleware: Wenn die Bedingung nicht erfüllt ist, wird die Anfrage an die nächste Middleware in der Pipeline übergeben.

Testen: Durch diese Einstellung können Sie die API testen, indem Sie auf http://localhost:5001/swagger/index.html zugreifen. Wenn Sie einfach http://localhost:5001/ eingeben, werden Sie automatisch zur Swagger-Seite weitergeleitet, was den Zugang zu Ihrer API-Dokumentation erleichtert.



## 7. CORS-Konfiguration durch Bearbeiten der Datei `Program.cs`


CORS (Cross-Origin Resource Sharing) ist ein Sicherheitsfeature moderner Webbrowser, 
das verhindert, dass Webanwendungen HTTP-Anfragen an eine andere Domain als die senden, 
von der die Anwendung geladen wurde, sofern diese andere Domain dies nicht ausdrücklich erlaubt.

```csharp
var builder = WebApplication.CreateBuilder(args);

// EINTRAGEN - CORS-Konfiguration hinzufügen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
                // erlaubte Domains:
                .AllowAnyOrigin() // Erlaubt alle Quellen (Domains). // Dies bedeutet, dass auf die API von jeder Domain aus zugegriffen werden kann.
                //.WithOrigins("https://www.ornek1.com", "https://www.ornek2.com") // Erlaubt nur die angegebenen Domains

                // erlaubte Methoden:
                .AllowAnyMethod() // Erlaubt alle HTTP-Methoden (z. B. GET, POST, PUT, DELETE). // Dies bedeutet, dass alle HTTP-Anfragen an die API akzeptiert werden.
                //.WithMethods("GET", "POST") // Erlaubt nur die Methoden GET und POST

                // erlaubte Header:
                .AllowAnyHeader() // Erlaubt alle HTTP-Anfrage-Header. // Diese Einstellung ermöglicht es, jeder Anfrage an die API beliebige Header hinzuzufügen.
                //.WithHeaders("Content-Type", "Authorization"); // Erlaubt nur die angegebenen Header
                ;

    });
});

...

var app = builder.Build();

// EINTRAGEN - CORS aktivieren
app.UseCors("AllowAll"); // Erlaubt alle Domains, Methoden und Header
//app.UseCors("AllowSpecificMethods"); // Wenn GET, POST, ... angegeben sind.
//app.UseCors("AllowSpecificOrigins"); // Erlaubt nur bestimmte Domains
//app.UseCors("AllowSpecificMethodsAndHeaders"); // Erlaubt bestimmte Domains, bestimmte Header und bestimmte HTTP-Methoden

...

app.Run();

```


## 8. Projekt kompilieren und testen
Führen Sie das Projekt im Terminal aus:

```bash
dotnet run
```

Testen Sie die API, indem Sie die Adresse `http://localhost:5001/swagger/index.html` mit Swagger aufrufen. 
Wenn Sie jetzt die Adresse `http://localhost:5001/` aufrufen, werden Sie automatisch auf die Swagger-Seite umgeleitet.


### NICHT BENÖTIGTE DATEIEN LÖSCHEN

Controllers / WeatherForecastController.cs wird gelöscht.

WeatherForecast.cs wird gelöscht.

----


