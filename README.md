# Bus Ticket Booking System

An in-memory, console-based ticket reservation engine built with C# and .NET Core. It showcases a clean, decoupled architecture using **SOLID principles** and design patterns to sync data without a database.

---

## 🚀 The Core Problem

* **Dynamic Capacities:** Seating capacity varies strictly based on bus class (Business vs. Economy).
* **Isolated Inventories:** A physical coach can run multiple trips; seat tracking must be handled independently per trip to prevent double-booking.
* **Transient State:** All data operations execute reliably in volatile memory (RAM) during runtime.

---

## 🏗️ Architectural Design

```text
BusTicketBookingSystem/
├── Program.cs                      # Main runner & console UI
├── Models/                         # Pure domain logic and schemas
│   ├── User.cs                     # Passenger profiles with Regex validation
│   ├── Schedule.cs                 # Trip manager with Dictionary-based seat tracking
│   ├── Ticket.cs                   # Passenger-to-seat mappings
│   ├── Invoice.cs                  # Secured financial billing data
│   └── Bus/                        
│       ├── Bus.cs                  # Physical coach definitions
│       └── Strategies/             # Strategy Pattern for seating layouts
└── Services/                       # In-memory storage engines (Repository Layer)
    ├── IUserRepository.cs          # Data abstraction contract
    └── UserRepository.cs           # RAM data store with sequential IDs (USR-1, USR-2)

```

---

## 🛠️ Design Patterns Implemented

* **Strategy Pattern (`Bus/Strategies/`)**: Encapsulates seating capacities. Removes `if/else` checks from `Bus.cs`, satisfying the **Open/Closed Principle** so new coach types can be added without altering existing code.
* **Builder Pattern (Nested in `Invoice.cs`)**: Uses a nested builder with a private constructor to guarantee financial records are never initialized in an incomplete or corrupt state.
* **Repository Pattern (`Services/`)**: Acts as an in-memory database wrapper. It handles data access through a centralized `List<User>` and manages auto-incrementing counters to assign system-generated sequential IDs.
