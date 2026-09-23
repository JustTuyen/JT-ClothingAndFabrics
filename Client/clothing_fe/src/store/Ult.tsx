export function formatPrice(value: number | undefined | null): string {
    const safeValue = value == null || isNaN(value) ? 0 : value;

    return new Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
        minimumFractionDigits: 0,
    }).format(safeValue);
}

// export function groupAttributesByType(variations: Variation[]): Record<string, string[]>{
//     const grouped: Record<string, Set<string>> = {};
//     for(const variation of variations){
//         for(const attr of variation.variantAttributes){
//             if(!grouped[attr.attributeTypes]){
//                 grouped[attr.attributeTypes] = new Set();
//             }

//             grouped[attr.attributeTypes].add(attr.attributeValues);
//         }
//     }

//     const result: Record<string, string[]> = {};
//     for(const key in grouped){
//         result[key] = Array.from(grouped[key])
//     }

//     return result;
// }