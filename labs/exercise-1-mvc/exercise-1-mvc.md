- [1. ASP.NET Core MVC](#1-aspnet-core-mvc)
  - [1.1 Models erstellen](#11-models-erstellen)
  - [1.2 Controllers erstellen](#12-controllers-erstellen)
  - [1.3 Views erstellen](#13-views-erstellen)
  - [1.4 Standardroute anpassen](#14-standardroute-anpassen)
  - [1.5 Navigation anpassen](#15-navigation-anpassen)

Bearbeitungszeit: 45 Minuten

The solution branch for the whole lab is `solution-1-mvc`

# 1. ASP.NET Core MVC

Um eine strukturierte und benutzerfreundliche Datenverwaltung zu ermöglichen, lernst du in dieser Aufgabe die Grundlagen von ASP.NET Core MVC anhand praktischer Beispiele mit Artikeln und Bestellungen kennen.

## 1.1 Models erstellen

Erstellen Sie im Verzeichnis `Models` die nachfolgenden Klassen:

- `Item` mit den Eigenschaften
  - `Id` (int)
  - `Name` (string)
  - `Description` (string)
  - `Price` (decimal)
  - `Stock` (int)
- `Order` mit den Eigenschaften
  - `Id` (int)
  - `OrderDate` (DateTime)
  - `Comments` (string)
  - `OrderItems` (ICollection)
- `OrderItem` mit den Eigenschaften
  - `Id` (int)
  - `OrderId` (int)
  - `Order` (Order)
  - `ItemId` (int)
  - `Item` (Item)
  - `Quantity` (int)

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Models/Item.cs**

```cs
namespace InventoryManagement.Mvc.Models
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

**/InventoryManagement.Mvc/Models/Order.cs**

```cs
namespace InventoryManagementWorkshop.Mvc.Models
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

**/InventoryManagement.Mvc/Models/OrderItem.cs**

```cs
namespace InventoryManagementWorkshop.Mvc.Models
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

</p>
</details>

## 1.2 Controllers erstellen

Erstellen Sie im Verzeichnis `Controllers` zwei Controller: `ItemsController` und `OrdersController`.

`ItemsController` enthält folgende Methoden:

- `Overview`, die eine Liste von Items zurückgibt.
- `CreateEdit`mit Parameter `id`, die später ein Formular zurückgibt
- `DeleteItem` mit Parameter `id`, die später den Artikel löschen soll

`OrdersController` enthält folgende Methoden:

- `Overview`, welche eine Liste von Orders zurückgibt.
- `OrderDetails` mit Parmeter `id`, welche die Details einer spezifischen Order inklusive OrderItems anzeigt.
- `CreateEdit`mit Parameter `id`, die später ein Formular zurückgibt
- `DeleteOrder` mit Parameter `id`, die später die Bestellung löschen soll

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Controllers/ItemsController.cs**

```cs
using InventoryManagement.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Mvc.Controllers
{
    public class ItemsController : Controller
    {
        public IActionResult Overview()
        {
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Laptop", Description = "Leistungsstarker Laptop", Price = 999.99m, Stock = 10 },
                new Item { Id = 2, Name = "Smartphone", Description = "Neuestes Modell", Price = 799.50m, Stock = 25 },
                new Item { Id = 3, Name = "Tablet", Description = "Handliches Tablet", Price = 399.00m, Stock = 15 }
            };

            return View(items);
        }

        public IActionResult CreateEdit(int id)
        {
            Item item = new Item { Id = id, Name = "Artikel " + id, Price = 1.50m, Stock = 10 };
            return View(item);
        }

        public IActionResult DeleteItem(int id)
        {
            return RedirectToAction(nameof(Overview));
        }
    }
}
```

**/InventoryManagement.Mvc/Controllers/OrdersController.cs**

```cs
using InventoryManagementWorkshop.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementWorkshop.Mvc.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Overview()
        {

            var orders = new List<Order>
            {
                new Order { Id = 1, OrderDate = DateTime.Now }
            };

            return View(orders);

        }

        public IActionResult OrderDetails(int id)
        {
            var order = new Order { Id = id, OrderDate = DateTime.Now, Comments = "Dies ist ein Kommentar zu dieser Bestellung." };
            order.OrderItems.Add(new OrderItem { Id = 1, ItemId = 1, Quantity = 2 });
            order.OrderItems.Add(new OrderItem { Id = 2, ItemId = 2, Quantity = 1 });
            order.OrderItems.Add(new OrderItem { Id = 3, ItemId = 3, Quantity = 5 });
            return View(order);
        }

        public IActionResult CreateEdit(int id)
        {
            var order = new Order { Id = id, OrderDate = DateTime.Now, Comments = "Dies ist ein Kommentar zu dieser Bestellung." };
            return View(order);
        }

        public IActionResult DeleteOrder(int id)
        {
            return RedirectToAction(nameof(Overview));
        }
    }
}
```

</p>
</details>

## 1.3 Views erstellen

Erstellen Sie für Items und Orders jeweils eine View namens `Overview.cshtml`. Beide Views sollen die Daten in einer Tabelle darstellen (orientiere dich am [Bootstrap Table-Beispiel](https://getbootstrap.com/docs/5.3/content/tables/)). Zusätzlich soll die Spalte "Actions" mit Edit- und Delete-Buttons vorhanden sein, welche die Funktionen `Edit` und `DeleteItem`/`DeleteOrder` mit der jeweiligen Id aufrufen.

**Beispiel Buttons für die Spalte "Actions"**

```html
<td>
    <a class="btn btn-secondary">Bearbeiten</a>
    <a class="btn btn-danger">Löschen</a>
</td>
```

Erstelle zusätzlich die View `OrderDetails.cshtml`, welche die Order und zugehörigen OrderItems in einer Tabelle darstellt.

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Views/Items/Overview.cshtml**

```html
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}

@model IEnumerable<Item>

@{
	ViewData["Title"] = "Artikel";
}

<h1>Artikel</h1>

<p>
	<a class="btn btn-primary" asp-controller="Items" asp-action="CreateEdit">Neuer Artikel</a>
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
	@foreach(var item in Model)
	{
		<tr>
			<td>@item.Id</td>
			<td>@item.Name</td>
			<td>@item.Description</td>
			<td>@item.Price</td>
			<td>@item.Stock</td>
			<td>
				<a class="btn btn-secondary" asp-controller="Items" asp-action="CreateEdit" asp-route-id="@item.Id">Bearbeiten</a>
				<a class="btn btn-danger" asp-controller="Items" asp-action="DeleteItem" asp-route-id="@item.Id">Löschen</a>
			</td>
		</tr>
	}
	</tbody>
</table>
```

**/InventoryManagement.Mvc/Views/Orders/Overview.cshtml**

```html
@model IEnumerable<Order>

@{
    ViewData["Title"] = "Bestellungen";
}

<h1>Bestellungen</h1>

<p>
	<a class="btn btn-primary">Neue Bestellung</a>
</p>

<hr />

<table class="table table-striped">
    <thead>
        <tr>
            <th>
                Id
            </th>
            <th>
                OrderDate
            </th>
            <th>
                Comments
            </th>
            <th></th>
        </tr>
    </thead>
    <tbody>
	@foreach (var item in Model) {
        <tr>
            <td>
                @item.Id
            </td>
            <td>
                @item.OrderDate
            </td>
            <td>
                @item.Comments
            </td>
	        <td>
		        <a class="btn btn-primary" asp-controller="Orders" asp-action="OrderDetails" asp-route-id="@item.Id">Details</a>
		        <a class="btn btn-secondary" asp-controller="Orders" asp-action="CreateEdit" asp-route-id="@item.Id">Bearbeiten</a>
		        <a class="btn btn-danger" asp-controller="Orders" asp-action="DeleteOrder" asp-route-id="@item.Id">Löschen</a>
	        </td>
        </tr>
    }
    </tbody>
</table>
```

**/InventoryManagement.Mvc/Views/Orders/OrderDetails.cshtml**

```html
@model Order

@{
	ViewData["Title"] = "Bestellung " + Model.Id;
}

<h1>Bestellung @Model.Id</h1>

<p>
	<a class="btn btn-primary" asp-controller="Orders" asp-action="CreateEdit" asp-route-id="@Model.Id">Bearbeiten</a>
	<a class="btn btn-danger" asp-controller="Orders" asp-action="DeleteOrder" asp-route-id="@Model.Id">Löschen</a>
</p>

<hr />

<p>
	<strong>Id:</strong> @Model.Id
	<strong>OrderDate</strong> @Model.OrderDate
	<br />
	<strong>Comments:</strong> @Model.Comments
</p>

<table class="table table-striped">
	<thead>
		<tr>
			<th>
				Name
			</th>
			<th>
				Quantity
			</th>
			<th>
				Price
			</th>
		</tr>
	</thead>
	<tbody>
	@foreach (var orderItem in Model.OrderItems)
	{
		<tr>
			<td>
					@orderItem.Item.Id
			</td>
			<td>
					@orderItem.Quantity
			</td>
			<td>
					@orderItem.Item.Price
			</td>
		</tr>
	}
	</tbody>
</table>
```

</p>
</details>

## 1.4 Standardroute anpassen

Passen Sie in der Datei `Program.cs` die Standardroute so an, dass beim Starten der Anwendung automatisch der `ItemsController` mit der Methode `Overview` aufgerufen wird.

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Program.cs**

```cs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Items}/{action=Overview}/{id?}");
```

</p>
</details>

## 1.5 Navigation anpassen

Passe die Navbar im `_Layout.cshtml` so an, dass "Artikel" auf `ItemsController.Overview` und "Bestellungen" auf `OrdersController.Overview` verweisen:

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Views/Shared/_Layout.cshtml**

```html
<div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
    <ul class="navbar-nav flex-grow-1">
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Items" asp-action="Overview">Artikel</a>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Orders" asp-action="Overview">Bestellungen</a>
        </li>
    </ul>
</div>
```

</p>
</details>

**Ziel:** Nach der Umsetzung dieser Aufgabe verstehen Sie:

- Die grundlegende Struktur von ASP.NET Core MVC (Model, View, Controller)
- Verwendung von verschachtelten Modellen und Beziehungen
- Die Interaktion zwischen Controller und View
- Verwendung von Razor Syntax zur Darstellung von Daten
- Integration von Bootstrap für die Gestaltung der Oberfläche
- Implementierung von Interaktionsbuttons (Edit & Delete)
- Anpassung der Standardroute zur Steuerung der Startseite
