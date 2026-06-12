// DiscountTests.cs — 滿額折扣 (目前僅測「非邊界」金額,所以是綠的)
//
// 注意:剛好等於門檻 (100、200) 的邊界測試「故意」還沒寫——那正是 bug 所在。
// 由家瑞在第一關先補上會失敗的邊界測試,重現 bug 後再修 (見 level-1)。

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
}
