// Receipt.cs — 訂單結帳與收據 (order checkout & receipt)
//
// 一筆訂單 (order) 是多杯飲料 (cup) 的陣列,每杯:
//   new Cup("milkTea", "L", new[] { "pearl" })
//
// ⚠️ 進階練習任務 (hotfix):AveragePerCup() 在「空訂單」時會 crash。
//    這個 bug 已經隨 v1.1.0 出貨,所以要走 hotfix 流程修在 release/v1.1.0 上。
//    詳見 docs/tasks/task-C-hotfix-empty-order.md。

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
        // BUG(v1.1.0): 空訂單時 Average() 對空序列直接 throw InvalidOperationException。
        return order.Select(LineTotal).Average();
    }
}
