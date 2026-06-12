# 🟠 第三關 —— 依序發佈兩個版本 (release v0.1.0 → v0.2.0)

> **目標**:扮演 **Release Owner**,把 `main` 上的成果**切成版本線**。
> 你會做兩次「發佈」:先 `v0.1.0`,接著在 `main` 加一點東西,再 `v0.2.0`。
> 做完會有**兩條同時在用的 release 線**——這是第四關 hotfix 跨版本傳播的前提。

## 重要觀念:發佈 = 「從 main 切分支 + 打 tag」

Hit 的模型裡(dev 與 release 環境相同),**發佈一個版本 = 從 `main` 切出 `release/v<X.Y.Z>` 分支 + 在切點打 `v<X.Y.Z>` tag**。
- `release/v<X.Y.Z>` 分支 = 那個版本的**維護線**(之後 hotfix 修在上面)。
- `v<X.Y.Z>` tag = 給 PM / 客服看的版本錨點,並發 GitHub Release。
- 切 release、打 tag、發 Release 都是 **Release Owner** 的職責(`git-github-flow` skill **不做**這步——它只負責 PR 流程)。所以這關用**手動 git 指令**,順便讓你看清楚發佈到底做了什麼。

> ⚠️ `release/*` 一旦建立就是**受保護分支**:不能直接 push commit 進去,只能透過 PR(第四關會用到)。但「切出分支」和「打 tag」本身是建立動作,由 Release Owner 執行。

---

## Part 1 — 承勳:發佈 v0.1.0

確認第一、二關的 PR 都 merge 了,`main` 是大家最新的成果:
```bash
git checkout main
git pull --ff-only origin main
dotnet test                       # 綠的再發佈

# 從 main 切出 v0.1.0 維護線並 push
git checkout -b release/v0.1.0
git push -u origin release/v0.1.0

# 在切點打 tag 並 push
git tag -a v0.1.0 -m "Release v0.1.0"
git push origin v0.1.0

git checkout main                 # 切回 main 繼續開發
```

到 GitHub 發 Release(可選但建議,體驗完整流程):
**Releases → Draft a new release → 選 tag `v0.1.0` → Generate release notes → Publish**。

✅ 現在有了第一條 release 線 `release/v0.1.0`。

---

## Part 2 — 家瑞:在 main 加一個小 feature

兩個版本之間 `main` 要有差異,才看得出 release 線是「某個時間點的快照」。
家瑞做一個小 feature(走完整的 feature → PR → review 流程):

**任務**:在 `Pricing.cs` 加一款季節限定飲料 `matchaLatte`(抹茶拿鐵)= 70 元。

```bash
git checkout main && git pull --ff-only origin main
git checkout -b feature/drink-matcha-latte
```
在 `BasePriceTable` 加 `["matchaLatte"] = 70, // 抹茶拿鐵`,commit(`[feat] Add matcha latte to the menu`)、開 PR、由承勳 review→approve→merge 進 `main`。

> 這一步沒有 conflict(只有一個人改),目的是讓 `main` 比 `v0.1.0` 多一款飲料。

---

## Part 3 — 家瑞:發佈 v0.2.0

`main` 現在比 `v0.1.0` 多了抹茶拿鐵。發佈第二個版本:
```bash
git checkout main
git pull --ff-only origin main
dotnet test

git checkout -b release/v0.2.0
git push -u origin release/v0.2.0

git tag -a v0.2.0 -m "Release v0.2.0"
git push origin v0.2.0

git checkout main
```
同樣到 GitHub 發 `v0.2.0` 的 Release。

✅ 現在**同時有兩條 release 線**:`release/v0.1.0`(較舊)和 `release/v0.2.0`(較新)。

---

## 確認成果
```bash
git fetch origin
git branch -a | grep release      # 應看到 release/v0.1.0 與 release/v0.2.0
git tag                           # 應看到 v0.1.0 與 v0.2.0
```
- `release/v0.1.0`:**沒有**抹茶拿鐵。
- `release/v0.2.0` 和 `main`:**有**抹茶拿鐵。
- 兩條線**都還有**那個空訂單 crash 的 bug(它在最初就存在)——第四關就要修它。

---

## 過關條件 (Definition of Done)
- [ ] GitHub 上有 `release/v0.1.0`、`release/v0.2.0` 兩條分支,和 `v0.1.0`、`v0.2.0` 兩個 tag。
- [ ] `release/v0.1.0` 不含抹茶拿鐵;`release/v0.2.0` 含。
- [ ] (建議)兩個版本都發了 GitHub Release。
- [ ] 兩條 release 線都設為受保護分支(顧問協助:Settings → Branches)。

➡️ 進入 [第四關:hotfix 與跨版本傳播](level-4-hotfix-propagation.md)。
