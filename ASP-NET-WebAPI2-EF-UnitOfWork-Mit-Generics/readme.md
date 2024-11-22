
# ASP.NET WebAPI 2 mit EF, Repository, UnitOfWork (Mit Generics)

Dieses Web API Projekt verwendet Entity Framework (EF) in Kombination mit den Designmustern Generic Repository und Unit of Work, um den Datenzugriff modular und verwaltbar zu gestalten. 

----


## Inhaltsverzeichnis

- [ASP.NET WebAPI 2 mit EF, Repository, UnitOfWork (Mit Generics)](#aspnet-webapi-2-mit-ef-repository-unitofwork-mit-generics)
  - [Inhaltsverzeichnis](#inhaltsverzeichnis)
  - [Designmuster](#designmuster)
    - [Generic Repository Muster](#generic-repository-muster)
    - [Repository Muster](#repository-muster)
    - [Unit of Work Muster](#unit-of-work-muster)
    - [Dependency Injection (DI)](#dependency-injection-di)
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
  - [Repositories / IGenericRepository.cs](#repositories--igenericrepositorycs)
  - [Repositories / GenericRepository.cs](#repositories--genericrepositorycs)
  - [Repositories / IPersonRepository.cs](#repositories--ipersonrepositorycs)
  - [Repositories / PersonRepository.cs](#repositories--personrepositorycs)
  - [Services / IService.cs](#services--iservicecs)
  - [Services / Service.cs](#services--servicecs)
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


----


## Designmuster

### Generic Repository Muster
Dieses Muster verwaltet alle CRUD-Operationen (Create, Read, Update, Delete) in einer einzigen generischen Struktur. Die Klasse `GenericRepository<T>` verhindert Wiederholungen im Code und erhöht die Wiederverwendbarkeit. Dieselben Basisoperationen können für unterschiedliche Entitäten angewandt werden. Das Zustandsmanagement der Entitäten wird automatisch von Entity Framework Core (EF Core) erledigt, was eine kontrollierte Änderung und eine verbesserte Datenkonsistenz ermöglicht.

### Repository Muster
Die Klasse `PersonRepository` in diesem Projekt befasst sich speziell mit `Person`-Objekten und enthält nur datenbezogene Zugriffsmethoden, die speziell für diese Objekte sind. Diese Klasse erweitert die `GenericRepository<Person>` Klasse und bietet neben den generischen Methoden auch spezifische Methoden wie `SearchAsync`, die speziell für die `Person`-Klasse entwickelt wurden. Dadurch werden spezielle Operationen, die Personen betreffen, zentralisiert und besser verwaltbar.

### Unit of Work Muster
Die `UnitOfWork` Klasse wird verwendet, um mehrere Datenbankoperationen zu verwalten. Dieses Muster verwaltet alle Operationen als eine einzige Einheit und gewährleistet die Atomarität, was bedeutet, dass im Fehlerfall alle Operationen zurückgesetzt werden, um die Konsistenz der Datenbank zu sichern. Die Methoden `Commit` und `CommitAsync` ermöglichen die Übernahme von Änderungen in die Datenbank. Dieses Muster hält alle Datenbankoperationen unter Kontrolle und macht das System sicherer und fehlertoleranter.

### Dependency Injection (DI)
DI ist ein Designprinzip, das die Entkopplung von Abhängigkeiten mithilfe eines DI-Containers erleichtert. In diesem Projekt wird DI verwendet, um Instanzen von Repositories und Services bereitzustellen. Typischerweise erfolgt dies durch die Registrierung von Schnittstellen wie `IPersonRepository`, `IUnitOfWork` und `IPersonService` in der Projektkonfiguration. Dadurch können Implementierungen automatisch zugewiesen werden, wenn diese Typen in Controllern oder anderen Diensten benötigt werden. DI ermöglicht eine flexible und testbare Architektur, indem es den Austausch konkreter Implementierungen ohne Codeänderungen erlaubt, was besonders bei Unit-Tests von Vorteil ist.

----

## Schichten der Architektur

### Präsentationsschicht (Controllers)
Die Präsentationsschicht beinhaltet den `PersonController`, der für die Verarbeitung von eingehenden HTTP-Anfragen verantwortlich ist. In `PersonController.cs` sind die API-Endpunkte für CRUD-Operationen auf `Person`-Entitäten definiert. Der Controller ruft entsprechende Services auf, um die Geschäftslogik auszuführen.

### Servicelayer (Services)
Dieser Layer setzt sich aus `IPersonService.cs` und `PersonService.cs` zusammen, die die Geschäftslogik zur Verwaltung von `Person`-Daten implementieren. Der Service-Layer nimmt Anfragen vom Controller entgegen und verwendet das Unit of Work-Muster, um auf die Repositories zuzugreifen und Transaktionen zu handhaben.

### Datenzugriffsschicht (Repositories)
In diesem Layer befinden sich `IPersonRepository.cs` und `PersonRepository.cs`. Diese Dateien kapseln den direkten Datenbankzugriff und führen die CRUD-Operationen auf `Person`-Entitäten aus. Zusätzlich verwalten `IUnitOfWork.cs` und `UnitOfWork.cs` das Transaktionsmanagement, indem sie mehrere Repository-Operationen in einer einzigen Einheit bündeln.

### Datenmodell-Schicht (Data)
Diese Schicht wird durch die Datei `Person.cs` repräsentiert, die die Definition der `Person`-Entität mit ihren Eigenschaften enthält. Sie beschreibt die Datenstruktur, die in der Datenbank gespeichert wird.

### Infrastrukturkomponenten
Eine zentrale Infrastrukturkomponente ist `PersonDbContext.cs`. Diese Datei bereitstellt den notwendigen Datenbankkontext, der die Kommunikation zwischen der Anwendung und der Datenbank über Entity Framework Core ermöglicht. 

Diese Schichten sorgen für eine klare Trennung von Verantwortlichkeiten und erleichtern das Testen, die Wartung sowie die Erweiterung der Anwendung.

----

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
│   │   ├── PersonRepository.cs
│   │   └── IUnitOfWork.cs
│   │   └── UnitOfWork.cs
│   ├── Services/
│   │   ├── IPersonService.cs
│   │   └── PersonService.cs
│   │   └── Service.cs
│   ├── Properties/
│   │   ├── appsettings.Development.json
│   │   └── appsettings.json
│   ├── Program.cs
│   ├── PersonApi.csproj
└── PersonApi.sln
```

- **appsettings.json**: Datei mit Konfigurationseinstellungen, einschließlich der Datenbank-Verbindungszeichenfolge und Logging-Optionen.
- **appsettings.Development.json**: Entwicklungsumgebungsspezifische Konfiguration, die die allgemeinen Einstellungen aus `appsettings.json` überschreibt oder ergänzt.
- **Controllers/**: Ordner, der die API-Controller enthält; `PersonController.cs` verwaltet die Anfragen für die 'Person'-Ressource.
- **Data/**: Ordner, der das Personenmodell (`Person.cs`) beherbergt, welches die Datenstruktur für Personen beschreibt.
- **Repositories/**: Ordner für Repository-Interfaces und -Implementierungen, enthält `IPersonRepository.cs`, `PersonRepository.cs`, und `IUnitOfWork.cs`, `UnitOfWork.cs`, die CRUD-Operationen und das Transaktionsmanagement definieren.
- **Services/**: Ordner für die Geschäftslogik, der die Schnittstelle und Implementierung von Services (z.B. `IPersonService.cs`, `PersonService.cs`, `Service.cs`) für Personenoperationen enthält.
- **Program.cs**: Hauptprogrammdatei, die die Anwendungskonfiguration, Dienstregistrierung und die Middleware-Pipeline steuert.
- **PersonApi.csproj**: Projektdatei, die das Ziel-Framework und die Paketabhängigkeiten definiert.
- **PersonApi.sln**: Visual Studio Lösungsdatei, die das Projekt organisiert und verwaltet.



## Abhängigkeiten

``` bash
PersonApi/
├── Data/
│   └── Person.cs: Datenmodell für Personen, genutzt von Repositories, Services und Controllern.
├── Repositories/
│   ├── IPersonRepository.cs: Definiert die Schnittstelle für den Personen-Datenzugriff.
│   ├── PersonRepository.cs: Implementierung der Schnittstelle für Personen-Datenzugriff, nutzt PersonDbContext.
│   ├── IUnitOfWork.cs: Definiert die Schnittstelle für das Transaktionsmanagement.
│   └── UnitOfWork.cs: Implementierung der Transaktionsverwaltung, koordiniert Repositories.
├── Services/
│   ├── IPersonService.cs: Definiert die Schnittstelle für Personen-Services.
│   ├── PersonService.cs: Implementiert die Geschäftslogik-Operationen, abhängig von IPersonRepository und IUnitOfWork.
│   └── Service.cs: Generische Service-Implementierung, von PersonService abgeleitet.
├── Controllers/
│   └── PersonController.cs: Verwaltung von HTTP-Anfragen, abhängig von IPersonService.
├── Program.cs
│   ├── Verwendet: appsettings.json, appsettings.Development.json für Konfigurationseinstellungen.
│   ├── Nutzt: DbContext, Repositories und Services für Dependency Injection und Anwendungskonfiguration.
├── appsettings.json: Allgemeine Konfiguration für die Anwendung, inkl. Verbindungszeichenfolgen und Logging.
├── appsettings.Development.json: Entwicklungspezifische Konfiguration, die allgemeine Einstellungen ergänzt oder überschreibt.
├── PersonApi.csproj: Konfigurationsdatei für Projektabhängigkeiten und das Target Framework.
└── PersonApi.sln: Lösungsdatei, die das Projekt PersonApi.csproj enthält.
```

Diese Abhängigkeitsdarstellung zeigt, wie zentrale Modelle und Schnittstellen die Grundlage für die Implementierung von Repositories, Services und Controllern bilden, und wie die Anwendung diese Strukturen nutzt, um Datenzugriffs- und Geschäftslogikoperationen effektiv zu verwalten.


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

         public IPersonRepository PersonRepository
         {
             get { return _personRepository ??= new PersonRepository(_context); }
         }

         public void Commit()
         {
             _context.SaveChanges();
         }

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


## Repositories / IGenericRepository.cs

```` csharp

 using System.Collections.Generic;
 using System.Threading.Tasks;

 namespace PersonApi.Repositories
 {
     public interface IGenericRepository<T> where T : class
     {
         Task<IEnumerable<T>> GetAllAsync();
         Task<T> GetByIdAsync(int id);
         Task AddAsync(T entity);
         void Update(T entity);
         void Delete(T entity);
     }
 }

````


**IGenericRepository<T>**: Diese generische Schnittstelle definiert die grundlegenden CRUD-Operationen (Create, Read, Update, Delete) für eine Entität vom Typ `T`, wobei `T` eine Klasse sein muss. Sie bietet eine abstrakte Basis für die Implementierung von Repositories, um die Datenzugriffslogik zu standardisieren. Die Schnittstelle beinhaltet:

- `GetAllAsync()`: Eine asynchrone Methode, die alle Entitäten vom Typ `T` aus der Datenquelle abruft und eine Liste zurückgibt.
- `GetByIdAsync(int id)`: Eine asynchrone Methode, die eine bestimmte Entität anhand ihrer ID abruft.
- `AddAsync(T entity)`: Fügt eine neue Entität hinzu und nimmt diese in die Datenquelle auf.
- `Update(T entity)`: Aktualisiert eine existierende Entität in der Datenquelle.
- `Delete(T entity)`: Entfernt eine Entität aus der Datenquelle.

Diese Schnittstelle dient als grundlegender Vertrag für das Management von Entitäten, das die Interaktionen mit der Datenebene vereinfacht und konsistent macht. Sie fördert die Wiederverwendbarkeit und sorgt für die Entkopplung der Datenzugriffsschicht von der Geschäftslogik.


## Repositories / GenericRepository.cs


```` csharp

 using System.Collections.Generic;
 using System.Threading.Tasks;
 using Microsoft.EntityFrameworkCore;

 namespace PersonApi.Repositories
 {
     public class GenericRepository<T> : IGenericRepository<T> where T : class
     {
         private readonly DbContext _context;

         public GenericRepository(DbContext context)
         {
             _context = context;
         }

         public async Task<IEnumerable<T>> GetAllAsync()
         {
             return await _context.Set<T>().ToListAsync();
         }

         public async Task<T> GetByIdAsync(int id)
         {
             return await _context.Set<T>().FindAsync(id);
         }

         public async Task AddAsync(T entity)
         {
             await _context.Set<T>().AddAsync(entity);
         }

         public void Update(T entity)
         {
             _context.Set<T>().Update(entity);
         }

         public void Delete(T entity)
         {
             _context.Set<T>().Remove(entity);
         }
     }
 }

````

**GenericRepository<T>**: Diese Klasse implementiert das `IGenericRepository<T>`-Interface und bietet eine generische Umsetzung der grundlegenden CRUD-Operationen für jede Entität vom Typ `T`, wobei `T` jegliche Klasse darstellen kann. Die Klasse arbeitet mit Entity Framework Core und nutzt einen `DbContext`, um auf die Datenbank zuzugreifen und die Datenverwaltung zu realisieren. 

- **Konstruktor**: Nimmt einen `DbContext` als Parameter, der zur Durchführung von Datenbankoperationen verwendet wird.
- **`GetAllAsync()`**: Ruft alle Entitäten vom Typ `T` ab und gibt sie als Liste zurück.
- **`GetByIdAsync(int id)`**: Findet eine Entität anhand ihrer ID und gibt sie zurück.
- **`AddAsync(T entity)`**: Fügt eine neue Entität zur Datenbank hinzu.
- **`Update(T entity)`**: Aktualisiert eine bestehende Entität in der Datenbank.
- **`Delete(T entity)`**: Entfernt eine Entität aus der Datenbank.

Diese Klasse kapselt die typischen Datenbankoperationen und stellt sie generisch zur Verfügung, wodurch sie mehrfach verwendet werden kann. Sie vereinfacht die Verwaltung von Entitäten und sorgt für eine standardisierte Art des Datenzugriffs innerhalb der Anwendung.


## Repositories / IPersonRepository.cs


```` csharp

using PersonApi.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonApi.Repositories
{
    public interface IPersonRepository : IGenericRepository<Person>
    {

        // Person'a özel metodlar burada tanımlanabilir
        Task<IEnumerable<Person>> SearchAsync(string searchTerm);
    }
}

````

**IPersonRepository**: Diese Schnittstelle erweitert das `IGenericRepository<Person>` und bietet eine zusätzliche Methode speziell für `Person`-Entitäten. Sie enthält die Methode `SearchAsync`, die das Durchführen von Suchabfragen ermöglicht. Diese Schnittstelle dient als Grundlage für die Implementierung der spezifischen Datenzugriffslogik für `Person`-Objekte und unterstützt die Trennung von Geschäfts- und Datenlogik. Die Implementierung dieser Schnittstelle erlaubt es, Personendaten effizient zu durchsuchen und zu verwalten, was die Erweiterbarkeit und Testbarkeit des Codes erleichtert.


## Repositories / PersonRepository.cs

Erstelle die Implementierung für Personendatenzugriff.


```` csharp

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;

namespace PersonApi.Repositories
{
    
    public class PersonRepository : GenericRepository<Person>,  IPersonRepository
    {

        private readonly PersonDbContext _personContext;

         public PersonRepository(PersonDbContext context) : base(context)
         {
             _personContext = context;
         }

        public async Task<IEnumerable<Person>> SearchAsync(string searchTerm)
         {
             return await _personContext.People
                 .Where(p => p.Vorname.Contains(searchTerm) ||
                             p.Nachname.Contains(searchTerm) ||
                             p.Email.Contains(searchTerm))
                 .ToListAsync();
         }

    }
}

````

**PersonRepository**: Diese Klasse implementiert das `IPersonRepository`-Interface und erweitert das `GenericRepository<Person>`, um den spezifischen Zugriff auf Personendaten zu ermöglichen. Sie arbeitet mit dem `PersonDbContext`, um die Datenbankinteraktion zu handhaben. 

Die Klasse liefert eine konkrete Implementierung für die Methode `SearchAsync`, die es ermöglicht, Personen anhand von Suchbegriffen zu finden. 

- Die Methode `SearchAsync` durchsucht die `People`-Datenbanktabelle nach Personen, deren Vorname, Nachname oder E-Mail den Suchbegriff enthalten.
- Der Konstruktor der Klasse nimmt einen `PersonDbContext` als Parameter, der für den Zugriff auf die Datenbank verwendet wird.

Diese Implementation bietet einen strukturierten und testbaren Ansatz zur Datenmanipulation und -abruf, indem sie die Logik des Datenzugriffs kapselt und den Rest der Anwendung von den Details des Datenbankzugriffs entkoppelt.


----

## Services / IService.cs

```` csharp

using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonApi.Services
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}

````


**IService<T>**: Diese generische Schnittstelle definiert die grundlegenden Service-Operationen für eine Entität vom Typ `T`, wobei `T` eine Klasse sein muss. Sie stellt eine abstrakte Ebene bereit, um die Geschäftslogik von der Datenzugriffsebene zu entkoppeln. Die Schnittstelle umfasst die folgenden Methoden:

- **`GetAllAsync()`**: Eine asynchrone Methode, die alle Entitäten vom Typ `T` abruft und als Liste zurückgibt.
- **`GetByIdAsync(int id)`**: Eine asynchrone Methode, die eine spezifische Entität anhand ihrer ID abfragt.
- **`AddAsync(T entity)`**: Fügt eine neue Entität hinzu und integriert sie in den Datenbestand.
- **`UpdateAsync(T entity)`**: Aktualisiert die Daten einer existierenden Entität.
- **`DeleteAsync(int id)`**: Entfernt eine Entität, identifiziert durch ihre ID, aus dem System.

Diese Schnittstelle dient als Vertragsvorlage für die Implementierung von Services, die auf bestimmte Datenoperationen abzielen, und unterstützt die saubere Trennung der Geschäftslogik von der Datenzugriffsschicht. Sie trägt zur Konsistenz der Applikationsarchitektur bei und erleichtert das Testing und die Wartung.


## Services / Service.cs

```` csharp

using System.Collections.Generic;
using System.Threading.Tasks;
using PersonApi.Repositories;

namespace PersonApi.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<T> _repository;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.PersonRepository as IGenericRepository<T>;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _repository.Delete(entity);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}

````


**Service<T>**: Diese generische Klasse implementiert das `IService<T>`-Interface und bietet konkrete Implementierungen für die grundlegenden Service-Operationen bei Entitäten vom Typ `T`, wobei `T` eine Klasse sein muss. Die Klasse kapselt die Geschäftslogik, die auf dem `IUnitOfWork`-Muster basiert, um Datenoperationen koordiniert und transaktionssicher auszuführen. 

- **Konstruktor**: Der Konstruktor nimmt ein `IUnitOfWork` als Parameter, um die Repository-Operationen auf konsistente Weise zu handhaben.
- **`GetAllAsync()`**: Ruft alle Entitäten vom Typ `T` ab und gibt sie als Liste zurück.
- **`GetByIdAsync(int id)`**: Holt eine bestimmte Entität anhand ihrer ID und liefert sie zurück.
- **`AddAsync(T entity)`**: Fügt eine neue Entität zum Repository hinzu und speichert die Änderungen in der Datenbank.
- **`UpdateAsync(T entity)`**: Aktualisiert eine bereits existierende Entität und speichert die Änderungen.
- **`DeleteAsync(int id)`**: Findet und entfernt eine Entität anhand ihrer ID und speichert die Änderungen, sofern die Entität existiert.

Diese Klasse dient als Grundgerüst für die Geschäftslogik der Anwendung, indem sie die gemeinsame Funktionalität für Entitäten kapselt und die Verwaltung von Datenkonsistenz und Transaktionen erleichtert.



## Services / IPersonService.cs

```` csharp

using System.Collections.Generic;
using System.Threading.Tasks;
using PersonApi.Data;

namespace PersonApi.Services
{

    public interface IPersonService : IService<Person>
    {
         Task<IEnumerable<Person>> SearchPeopleAsync(string searchTerm);
         
        // Burada Person'a özgü başka metotlar da tanımlayabilirsiniz.
        
    }

}

````


**IPersonService**: Diese Schnittstelle erweitert das generische `IService<Person>`-Interface und fügt spezialisierte Methoden hinzu, die spezifisch für `Person`-Entitäten sind. Sie ist dafür konzipiert, die Geschäftslogik für Personen zu kapseln und deren Verwaltung zu erleichtern. 

- **`SearchPeopleAsync(string searchTerm)`**: Eine asynchrone Methode, die es ermöglicht, Personen basierend auf einem Suchbegriff zu suchen und eine Liste von passenden `Person`-Entitäten zurückzugeben.

Die Schnittstelle stellt sicher, dass alle implementierenden Services die wesentlichen CRUD-Operationen für Personen sowie zusätzliche spezialisierte Methoden wie das Suchen von Personen kapseln. Dies fördert die Konsistenz und Wiederverwendbarkeit in der Anwendung und erleichtert die Erweiterung bei zukünftigen Anforderungen.



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

    public class PersonService : Service<Person>, IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IUnitOfWork unitOfWork, IPersonRepository personRepository) 
            : base(unitOfWork)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<Person>> SearchPeopleAsync(string searchTerm)
        {
            return await _personRepository.SearchAsync(searchTerm);
        }
    }

}

````


**PersonService**: Diese Klasse implementiert das `IPersonService`-Interface und erweitert die generische `Service<Person>`-Klasse, um spezifische Geschäftslogik für `Person`-Entitäten bereitzustellen. Sie verbindet die allgemeinen Service-Operationen mit den speziellen Anforderungen für Personen und nutzt das Repository-Muster zur Datenverwaltung.

- **Konstruktor**: Der Konstruktor akzeptiert ein `IUnitOfWork` und ein `IPersonRepository`, um die Repository- und Unit-Of-Work-Operationen zu koordinieren.
- **`SearchPeopleAsync(string searchTerm)`**: Implementiert die Suche von Personen basierend auf einem Suchbegriff und gibt eine Liste von `Person`-Entitäten zurück, die dem Kriterium entsprechen.

Diese Klasse kapselt die spezifische Geschäftslogik für das Verwalten von Personen, bietet eine saubere Trennung der Geschäftslogik von der Datenzugriffslogik und erleichtert die Anwendung von Änderungen oder Erweiterungen innerhalb des Services.


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

Um die oben genannten Klassen in Ihrer ASP.NET Core-Anwendung zu verwenden, müssen Sie das `IPersonRepository`, die `UnitOfWork`, den generischen Service und den speziellen `PersonService` in Ihrer `Program.cs` oder `Startup.cs` (je nach ASP.NET-Version) registrieren. Diese Registrierung ermöglicht die Nutzung von Dependency Injection, um die Komponenten effizient und strukturiert zu verwalten.

```` csharp
// Add repositories
builder.Services.AddScoped<IPersonRepository, PersonRepository>(); // Registriert das PersonRepository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Registriert die UnitOfWork

// Add services
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>)); // Registriert den generischen Service
builder.Services.AddScoped<IPersonService, PersonService>(); // Registriert den speziellen PersonService
````

Durch diese Implementierungen erhält Ihr Projekt eine modulare und erweiterbare Struktur, die eine organisierte Handhabung von Geschäftslogik- und Datenbankoperationen ermöglicht. Dies verbessert die Wartbarkeit und Testbarkeit der Anwendung erheblich.


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
  - `AddDbContext`: Registriert den `PersonDbContext` für die Verwendung mit Dependency Injection, indem die Verbindungskette "DefaultConnection" aus der Konfigurationsdatei `appsettings.json` genutzt wird. Dies ermöglicht den Zugriff auf die Datenbank innerhalb der Anwendung.
  - `AddScoped`: Registriert die Repositories und Services, darunter `IPersonRepository` mit `PersonRepository`, `IUnitOfWork` mit `UnitOfWork`, sowie `IPersonService` mit `PersonService`. Diese Registrierungen sorgen dafür, dass die entsprechenden Instanzen durch den DI-Container bereitgestellt werden und in den Controllern und anderen Diensten der Anwendung verwendet werden können.
  - `AddCors`: Konfiguriert CORS-Richtlinien, um Cross-Origin-Anfragen zu ermöglichen, was den Zugriff von verschiedenen Domains auf die API erlaubt. Die Richtlinie "AllowAll" ist definiert, die alle Methoden, Header und Ursprünge zulässt.
  - `AddControllers`: Fügt den Controller-Support zur Anwendung hinzu, um die Web API-Endpunkte zu verwenden.
  - `AddSwaggerGen`: Integriert Swagger für die API-Dokumentation, um automatisch generierte Beschreibungen der API-Endpunkte bereitzustellen und zu testen.

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


