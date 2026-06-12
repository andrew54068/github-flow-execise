// Receipt.cs — 訂單結帳與收據 (order checkout & receipt)
//
// 一筆訂單 (order) 是多杯飲料 (cup) 的陣列,每杯:
//   new Cup("milkTea", "L", new[] { "pearl" })
//
// ⚠️ 第四關練習 (hotfix):AveragePerCup() 在「空訂單」時會 crash。
//    這個 bug 在 v0.1.0 出貨前就存在,所以 v0.1.0 與 v0.2.0 兩條 release 線都中。
//    要走 hotfix 流程:修在「最早受影響的線」release/v0.1.0 上,再傳播到 main 與 release/v0.2.0。
//    詳見 docs/tasks/level-4-hotfix-propagation.md。

using System.Collections.Generic;
using System.Linq;

namespace HitDrinkPos;

// 一杯飲料:杯款、尺寸、加料 (toppings 可省略,預設無加料)
public readonly record struct Cup(string Drink, string Size, IReadOnlyList<string>? Toppings = null);

public static class Receipt
{
    // 單杯總價 = 杯款價 + 加料價
    public static int LineTotal(Cup cup)
    {
        return Pricing.CupPrice(cup.Drink, cup.Size) + Toppings.ToppingsTotal(cup.Toppings);
    }

    // 整筆訂單小計 (subtotal,折扣前)
    public static int Subtotal(IEnumerable<Cup> order)
    {
        return order.Sum(LineTotal);
    }

    // 整筆訂單應付金額 (policy: 小計 - 滿額折扣)
    public static int OrderTotal(IEnumerable<Cup> order)
    {
        var sub = Subtotal(order);
        return sub - Discount.DiscountFor(sub);
    }

    // 平均每杯價格 (average price per cup)
    public static double AveragePerCup(IReadOnlyCollection<Cup> order)
    {
        // BUG(v0.1.0/v0.2.0): 空訂單時 Average() 對空序列直接 throw InvalidOperationException。
        return order.Select(LineTotal).Average();
    }
}
