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
