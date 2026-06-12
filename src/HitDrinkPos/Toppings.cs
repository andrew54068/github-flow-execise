// Toppings.cs — 加料計價 (topping pricing)
//
// ⚠️ 練習任務 (承勳):目前這是一個「空殼 stub」,任何加料都回傳 0 元。
//    請依 docs/tasks/task-A-feature-toppings.md 把它實作成真正的加料計價。
//
// 預期的加料價目 (topping menu)，單位:元:
//   珍珠 pearl  +10
//   椰果 coconut +10
//   布丁 pudding +15

using System.Collections.Generic;

namespace HitDrinkPos;

public static class Toppings
{
    public static int ToppingPrice(string topping)
    {
        // TODO(承勳): 實作真正的加料計價,並處理未知加料的錯誤。
        return 0;
    }

    // 一杯飲料可以加多個料,回傳加料總額。
    public static int ToppingsTotal(IEnumerable<string>? toppings = null)
    {
        if (toppings is null)
        {
            return 0;
        }

        var sum = 0;
        foreach (var t in toppings)
        {
            sum += ToppingPrice(t);
        }
        return sum;
    }
}
