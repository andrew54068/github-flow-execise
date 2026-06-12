# EXERCISE.md — 練習流程地圖 🗺️

這份是整套練習的**總導覽 + 速查表 (cheat sheet)**。先讀完這頁,再去做各關的任務卡。

---

## 這套練習要達成什麼

四關循序漸進,走完你會親手做過 **Hit 完整流程**的每個重要場景:

| 關卡 | 主題 | 你會學到 |
|---|---|---|
| 🟢 [第一關](docs/tasks/level-1-feature-and-review.md) | feature / bug + review (happy path) | 改不同檔案、commit、PR、**GitHub review 迴圈** |
| 🟡 [第二關](docs/tasks/level-2-merge-conflict.md) | 兩人改同一檔 → **解 merge conflict** | 衝突標記怎麼讀、怎麼解、解完驗證 |
| 🟠 [第三關](docs/tasks/level-3-sequential-releases.md) | 依序**發佈兩個版本** v0.1.0 → v0.2.0 | release 分支 + tag、Release Owner 的角色 |
| 🔴 [第四關](docs/tasks/level-4-hotfix-propagation.md) | **hotfix + 跨版本傳播** | hotfix 修在哪條線、cherry-pick 傳到 main + 更新線 |

> 不限時、循序做。**每一關都要等上一關 merge 完才開始**(後面的關卡依賴前面的成果)。

---

## 角色 (roles)

兩人**同時開工、互相當 reviewer**。每關開頭都會講該關誰做什麼。

| 人 | GitHub 帳號 |
|---|---|
| **承勳** | (填你的帳號) |
| **家瑞** | (填你的帳號) |

- 第一、二關:兩人**平行**各做一條 PR,互相 review。
- 第三、四關:兩人**輪流**扮演 Release Owner / Developer,一起完成。
- review 後 **由開 PR 的人自己 merge**。

---

## 黃金法則:改動性質 → 分支 (branch decision)

這張表是 `git-github-flow` 的心臟。**先問「這是什麼性質的改動」,分支自然就決定了。**

| 改動是… | 分支 | base(PR 要合進哪) | commit type | 哪一關 |
|---|---|---|---|---|
| 對外可見的**新功能** | `feature/<描述>` | `main` | `[feat]` | 1, 2, 3 |
| **非緊急** bug 修正 | `bug/<描述>` | `main` | `[fix]` | 1 |
| 純內部重構(行為不變) | `refactor/<描述>` | `main` | `[refactor]` | — |
| 只動測試 | `test/<描述>` | `main` | `[test]` | — |
| 文件 / 建置 / CI / 版號 | `chore/<描述>` | `main` | `[chore]`/`[docs]`… | — |
| **已出貨版本**的緊急修 | `hotfix/v<X.Y.Z>-<描述>` | `release/v<X.Y.Z>` | `[hotfix]` | 4 |
| 把 hotfix **傳播**到別條線 | `port/v<X.Y.Z>-<描述>-to-<target>` | 該 target | `[hotfix]`(cherry-pick) | 4 |

> ⚠️ **沒有 `docs/`、`style/`、`perf/` 分支**——這些都搭 `chore/`,但 commit 的 *type* 仍照實寫。
> 發佈版本(切 `release/*` + 打 tag)是 **Release Owner** 的動作,不走 PR(見第三關)。

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
2. **hotfix 立刻傳播,不要累積**:修在最早受影響的線,合併後**馬上**傳到 main 及更新的線。(第四關)
3. **衝突時 hotfix 內容為準**。(第四關 cherry-pick)
4. **短命分支要乾淨、測過再合**。
5. **每個 PR 都要附測試紀錄 (test record)** —— 沒附,reviewer 直接 Request changes。👈 一定要做到。

---

## PR 內文模板(開 PR 時貼上,`git-github-flow` 會自動帶)

```markdown
## Summary
- <這個 PR 做了什麼、為什麼>

Closes #<issue 編號>

## Test record (Discipline 5 — required before merge)
- [ ] Build: <輸出 / N/A — 原因>
- [ ] Unit tests: <`dotnet test` 的輸出 / N/A — 原因>
- [ ] Integration tests: <輸出 / N/A — 原因>
- [ ] Manual / on-device verification: <截圖 / 說明 / N/A — 原因>
```

> 本練習只有單元測試,把 `dotnet test` 的結果貼進 **Unit tests**,其餘標 `N/A — 本練習無此項`。

---

## GitHub review 迴圈怎麼走

當對方的 reviewer 時:
1. 打開對方的 PR → **Files changed** 分頁。
2. 檢查三件事:**測試紀錄有沒有附**、**branch / base 對不對**、**commit 訊息符不符合格式**。
3. 至少在一行程式上留一則 **inline comment**(就算只是稱讚也好,練習留言操作)。
4. 故意先按 **Request changes**,寫清楚要對方改什麼。
5. 對方 push 修正後,再 **Approve**。
6. **Approve 後,由開 PR 的人自己負責 merge**。

---

## 跑測試

```bash
dotnet test          # 還原套件、build、跑整個 xUnit 測試專案
```
一開始 **12 passing**。每一關你會*新增*一個一開始**紅的**測試,修好後變綠。

---

## 卡住時的求救順序
1. 先看當關的任務卡 `docs/tasks/level-?.md`。
2. 問 Claude Code / Codex / Cursor(skill 會自動引用):「我要開始第 N 關,請用 git-github-flow 帶我走」。
3. 還是卡 → 舉手問顧問。

開始吧 → [第一關](docs/tasks/level-1-feature-and-review.md) 🚀
