// Pricing.cs — 杯款基礎定價 (base pricing)
// 這是穩定核心,練習中不需要修改。

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
