# 團隊共用 Skills 設定 (shared skills)

這個 repo 內建兩個 Hit 工作流程 skill,讓團隊在 **Claude Code、Codex、Cursor** 都能用到**同一份**:

| Skill | 用途 |
|---|---|
| `hit-committer` | 產生 atomic、符合 Hit 格式的 commit (`[type] Description`)。 |
| `git-github-flow` | 選對 branch / base、開出正確的 PR,處理 hotfix 與 backflow。 |

## 為什麼是「一份來源 + symlink」

不同工具讀 skill 的資料夾不一樣:

| 工具 | 讀取路徑 (專案層級) |
|---|---|
| **Claude Code** | `.claude/skills/` |
| **OpenAI Codex** | `.agents/skills/` |
| **Cursor** | `.agents/skills/`、`.cursor/skills/`,**也相容讀** `.claude/skills/` |

為了避免維護多份、改了一邊忘了另一邊,本 repo 採:

```
.claude/skills/          ← ✅ 唯一來源 (source of truth,真正的檔案)
├── hit-committer/
└── git-github-flow/

.agents/skills  ->  ../.claude/skills   ← 🔗 相對 symlink,指向上面那份
```

- **改 skill 時,只改 `.claude/skills/` 底下的檔案。** `.agents/skills` 是 symlink,會自動跟著變。
- Claude Code 與 Cursor 直接讀 `.claude/skills/`;Codex 透過 `.agents/skills`(→ 同一份)讀到。
- symlink 用**相對路徑**,所以 `git clone` 到別台機器後仍然有效。

## 確認 symlink 正常

```bash
readlink .agents/skills          # 應印出: ../.claude/skills
ls .agents/skills                # 應印出: git-github-flow  hit-committer
```

若 `ls .agents/skills` 是空的或報錯,通常是 clone 時 symlink 沒被還原:

```bash
git config core.symlinks true
git checkout -- .agents/skills   # 重新還原 symlink
```

> ⚠️ **Windows 注意**:Windows 預設不一定會把 git symlink 還原成連結。若團隊有人用 Windows,
> 請開啟 Git 的 symlink 支援(`git config --global core.symlinks true`,且 Developer Mode 開啟),
> 或改在 WSL 下作業。Mac / Linux 不受影響。

## 怎麼用

在 Claude Code / Codex / Cursor 裡,直接用自然語言觸發即可,例如:
- 「幫我用 hit-committer 把這些改動 commit 起來」
- 「我要開始 task A,請用 git-github-flow 帶我走 feature 流程」

工具會依 skill 的 description 自動判斷何時引用。
