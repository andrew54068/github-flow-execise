// ReceiptTests.cs — 訂單結帳 (非空訂單,綠的)
//
// 注意:空訂單的 AveragePerCup() 會 crash——那是隨 v1.1.0 出貨的 bug,
// 走 hotfix 流程處理 (見 task C),這裡先不測空訂單。

using System;
using Xunit;

namespace HitDrinkPos.Tests;

public class ReceiptTests
{
    private static readonly Cup[] Order =
    {
        new Cup("milkTea", "L", Array.Empty<string>()),  // 60 (加料目前都是 0)
        new Cup("greenTea", "M", Array.Empty<string>()), // 30
    };

    [Fact]
    public void LineTotal_計算單杯總價()
    {
        Assert.Equal(60, Receipt.LineTotal(Order[0]));
        Assert.Equal(30, Receipt.LineTotal(Order[1]));
    }

    [Fact]
    public void Subtotal_加總所有杯款()
    {
        Assert.Equal(90, Receipt.Subtotal(Order));
    }

    [Fact]
    public void OrderTotal_扣掉滿額折扣_90未達門檻不折()
    {
        Assert.Equal(90, Receipt.OrderTotal(Order));
    }

    [Fact]
    public void AveragePerCup_對非空訂單算平均()
    {
        Assert.Equal(45d, Receipt.AveragePerCup(Order)); // (60 + 30) / 2
    }
}
