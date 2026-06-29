Here is the requirements for the FullStack Trade Blotter application:

## Trade Blotter Application

# Stack
C# / .NET 8 backend • Vue 3 (Composition API) frontend

# Overview
Build a trade blotter application where a user can enter trades, view them in a live blotter table, and see their current positions automatically derived from the trade history.
This is the kind of tool traders use every day. 
Model the domain, design the UI, and wire the full stack together.

# Backend (C# / .NET 8):
Build a REST API with the following endpoints:
Endpoint
POST /trades - Submit a new trade
GET /trades - Return all trades, newest first
GET /positions - Return derived positions (net qty, avg cost) per symbol

A trade record must include:
Symbol (e.g. AAPL, MSFT)
Side — Buy or Sell
Quantity (shares)
Price (per share)
Timestamp

Positions should be derived from trades, not stored separately. A net position of zero means the symbol can be omitted from the response.

# Persistence: SQLite or SQL Server LocalDB is fine.

# Frontend (Vue 3, Composition API):
Build a single-page application with two main sections:
Trade entry form
Fields: symbol, side (Buy/Sell), quantity, price
Basic validation — no empty fields, quantity and price must be positive
On submit, the blotter should update immediately without a page reload

# Blotter table
Displays all trades, newest first
Columns: timestamp, symbol, side, quantity, price, notional value
Sortable by at least one column
Trades should be scannable at a glance — consider how side (Buy/Sell) is presented visually

# Positions panel
Shows current net position and average cost per symbol
Updates reactively when a new trade is submitted

Use Pinia for state management and Vite for tooling. Any component library is fine, or none at all.

# Guidlines:
Best practices for each layer (persistence layer, domain model, Api layer, frontend layer)
Follow industry standard coding conventions and guidlines for all layers
Clean and well defined backend folder structure ((/Controllers, /Services, /Repositories, /Models, /Dtos)
Well designed Domain models
Clean, well reasoned Trade and Position entities 
Create DTO objects to communicate with Frontend layer
Controllers should use only DTOs and not use Entity object
Correct Position logic (average cost on mixed buys/sells)
Calls to repository layer should only be allowed from Service layer
Proper API design
Variable names should be meaningful 
Clear endpoint contracts 
Well defined error handling with appropriate HTTP status codes
For frontend follow industry-standard layout for a clean, scalable Vue application
Vue patterns
Composition API used naturally, reactivity handled cleanly, state in Pinia not scattered
UI judgment
Scannable blotter. Intuitive form

# Tests
Full test coverage including tests for position calculation logic.
Include tests for each layer - database layer, service layer, api layer, front-end layer

# Constraints and guidance
No need to integrate real market data or authentication.
Seed data test data.
Prioritize a working, polished core over a feature-complete but rough submission.

# Include the following:
Include README with setup instructions
Instructions to run the project locally (backend and frontend)
Assumptions or design decisions worth calling out

# Scope and Boundary Rules
- **Strict Milestone Isolation**: You must work strictly within the boundaries of the currently active milestone defined in `PLANS.md`. 
- **No Feature Creep**: Do not write placeholder code, stub endpoints, UI components, or database structures for future milestones until explicitly directed via a `/goal` command.
- **No Cross-Layer Contamination**: When working on the backend, do not modify or generate frontend files. When working on database schemas, do not write API endpoints or logic layers.
- **Scope Verification**: Before implementing any code change, verify it directly correlates to an explicitly requested requirement of the current task. If an edit overlaps with a future milestone, stop and flag it to the user.

