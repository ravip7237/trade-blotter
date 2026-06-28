export interface TradePayload {
  symbol: string;
  side: 'Buy' | 'Sell' | string;
  quantity: number;
  price: number;
  timestamp: string;
}

export interface TradeResponse {
  id: number;
  symbol: string;
  side: string;
  quantity: number;
  price: number;
  timestamp: string;
}

export interface PositionResponse {
  symbol: string;
  netQuantity: number;
  averageCost: number;
}

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? window.location.origin;

function buildApiUrl(path: string) {
  return new URL(path, `${API_BASE_URL}/`).toString();
}

async function readErrorMessage(response: Response, fallbackMessage: string) {
  try {
    const contentType = response.headers.get('content-type') ?? '';

    if (contentType.includes('application/problem+json') || contentType.includes('application/json')) {
      const payload = await response.json();

      if (payload?.detail) {
        return payload.detail;
      }

      if (payload?.title) {
        return payload.title;
      }
    }

    const text = await response.text();
    return text || fallbackMessage;
  } catch {
    return fallbackMessage;
  }
}

async function parseResponse<T>(response: Response, fallbackMessage: string): Promise<T> {
  if (!response.ok) {
    throw new Error(await readErrorMessage(response, fallbackMessage));
  }

  return response.json() as Promise<T>;
}

export async function getTrades() {
  const response = await fetch(buildApiUrl('trades'));
  return parseResponse<TradeResponse[]>(response, 'Unable to load trades');
}

export async function getPositions() {
  const response = await fetch(buildApiUrl('positions'));
  return parseResponse<PositionResponse[]>(response, 'Unable to load positions');
}

export async function createTrade(payload: TradePayload) {
  const response = await fetch(buildApiUrl('trades'), {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });

  return parseResponse<TradeResponse>(response, 'Unable to submit trade');
}