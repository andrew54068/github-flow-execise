// Discount.cs — 滿額折扣 (spend-threshold discount)
//
// 規則:整筆訂單金額達到門檻就折抵固定金額。
//   滿 100 折 10
//   滿 200 折 30
//
// ⚠️ 第一關任務 (家瑞):這裡有一個「邊界 bug」。當金額「剛好」等於門檻時,
//    折扣沒有生效。請依 docs/tasks/level-1-feature-and-review.md 先寫測試重現,再修正。

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
            // BUG: 用 > 而非 >=,導致金額剛好等於門檻時拿不到折扣。
            if (subtotal > tier.Threshold)
            {
                return tier.Discount;
            }
        }
        return 0;
    }
}
