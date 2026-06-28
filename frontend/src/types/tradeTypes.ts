export interface Trade {
  id: number;
  symbol: string;
  side: string;
  quantity: number;
  price: number;
  timestamp: string;
}

export interface Position {
  symbol: string;
  netQuantity: number;
  averageCost: number;
}