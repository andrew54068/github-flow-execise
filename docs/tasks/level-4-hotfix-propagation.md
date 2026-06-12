# 🔴 第四關 —— Hotfix 與跨版本傳播 (v0.1.0 → v0.1.1 → main + v0.2.0)

> **目標**:修一個**已出貨**的 bug,並把它**正確地傳播到每一條受影響的線**。
> 這是 Hit 流程裡最容易做錯、也最重要的一關。兩人一起做,輪流操作。

## 情境

客戶回報:**結帳一個空訂單(沒點任何飲料)時系統 crash**。
`Receipt.cs` 的 `AveragePerCup([])` 會 throw `InvalidOperationException`。
這個 bug 在 v0.1.0 出貨前就存在,所以 **`v0.1.0` 和 `v0.2.0` 兩條線都中**。

## 關鍵決策:修在哪條線?往哪裡傳播?(這就是這關要學的)

Hit 的規則(`git-github-flow` skill 的 §7.2):

1. **修在「最早仍在用、且含此 bug」的 release 線** → 這裡是 **`release/v0.1.0`**。
   - 為什麼不是 v0.2.0?因為從最早的線修起,往後傳播才能蓋住每一條受影響的版本,不會漏掉舊線。
2. 修好、合併、打 patch tag `v0.1.1`。
3. **判斷 v0.1.0 是不是最新線?** 不是——還有更新的 `release/v0.2.0`。
   → 所以走 **§7.2「superseded line」**:用 **cherry-pick** 把修正傳播到
     **`main`** 和**每一條更新的在用線**(這裡是 `release/v0.2.0`),各開一個 `port/*` 分支 + PR。
   → (如果 v0.1.0 剛好是最新線,才會改用 §7.3「直接 merge release → main」。這關不是這種情況。)

```
          修在最早受影響線              再傳播 (cherry-pick)
release/v0.1.0 ──[hotfix]──► tag v0.1.1 ──┬──► port → main
                                          └──► port → release/v0.2.0
```

---

## Part A — 在最早受影響線上修 (release/v0.1.0)

### 1. 開 hotfix 分支(交給 git-github-flow)
對工具說:**「Receipt 有個空訂單 crash,v0.1.0 和 v0.2.0 都中,請用 git-github-flow 帶我走 hotfix 流程」**。
它會從 **`release/v0.1.0`** 切出 `hotfix/v0.1.0-empty-order-crash`。

> 手動版:
> ```bash
> git fetch origin
> git checkout release/v0.1.0 && git pull --ff-only origin release/v0.1.0
> git checkout -b hotfix/v0.1.0-empty-order-crash
> ```

### 2. 先寫重現測試
在 `ReceiptTests.cs` 加:
```csharp
[Fact]
public void 空訂單的平均應為0_不可crash()
{
    Assert.Equal(0d, Receipt.AveragePerCup(Array.Empty<Cup>()));
}
```
`dotnet test` → 應**紅的 (crash)**。

### 3. 修正
`Receipt.cs` 的 `AveragePerCup`:空訂單回傳 0,避免對空序列呼叫 `Average()`。
```csharp
public static double AveragePerCup(IReadOnlyCollection<Cup> order)
{
    if (order.Count == 0)
    {
        return 0d;
    }
    return order.Select(LineTotal).Average();
}
```
`dotnet test` → **全綠**。

### 4. Commit(hit-committer)→ type 是 `[hotfix]`
```
[hotfix] Prevent crash when averaging an empty order

- Return 0 for an empty order instead of averaging an empty sequence.
- Add a regression test for the empty-order path.
```

### 5. 開 PR → base 是 `release/v0.1.0`(不是 main!)
- head = `hotfix/v0.1.0-empty-order-crash`,base = **`release/v0.1.0`**。
- 附測試紀錄,互相 review → approve → merge 進 `release/v0.1.0`。

### 6. (Release Owner)打 patch tag v0.1.1
```bash
git checkout release/v0.1.0 && git pull --ff-only origin release/v0.1.0
git tag -a v0.1.1 -m "Hotfix release v0.1.1"
git push origin v0.1.1
```
記下這個 hotfix 在 `release/v0.1.0` 上的 commit SHA(下面 cherry-pick 要用):
```bash
git log --oneline -5            # 找到 [hotfix] 那筆的 SHA → 記為 <hotfix-sha>
```

---

## Part B — 傳播到 main(cherry-pick + port PR)

**立刻**做(Discipline 2:hotfix 不能累積,否則下個從 main 切的版本會把 bug 帶回來)。

```bash
git checkout main && git pull --ff-only origin main
git checkout -b port/v0.1.1-empty-order-crash-to-main
git cherry-pick <hotfix-sha>
#   若有 conflict:hotfix 內容為準 (Discipline 3),解完 git add <file> && git cherry-pick --continue
git push -u origin port/v0.1.1-empty-order-crash-to-main
```
開 PR:
- base = **`main`**,head = `port/v0.1.1-empty-order-crash-to-main`。
- 標題:`[hotfix] Backport v0.1.1 empty-order fix to main`。
- 內文連結 v0.1.1 Release / 原 hotfix PR,附測試紀錄。
- review → approve → merge。

---

## Part C — 傳播到更新的線 release/v0.2.0(cherry-pick + port PR)

因為 `release/v0.2.0` 比 v0.1.0 新、且也含這個 bug,同樣要 cherry-pick 過去(**另開一條 port 分支**,名字結尾 `-to-v0.2.0`):

```bash
git checkout release/v0.2.0 && git pull --ff-only origin release/v0.2.0
git checkout -b port/v0.1.1-empty-order-crash-to-v0.2.0
git cherry-pick <hotfix-sha>
#   conflict → hotfix 為準
git push -u origin port/v0.1.1-empty-order-crash-to-v0.2.0
```
開 PR:
- base = **`release/v0.2.0`**,head = `port/v0.1.1-empty-order-crash-to-v0.2.0`。
- 標題:`[hotfix] Backport v0.1.1 empty-order fix to v0.2.0`。
- review → approve → merge。
- (Release Owner)`release/v0.2.0` 上線後可再打 `v0.2.1` patch tag(視需要)。

---

## 為什麼要這麼麻煩?(顧問會問)
- **同一個修正在每條線會有不同的 commit SHA**(原始在 v0.1.0,加上每個 port 一個)。這是 cherry-pick 的正常代價,一次性、有界,不會像舊的雙向 rebase 那樣重複衝突。
- **少做任何一條 = 那條線之後會 regress(bug 跑回來)**:
  - 漏了 main → 下次從 main 切的新版本又有 bug。
  - 漏了 v0.2.0 → 已出貨的 v0.2.x 客戶還在 crash。
- 因為從**最早受影響的線**修起,只要「往更新方向」傳播就能蓋滿,不必回頭管更舊的線。

---

## 過關條件 (Definition of Done)
- [ ] hotfix 修在 `hotfix/v0.1.0-*`,從 **`release/v0.1.0`** 切出,PR base 也是 `release/v0.1.0`。
- [ ] 空訂單測試先紅後綠;commit type 是 `[hotfix]`。
- [ ] 打了 `v0.1.1` tag。
- [ ] 用 **cherry-pick** 經 `port/*-to-main`、`port/*-to-v0.2.0` 兩條 PR,把修正傳到 **main** 和 **release/v0.2.0**。
- [ ] 三條線(v0.1.0 / main / v0.2.0)現在空訂單都不再 crash。

## 常見錯誤(自我檢查)
- ❌ 直接修在 `main` 上、開成 `bug/*`。→ 已出貨的 bug 要走 hotfix、修在 release 線。
- ❌ 修在 v0.2.0(較新線)。→ 要修在**最早受影響**的 v0.1.0,否則 v0.1.x 客戶沒被修到。
- ❌ hotfix 合進 v0.1.0 就收工,忘了傳播。→ 違反 Discipline 2,main 和 v0.2.0 會 regress。
- ❌ 一條 port 分支想同時 PR 到兩個 base。→ 一個 target 一條 `port/*-to-<target>`。

🎉 四關全破!你已經走過 Hit 流程的:feature/bug、conflict、release、hotfix 跨版本傳播。
