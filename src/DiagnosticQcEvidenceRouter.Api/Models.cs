namespace DiagnosticQcEvidenceRouter.Api;

public sealed record DiagnosticSnapshot(
    string Id,
    string Name,
    string LabLane,
    string Status,
    string SnapshotStatus,
    string Owner,
    int OpenRuns,
    int BlockedSamples,
    DateTimeOffset CollectedAt
);

public sealed record QcGap(
    string Id,
    string SnapshotId,
    string ControlFamily,
    string Severity,
    string Subject,
    string ExpectedState,
    string ObservedState,
    int HoursOpen,
    bool BlocksRelease
);

public sealed record LanePacket(
    string Id,
    string Lane,
    string Owner,
    string Status,
    string Focus,
    string NextAction,
    string Note
);

public sealed record ReviewPacket(
    string PacketId,
    string Lane,
    string Owner,
    string Status,
    int CompletenessScore,
    string Blocker,
    string DecisionNote,
    int LaunchWindowHours
);

public sealed record DiagnosticExport(
    IReadOnlyList<DiagnosticSnapshot> Snapshots,
    IReadOnlyList<QcGap> Gaps
);

public sealed record DiagnosticFinding(
    string Code,
    string Severity,
    string Subject,
    string Message,
    string Owner
);

public sealed record PostureReport(
    int Snapshots,
    int CurrentSnapshots,
    int Gaps,
    int BlockingGaps,
    int EvidenceRisks,
    int ReleaseRisks,
    IReadOnlyList<DiagnosticFinding> Findings,
    bool Ok
);
