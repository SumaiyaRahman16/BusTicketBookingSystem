# Bus Ticket Booking System

An in-memory, console-based ticket reservation engine built with C# and .NET Core. It showcases a clean, decoupled architecture using **SOLID principles** and design patterns to sync data without a database.

---

##  The Core Problem

* **Dynamic Capacities:** Seating capacity varies strictly based on bus class (Business vs. Economy).
* **Isolated Inventories:** A physical coach can run multiple trips; seat tracking must be handled independently per trip to prevent double-booking.
* **Transient State:** All data operations execute reliably in volatile memory (RAM) during runtime.

---

##  Architectural Design

```
BusTicketBookingSystem/
├── Program.cs                  # Main runner entry point of the application
├── Models/                     # Pure domain logic layers and data schemas
│   ├── Invoice.cs              # Financial record handling with internal Builder logic
│   ├── Schedule.cs             # Route management and dynamic repo-driven layout rendering
│   ├── Ticket.cs               # Core core seat-to-passenger data structures
│   ├── User.cs                 # Profiling rules for system passengers
│   └── Bus/
│       ├── Bus.cs              # Fleet profile properties and structure tracking
│       └── Strategies/         # Dynamic seat calculation layout strategies
│           ├── BusinessSeatingStrategy.cs
│           ├── EconomySeatingStrategy.cs
│           └── ISeatingStrategy.cs
├── Services/                   # Persistence wrappers (In-Memory Repository Layer)
│   ├── BusRepository.cs
│   ├── IBusRepository.cs
│   ├── IInvoiceRepository.cs   # Note: Double 'I' naming constraint contract
│   ├── InvoiceRepository.cs
│   ├── IScheduleRepository.cs
│   ├── IUserRepository.cs
│   ├── ScheduleRepository.cs
│   └── UserRepository.cs
└── UI/                         # Presentation rendering controllers
    └── MenuController.cs       # Workflow state loop handlers, grids, and inputs

```
---

## Design Patterns Implemented

* **Strategy Pattern (`Bus/Strategies/`)**: Encapsulates seating capacities. Removes `if/else` checks from `Bus.cs`, satisfying the **Open/Closed Principle** so new coach types can be added without altering existing code.
* **Builder Pattern (Nested in `Invoice.cs`)**: Uses a nested builder with a private constructor to guarantee financial records are never initialized in an incomplete or corrupt state.
* **Repository Pattern (`Services/`)**: Acts as an in-memory database wrapper. It handles data access through a centralized `List<User>` and manages auto-incrementing counters to assign system-generated sequential IDs.
