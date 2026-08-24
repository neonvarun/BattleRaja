# Privacy and Data-Safety Worksheet — Draft

No legal compliance or store declaration is asserted. Complete this worksheet with counsel
and the service owners before publication.

## Current checkpoint

- No Photon or PlayFab SDK is installed.
- No account, analytics, crash-reporting or advertising service is active.
- `ProjectSettings/ProjectSettings.asset` has `submitAnalytics: 0` and
  `ProjectSettings/UnityConnectSettings.asset` disables Unity Connect, Unity
  Analytics, Ads and Performance Reporting for the offline candidate.
- Development analytics is an in-memory bounded sink only; it records event name, build,
  platform and match duration, not identity, tokens or free-form text.
- No purchases or premium-currency grants are implemented.
- PlayerPrefs stores only local settings, selected fighter and tutorial-completion state;
  no server copy or account identity is created by the V1 offline flow.

## Proposed V1 Data Safety facts — pending signed-artifact review

These are source/configuration findings for the offline candidate, not a completed Play
Console declaration. Recheck the final signed AAB, bundled SDKs and release configuration
before publication:

- Data collected by the V1 app: none intended.
- Data shared with third parties: none intended.
- Network access: not required; the exact candidate manifest gate rejects
  `INTERNET` and `ACCESS_NETWORK_STATE`.
- Local-only storage: settings, selected fighter and tutorial completion in PlayerPrefs.
- Account creation, authentication, payments, ads, analytics upload, crash upload and
  cloud progression: not present in the V1 scope.
- User deletion: no account or server record exists; local data is removed by the device's
  normal app-data clear/uninstall controls. The owner must provide the final privacy-policy
  URL and any jurisdiction-specific wording.

## Questions requiring approval

- What account identifiers, device identifiers, diagnostics and retention periods will the
  approved services collect?
- Which consent, deletion, recovery and cross-platform linking flows are required in each
  target jurisdiction?
- What privacy policy, terms, child-safety and data-safety disclosures are legally required?
- Who may access server logs, and how are tokens/PII redacted and deleted?
- What support contact and privacy-policy URL will the owner publish with the Play listing?
