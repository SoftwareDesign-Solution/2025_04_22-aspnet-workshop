- [3. Entity Framework Core in ASP.NET Core MVC](#3-entity-framework-core-in-aspnet-core-mvc)
  - [3.1 Installation Entity Framework Core](#31-installation-entity-framework-core)
  - [3.2 Datenbank-Zugangsdaten in appsettings.json](#32-datenbank-zugangsdaten-in-appsettingsjson)
  - [3.3 ApplicationDbContext erstellen](#33-applicationdbcontext-erstellen)
  - [3.4 Registrierung in Program.cs](#34-registrierung-in-programcs)
  - [3.5 Controller erweitern](#35-controller-erweitern)

Bearbeitungszeit: n Minuten

The solution branch for the whole lab is `solution-3-mvc-entity-framework-core`

# 3. Entity Framework Core in ASP.NET Core MVC

Um eine effiziente und strukturierte Datenhaltung zu ermöglichen, werden in dieser Aufgabe alle bisher verwendeten Daten mit Entity Framework Core in einer SQL-Datenbank persistiert.

## 3.1 Installation Entity Framework Core

Installiere Entity Framework Core für die Nutzung mit einer SQL-Datenbank. Nutze entweder die .NET CLI oder den NuGet Package Manager.

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

## 3.2 Datenbank-Zugangsdaten in appsettings.json

Füge die Zugangsdaten für deine Datenbankverbindung in der Datei `appsettings.json` hinzu.

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/appsettings.json**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventoryManagementDb;Trusted_Connection=True;"
}
```

</p>
</details>

## 3.3 ApplicationDbContext erstellen

Erstelle eine Klasse `ApplicationDbContext` im Ordner `Data`, die von `DbContext` erbt, füge die Models hinzu und hinterlege in `OnModelCreating` die zugehörigen Relationen:

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Data/ApplicationDbContext.cs**

```cs
using InventoryManagement.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Mvc.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany()
                .HasForeignKey(oi => oi.ItemId);

        }
        
    }
}

```

</p>
</details>

## 3.4 Erstellung der Datenbank und Migrationen

Führe folgende CLI-Befehle aus, um Migrationen zu erstellen und die Datenbank im SQL Server zu erzeugen:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 3.4 Registrierung in Program.cs

Registrieren Sie die erstellte Klasse `ApplicationDbContext` in der Program.cs

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Program.cs**

```cs
using InventoryManagement.Mvc.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
```

</p>
</details>

## 3.5 Controller erweitern

Erweitere die Controller (ItemsController und OrdersController), um den ApplicationDbContext für die Datenzugriffe zu verwenden.

Beispielkonstruktor:

```cs
private readonly ApplicationDbContext _context;

public ItemsController(ApplicationDbContext context)
{
    _context = context;
}
```

Beispiel-Methoden:

```cs
public IActionResult Overview()
{
    var items = _context.Items.ToList();
    return View(items);
}

[HttpPost]
public IActionResult CreateEditItem(Item item)
{
    if(ModelState.IsValid)
    {
        if(item.Id == 0)
            _context.Items.Add(item);
        else
            _context.Items.Update(item);

        _context.SaveChanges();
        return RedirectToAction("Overview");
    }
    return View(item);
}
```

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Controllers/ItemsController.cs**

```cs
using InventoryManagementWorkshop.Mvc.Data;
using InventoryManagementWorkshop.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementWorkshop.Mvc.Controllers
{
    public class ItemsController : Controller
    {

        private readonly ApplicationDbContext _context;
        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Overview()
        {
            var items = _context.Items.ToList();
            return View(items);
        }

        public IActionResult CreateEdit(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            return View(item);
        }

        public IActionResult CreateEditItem(Item item)
        {
            
            if (item.Id == 0)
                _context.Items.Add(item);
            else
                _context.Items.Update(item);

            _context.SaveChanges();

            return RedirectToAction(nameof(Overview));

        }

        public IActionResult DeleteItem(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Overview));
        }

    }
}
```

**/InventoryManagement.Mvc/Controllers/OrdersController.cs**

```cs
```

</p>
</details>

**Ziel:** Nach Abschluss dieser Aufgabe beherrschst du:

- Die Integration von Entity Framework Core in ASP.NET Core MVC
- Konfiguration und Nutzung von SQL-Datenbanken
- Erstellung und Konfiguration eines DbContext
- Arbeiten mit Datenmodellen und Relationen
- Persistierung von Daten mithilfe von Entity Framework Core
