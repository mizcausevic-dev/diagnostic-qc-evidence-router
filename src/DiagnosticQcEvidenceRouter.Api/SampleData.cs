namespace DiagnosticQcEvidenceRouter.Api;

public static class SampleData
{
    public static readonly DiagnosticExport Payload = new(
        Snapshots:
        [
            new(
                "diag-core",
                "Central diagnostics QC snapshot",
                "Core assay lane",
                "WATCH",
                "CURRENT",
                "Lab QA",
                12,
                4,
                DateTimeOffset.Parse("2026-05-28T14:00:00Z")
            ),
            new(
                "diag-release",
                "Release assay evidence snapshot",
                "Release readiness lane",
                "CRITICAL",
                "STALE",
                "Diagnostics Operations",
                7,
                3,
                DateTimeOffset.Parse("2026-05-26T08:30:00Z")
            )
        ],
        Gaps:
        [
            new(
                "gap-chain-of-custody",
                "diag-core",
                "Evidence",
                "high",
                "Specimen chain-of-custody packet",
                "All diagnostic samples retain signed evidence packets through release review.",
                "One batch is missing signed custody evidence after a handoff between intake and assay verification.",
                18,
                true
            ),
            new(
                "gap-instrument-calibration",
                "diag-release",
                "Calibration",
                "high",
                "PCR-07 calibration file",
                "Release lane instruments retain current calibration artifacts before assay sign-off.",
                "The current run depends on a stale calibration artifact outside the approved review window.",
                34,
                true
            ),
            new(
                "gap-qc-review",
                "diag-release",
                "Review",
                "high",
                "Quarterly assay QC review",
                "High-sensitivity diagnostics remain inside an active QC review cadence.",
                "The latest review packet is incomplete and release sign-off is still pending.",
                26,
                true
            ),
            new(
                "gap-lims-telemetry",
                "diag-core",
                "Telemetry",
                "medium",
                "LIMS export continuity",
                "Run telemetry and QC evidence exports remain replayable for every assay lane.",
                "Two export windows are missing from the evidence trail, weakening audit continuity.",
                29,
                false
            ),
            new(
                "gap-reagent-variance",
                "diag-core",
                "Variance",
                "medium",
                "Reagent lot variance",
                "Variance stays inside the accepted envelope before routed release review.",
                "A reagent lot drifted beyond the expected range and needs secondary validation.",
                12,
                false
            ),
            new(
                "gap-release-packet",
                "diag-release",
                "Release",
                "high",
                "Assay release packet",
                "Release packets stay complete before result publication.",
                "The release packet is missing one reviewer attestation and one supporting evidence link.",
                16,
                true
            )
        ]
    );

    public static readonly IReadOnlyList<LanePacket> QcLane =
    [
        new(
            "evidence-spine",
            "Evidence routing lane",
            "Lab QA",
            "red",
            "Chain-of-custody packets, LIMS replay, and assay evidence continuity",
            "Restore the missing custody packet and replay missing LIMS exports before release review.",
            "Evidence continuity is not strong enough for a clean release decision."
        ),
        new(
            "instrument-readiness",
            "Instrument readiness lane",
            "Diagnostics Operations",
            "red",
            "Calibration freshness, run health, and instrument release confidence",
            "Replace the stale calibration artifact and rerun the release dependency check.",
            "One stale calibration file is blocking a credible release posture."
        ),
        new(
            "variance-watch",
            "Variance review lane",
            "Lab QA",
            "yellow",
            "Reagent variance, assay drift, and second-pass validation triggers",
            "Route the affected reagent lot into secondary validation before sign-off.",
            "Variance is recoverable if the second-pass review happens inside the next window."
        ),
        new(
            "release-board",
            "Release readiness lane",
            "Clinical Quality",
            "red",
            "Review completeness, reviewer attestations, and release packet confidence",
            "Close the missing attestation and evidence-link gaps before publication.",
            "The release packet is still incomplete."
        )
    ];

    public static readonly IReadOnlyList<ReviewPacket> ReleasePackets =
    [
        new(
            "DQC-12",
            "Central diagnostics release",
            "Clinical Quality",
            "red",
            58,
            "Chain-of-custody evidence and one attestation are still missing.",
            "Do not release until the evidence spine and attestation chain are complete.",
            8
        ),
        new(
            "DQC-18",
            "Instrument recalibration checkpoint",
            "Diagnostics Operations",
            "red",
            63,
            "The current run is still anchored to a stale calibration artifact.",
            "Block release and reroute this lane through instrument readiness review.",
            10
        ),
        new(
            "DQC-21",
            "Variance review packet",
            "Lab QA",
            "yellow",
            74,
            "Secondary validation has not closed on the reagent variance packet.",
            "Release can recover if variance validation closes in the next cycle.",
            16
        ),
        new(
            "DQC-27",
            "Audit continuity replay",
            "Clinical Quality",
            "yellow",
            71,
            "Two missing telemetry windows need replay before the next evidence export.",
            "Audit posture is recoverable if replay completes before the next checkpoint.",
            20
        )
    ];
}
