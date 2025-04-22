# 2025_04_22-aspnet-workshop

## ✨ Beschreibung

Dieses Repository enthält das Schulungsprojekt zur Schulung **„ASP.NET Einführung“**. Ziel ist es, die grundlegenden und fortgeschrittenen Konzepte von ASP.NET Core kennenzulernen, inklusive MVC, Razor Pages, Entity Framework Core, ASP.NET Core Identity und Testing mit xUnit.

Das Projekt besteht aus mehreren Teilprojekten und beinhaltet praktische Schulungsaufgaben im Verzeichnis `labs`.

---

## 🧱 Projektstruktur

- **InventoryManagement.Mvc**: Webanwendung mit ASP.NET Core MVC (Model-View-Controller)
- **InventoryManagement.RazorPages**: Webanwendung mit ASP.NET Core Razor Pages
- **InventoryManagement.WebAPI**: RESTful Web API mit ASP.NET Core
- **InventoryManagement.Tests**: Testprojekt mit xUnit für automatisierte Tests

---

## 🚀 Technologien

- **ASP.NET Core**: Framework zur Entwicklung moderner Webanwendungen
- **Entity Framework Core**: Objekt-Relationales Mapping (ORM) zur Datenbankanbindung
- **ASP.NET Core Identity**: Authentifizierung & Autorisierung
- **xUnit**: Testframework für .NET-Anwendungen

---

## 📚 Schulungsaufgaben

Die Schulungsaufgaben sind im Verzeichnis `labs` abgelegt und bauen thematisch aufeinander auf:

- **exercise-1-mvc**: Einstieg in ASP.NET Core MVC
- **exercise-2-mvc-forms**: Formularverarbeitung in MVC
- **exercise-3-mvc-entity-framework-core**: Datenbankzugriffe mit Entity Framework Core
- **exercise-4-razorpages**: Einstieg in Razor Pages
- **exercise-5-testing**: Unit- und Integrationstests mit xUnit
- **exercise-6-localization**: Lokalisierung und Mehrsprachigkeit in ASP.NET Core
- **exercise-7-identity**: Benutzerverwaltung mit ASP.NET Core Identity

### 🔁 Lösungen

Für jede Aufgabe existiert ein eigener Lösungsbranch:

- `solution-1-mvc`
- `solution-2-mvc-forms`
- `solution-3-mvc-entity-framework-core`
- `solution-4-razorpages`
- `solution-5-testing`
- `solution-6-localization`
- `solution-7-identity`

Zum Wechseln in einen Lösungs-Branch:

```bash
   git checkout solution-1-mvc
```

Ersetze `solution-1-mvc` durch den gewünschten Branch.

---

## ⚙️ Voraussetzungen

- [.NET SDK 9.x](https://dotnet.microsoft.com/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) oder [VS Code](https://code.visualstudio.com/) mit .NET-Erweiterungen
- SQL Server LocalDB oder eine andere unterstützte Datenbank

---

## 🚦 Build & Ausführung

Die Projekte können direkt mit dem .NET CLI oder aus Visual Studio heraus gestartet werden:

### Beispiel (Kommandozeile)

```bash
cd InventoryManagement.Mvc
dotnet run
```

### Tests ausführen

```bash
cd InventoryManagement.Tests
dotnet test
```

---

## 🙏 Beitrag

Beiträge zur Schulung oder Verbesserung des Codes sind herzlich willkommen. Gerne per Pull-Request oder Issue einreichen!
