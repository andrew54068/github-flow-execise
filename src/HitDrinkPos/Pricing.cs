// Pricing.cs — 杯款基礎定價 (base pricing)
//
// 第一關:這是穩定核心,不用改。
// ⚠️ 第二關 (兩人一起):雙方會「各自在自己的 branch」往 BasePriceTable 同一個位置
//    加一款新飲料,故意製造 merge conflict 來練習解衝突。見 docs/tasks/level-2-merge-conflict.md。

using System;
using System.Collections.Generic;

namespace HitDrinkPos;

public static class Pricing
{
    // 各杯款的基礎價格 (base price)，單位:元
    public static readonly IReadOnlyDictionary<string, int> BasePriceTable = new Dictionary<string, int>
    {
        ["greenTea"] = 30, // 綠茶
        ["blackTea"] = 30, // 紅茶
        ["milkTea"] = 50,  // 奶茶
        ["latte"] = 65,    // 拿鐵
        // 👇 第二關:把你的新杯款加在這一行下面 (兩人都加在這裡 → 製造 conflict)
    };

    // 尺寸加價 (size surcharge)
    public static readonly IReadOnlyDictionary<string, int> SizeExtraTable = new Dictionary<string, int>
    {
        ["M"] = 0,  // 中杯
        ["L"] = 10, // 大杯
    };

    public static int BasePrice(string drink)
    {
        if (!BasePriceTable.TryGetValue(drink, out var price))
        {
            throw new ArgumentException($"未知杯款 (unknown drink): {drink}");
        }
        return price;
    }

    public static int SizeExtra(string size)
    {
        if (!SizeExtraTable.TryGetValue(size, out var extra))
        {
            throw new ArgumentException($"未知尺寸 (unknown size): {size}");
        }
        return extra;
    }

    // 單杯價格 = 基礎價 + 尺寸加價 (尚未含加料 toppings)
    public static int CupPrice(string drink, string size)
    {
        return BasePrice(drink) + SizeExtra(size);
    }
}
