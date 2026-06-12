# 🅰️ Task A —— 實作加料計價 (feature) · 負責人:承勳

## 背景情境

飲料店要開始**收加料的錢**了。目前 `src/HitDrinkPos/Toppings.cs` 是一個空殼 (stub):不管加什麼料都算 0 元。
你的工作是把它實作成真正的加料計價。這是一個**對外可見的新功能 (feature)**。

## 加料價目 (topping menu)

| 加料 | key | 價格 |
|---|---|---|
| 珍珠 | `pearl` | +10 |
| 椰果 | `coconut` | +10 |
| 布丁 | `pudding` | +15 |

未知的加料要**丟出明確的錯誤 (throw)**,不能默默算成 0。

---

## 這是什麼性質的改動?(自己先判斷)

> 對外可見的新功能 → 分支 `feature/<描述>`,base 是 `main`,commit type `[feat]`。
> (對照 EXERCISE.md 的「黃金法則」表)

---

## 步驟

### 1. 確認起點是綠的
```bash
dotnet test       # 應該 12 passing
```

### 2. 開分支(交給 git-github-flow)
對 Claude Code 說:**「我要開始 task A,請用 git-github-flow 帶我走 feature 流程」**。
它會幫你:確認不在受保護分支上 → 從最新的 `main` 切出 `feature/topping-pricing`。

> 手動版(理解一下它做了什麼):
> ```bash
> git fetch origin
> git checkout main && git pull --ff-only origin main
> git checkout -b feature/topping-pricing
> ```

### 3. 實作
編輯 `src/HitDrinkPos/Toppings.cs` 的 `ToppingPrice()`:讓三種加料回傳正確價格,未知加料 `throw`。

### 4. 補上會證明它正確的測試
在 `test/HitDrinkPos.Tests/ToppingsTests.cs` 新增測試(這是你功能的證據):
```csharp
[Fact]
public void 各種加料的價格正確()
{
    Assert.Equal(10, Toppings.ToppingPrice("pearl"));
    Assert.Equal(15, Toppings.ToppingPrice("pudding"));
}

[Fact]
public void 未知加料會throw()
{
    var ex = Assert.Throws<ArgumentException>(() => Toppings.ToppingPrice("gold"));
    Assert.Contains("unknown topping", ex.Message);
}
```
> 檔案最上面記得有 `using System;` 與 `using Xunit;`(範本已經 import 了)。

跑 `dotnet test`,要看到**新測試通過、總數變多、全綠**。

### 5. Commit(交給 hit-committer)
對 Claude Code 說:**「幫我用 hit-committer 把這些改動 commit 起來」**。

合格的 commit 長這樣:
```
[feat] Charge for drink toppings at checkout

- Implement per-topping pricing (pearl/coconut +10, pudding +15).
- Reject unknown toppings with a clear error.
- Cover pricing and the unknown-topping path with unit tests.
```
> ⚠️ 注意 atomic:**功能實作 + 它的測試**算同一件事,可以放同一個 commit;但不要把無關的格式調整也塞進來。

### 6. 開 PR(交給 git-github-flow)
- base = `main`,head = `feature/topping-pricing`。
- 標題:`[feat] Charge for drink toppings at checkout (#<issue>)`。
- 內文:用 EXERCISE.md 的模板,**把 `dotnet test` 的輸出貼進 Test record 的 Unit tests**,寫上 `Closes #<你的 issue 編號>`。

### 7. 進入 review 迴圈
- 通知**家瑞**來 review 你的 PR。
- 同時你也去 review 家瑞的 🅱️ PR。
- 對方 Request changes → 你修 → push → 對方 Approve → 顧問合併。

---

## 完成的定義 (Definition of Done)
- [ ] 在 `feature/topping-pricing` 分支上,不是直接在 `main`。
- [ ] `dotnet test` 全綠,且測試數比一開始多。
- [ ] commit 是 `[feat]` 格式、atomic。
- [ ] PR base 是 `main`,內文有 `Closes #N` 和填好的測試紀錄。
- [ ] 收到家瑞的 Approve,並被合併進 `main`。

## 給 reviewer 的提示(家瑞看)
重點檢查:加料價目對不對、未知加料有沒有 throw、commit 格式、**測試紀錄有沒有填**。
