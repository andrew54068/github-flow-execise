# 🟡 第二關 —— 兩人改同一份檔案,解 merge conflict

> **目標**:故意製造一次 **merge conflict**,然後學會冷靜地解掉它。
> 這一關兩人都改 **`Pricing.cs` 的同一個位置**,所以**第二個 merge 的人一定會撞到衝突**——這正是重點。

## 情境

老闆要新增兩款飲料。兩人**各自**負責一款,但都要加在 `Pricing.cs` 的 `BasePriceTable`**同一個位置**(檔案裡有 `👇 第二關` 註解標記)。

| 人 | 新增杯款 | key | 價格 | 分支 |
|---|---|---|---|---|
| **承勳** | 烏龍茶 | `oolongTea` | 35 | `feature/drink-oolong` |
| **家瑞** | 美式咖啡 | `americano` | 45 | `feature/drink-americano` |

> ⚠️ **先講好誰先 merge**。先 merge 的人很順;**後 merge 的人會遇到 conflict**,那個人負責解。建議讓兩人都體驗一次解 conflict(做兩輪,或這輪換手)。

---

## 步驟

### 1. 各自從**最新的 main** 開分支
第一關的 PR 都 merge 後,`main` 已經更新,先同步:
```bash
git checkout main && git pull --ff-only origin main
git checkout -b feature/drink-oolong      # 家瑞:feature/drink-americano
```

### 2. 在標記處加你的杯款
打開 `Pricing.cs`,找到:
```csharp
        ["latte"] = 65,    // 拿鐵
        // 👇 第二關:把你的新杯款加在這一行下面 (兩人都加在這裡 → 製造 conflict)
    };
```
承勳加:
```csharp
        ["oolongTea"] = 35, // 烏龍茶
```
家瑞加:
```csharp
        ["americano"] = 45, // 美式咖啡
```
兩人都加在**同一行底下**——這就是衝突的來源。

### 3. commit + 開 PR(照第一關的流程)
- 分支類型是 `feature/*`、base `main`、commit type `[feat]`。
- commit 範例:`[feat] Add oolong tea to the base price menu`。
- 兩人各自開 PR,互相 review。

### 4. 第一個 PR:正常 merge ✅
先講好的那位先 review→approve→**merge**。`main` 現在有了第一款新飲料。

### 5. 第二個 PR:會出現 conflict ⚠️
GitHub 會在第二個 PR 顯示 **"This branch has conflicts that must be resolved"**。
因為兩人改了同一行區域。**由開這個 PR 的人在本機解**:

```bash
# 在你的分支上 (例如 feature/drink-americano)
git fetch origin
git checkout feature/drink-americano
git merge origin/main          # 把已更新的 main 併進來 → 這時跳出 conflict
```

打開 `Pricing.cs`,你會看到衝突標記:
```
<<<<<<< HEAD
        ["americano"] = 45, // 美式咖啡
=======
        ["oolongTea"] = 35, // 烏龍茶
>>>>>>> origin/main
```

**解法:兩款都要保留**(這不是二選一,是兩款新飲料都要)。把它編輯成:
```csharp
        ["latte"] = 65,    // 拿鐵
        ["oolongTea"] = 35, // 烏龍茶
        ["americano"] = 45, // 美式咖啡
        // 👇 第二關:...
    };
```
把 `<<<<<<<`、`=======`、`>>>>>>>` 三行**全部刪掉**,只留乾淨的程式。

### 6. 完成 merge、驗證、push
```bash
dotnet test                    # 確認還是綠的
git add src/HitDrinkPos/Pricing.cs
git commit                     # 產生一個 merge commit (訊息保留預設即可)
git push origin feature/drink-americano
```
回到 GitHub,PR 的 conflict 消失了 → review → approve → merge。

---

## 解 conflict 的心法(顧問會強調)
- conflict **不是錯誤,是 git 在問你「這兩個改動我不知道怎麼合,你決定」**。
- `<<<<<<< HEAD` 到 `=======` 是**你的**改動;`=======` 到 `>>>>>>>` 是**對方(main)的**。
- 大多數情況**兩邊都要保留**(像這關);有時要選一邊;偶爾要重寫。看語意,不要無腦選。
- 解完一定要 `dotnet test` 再 push。

> 💡 也可以對工具說:「我 merge main 之後 Pricing.cs 有 conflict,幫我解,兩款飲料都要保留」,讓它帶你走。

---

## 過關條件 (Definition of Done)
- [ ] 兩款新飲料 `oolongTea`、`americano` 最後都在 `main` 的 `BasePriceTable` 裡。
- [ ] 後 merge 的人**親手解過一次 conflict**(看過衝突標記、刪掉、留兩款)。
- [ ] 解完 `dotnet test` 仍全綠。
- [ ] 兩條 PR 都 merge 進 `main`。

➡️ 進入 [第三關:依序發佈兩個版本](level-3-sequential-releases.md)。
