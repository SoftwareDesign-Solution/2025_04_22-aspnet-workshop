- [2. Formulare in ASP.NET Core MVC](#2-formulare-in-aspnet-core-mvc)
  - [2.1 Formulare erstellen](#21-formulare-erstellen)
  - [2.2 Controller-Funktionen erstellen](#22-controller-funktionen-erstellen)
  - [2.3 Validierung hinzufügen](#23-validierung-hinzufügen)

Bearbeitungszeit: n Minuten

The solution branch for the whole lab is `solution-2-mvc-forms`

# 2. Formulare in ASP.NET Core MVC

Um eine intuitive Benutzererfahrung und eine zuverlässige Datenverarbeitung sicherzustellen, erstellen Sie in dieser Aufgabe Formulare zur Eingabe und Bearbeitung von Artikeln und Bestellungen mit ASP.NET Core MVC.

## 2.1 Formulare erstellen

Erstellen Sie für die Klassen `Item` und `Order` jeweils eine View namens `CreateEdit.cshtml`. Diese Views sollen Formulare zur Eingabe und Bearbeitung von Artikeln und Bestellungen enthalten. Nutze zur Gestaltung das Bootstrap-Beispiel für Forms (<https://getbootstrap.com/docs/5.3/forms/overview/>).

**Formular für Item:**

- `Id` (verstecktes Feld - Hidden)
- `Name` (Text)
- `Description` (TextArea)
- `Price` (Numerisch)
- `Stock` (Numerisch)

**Formular für Order:**

- `Id` (verstecktes Feld - Hidden)
- `OrderDate` (Date)
- `Comment` (TextArea)
- Möglichkeit, mehrere OrderItems hinzuzufügen:
  - Auswahl des Items (Dropdown)
  - Eingabe der Quantity (Numerisch)

Bereite für jedes Eingabefeld bereits die Ausgabe der Validierungs-Fehlermeldungen mit asp-validation-for vor.

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Views/Items/CreateEdit.cshtml**

```html
@model Item

@{
	ViewData["Title"] = Model.Id == 0 ? "Neuer Artikel" : "Artikel " + Model.Id;
}

<h1>@ViewData["Title"]</h1>

<p>
	<a class="btn btn-primary" asp-controller="Items" asp-action="Overview">Zurück zur Übersicht</a>
</p>

<hr />

<form method="post" asp-controller="Items" asp-action="CreateEditItem">
	<div class="mb-3">
		<label asp-for="Id" class="form-label">Id</label>
		<input asp-for="Id" class="form-control" disabled readonly />
	</div>
	<div class="mb-3">
		<label asp-for="Name" class="form-label">Name</label>
		<input asp-for="Name" class="form-control" />
		<span class="text-danger" asp-validation-for="Name"></span>
	</div>
	<div class="mb-3">
		<label asp-for="Description" class="form-label">Description</label>
		<textarea asp-for="Description" class="form-control"></textarea>
	</div>
	<div class="mb-3">
		<label asp-for="Price" class="form-label">Price</label>
		<input type="number" asp-for="Price" class="form-control" />
		<span class="text-danger" asp-validation-for="Price"></span>
	</div>
	<div class="mb-3">
		<label asp-for="Stock" class="form-label">Stock</label>
		<input type="number" min="0" asp-for="Stock" class="form-control" />
		<span class="text-danger" asp-validation-for="Stock"></span>
	</div>
	<button type="submit" class="btn btn-primary">Save</button>
</form>
```

**/InventoryManagement.Mvc/Views/**

```html
```

</p>
</details>

## 2.2 Controller-Funktionen erstellen

Fügen Sie in den Controllern (`ItemsController` und `OrdersController`) jeweils die folgenden Methoden hinzu:

- `CreateEdit (GET)`: Zeigt das Formular an, entweder leer (zum Anlegen neuer Items/Orders) oder vorausgefüllt (zum Bearbeiten existierender Items/Orders).
- `CreateEditItem (POST)` bzw. `CreateEditOrder (POST)`: Nimmt die Daten aus dem Formular entgegen und speichert sie (simulierte Speicherung ohne Datenbank).

<details>
<summary>Show solution</summary>
<p>

**/InventoryManagement.Mvc/Controllers/ItemsController.cs**

```cs
// GET-Methode
public IActionResult CreateEdit(int? id)
{
    // Logik zum Laden existierender Daten oder Erstellen neuer Instanz
    return View(item);
}

// POST-Methode
public IActionResult CreateEditItem(Item item)
{
    // Logik zur Verarbeitung der eingegebenen Daten
    return RedirectToAction(nameof(Overview));
}
```

**/InventoryManagement.Mvc/Controllers/OrdersController.cs**

```cs
// GET-Methode
public IActionResult CreateEdit(int? id)
{
    // Logik zum Laden existierender Daten oder Erstellen neuer Instanz
    return View(order);
}

// POST-Methode
public IActionResult CreateEditOrder(Order order)
{
    // Logik zur Verarbeitung der eingegebenen Daten
    return RedirectToAction(nameof(Overview));
}
```

</p>
</details>

## 2.3 Validierung hinzufügen

Nutze DataAnnotations für die Validierung der Eingabefelder im Model (z.B. Pflichtfelder, Wertebereiche).

**Validierungsattribute:**

- `Item`
  - `Id`
  - `Name` (Required)
  - `Description`
  - `Price` (Required)
  - `Stock` (Required)
- `Order`
  - `Id`
  - `OrderDate` (Required)
  - `Comment`
- `OrderItem`
  - `Id`
  - `OrderId`
  - `ItemId` (Required)
  - `Quantity` (Required)

Validierungsnachrichten sollen in den Views angezeigt werden.

Nutze DataAnnotations für die Validierung der Eingabefelder im Model (z.B. Pflichtfelder, Wertebereiche).

**Beispiel für Item:**

```cs
[Required]
public string Name { get; set; }

[Range(0.01, 10000.00)]
public decimal Price { get; set; }
```

Validierungsnachrichten sollen in den Views angezeigt werden.

<details>
<summary>Show solution</summary>
<p>

**/**

```cs
```

</p>
</details>

**Ziel:** Nach Abschluss der Aufgabe beherrschst du:

- Erstellung und Nutzung von Formularen in ASP.NET Core MVC
- Implementierung von GET- und POST-Methoden zur Dateneingabe und -bearbeitung
- Nutzung von Model Validation mit DataAnnotations
- Integration von Bootstrap zur Gestaltung von Formularen
