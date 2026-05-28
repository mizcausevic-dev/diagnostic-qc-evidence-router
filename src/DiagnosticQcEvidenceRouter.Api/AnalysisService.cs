namespace DiagnosticQcEvidenceRouter.Api;

public static class AnalysisService
{
    public static PostureReport Analyze(DiagnosticExport payload)
    {
        var findings = new List<DiagnosticFinding>();

        foreach (var snapshot in payload.Snapshots)
        {
            if (snapshot.SnapshotStatus == "STALE")
            {
                findings.Add(new DiagnosticFinding(
                    "stale-diagnostic-snapshot",
                    "medium",
                    snapshot.Name,
                    $"Snapshot \"{snapshot.Name}\" is stale and should be recollected before release confidence is asserted.",
                    snapshot.Owner
                ));
            }
        }

        foreach (var gap in payload.Gaps)
        {
            var code = gap.ControlFamily switch
            {
                "Evidence" => "evidence-packet-gap",
                "Calibration" => "stale-calibration-risk",
                "Review" => "qc-review-gap",
                "Telemetry" => "telemetry-replay-gap",
                "Variance" => "assay-variance-watch",
                "Release" => "release-packet-gap",
                _ => "diagnostic-gap"
            };

            findings.Add(new DiagnosticFinding(
                code,
                gap.Severity,
                gap.Subject,
                gap.ObservedState,
                ResolveOwner(gap.ControlFamily)
            ));

            if (gap.HoursOpen > 24)
            {
                findings.Add(new DiagnosticFinding(
                    "stale-gap-window",
                    gap.HoursOpen > 32 ? "medium" : "low",
                    gap.Subject,
                    $"Gap \"{gap.Subject}\" has remained open for {gap.HoursOpen} hours.",
                    ResolveOwner(gap.ControlFamily)
                ));
            }
        }

        var blocking = payload.Gaps.Count(g => g.BlocksRelease);
        var evidenceRisks = payload.Gaps.Count(g => g.ControlFamily is "Evidence" or "Telemetry" or "Calibration");
        var releaseRisks = payload.Gaps.Count(g => g.ControlFamily is "Review" or "Release" or "Variance");

        return new PostureReport(
            payload.Snapshots.Count,
            payload.Snapshots.Count(s => s.SnapshotStatus == "CURRENT"),
            payload.Gaps.Count,
            blocking,
            evidenceRisks,
            releaseRisks,
            findings,
            !findings.Any(f => f.Severity == "high")
        );
    }

    public static object Summary()
    {
        var report = Analyze(SampleData.Payload);

        return new
        {
            bundles = report.Snapshots,
            currentBundles = report.CurrentSnapshots,
            gaps = report.Gaps,
            blockingGaps = report.BlockingGaps,
            evidenceRisks = report.EvidenceRisks,
            releaseRisks = report.ReleaseRisks,
            recommendation = "Repair evidence continuity, refresh calibration, and close release attestations before assay publication."
        };
    }

    private static string ResolveOwner(string family) => family switch
    {
        "Evidence" => "Lab QA",
        "Calibration" => "Diagnostics Operations",
        "Review" => "Clinical Quality",
        "Telemetry" => "Lab QA",
        "Variance" => "Clinical Quality",
        "Release" => "Clinical Quality",
        _ => "Diagnostics Operations"
    };
}
