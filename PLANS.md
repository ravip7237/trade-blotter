# PLANS.md

## Implementation checklist

Use this checklist as a concrete review list while building the application.

### Milestone 1 — Project scaffolding and baseline
- [x] Create or confirm the .NET 8 backend structure and initial ASP.NET Core app.
- [x] Create or confirm the Vue 3 + Vite frontend structure with Pinia and Vitest.
- [x] Add shared project conventions, dependency layout, and local run commands.
- [x] Write initial README setup instructions for backend and frontend.
- [x] Verify the backend builds and the frontend starts locally.
**CRITICAL CHECKPOINT**: Present verification results to the user. Stop and await explicit user approval before proceeding to next Milestone.

### Milestone 2 — Trade domain model and persistence
- [x] Define the trade domain model with symbol, side, quantity, price, and timestamp.
- [x] Define the position model and position calculation rules.
- [x] Define the database schema for trades with necessary constraints such as required fields, positive quantity/price, and valid side values.
- [x] Implement SQLite persistence with a repository layer for storing and reading trades.
- [x] Add seed data for local development and demos.
- [x] Verify trades can be stored and retrieved in the expected order.
**CRITICAL CHECKPOINT**: Present verification results to the user. Stop and await explicit user approval before proceeding to next Milestone.

### Milestone 3 — Position calculation and business logic
- [x] Implement net position logic from trade history.
- [x] Implement average cost logic for mixed buy/sell sequences.
- [x] Ensure zero-net positions are omitted from the response.
- [x] Add unit tests for basic, mixed, and edge-case trade sequences.
- [x] Verify position calculations match the expected business rules.
**CRITICAL CHECKPOINT**: Present verification results to the user. Stop and await explicit user approval before proceeding to next Milestone.


### Milestone 4 — API contract definition and validation
- [x] Define the API contract for POST /trades, GET /trades, and GET /positions.
- [x] Create DTOs for trade submission, trade responses, and position responses.
- [x] Implement the endpoints using controllers and services.
- [x] Add validation for required fields, positive quantities, positive prices, and valid sides.
- [x] Return consistent success responses and useful error responses with appropriate HTTP status codes.
- [x] Add API tests for successful and invalid requests.
**CRITICAL CHECKPOINT**: Present verification results to the user. Stop and await explicit user approval before proceeding to next Milestone.

### Milestone 5 — Frontend UI and state management
- [x] Build a trade entry form with validation for required fields and positive values.
- [x] Create a blotter table that displays trades newest-first.
- [x] Make the blotter sortable by timestamp, symbol, side, quantity, price, and notional value.
- [x] Add a positions panel that updates reactively after each new trade.
- [x] Use Pinia for shared state and keep UI updates reactive.
- [x] Verify the form submission updates the table and positions without a page reload.
**CRITICAL CHECKPOINT**: Present verification results to the user. Stop and await explicit user approval before proceeding to next Milestone.

### Milestone 6 — Test coverage, polish, and documentation
- [x] Add backend tests for persistence, services, and API behavior and exception conditions.
- [x] Add frontend tests for components and Pinia store behavior and exception conditions.
- [x] Refine the layout, table readability, and visual cues for buy/sell trades.
- [x] Update the README with setup steps, assumptions, and local run guidance.
- [x] Verify the core user journey works end-to-end.
Verification: frontend Vitest passed with 4 files and 7 tests; backend xUnit passed with 21 tests on .NET 8; the backend app started successfully on .NET 8 and responded on /health.
**CRITICAL CHECKPOINT**: Present verification results to the user. Stop and await explicit user approval before proceeding to next Milestone.
