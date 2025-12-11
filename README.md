# Temple of Doom (Herkansing)

Dit is een C# Console Applicatie ontwikkeld voor de herkansingsopdracht van het vak Code Design. De applicatie simuleert een level gebaseerd op *Indiana Jones and the Temple of Doom*, waarbij de speler Sankara Stones moet verzamelen, sleutels moet vinden en valstrikken moet ontwijken.

Deze uitwerking implementeert **Module B (Portals & Conveyor Belts)** en voldoet aan de eisen voor Clean Code, Modulariteit en Design Patterns.

## 🚀 Features

*   **Verkenning**: Loop door verschillende kamers in een tempel.
*   **Interactieve Items**:
    *   **Sankara Stones**: Verzamel er 5 om te winnen.
    *   **Sleutels**: Nodig om gekleurde deuren te openen.
    *   **Boobytraps**: Doen schade aan de speler (sommige verdwijnen na activatie).
*   **Deuren & Obstakels**:
    *   **Gekleurde deuren**: Vereisen een specifieke sleutel.
    *   **Closing Gates**: Sluiten permanent na passage.
    *   **Toggle deuren**: Wisselen van status.
*   **Module B Specifics**:
    *   **Portals**: Teleporteer direct naar andere kamers.
    *   **Conveyor Belts**: Bewegen de speler (en vijanden) automatisch in een specifieke richting.
*   **Vijanden**: Autonoom bewegende vijanden (geïntegreerd via externe DLL).
*   **HUD**: Live weergave van levens, verzamelde stenen, tijd en inventory.

## 🛠️ Toegepaste Design Patterns

Om aan de beoordelingscriteria te voldoen, zijn de volgende patronen geïmplementeerd:

*   **Layered Architecture (Modulariteit)**: De applicatie is strikt gescheiden in **Core** (Domein & Interfaces), **Logic** (Spelregels & Laden) en **Presentation** (Console UI).
*   **Observer Pattern**:
    *   De `GameLoop` fungeert als Subject.
    *   De `ConsoleRenderer` en alle Enemies (via `EnemyAdapter`) zijn Observers die reageren op elke GameTick.
*   **Strategy Pattern**: Toegepast bij het laden van levels via de `ILevelLoader` interface. De `JsonLevelLoader` is de concrete strategie.
*   **Factory Pattern**: De `TileFactory` is verantwoordelijk voor het instantiëren van complexe tegels, items en deuren op basis van string-data uit de JSON.
*   **Decorator Pattern**: Gebruikt voor de deuren. Een `BasicDoor` kan worden ingepakt in een `ColoredDoor`, `ToggleDoor` of `ClosingGate` om gedrag dynamisch toe te voegen.
*   **Adapter Pattern**: Gebruikt om de externe bibliotheek (`CODE_TempleOfDoom_DownloadableContent.dll`) te integreren.
    *   `EnemyAdapter` vertaalt de externe Enemy naar de interne `IEnemy` interface.
    *   `RoomAdapter` vertaalt de interne Room naar de externe `IField` interface die de DLL verwacht.
*   **Composite / Polymorphism**:
    *   Tegels (`Tile`) bevatten items via compositie (`CurrentItem`).
    *   Alle interacties verlopen via de `IGameObject` interface.

## 🎮 Besturing

| Toets | Actie |
| :--- | :--- |
| **W** | Beweeg Noord (Omhoog) |
| **A** | Beweeg West (Links) |
| **S** | Beweeg Zuid (Omlaag) |
| **D** | Beweeg Oost (Rechts) |
| **ESC** | Sluit het spel af |

## 📂 Projectstructuur

1.  **TempleOfDoom.Core**: Bevat de Domain Entities (`Player`, `Room`, `Tile`), Interfaces (`IGameObject`, `ILevelLoader`) en DTO's. Dit project heeft geen afhankelijkheden naar andere lagen.
2.  **TempleOfDoom.Logic**: Bevat de Business Logic. Hierin zit de `GameLoop`, de `JsonLevelLoader`, de `TileFactory` en de implementatie van de Adapters voor de vijanden.
3.  **TempleOfDoom.Presentation**: De Entry Point van de applicatie. Verzorgt de input van de gebruiker en tekent de status op het scherm (`ConsoleRenderer`).