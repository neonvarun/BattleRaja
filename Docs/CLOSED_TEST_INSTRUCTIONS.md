# Closed-Test Instructions (Draft)

## Scope

This draft is for invited local/closed testing only. It does not authorize public release,
store submission or online service access.

## Android

1. Install the exact APK supplied by the owner on the connected Lava device or an explicitly
   approved tester device.
2. Launch the app, complete one movement/combat/offline-match smoke, background/resume it,
   and record any crash, visual issue or input failure.
3. Do not enter real credentials or send private data in logs. Attach build label and device
   model to feedback.

## Web

1. Serve `Builds/M11/Web` over HTTP; do not open it with `file://`.
2. Test Chrome and Edge desktop startup, focus loss/return, refresh and a short offline lab
   session. Mobile Web, Safari and Firefox are not covered by the available tooling.

## Feedback template

- Build label / platform / device or browser:
- Scene and steps to reproduce:
- Expected vs observed:
- Frequency and severity:
- Sanitized log excerpt (no tokens or personal data):
- Screenshot/video if approved:
