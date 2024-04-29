BankWebApp är en webbapplikation som erbjuder banktjänster för användare. Applikationen är utformad för att vara användarvänlig och säker, med fokus på att tillhandahålla enkel åtkomst till banktjänster och hantering av transaktioner.

Den här appen erbjuder följande funktioner:

- Användarautentisering: Användare kan logga in på sina konton med användarnamn och lösenord för att få tillgång till sina banktjänster. Man kan logga in som Admin och få tillgång till CRUD över Users,
  eller så loggar man in som Cashier och får tillgång till CRUD över Customers.
- Hantering av transaktioner: Användare kan utföra olika banktransaktioner, såsom insättningar, uttag och överföringar mellan konton.
- Landsspecifika statistik: Applikationen visar landsspecifik statistik över antalet kunder, antalet konton och den totala balansen för varje land.

Tekniska detaljer:

- Frontend-teknologier: HTML, CSS, JavaScript, Razor Pages (ASP.NET Core)
- Backend-teknologier: ASP.NET Core Razor Pages, C#
- Databashantering: Entity Framework Core (Code First Approach)
- Autentisering och auktorisering: ASP.NET Core Identity
- Animeringar: CSS-animations och CSS-transitioner för användarupplevelseförbättringar
- Extern bibliotek: Bootstrap för responsiv design och jQuery för DOM-manipulation

Teknisk översikt: 

- Jag använder mig utav microservices i ett externt bibliotek för att hålla koden strukturerad och DRY.
- ViewModels (För att inte exponera databasen).
- AutoMapper, för att enkelt mappa om properties tack vare Naming Convention.
- Interfaces, så att mina dependencies är loosely coupled.


-- MoneyLaunderingGuard --

Denna app har även en konsoll-applikation som letar efter misstänksamma transaktioner och ger dig en lands specifik rapport när den körs.

-- BankAPI -- 

Här kan vi prata med databasen direkt genom ett API och få tillgång till en specifik kunds uppgifter om man anger ett ID, eller så kan man se en kunds transaktioner.
Transaktionerna går att begränsa antalet man får tillbaka, med offset och limit (Skip / Take).
