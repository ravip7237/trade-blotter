export function useCurrency(locale = 'en-US', currency = 'USD') {
  const formatter = new Intl.NumberFormat(locale, {
    style: 'currency',
    currency,
    maximumFractionDigits: 2,
  });

  function formatCurrency(value: number | string | null | undefined) {
    const numericValue = Number(value ?? 0);
    return formatter.format(Number.isFinite(numericValue) ? numericValue : 0);
  }

  return {
    formatCurrency,
  };
}