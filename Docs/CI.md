# CI and validation design

## Active workflow

`.github/workflows/repository-validation.yml` runs on every branch push, pull request
and manual dispatch. It uses a Windows runner because the repository's authoritative
validation script is PowerShell and uses Windows Unity path conventions. The workflow:

1. checks out Git LFS objects;
2. runs `Tools/Validation/validate.ps1` without requiring a Unity license;
3. checks whitespace and LFS pointer integrity; and
4. scans tracked source/configuration for obvious forbidden secret assignments.

The workflow has read-only repository permissions and does not receive credentials.
It must not be interpreted as Unity compilation, EditMode/PlayMode, Android, Web,
multiplayer, backend or release-signing evidence.

## Manual Unity gate design

The Unity tests/builds remain a separate owner-approved gate because batchmode Unity
licensing and any hosted-runner secret delivery require explicit authorization. When
approved, add a protected manual workflow using the exact editor `6000.5.6f1` and
owner-managed secret storage. Its required jobs are:

- repository validation;
- EditMode and PlayMode test XML upload;
- content/scene validation;
- Android development APK build and artifact hash; and
- Web development build, local HTTP smoke and artifact retention.

Unity license values, Photon App IDs/secrets, PlayFab credentials and signing keys must
be injected only as protected secrets or files outside the repository. They must never
be echoed, archived in logs, or included in Web/Android artifacts. Public deployment,
store submission and production signing remain separate human-approval gates.

## Local equivalents

```powershell
pwsh -File Tools/Validation/validate.ps1 -ProjectRoot .
git diff --check
git lfs fsck --pointers
```

The exact Unity, Android and Web commands remain documented in the build runbooks and
the current evidence reports; local green checks do not replace physical Lava or
multi-browser runtime evidence.

## Repository validation repair — 2026-09-04

Run [`33816621445`](https://github.com/neonvarun/BattleRaja/actions/runs/33816621445)
failed only in the tracked-source secret scan. The original expression used the PCRE
inline `(?i)` prefix with `git grep -E`, which uses POSIX ERE, and the Windows PowerShell
runner treated the clean no-match exit code `1` as a terminating native-command failure.
The workflow now uses a portable case-insensitive POSIX pattern, captures and interprets
the native exit code explicitly, and uses `actions/checkout@v5` for the Node 24-compatible
checkout runtime. The repair was kept as focused commits `e728ef8`, `1170fe1` and
`840097b`.

The final tutorial source commit `a7ea3ce` passed
[`33836993117`](https://github.com/neonvarun/BattleRaja/actions/runs/33836993117): the
checkout, repository contract, whitespace/LFS and forbidden-secret checks all completed
successfully. This workflow remains a repository-integrity gate; it is not Unity, Android,
Web, replay, performance or store-submission evidence.
