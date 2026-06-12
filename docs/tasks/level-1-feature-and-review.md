# 🟢 第一關 —— 各做各的功能,互相 review (happy path)

> **目標**:走一次最乾淨的 Hit 流程——兩人改**不同檔案**,所以不會衝突。
> 練 `feature/*`、`bug/*` 分支、commit、PR、以及 **GitHub review 迴圈**。

## 角色分配

兩人**同時開工**,各做各的,再**互相當 reviewer**。

| 人 | 任務 | 改的檔案 | 分支 | base | commit type |
|---|---|---|---|---|---|
| **承勳** | 實作加料計價 (feature) | `Toppings.cs` | `feature/topping-pricing` | `main` | `[feat]` |
| **家瑞** | 修滿額折扣邊界 bug | `Discount.cs` | `bug/discount-boundary` | `main` | `[fix]` |

> 兩人改的是**不同檔案**,所以兩條 PR 互不衝突——這一關就是要你體會「正常情況有多順」。

---

## 共通步驟(兩人都一樣)

### 0. 確認起點是綠的
```bash
dotnet test          # 應該 12 passing
```

### 1. 開分支(交給 git-github-flow)
對 Claude Code / Codex / Cursor 說:**「我要開始第一關,請用 git-github-flow 帶我走 <feature 或 bug> 流程」**。
它會確認你不在受保護的 `main` 上,然後從最新的 `main` 切出你的分支。

> 手動版(理解它做了什麼):
> ```bash
> git fetch origin
> git checkout main && git pull --ff-only origin main
> git checkout -b feature/topping-pricing   # 家瑞則是 bug/discount-boundary
> ```

### 2. 改 code +（先紅後綠的）測試 → 見下面各自的細節

### 3. Commit(交給 hit-committer)
對工具說:**「幫我用 hit-committer 把這些改動 commit 起來」**。
commit 標題講「為什麼」,不是 `Update XXX.cs`。

### 4. 開 PR(交給 git-github-flow)
- base = `main`,head = 你的分支。
- 內文用 [EXERCISE.md](../../EXERCISE.md) 的模板,**把 `dotnet test` 的輸出貼進 Test record 的 Unit tests**,寫 `Closes #<issue>`。

### 5. review 迴圈(這關的重點)
- 通知對方來 review 你的 PR;你也去 review 對方的。
- reviewer:在 **Files changed** 留至少一則 inline comment → 先按 **Request changes**(挑一個可改進點)→ 對方修正 push 後 → **Approve**。
- **Approve 後,由開 PR 的人自己 merge**(Squash and merge)。

---

## 承勳的細節 —— 加料計價 (feature)

`Toppings.cs` 現在是空殼,任何加料都回傳 0。實作真正的價目:

| 加料 | key | 價格 |
|---|---|---|
| 珍珠 | `pearl` | +10 |
| 椰果 | `coconut` | +10 |
| 布丁 | `pudding` | +15 |

未知加料要 **throw**(別默默回 0)。

`Toppings.cs` 的 `ToppingPrice` 參考實作:
```csharp
private static readonly IReadOnlyDictionary<string, int> ToppingPriceTable =
    new Dictionary<string, int>
    {
        ["pearl"] = 10,
        ["coconut"] = 10,
        ["pudding"] = 15,
    };

public static int ToppingPrice(string topping)
{
    if (!ToppingPriceTable.TryGetValue(topping, out var price))
    {
        throw new ArgumentException($"未知加料 (unknown topping): {topping}");
    }
    return price;
}
```
> 記得在檔案頂端 `using System;`(用到 `ArgumentException`)。

在 `ToppingsTests.cs` 補上證明它正確的測試:
```csharp
[Fact]
public void 各種加料的價格正確()
{
    Assert.Equal(10, Toppings.ToppingPrice("pearl"));
    Assert.Equal(15, Toppings.ToppingPrice("pudding"));
}

[Fact]
public void 未知加料會_throw()
{
    var ex = Assert.Throws<ArgumentException>(() => Toppings.ToppingPrice("gold"));
    Assert.Contains("unknown topping", ex.Message);
}
```
跑 `dotnet test` → 全綠、測試數變多。

合格 commit:
```
[feat] Charge for drink toppings at checkout

- Implement per-topping pricing (pearl/coconut +10, pudding +15).
- Reject unknown toppings with a clear error.
- Cover pricing and the unknown-topping path with unit tests.
```

---

## 家瑞的細節 —— 滿額折扣邊界 (bug,先紅後綠)

`Discount.cs` 用了 `>` 而非 `>=`,導致金額**剛好等於門檻**時拿不到折扣。
正確規則:滿 100(含)折 10、滿 200(含)折 30。

**先寫會失敗的測試**,在 `DiscountTests.cs`:
```csharp
[Fact]
public void 剛好達到門檻就要折扣_邊界()
{
    Assert.Equal(10, Discount.DiscountFor(100)); // 目前會得到 0 → 失敗
    Assert.Equal(30, Discount.DiscountFor(200)); // 目前會得到 10 → 失敗
}
```
跑 `dotnet test`,**確認這個測試是紅的**(證明重現了 bug)。

**再修**:把 `Discount.cs` 的 `subtotal > tier.Threshold` 改成 `subtotal >= tier.Threshold`。
再跑 `dotnet test` → 全綠。

合格 commit:
```
[fix] Apply spend discount at the exact threshold

- Use >= so an order landing exactly on 100/200 gets its discount.
- Add boundary tests at both tiers to lock the behavior.
```

---

## 過關條件 (Definition of Done)
- [ ] 你在自己的短命分支上,不是直接在 `main`。
- [ ] `dotnet test` 全綠,測試數比一開始多。
- [ ] commit 是正確的 `[type]` 格式、atomic。
- [ ] PR base = `main`,內文有 `Closes #N` 和填好的測試紀錄。
- [ ] 你 review 了對方、也收到對方的 Approve,兩條 PR 都 merge 進 `main`。

➡️ 兩條都 merge 後,進入 [第二關:解 conflict](level-2-merge-conflict.md)。
