# trade-blotter

Trade Blotter is a full-stack application for entering trades, viewing the blotter, and deriving open positions from trade history.

## Prerequisites

- .NET 8 SDK
- Node.js 22.18+ or 24.12+
- npm

## Setup

1. Clone the repository and change into the workspace.
2. Install frontend dependencies:

```bash
cd frontend
npm install
```

3. Restore backend dependencies the first time you build or run the API:

```bash
cd ../backend/TradeBlotterApi
dotnet restore
```

4. Run the backend tests once to confirm the environment is working:

```bash
cd ../TradeBlotterApi.Tests
dotnet test
```

## Run locally

### Backend

Start the API from the backend project folder:

```bash
cd backend/TradeBlotterApi
dotnet run
```

The API runs at http://localhost:5048 and exposes a health endpoint at http://localhost:5048/health.
The backend targets .NET 8.

### Frontend

Start the Vue app from the frontend folder:

```bash
cd frontend
npm run dev -- --host 0.0.0.0
```

The app runs at http://localhost:5173 by default and uses a Vite proxy to talk to the backend during development.

## Test and validation

Run the frontend checks:

```bash
cd frontend
npm test
npm run typecheck
npm run build
```

Run the backend tests:

```bash
cd backend/TradeBlotterApi.Tests
dotnet test
```

Run the backend application:

```bash
cd backend/TradeBlotterApi
dotnet run
```

## Assumptions and design decisions

- Positions are derived from trade history rather than stored in a separate table.
- Trade data is treated as the source of truth; the UI updates the blotter immediately after a successful create call.
- SQLite is used for local development simplicity and to keep the project lightweight.
- The frontend uses Vue 3 with the Composition API, Pinia for state management, Vue Router, and Vite.
- Validation and error handling are split between the frontend form, backend API, and shared service/store boundaries so the UI stays responsive and the backend remains authoritative.
- The application does not include authentication, user accounts, or live market data.

## What I would improve with more time

- Add broader component, store, and API integration tests for more negative-path coverage.
- Add trade filtering, search, and pagination for larger blotters.
- Improve error recovery in the UI with retry actions and richer validation messages.
- Add seeded sample data and a one-command local bootstrap flow.
- Add export/reporting support for trades and positions.
- Replace the remaining ad hoc styling with a more complete design system or shared component library.

## Current milestone

The repository currently includes a working trade entry flow, a live blotter, derived positions, and a Vue-based frontend backed by a SQLite repository.