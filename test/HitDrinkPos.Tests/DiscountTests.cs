// DiscountTests.cs — 滿額折扣

using Xunit;

namespace HitDrinkPos.Tests;

public class DiscountTests
{
    [Fact]
    public void 未達門檻不折扣()
    {
        Assert.Equal(0, Discount.DiscountFor(0));
        Assert.Equal(0, Discount.DiscountFor(99));
    }

    [Fact]
    public void 超過100折10()
    {
        Assert.Equal(10, Discount.DiscountFor(150));
    }

    [Fact]
    public void 超過200折30()
    {
        Assert.Equal(30, Discount.DiscountFor(250));
    }

    [Fact]
    public void 剛好達到門檻就要折扣_邊界()
    {
        Assert.Equal(10, Discount.DiscountFor(100));
        Assert.Equal(30, Discount.DiscountFor(200));
    }
}
