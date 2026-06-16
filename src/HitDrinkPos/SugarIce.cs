// SugarIce.cs — 甜度 / 冰塊選項驗證 (sugar & ice levels)
//
// 手搖飲最核心的兩個選項。結帳時若帶了不存在的甜度 (例如 "super-sweet")
// 或冰塊,應該擋下來丟錯,而不是默默放行。
// 錯誤訊息沿用 Pricing / Toppings 的「未知… (unknown …)」風格,
// 並分得出是 sugar 還是 ice。

using System;
using System.Collections.Generic;
using System.Linq;

namespace HitDrinkPos;

public static class SugarIce
{
    // 合法甜度 (sugar levels)
    //   full 全糖、less 少糖、half 半糖、light 微糖、free 無糖
    public static readonly IReadOnlyList<string> SugarLevels =
        new[] { "full", "less", "half", "light", "free" };

    // 合法冰塊 (ice levels)
    //   regular 正常冰、less 少冰、light 微冰、free 去冰、hot 熱
    public static readonly IReadOnlyList<string> IceLevels =
        new[] { "regular", "less", "light", "free", "hot" };

    // 驗證一杯飲料的甜度與冰塊;合法則通過(不丟錯),未知則 throw。
    public static void EnsureValid(string sugar, string ice)
    {
        if (!SugarLevels.Contains(sugar))
        {
            throw new ArgumentException($"未知甜度 (unknown sugar level): {sugar}");
        }
        if (!IceLevels.Contains(ice))
        {
            throw new ArgumentException($"未知冰塊 (unknown ice level): {ice}");
        }
    }
}
