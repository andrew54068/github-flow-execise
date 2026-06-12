// PricingTests.cs — 杯款基礎定價的測試 (穩定核心,已通過)

using System;
using Xunit;

namespace HitDrinkPos.Tests;

public class PricingTests
{
    [Fact]
    public void BasePrice_回傳各杯款基礎價()
    {
        Assert.Equal(30, Pricing.BasePrice("greenTea"));
        Assert.Equal(65, Pricing.BasePrice("latte"));
    }

    [Fact]
    public void BasePrice_對未知杯款_throw()
    {
        var ex = Assert.Throws<ArgumentException>(() => Pricing.BasePrice("bubbleCoffee"));
        Assert.Contains("unknown drink", ex.Message);
    }

    [Fact]
    public void SizeExtra_大杯加10_中杯不加()
    {
        Assert.Equal(0, Pricing.SizeExtra("M"));
        Assert.Equal(10, Pricing.SizeExtra("L"));
    }

    [Fact]
    public void CupPrice_等於基礎價加尺寸加價()
    {
        Assert.Equal(60, Pricing.CupPrice("milkTea", "L")); // 50 + 10
        Assert.Equal(30, Pricing.CupPrice("greenTea", "M"));
    }
}
