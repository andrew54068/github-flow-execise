// SugarIceTests.cs — 甜度 / 冰塊選項驗證的測試 (sugar & ice levels)

using System;
using Xunit;

namespace HitDrinkPos.Tests;

public class SugarIceTests
{
    [Theory]
    [InlineData("full", "regular")]   // 全糖 / 正常冰
    [InlineData("half", "less")]      // 半糖 / 少冰
    [InlineData("free", "hot")]       // 無糖 / 熱
    [InlineData("light", "light")]    // 微糖 / 微冰
    [InlineData("less", "free")]      // 少糖 / 去冰
    public void EnsureValid_合法甜度與冰塊_不丟錯(string sugar, string ice)
    {
        var ex = Record.Exception(() => SugarIce.EnsureValid(sugar, ice));
        Assert.Null(ex);
    }

    [Fact]
    public void EnsureValid_對未知甜度_throw並指出是sugar()
    {
        var ex = Assert.Throws<ArgumentException>(() => SugarIce.EnsureValid("super-sweet", "regular"));
        Assert.Contains("unknown sugar level", ex.Message);
        Assert.Contains("super-sweet", ex.Message);
    }

    [Fact]
    public void EnsureValid_對未知冰塊_throw並指出是ice()
    {
        var ex = Assert.Throws<ArgumentException>(() => SugarIce.EnsureValid("full", "frozen"));
        Assert.Contains("unknown ice level", ex.Message);
        Assert.Contains("frozen", ex.Message);
    }
}
