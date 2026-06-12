// ToppingsTests.cs — 加料計價 (目前是 stub,僅測得了「空加料 = 0」)
//
// 注意:加料目前還沒實作,所以這裡只放「空加料總額為 0」這種在 stub 上也成立的測試。
// 真正的加料價目測試,由承勳在第一關實作 feature 時補上 (見 level-1)。

using System;
using Xunit;

namespace HitDrinkPos.Tests;

public class ToppingsTests
{
    [Fact]
    public void 沒有加料時_加料總額為0()
    {
        Assert.Equal(0, Toppings.ToppingsTotal(Array.Empty<string>()));
        Assert.Equal(0, Toppings.ToppingsTotal());
    }
}
