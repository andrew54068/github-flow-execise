# 🅲 Task C —— 修空訂單 crash (hotfix · 進階) · 兩人一起

> 這題是進階關卡,做完 A / B 還有時間再做。它會帶你走 Hit 最特別、也最容易做錯的部分:
> **hotfix 修在 release 線上,再 backflow 回 `main`**。

## 背景情境

`v1.1.0` 已經出貨給客戶了。客戶回報:**結帳一個空訂單(沒點任何飲料)時,系統直接 crash**。
`src/HitDrinkPos/Receipt.cs` 的 `AveragePerCup(empty)` 會丟出 `InvalidOperationException: Sequence contains no elements`。

因為這個 bug **已經隨 `v1.1.0` 出貨**,不能只在 `main` 修——要走 **hotfix 流程**:修在 `release/v1.1.0` 上,再帶回 `main`。

---

## 關鍵觀念:為什麼 hotfix 不一樣?

| 一般 bug (task B) | hotfix (task C) |
|---|---|
| 修在 `main` | 修在 **`release/v<X.Y.Z>`** |
| 分支 `bug/*` | 分支 **`hotfix/v1.1.0-*`** |
| PR base = `main` | PR base = **`release/v1.1.0`** |
| 合併就結束 | 合併後**還要 backflow 回 `main`** |

> 這題的 `release/v1.1.0` 是目前**最新且唯一**的 release 線,所以 backflow 用「**把 release 線整條 merge 回 main**」(規範 §7.3),**不需要 cherry-pick / port 分支**。

---

## 步驟

### Part 1 — 修在 release 線上

#### 1. 開 hotfix 分支(交給 git-github-flow)
對 Claude Code 說:**「receipt 有個 v1.1.0 出貨的 crash,請用 git-github-flow 帶我走 hotfix 流程」**。
它會從 `release/v1.1.0` 切出 `hotfix/v1.1.0-empty-order-crash`。

> 手動版:
> ```bash
> git fetch origin
> git checkout release/v1.1.0 && git pull --ff-only origin release/v1.1.0
> git checkout -b hotfix/v1.1.0-empty-order-crash
> ```

#### 2. 先寫重現測試
在 `test/HitDrinkPos.Tests/ReceiptTests.cs` 新增:
```csharp
[Fact]
public void 空訂單的平均應為0_不可crash()
{
    Assert.Equal(0, Receipt.AveragePerCup(Array.Empty<Cup>()));
}
```
跑 `dotnet test` → 應該**紅的(crash)**。

#### 3. 修正
編輯 `src/HitDrinkPos/Receipt.cs` 的 `AveragePerCup()`:空訂單時回傳 `0`。
(提示:在 `order.Count == 0` 時提前 `return 0`,避免對空序列呼叫 `Average()`。)
跑 `dotnet test` → **全綠**。

#### 4. Commit(hit-committer)→ commit type 是 `[hotfix]`
```
[hotfix] Prevent crash when averaging an empty order

- Return 0 for an empty order instead of averaging an empty sequence.
- Add a regression test for the empty-order path.
```

#### 5. 開 PR(git-github-flow)
- base = **`release/v1.1.0`**(不是 main!),head = `hotfix/v1.1.0-empty-order-crash`。
- 附測試紀錄。
- 通知對方 review → Approve → 合併進 `release/v1.1.0`。

> 合併後,正式情境裡 Release Owner 會打 patch tag `v1.1.1` 並發 Release。練習中由顧問示範或口頭帶過。

### Part 2 — Backflow 回 main(Discipline 2:立刻傳播)

hotfix 合併進 `release/v1.1.0` 後,**馬上**要讓 `main` 也拿到這個修正,否則下一個版本會把 bug 帶回來。

因為 `release/v1.1.0` 是最新線,做法是**開一個 `release/v1.1.0 → main` 的 PR**:

對 Claude Code 說:**「hotfix 已合併進 release/v1.1.0,請用 git-github-flow 開 backflow PR 回 main」**。
- base = `main`,head = **`release/v1.1.0` 本身**(不開新分支)。
- ⚠️ 合併方式必須是 **Create a merge commit**(不能 squash / rebase,否則會破壞受保護的 release 線歷史)。
- 這個 PR 一樣由 Release Owner 合併。

---

## 完成的定義 (Definition of Done)
- [ ] hotfix 修在 `hotfix/v1.1.0-*` 分支,**從 `release/v1.1.0` 切出**。
- [ ] hotfix PR 的 base 是 `release/v1.1.0`(不是 main)。
- [ ] 空訂單測試先紅後綠。
- [ ] commit type 是 `[hotfix]`。
- [ ] 合併後開了 `release/v1.1.0 → main` 的 backflow PR,且註明用 merge commit。

## 常見錯誤(自我檢查)
- ❌ 把 hotfix 開成 `bug/*`、base 設成 `main`。→ 已出貨的 bug 要修在 release 線。
- ❌ hotfix 合併進 release 後就收工,忘了 backflow 回 `main`。→ 違反 Discipline 2。
- ❌ backflow PR 用 squash。→ release 線必須用 merge commit。
