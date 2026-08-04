# Security and Privacy

## Baseline

- No secrets in Git or client builds
- External credentials supplied through approved secret stores
- Do not trust client-reported combat, rewards or results
- Minimise personal data
- Redact tokens and sensitive values from logs
- Track third-party SDK data collection and licences
- Threat-model accounts, economy and match-result abuse before online release

This document must be expanded before Photon, PlayFab, analytics or store submission.

## CI hygiene

- `.github/workflows/repository-validation.yml` runs read-only repository, LFS,
  whitespace and source-secret checks without Unity, Photon, PlayFab or signing
  credentials.
- Unity test/build jobs are intentionally manual and require owner-approved protected
  secrets; no license, App ID, backend secret or signing key is stored in GitHub
  workflow text or artifacts.

## Web security

- Assume browser code/configuration are downloadable and inspectable.
- Never embed trusted secrets in WebAssembly, JavaScript or static hosting files.
- Use HTTPS and tested hosting headers.
- Treat browser storage as clearable and untrusted.
- Validate account linking, rewards, purchases and match results on trusted services.
