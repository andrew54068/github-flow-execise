# hit-drink-pos 🧋

> Hit 分支策略 + GitHub 流程的**動手練習 (hands-on exercise)** repo。
> 一個迷你的「飲料店計價 POS」C# 函式庫,用來練 **commit / branch / Pull Request / review** 的完整流程。

這個 repo 是設計來讓你**邊做邊學**的。程式本身很小、很好懂——重點不是寫扣,而是把 **Hit 的 git / GitHub 規範**走過一遍。

## 為什麼選這個題目

- **單一工具鏈**:測試用 `dotnet test`(xUnit),只要裝好 .NET SDK 就能跑。
- **程式小到一眼看完**:你才能把注意力放在 *git 流程* 而不是業務邏輯。
- **有故意種好的 (planted) 任務**:一個缺的 feature、一個 bug、一個已出貨的 crash——剛好對應三種分支類型。

## 先決條件 (prerequisites)

```bash
dotnet --version   # 需 .NET SDK 8.0 以上
git --version
gh --version       # GitHub CLI,用來開 PR;或你也可以在網頁上開 PR
```

> 還沒裝 .NET? 到 https://dotnet.microsoft.com/download 裝 SDK 8.0,或 macOS 上 `brew install dotnet-sdk`。

## 跑測試 (run tests)

```bash
dotnet test       # 還原套件、build,然後跑整個測試專案
```

一開始所有測試都應該是**綠的 (12 passing)**。你的任務會讓你*新增*一個一開始**紅的**測試,修好後再變綠。

## 程式結構

```
src/HitDrinkPos/
├── Pricing.cs    杯款基礎定價 (穩定核心,不用改)
├── Toppings.cs   加料計價 —— 🅰️ 承勳的 feature 任務 (目前是空殼 stub)
├── Discount.cs   滿額折扣 —— 🅱️ 家瑞的 bug 任務 (邊界 off-by-one)
└── Receipt.cs    訂單結帳 —— 🅲 進階 hotfix 任務 (空訂單會 crash)
test/HitDrinkPos.Tests/   對應每個 src 檔的 xUnit 測試
docs/tasks/               三份任務卡 (task A / B / C)
HitDrinkPos.sln           方案檔 (solution),含 library 與 test 兩個專案
```

## 你的任務卡

| 任務 | 負責人 | 分支類型 | 檔案 |
|---|---|---|---|
| 🅰️ 實作加料計價 | 承勳 | `feature/*` → `main` | [task-A-feature-toppings.md](docs/tasks/task-A-feature-toppings.md) |
| 🅱️ 修滿額折扣邊界 bug | 家瑞 | `bug/*` → `main` | [task-B-bug-discount.md](docs/tasks/task-B-bug-discount.md) |
| 🅲 修空訂單 crash (進階) | 兩人一起 | `hotfix/v1.1.0-*` → `release/v1.1.0` | [task-C-hotfix-empty-order.md](docs/tasks/task-C-hotfix-empty-order.md) |

## 怎麼開始

1. 讀 [EXERCISE.md](EXERCISE.md) —— 整場練習的流程地圖 + 速查表。
2. 打開你自己的任務卡,照著做。
3. 兩個 skill 已經放在 repo 裡,**Claude Code、Codex、Cursor 共用同一份**,工具會自動引用:
   - `hit-committer` —— 幫你做出 atomic、符合 Hit 格式的 commit。
   - `git-github-flow` —— 幫你選對 branch、base,開出正確的 PR。
   - 路徑與 symlink 設定見 [SKILLS.md](SKILLS.md)。

> 規範全文在主 repo 的 `docs/hit-branch-strategy-and-github-manual.md`。練習時不必全讀,卡片會帶你走到需要的段落。
