# 🅱️ Task B —— 修滿額折扣的邊界 bug (bug fix) · 負責人:家瑞

## 背景情境

客訴來了:「我消費**剛好滿 100**,為什麼沒折到 10 元?」
`src/HitDrinkPos/Discount.cs` 的滿額折扣,在金額**剛好等於門檻**時不生效。這是一個**非緊急的 bug**(還沒出貨到 release 線,在 `main` 上修就好)。

## 規則(正確版)

| 消費金額 | 折扣 |
|---|---|
| 滿 100(含)| 折 10 |
| 滿 200(含)| 折 30 |

「滿」= **大於等於 (>=)**。現在的程式用了 `>`,所以剛好 100、剛好 200 都拿不到折扣 —— 經典的 **off-by-one / 邊界 bug**。

---

## 這是什麼性質的改動?(自己先判斷)

> 非緊急 bug 修正 → 分支 `bug/<描述>`,base 是 `main`,commit type `[fix]`。

---

## 步驟(這題請「先寫測試重現,再修」)

### 1. 確認起點是綠的
```bash
dotnet test       # 12 passing
```

### 2. 開分支(交給 git-github-flow)
對 Claude Code 說:**「我要開始 task B,請用 git-github-flow 帶我走 bug 流程」**。
它會從最新的 `main` 切出 `bug/discount-boundary`。

### 3. 先寫一個會「失敗」的測試,重現 bug
在 `test/HitDrinkPos.Tests/DiscountTests.cs` 新增:
```csharp
[Fact]
public void 剛好達到門檻就要折扣_邊界()
{
    Assert.Equal(10, Discount.DiscountFor(100)); // 目前會得到 0 → 失敗
    Assert.Equal(30, Discount.DiscountFor(200)); // 目前會得到 10 → 失敗
}
```
跑 `dotnet test`,**確認這個新測試是紅的**。這一步證明你真的重現了 bug。

> 💡 為什麼先寫測試?因為「紅 → 綠」能證明你的修正真的有效,而不是自我感覺良好。

### 4. 修正
編輯 `src/HitDrinkPos/Discount.cs`,把 `>` 改成 `>=`。
再跑 `dotnet test`,這次要**全綠**。

### 5. Commit(交給 hit-committer)
對 Claude Code 說:**「幫我用 hit-committer commit」**。
合格範例:
```
[fix] Apply spend discount at the exact threshold

- Use >= so an order landing exactly on 100/200 gets its discount.
- Add boundary tests at both tiers to lock the behavior.
```
> 標題講「為什麼」(在邊界要折扣),不是 `Change > to >=`。

### 6. 開 PR(交給 git-github-flow)
- base = `main`,head = `bug/discount-boundary`。
- 標題:`[fix] Apply spend discount at the exact threshold (#<issue>)`。
- 內文用模板:`Closes #<你的 issue>`,Test record 的 Unit tests 貼 `dotnet test` 輸出。
  - 加分:在內文說明「修正前 DiscountFor(100)=0,修正後 =10」當作證據。

### 7. 進入 review 迴圈
- 通知**承勳**來 review。
- 同時你去 review 承勳的 🅰️ PR。
- Request changes → 修 → Approve → 合併。

---

## 完成的定義 (Definition of Done)
- [ ] 在 `bug/discount-boundary` 分支上。
- [ ] 有一個**先紅後綠**的邊界測試。
- [ ] `dotnet test` 全綠。
- [ ] commit 是 `[fix]` 格式、atomic。
- [ ] PR base 是 `main`,有 `Closes #N` 和測試紀錄。
- [ ] 收到承勳 Approve 並合併。

## 給 reviewer 的提示(承勳看)
重點檢查:有沒有**先寫重現測試**、邊界值 100/200 有沒有測到、commit 格式、**測試紀錄有沒有填**。
