// Discount.cs — 滿額折扣 (spend-threshold discount)
//
// 規則:整筆訂單金額達到門檻就折抵固定金額。
//   滿 100(含)折 10
//   滿 200(含)折 30

using System.Collections.Generic;

namespace HitDrinkPos;

public readonly record struct DiscountTier(int Threshold, int Discount);

public static class Discount
{
    public static readonly IReadOnlyList<DiscountTier> Tiers = new[]
    {
        new DiscountTier(200, 30),
        new DiscountTier(100, 10),
    };

    public static int DiscountFor(int subtotal)
    {
        foreach (var tier in Tiers)
        {
            if (subtotal >= tier.Threshold)
            {
                return tier.Discount;
            }
        }
        return 0;
    }
}
