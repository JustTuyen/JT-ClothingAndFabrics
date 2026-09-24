export function formatPrice(value: number | undefined | null): string {
    const safeValue = value == null || isNaN(value) ? 0 : value;

    return new Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
        minimumFractionDigits: 0,
    }).format(safeValue);
}

