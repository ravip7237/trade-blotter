# AGENTS.md

## Mission
Build a polished full-stack Trade Blotter application that satisfies the requirements in Requirements.md.

## Project context
- Backend: C# / .NET 8 with ASP.NET Core.
- Frontend: Vue 3 with the Composition API, Vite, Pinia, and Vitest.
- Storage: SQLite is preferred for local development simplicity.
- Scope is limited to trades, blotter display, derived positions, and local testing; no authentication or real market data.

## Architecture expectations
- Keep backend responsibilities separated by layer:
  - Controllers for HTTP handling
  - Services for business logic
  - Repositories for persistence
  - Models and DTOs for domain/API contracts
- Keep trade and position logic domain-focused and testable.
- Derive positions from trade history rather than storing a separate position table.
- Use DTOs for API requests and responses so the frontend contract stays explicit.
- Return appropriate HTTP status codes for validation and error cases.

## Frontend expectations
- Use Vue 3 Composition API patterns naturally.
- Keep state in Pinia stores rather than scattering it across components.
- Make the UI reactive so a submitted trade updates the blotter and positions immediately.
- Use clear visual cues for buy/sell trades and keep the table easy to scan.

## Testing expectations
- Write tests for the core domain logic first, especially position calculation.
- Cover backend layers: persistence, service, and API.
- Cover the frontend with component/store tests.
- Prefer real behavior tests over brittle mocks.

## Implementation guidance
1. Start with the domain model and position calculation rules.
2. Implement persistence and API endpoints next.
3. Build the Vue UI and Pinia store once the API contract is stable.
4. Add seed data and local run instructions before finalizing.

## Definition of done
- The app supports submitting trades, listing trades newest-first, and returning derived positions.
- Validation prevents empty or invalid trade values.
- Tests cover domain, persistence, API, and frontend behavior.
- The README includes setup and run instructions for both frontend and backend.
