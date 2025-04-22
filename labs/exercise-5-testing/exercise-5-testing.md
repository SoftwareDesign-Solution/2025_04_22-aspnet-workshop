- [5. Testing in ASP.NET Core (MVC oder Razor Pages)](#5-testing-in-aspnet-core-mvc-oder-razor-pages)
  - [5.1 Projekt für Tests vorbereiten](#51-projekt-für-tests-vorbereiten)
  - [5.2 Unit Tests erstellen](#52-unit-tests-erstellen)
  - [5.3 Integrationstests erstellen](#53-integrationstests-erstellen)
  - [5.4 Tests ausführen und Ergebnisse auswerten](#54-tests-ausführen-und-ergebnisse-auswerten)

Bearbeitungszeit: n Minuten

The solution branch for the whole lab is `solution-5-testing`

# 5. Testing in ASP.NET Core (MVC oder Razor Pages)

In dieser Aufgabe lernst du, wie du in ASP.NET Core-Anwendungen automatisierte Tests durchführen kannst. Du kannst frei wählen, ob du diese Aufgabe mit deinem MVC-Projekt "InventoryManagement.Mvc" oder deinem Razor Pages-Projekt "InventoryManagement.RazorPages" umsetzen möchtest.

## 5.1 Projekt für Tests vorbereiten

- Erstelle in deiner Solution ein neues Test-Projekt (z.B. InventoryManagement.Tests) vom Typ "xUnit Test Project".
- Füge deinem Test-Projekt eine Referenz auf dein gewähltes Hauptprojekt hinzu (MVC oder Razor Pages).

## 5.2 Unit Tests erstellen

Erstelle Unit Tests für folgende Komponenten:

- Erstelle Tests für eine Methode aus dem ItemsController (MVC) oder aus dem PageModel der Razor Page Items/Overview.
- Überprüfe, ob diese Methode korrekt Daten zurückliefert.

Beispiel eines Unit Tests:

```cs
[Fact]
public void Overview_Returns_ViewResult_With_ListOfItems()
{
    // Arrange
    var context = GetTestDbContext();
    var controller = new ItemsController(context);

    // Act
    var result = controller.Overview();

    // Assert
    var viewResult = Assert.IsType<ViewResult>(result);
    var model = Assert.IsAssignableFrom<IEnumerable<Item>>(viewResult.ViewData.Model);
    Assert.NotEmpty(model);
}

private ApplicationDbContext GetTestDbContext()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    var context = new ApplicationDbContext(options);
    context.Items.Add(new Item { Id = 1, Name = "Test Item", Description = "Test Description", Price = 10.0m, Stock = 5 });
    context.SaveChanges();

    return context;
}
```

## 5.3 Integrationstests erstellen

- Erstelle Integrationstests, um das Zusammenspiel von mehreren Komponenten (z.B. Controller/Pages, Datenbank) zu prüfen.
- Nutze den `WebApplicationFactory` zur Erstellung von Integrationstests.

Beispiel eines Integrationstests:

```cs
[Fact]
public async Task Get_ItemsOverview_ReturnsSuccessAndCorrectContentType()
{
    var factory = new WebApplicationFactory<Program>();
    var client = factory.CreateClient();

    var response = await client.GetAsync("/Items/Overview");

    response.EnsureSuccessStatusCode();
    Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
}
```

## 5.4 Tests ausführen und Ergebnisse auswerten

- Führe die Unit- und Integrationstests durch.
- Analysiere die Testergebnisse und stelle sicher, dass deine Anwendung erwartungsgemäß funktioniert.

Führe die Unit- und Integrationstests durch.

Analysiere die Testergebnisse und stelle sicher, dass deine Anwendung erwartungsgemäß funktioniert.

**Ziel:** Nach Abschluss dieser Aufgabe verstehst du:

- Unterschied zwischen Unit Tests und Integrationstests
- Erstellung und Ausführung von Tests mit xUnit in ASP.NET Core
- Nutzung von WebApplicationFactory zur Durchführung von Integrationstests
