# EXERCISE.md — 練習流程地圖 🗺️

這份是整場 60 分鐘練習的**總導覽 + 速查表 (cheat sheet)**。先讀完這頁,再去做你的任務卡。

---

## 這場練習要達成什麼

走完你會親手做過一次 **Hit 完整流程**:

```
改扣 → hit-committer 做 commit → git-github-flow 開 PR → 互相 review → 修正 → Approve → 合併
                                                          ↑ GitHub review 迴圈
```

並且理解 **「不同性質的改動,要走不同的 branch」** ——這是 Hit 規範的核心。

---

## 角色分配 (roles)

兩人**同時開工**,各做各的任務,再**互相當對方的 reviewer**。

| 人 | 你的任務 | 你 review 誰 |
|---|---|---|
| **承勳** | 🅰️ feature:實作加料計價 | 去 review 家瑞的 🅱️ PR |
| **家瑞** | 🅱️ bug:修滿額折扣邊界 | 去 review 承勳的 🅰️ PR |

做完 A / B 後,如果還有時間,兩人**一起**做 🅲 進階 hotfix。

---

## 三階段時間軸 (約 60 分鐘)

| 時間 | 階段 | 你做什麼 |
|---|---|---|
| 0–10 分 | **暖身** | clone repo、`dotnet test` 看到 12 綠、讀自己的任務卡 |
| 10–35 分 | **第一輪:feature / bug** | 各自開 branch → 改扣 → commit → 開 PR(含測試紀錄) |
| 35–50 分 | **review 迴圈** | 互相 review → Request changes → 對方修 → Approve → 合併 |
| 50–60 分 | **第二輪:hotfix(進階)** | 一起走 `release/v1.1.0` 的 hotfix + backflow |

---

## 黃金法則:改動性質 → 分支 (branch decision)

這張表是 `git-github-flow` 的心臟。**先問「這是什麼性質的改動」,分支自然就決定了。**

| 改動是… | 分支 | base(PR 要合進哪) | commit type |
|---|---|---|---|
| 對外可見的**新功能** | `feature/<描述>` | `main` | `[feat]` |
| **非緊急** bug 修正 | `bug/<描述>` | `main` | `[fix]` |
| 純內部重構(行為不變) | `refactor/<描述>` | `main` | `[refactor]` |
| 只動測試 | `test/<描述>` | `main` | `[test]` |
| 文件 / 建置 / CI / 版號 | `chore/<描述>` | `main` | `[chore]`/`[docs]`… |
| **已出貨版本**的緊急修 | `hotfix/v<X.Y.Z>-<描述>` | `release/v<X.Y.Z>` | `[hotfix]` |

> ⚠️ **沒有 `docs/`、`style/`、`perf/` 分支**——這些都搭 `chore/`,但 commit 的 *type* 仍照實寫。

---

## commit 訊息格式 (hit-committer 的規則)

```
[type] 一句話講「為什麼 / 意圖」(英文,首字大寫,≤72 字元)

- 模組層級的「改了什麼」(不是逐行細節)
- 3–6 行最理想,最多 10 行
```

- **英文**;type 小寫:`[feat] [fix] [hotfix] [refactor] [perf] [docs] [test] [style] [chore]`。
- **標題 = 意圖 / 為什麼;內文 = 改了什麼**。不要寫 `Update X`、`Change Y` 這種廢話標題。
- **atomic**:一個 commit 只做一件邏輯上完整的事。

範例:

```
[feat] Charge for drink toppings at checkout

- Implement per-topping pricing (pearl/coconut +10, pudding +15).
- Reject unknown toppings with a clear error.
```

---

## 五大紀律(練習中你會碰到的)

1. **`main` 與 `release/*` 受保護**:不能直接 push,一律走 PR。
2. **hotfix 立刻傳播**:修在 release 線上,合併後**馬上**帶回 `main`,不要累積。
3. **衝突時 hotfix 內容為準**。
4. **短命分支要乾淨、測過再合**。
5. **每個 PR 都要附測試紀錄 (test record)** —— 沒附,reviewer 直接 Request changes。👈 練習中一定要做到。

---

## PR 內文模板(開 PR 時貼上,`git-github-flow` 會自動帶)

```markdown
## Summary
- <這個 PR 做了什麼、為什麼>

Closes #<issue 編號>

## Test record (Discipline 5 — required before merge)
- [ ] Build: <`dotnet build` 的輸出 / N/A — 原因>
- [ ] Unit tests: <`dotnet test` 的輸出 / N/A — 原因>
- [ ] Integration tests: <輸出 / N/A — 原因>
- [ ] Manual / on-device verification: <截圖 / 說明 / N/A — 原因>
```

> 第一輪只有單元測試,所以把 `dotnet test` 的結果貼進 **Unit tests**,其餘標 `N/A — 本練習無此項`。

---

## GitHub review 迴圈怎麼走

當對方的 reviewer 時:

1. 打開對方的 PR → **Files changed** 分頁。
2. 檢查三件事:**測試紀錄有沒有附**、**branch / base 對不對**、**commit 訊息符不符合格式**。
3. 至少在一行程式上留一則 **inline comment**(就算只是稱讚也好,練習留言操作)。
4. 故意先按 **Request changes**,寫清楚要對方改什麼。
5. 對方 push 修正後,再 **Approve**。
6. **Approve 後才由 Release Owner(顧問 / 你們其中一人)合併**。Developer 不自己合。

---

## 卡住時的求救順序

1. 先看你的任務卡 `docs/tasks/task-?.md`。
2. 問 Claude Code(skill 會自動引用):「我要開始 task A,請用 git-github-flow 帶我走」。
3. 還是卡 → 舉手問顧問。

開始吧 → 打開你的任務卡 🚀
