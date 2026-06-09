# diagnostic-qc-evidence-router

Biotech / diagnostics operator surface in C# for routing assay QC evidence, calibration drift, telemetry replay gaps, and release-signoff pressure into one readable control plane.

## Why this matters

Diagnostics teams do not need another vague compliance landing page. They need a board that keeps evidence continuity, calibration freshness, telemetry coverage, variance review, and release readiness visible together before weak packets reach publication or downstream clinicians.

This repo is the public proof surface for that pattern:

- `Hosted preview planned` for a browser-based diagnostics evidence router
- `Embedded by engagement` for teams that need the routing model inside a regulated lab or assay workflow

## Product depth

Diagnostic QC Evidence Router turns assay evidence into a release-confidence operating packet. It gives lab QA, diagnostics operations, clinical quality, and release stakeholders one shared view of QC evidence before custody gaps, stale calibration, telemetry breaks, reagent variance, or missing attestations weaken result publication decisions.

- **For executives:** shows where diagnostic release exposure is building and which assay lanes need attention before publication.
- **For operators:** maps every QC evidence gap to owner, instrument or assay lane, release impact, and next repair step.
- **For technical reviewers:** ships a C# analyzer, minimal API, structured JSON endpoints, static proof pages, synthetic fixtures, screenshots, and CI verification.

## What these repos have in common

This is part of the Kinetic Gain control-plane pattern: narrow operating problems packaged as buyer-readable product surfaces with evidence, data contracts, verification routes, screenshots, and deployment metadata.

- named business lane with a board question, operating owner, and remediation motion
- offline-safe sample data so the surface can prove value without exposing patient, assay, customer, credential, or production system data
- public page, API routes, analyzer path, README, screenshots, and CI checks that support a real diligence trail

## Operating workflow

1. Ingest synthetic diagnostics snapshots, QC evidence gaps, and release packets.
2. Score release-blocking gaps across evidence continuity, calibration, telemetry, variance, review, and release attestation.
3. Route each gap to the owner who can repair it before publication or downstream clinical handoff.
4. Publish a static operator page and structured JSON endpoints that non-technical and technical reviewers can inspect.

## What it includes

- ASP.NET Core minimal API in C#
- synthetic diagnostics snapshots, QC gaps, and release packets
- operator surfaces for:
  - `/qc-lane`
  - `/evidence-routing`
  - `/release-posture`
  - `/verification`
  - `/docs`
- structured JSON endpoints under `/api/*`
- static Pages export with `robots.txt`, `sitemap.xml`, and `CNAME`

## Screenshots

![Overview](./screenshots/01-overview.svg)
![QC lane](./screenshots/02-qc-lane.svg)
![Release posture](./screenshots/03-release-posture.svg)

## Verification

- synthetic diagnostics and assay evidence only
- no patient, clinician, or lab secrets
- no claim of HIPAA, CLIA, CAP, or FDA compliance
- this is a control-plane proof surface for workflow depth, not a compliance certification claim

## Local run

```powershell
dotnet test
dotnet run --project src/DiagnosticQcEvidenceRouter.Api -- --demo
dotnet run --project src/DiagnosticQcEvidenceRouter.Api
```

Then open:

- `http://127.0.0.1:5087/`
- `http://127.0.0.1:5087/qc-lane`
- `http://127.0.0.1:5087/evidence-routing`
- `http://127.0.0.1:5087/release-posture`

## Render static site

```powershell
dotnet run --project src/DiagnosticQcEvidenceRouter.Api -- --prerender
```

## Related docs

- [Embedded framing](./docs/KINETIC_GAIN_EMBEDDED.md)
- [Origin story](./docs/ORIGIN.md)
