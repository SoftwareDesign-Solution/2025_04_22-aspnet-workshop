- [4. Razor Pages in ASP.NET Core](#4-razor-pages-in-aspnet-core)
  - [4.1 Razor Pages für Items erstellen](#41-razor-pages-für-items-erstellen)
  - [4.2 Razor Pages für Orders erstellen](#42-razor-pages-für-orders-erstellen)

Bearbeitungszeit: n Minuten

The solution branch for the whole lab is `solution-4-razorpages`

# 4. Razor Pages in ASP.NET Core

In den letzten Aufgaben hast du im MVC-Projekt "InventoryManagement.Mvc" Models, Controllers, Views und die Integration von Entity Framework Core erstellt. In dieser Aufgabe wirst du ein neues Projekt namens "InventoryManagement.RazorPages" erstellen und die bisherigen Aufgaben mit Razor Pages durchführen. Die Views, Models und der vorhandene `ApplicationDbContext` aus dem MVC-Projekt kannst du übernehmen, alles andere soll neu erstellt werden.

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.RazorPages/appsettings.json**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventoryManagementDb;Trusted_Connection=True;"
}
```

**/InventoryManagement.RazorPages/Models/Item.cs**

```cs
namespace InventoryManagement.RazorPages.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
```

**/InventoryManagement.RazorPages/Models/Order.cs**

```cs
namespace InventoryManagement.RazorPages.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Comments { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
```

**/InventoryManagement.RazorPages/Models/OrderItem.cs**

```cs
namespace InventoryManagement.RazorPages.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
```

**/InventoryManagement.RazorPages/Data/ApplicationDbContext.cs**

```cs
using InventoryManagementWorkshop.RazorPages.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementWorkshop.RazorPages.Data
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

**/InventoryManagement.RazorPages/Program.cs**

```cs
using InventoryManagement.RazorPages.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Items/Overview", "");
});
```

</p>
</details>

## 4.1 Razor Pages für Items erstellen

Erstelle folgende Razor Pages für die Items:

- `Pages/Items/Overview.cshtml` (View aus MVC-Projekt übernehmen)
- `Pages/Items/CreateEdit.cshtml` (View aus MVC-Projekt übernehmen)
- Implementiere jeweils die zugehörigen PageModel-Klassen, inklusive Nutzung des vorhandenen ApplicationDbContext.

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.RazorPages/Pages/Items/Overview.cshtml**

```html
@page
@model InventoryManagementWorkshop.RazorPages.Pages.Items.OverviewModel
@{
	ViewData["Title"] = "Artikel";
}

<h1>Artikel</h1>

<p>
	<a class="btn btn-primary" asp-page-handler="CreateEdit">Neuer Artikel</a>
</p>

<hr />

<table class="table table-striped">
	<thead>
		<tr>
			<th>Id</th>
			<th>Name</th>
			<th>Description</th>
			<th>Price (€)</th>
			<th>Stock</th>
			<th>Actions</th>
		</tr>
	</thead>
	<tbody>
		@foreach (var item in Model.Items)
		{
			<tr>
				<td>@item.Id</td>
				<td>@item.Name</td>
				<td>@item.Description</td>
				<td>@item.Price</td>
				<td>@item.Stock</td>
				<td>
					<a class="btn btn-secondary" asp-page="CreateEdit" asp-route-id="@item.Id">Bearbeiten</a>
					
					<!-- Löschen-Button -->
					<form method="post" asp-page-handler="Delete" asp-route-id="@item.Id" style="display:inline;">
						<button type="submit" class="btn btn-danger" onclick="return confirm('Wirklich löschen?');">Löschen</button>
					</form>
				</td>
			</tr>
		}
	</tbody>
</table>
```

**/InventoryManagement.RazorPages/Pages/Items/Overview.cshtml.cs**

```cs
using InventoryManagement.RazorPages.Data;
using InventoryManagement.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.RazorPages.Pages.Items
{
    public class OverviewModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public IEnumerable<Item> Items { get; private set; } = new List<Item>();

        public OverviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Items = _context.Items.ToList();
        }

        public void OnPostDelete(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
            Items = _context.Items.ToList();
        }

    }
}
```

**/InventoryManagement.RazorPages/Pages/Items/CreateEdit.cshtml**

```html
@page
@model InventoryManagement.RazorPages.Pages.Items.CreateEditModel
@{
	ViewData["Title"] = "Neuer Artikel";
}

<h1>Neuer Artikel </h1>
<p>
	<a class="btn btn-primary" asp-page="Overview">Zurück zur Übersicht</a>
</p>

<hr />

<form method="post">
	<div class="mb-3">
		<label asp-for="@Model.Item.Id" class="form-label">Id</label>
		<input asp-for="@Model.Item.Id" class="form-control" />
	</div>
	<div class="mb-3">
		<label asp-for="@Model.Item.Name" class="form-label">Name</label>
		<input asp-for="@Model.Item.Name" class="form-control" />
		<span class="text-danger" asp-validation-for="@Model.Item.Name"></span>
	</div>
	<div class="mb-3">
		<label asp-for="@Model.Item.Description" class="form-label">Description</label>
		<textarea asp-for="@Model.Item.Description" class="form-control"></textarea>
	</div>
	<div class="mb-3">
		<label asp-for="@Model.Item.Price" class="form-label">Price</label>
		<input type="number" asp-for="@Model.Item.Price" class="form-control" />
		<span class="text-danger" asp-validation-for="@Model.Item.Price"></span>
	</div>
	<div class="mb-3">
		<label asp-for="@Model.Item.Stock" class="form-label">Stock</label>
		<input type="number" min="0" asp-for="@Model.Item.Stock" class="form-control" />
		<span class="text-danger" asp-validation-for="@Model.Item.Stock"></span>
	</div>
	<button type="submit" class="btn btn-primary">Save</button>
</form>
```

**/InventoryManagement.RazorPages/Pages/Items/CreateEdit.cshtml.cs**

```cs
using InventoryManagement.RazorPages.Data;
using InventoryManagement.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.RazorPages.Pages.Items
{
    public class CreateEditModel : PageModel
    {

        [BindProperty]
        public Item Item { get; set; } = new Item();

        private readonly ApplicationDbContext _context;

        public CreateEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                Item = _context.Items.FirstOrDefault(i => i.Id == id);
                if (Item == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }
        
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Item.Id > 0)
            {
                // Update existing item
                _context.Items.Update(Item);
            }
            else
            {
                // Create new item
                _context.Items.Add(Item);
            }
            
            _context.SaveChanges();
            
            // Redirect to the overview page after saving
            return RedirectToPage("Overview");
        }

    }
}
```

</p>
</details>

## 4.2 Razor Pages für Orders erstellen

Erstelle folgende Razor Pages für die Bestellungen:

- `Pages/Orders/Overview.cshtml` (View aus MVC-Projekt übernehmen)
- `Pages/Orders/OrderDetails.cshtml` (View aus MVC-Projekt übernehmen)
- `Pages/Orders/CreateEdit.cshtml` (View aus MVC-Projekt übernehmen)
- Implementiere jeweils die zugehörigen PageModel-Klassen, inklusive Nutzung des vorhandenen ApplicationDbContext.

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.RazorPages/Pages/Orders/Overview.cshtml**

```cs
```

**/InventoryManagement.RazorPages/Pages/Orders/Overview.cshtml.cs**

```cs
```

**/InventoryManagement.RazorPages/Pages/Orders/OrderDetails.cshtml**

```cs
```

**/InventoryManagement.RazorPages/Pages/Orders/OrderDetails.cshtml.cs**

```cs
```

**/InventoryManagement.RazorPages/Pages/Orders/**

```cs
```

</p>
</details>

**Ziel:** Nach Abschluss der Aufgabe beherrschst du:

- Erstellung und Strukturierung eines Razor Pages-Projekts
- Migration bestehender MVC-Views zu Razor Pages
- Datenzugriff und -verwaltung mit Entity Framework Core in Razor Pages
- Umsetzung von MVC-Aufgaben in Razor Pages
