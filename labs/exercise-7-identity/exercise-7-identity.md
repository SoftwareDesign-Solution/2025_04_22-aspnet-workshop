- [7. Authentifizierung & Autorisierung mit ASP.NET Core Identity](#7-authentifizierung--autorisierung-mit-aspnet-core-identity)
  - [7.1 ASP.NET Core Identity einrichten](#71-aspnet-core-identity-einrichten)
  - [7.2 Benutzer & Rollen anlegen](#72-benutzer--rollen-anlegen)
  - [7.3 Login- & Register-Funktionalität hinzufügen](#73-login---register-funktionalität-hinzufügen)
  - [7.4 Autorisierung konfigurieren](#74-autorisierung-konfigurieren)

Bearbeitungszeit: n Minuten

The solution branch for the whole lab is `solution-7-identity`

# 7. Authentifizierung & Autorisierung mit ASP.NET Core Identity

In dieser Aufgabe lernst du die Implementierung von Authentifizierung und Autorisierung mithilfe von ASP.NET Core Identity. Du kannst frei wählen, ob du diese Aufgabe im MVC-Projekt "InventoryManagement.Mvc" oder im Razor Pages-Projekt "InventoryManagement.RazorPages" umsetzt.

## 7.1 ASP.NET Core Identity einrichten

- Installiere ASP.NET Core Identity und konfiguriere es in deinem Projekt.
- Füge Identity zur Program.cs hinzu:

- In ApplicationDbContext DbContext durch IdentityContext ersetzen

```cs
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

app.UseAuthentication();
app.UseAuthorization();
```

## 7.2 Benutzer & Rollen anlegen

- Erstelle in deinem Projekt zwei Rollen:
  - Admin
  - User
- Erstelle zwei Benutzer:
  - Benutzer mit Rolle Admin
  - Benutzer mit Rolle User

<details>
<summary>Show solution</summary>
<p>

**/**

```cs
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

  var services = scope.ServiceProvider;

  var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
  var userManager = services.GetRequiredService<UserManager<IdentityUser>>();


  var adminRoleCheck = await roleManager.RoleExistsAsync("Admin");

  if(adminRoleCheck == false)
  {
    await roleManager.CreateAsync(new IdentityRole("Admin"));
  }

  var userRoleCheck = await roleManager.RoleExistsAsync("User");

  if(userRoleCheck == false)
  {
    await roleManager.CreateAsync(new IdentityRole("User"));
  }


  var adminUser = await userManager.FindByEmailAsync("admin@admin.de");
  
  if(adminUser == null)
  {

    var user = new IdentityUser
    {
      UserName = "admin@admin.de",
      Email = "admin@admin.de",
      EmailConfirmed = true
    };

    var createUserResult = await userManager.CreateAsync(user, "Admin123.");

    if (createUserResult.Succeeded)
    {
      await userManager.AddToRoleAsync(user, "Admin");
    }

  }

  var userUser = await userManager.FindByEmailAsync("user@user.de");
  
  if(userUser == null)
  {

    var user = new IdentityUser
    {
      UserName = "user@user.de",
      Email = "user@user.de",
      EmailConfirmed = true
    };

    var createUserResult = await userManager.CreateAsync(user, "User123.");

    if (createUserResult.Succeeded)
    {
      await userManager.AddToRoleAsync(user, "User");
    }

  }

}
```

</p>
</details>

## 7.3 Login- & Register-Funktionalität hinzufügen

Erweitere den Header (_Layout.cshtml), um Links für Login und Registrierung einzubinden:

```html
<a asp-area="Identity" asp-page="/Account/Login">Login</a>
<a asp-area="Identity" asp-page="/Account/Register">Registrieren</a>
```

## 7.4 Autorisierung konfigurieren

Setze Autorisierungsregeln in deinen Controllern bzw. Razor Pages:

- Nicht eingeloggt:
  - Kann Artikelliste (Overview) einsehen.
- Benutzerrolle Admin:
  - Kann Artikel anlegen, bearbeiten und löschen.
  - Kann Bestellungen löschen.
- Benutzerrolle User:
  - Kann Bestellungen anlegen und bearbeiten.

Beispiel Controller/Razor Page-Methoden:

```cs
[Authorize(Roles = "Admin")]
public IActionResult CreateEditItem(int id) { /*...*/ }

[Authorize(Roles = "Admin")]
public IActionResult DeleteOrder(int id) { /*...*/ }

[Authorize(Roles = "User")]
public IActionResult CreateEditOrder(int id) { /*...*/ }
```

**Ziel:** Nach Abschluss dieser Aufgabe beherrschst du:

- Integration und Verwendung von ASP.NET Core Identity
- Verwaltung von Benutzern und Rollen
- Implementierung von Login und Registrierungsfunktionen
- Einrichtung detaillierter Autorisierungsregeln
