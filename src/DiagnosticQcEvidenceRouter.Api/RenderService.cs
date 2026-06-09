using System.Text;
using System.Text.Json;

namespace DiagnosticQcEvidenceRouter.Api;

public static class RenderService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static string Overview() => Layout(
        "Diagnostic QC Evidence Router",
        "/",
        $$"""
        <section class="section">
          <div class="sh"><h2>Operator Snapshot</h2><div class="note">diagnostics · qc · evidence routing</div></div>
          <div class="kpis">
            {{Metric("2", "diagnostic snapshots", "Synthetic diagnostics and release-readiness bundles across active assay lanes.", "cyan")}}
            {{Metric("1", "current bundles", "Only one bundle is fresh enough to trust without re-collection.", "green")}}
            {{Metric("6", "gaps", "Control deviations across evidence packets, calibration, telemetry, variance, and release review.", "plum")}}
            {{Metric("4", "blocking gaps", "Release-blocking issues still need closure before result publication.", "red")}}
            {{Metric("4", "evidence risks", "Evidence and telemetry continuity still need replay or repair.", "amber")}}
            {{Metric("2", "release risks", "Review and release packet posture is still incomplete.", "red")}}
          </div>
        </section>
        <section class="section">
          <div class="sh"><h2>Why this lane matters</h2><div class="note">biotech / diagnostics / csharp</div></div>
          <div class="stack">
            <div class="src"><div class="src-name">release confidence</div><div class="src-tit">Stop weak evidence from reaching assay publication</div><p>Diagnostics teams need one board where evidence continuity, calibration freshness, variance review, and release attestations stay readable together.</p></div>
            <div class="src"><div class="src-name">founder edge</div><div class="src-tit">Enterprise workflow depth, not biotech keyword stuffing</div><p>This fits the Kinetic Gain pattern: routing, evidence, approvals, and operator-safe remediation posture.</p></div>
            <div class="src"><div class="src-name">monetization path</div><div class="src-tit">Hosted preview planned · Embedded by engagement</div><p>The free surface shows the operator model; the commercial path is an embedded evidence-routing module for regulated lab workflows.</p></div>
          </div>
        </section>
        {{ProductDepthSection()}}
        <section class="section">
          <div class="sh"><h2>Board questions this answers</h2><div class="note">release exposure · evidence cost · lab investment</div></div>
          <div class="stack">
            <div class="src"><div class="src-name">exposure</div><div class="src-tit">Which assay releases are exposed to weak QC evidence?</div><p>Evidence continuity, calibration freshness, telemetry replay, variance review, and final release posture stay visible before result publication confidence is asserted.</p></div>
            <div class="src"><div class="src-name">savings</div><div class="src-tit">Where is the lab paying a manual evidence tax?</div><p>The router collapses calibration artifacts, instrument telemetry, variance notes, and release attestations into one packet so QA does not reconstruct proof across systems.</p></div>
            <div class="src"><div class="src-name">investment</div><div class="src-tit">Which diagnostics control should be strengthened first?</div><p>Blocking evidence gaps show whether calibration capture, telemetry replay, variance review, or release signoff deserves the next instrumentation pass.</p></div>
          </div>
        </section>
        <section class="section">
          <div class="sh"><h2>Evidence model</h2><div class="note">signal · proof · decision</div></div>
          <table class="ttbl">
            <thead><tr><th>Signal</th><th>Owner</th><th>Required proof</th><th>Decision supported</th></tr></thead>
            <tbody>
              <tr><td><b>Calibration freshness</b></td><td>Diagnostics Operations</td><td>Instrument ID, calibration window, operator, variance note</td><td>Release, retest, or block assay output</td></tr>
              <tr><td><b>Telemetry continuity</b></td><td>Lab QA</td><td>Run log, missing sample window, replay status, repair owner</td><td>Trust, replay, or quarantine evidence</td></tr>
              <tr><td><b>Release attestation</b></td><td>Clinical Quality</td><td>QC review, variance disposition, approver, publication memo</td><td>Publish result or require release-packet repair</td></tr>
            </tbody>
          </table>
        </section>
        """
    );

    public static string QcLane() => Layout(
        "Diagnostic QC Evidence Router — QC Lane",
        "/qc-lane",
        $$"""
        <section class="section">
          <div class="sh"><h2>QC Lane</h2><div class="note">owner · focus · next action</div></div>
          <table class="ttbl">
            <thead><tr><th>Lane</th><th>Owner</th><th>Status</th><th>Focus</th><th>Next action</th></tr></thead>
            <tbody>
              {{string.Join("", SampleData.QcLane.Select(lane => $$"""
                <tr>
                  <td><b>{{lane.Lane}}</b><br />{{lane.Note}}</td>
                  <td>{{lane.Owner}}</td>
                  <td><span class="st {{SeverityClass(lane.Status)}}">{{lane.Status}}</span></td>
                  <td>{{lane.Focus}}</td>
                  <td>{{lane.NextAction}}</td>
                </tr>
              """))}}
            </tbody>
          </table>
        </section>
        """
    );

    public static string EvidenceRouting() => Layout(
        "Diagnostic QC Evidence Router — Evidence Routing",
        "/evidence-routing",
        $$"""
        <section class="section">
          <div class="sh"><h2>Evidence Routing</h2><div class="note">severity · owner · subject</div></div>
          <table class="ttbl">
            <thead><tr><th>Risk</th><th>Owner</th><th>Subject</th><th>Observed state</th></tr></thead>
            <tbody>
              {{string.Join("", SampleData.Payload.Gaps.Select(gap => $$"""
                <tr>
                  <td><span class="st {{SeverityClass(gap.Severity)}}">{{gap.Severity}}</span><br /><b>{{gap.ControlFamily}}</b></td>
                  <td>{{OwnerForGap(gap.ControlFamily)}}</td>
                  <td>{{gap.Subject}}</td>
                  <td>{{gap.ObservedState}}</td>
                </tr>
              """))}}
            </tbody>
          </table>
        </section>
        """
    );

    public static string ReleasePosture() => Layout(
        "Diagnostic QC Evidence Router — Release Posture",
        "/release-posture",
        $$"""
        <section class="section">
          <div class="sh"><h2>Release Posture</h2><div class="note">packet readiness · blocker · timing</div></div>
          <div class="board">
            {{string.Join("", SampleData.ReleasePackets.Select(packet => $$"""
              <article class="pcard">
                <div class="ptop">
                  <div class="pnum">{{packet.CompletenessScore}}%</div>
                  <div class="ppri">{{packet.Owner}}</div>
                </div>
                <h3>{{packet.Lane}}</h3>
                <p class="pdesc">{{packet.DecisionNote}}</p>
                <ul class="check">
                  <li>{{packet.Blocker}}</li>
                  <li>{{packet.LaunchWindowHours}} hours to the next release checkpoint</li>
                  <li>Status: <span class="st {{SeverityClass(packet.Status)}}">{{packet.Status}}</span></li>
                </ul>
                <div class="pfoot"><code>{{packet.PacketId}}</code></div>
              </article>
            """))}}
          </div>
        </section>
        """
    );

    public static string Verification() => Layout(
        "Diagnostic QC Evidence Router — Verification",
        "/verification",
        $$"""
        <section class="section">
          <div class="sh"><h2>Verification</h2><div class="note">operator-safe claims only</div></div>
          <div class="stack">
            {{VerificationCard("This repo uses synthetic diagnostics and QC evidence only; no live patient, lab, or assay data is published.")}}
            {{VerificationCard("The control plane is rooted in evidence routing, calibration posture, telemetry replay, and release-signoff mechanics.")}}
            {{VerificationCard("This is a biotech / diagnostics operator surface, not a compliance-overclaim page.")}}
            {{VerificationCard("Hosted preview is planned; embedded module delivery is available by engagement.")}}
          </div>
        </section>
        """
    );

    public static string Docs() => Layout(
        "Diagnostic QC Evidence Router — Docs",
        "/docs",
        $$"""
        <section class="section">
          <div class="sh"><h2>Docs</h2><div class="note">routes · runbook · api</div></div>
          <div class="stack">
            <div class="src"><div class="src-name">routes</div><div class="src-tit">Public proof surface</div><p><code>/</code>, <code>/qc-lane</code>, <code>/evidence-routing</code>, <code>/release-posture</code>, <code>/verification</code>, <code>/docs</code></p></div>
            <div class="src"><div class="src-name">api</div><div class="src-tit">Structured payloads</div><p><code>/api/dashboard/summary</code>, <code>/api/qc-lane</code>, <code>/api/evidence-routing</code>, <code>/api/release-posture</code>, <code>/api/verification</code>, <code>/api/sample</code></p></div>
            <div class="src"><div class="src-name">runbook</div><div class="src-tit">Local execution</div><p><code>dotnet run --project src/DiagnosticQcEvidenceRouter.Api -- --demo</code> prints the same QC posture used by the public proof surface.</p></div>
          </div>
        </section>
        """
    );

    public static string Sample() => JsonSerializer.Serialize(new
    {
        summary = AnalysisService.Summary(),
        qcLane = SampleData.QcLane,
        evidenceRouting = SampleData.Payload.Gaps,
        releasePosture = SampleData.ReleasePackets,
        sample = SampleData.Payload
    }, JsonOptions);

    private static string VerificationCard(string title) =>
        $$"""<div class="src"><div class="src-name">verification</div><div class="src-tit">{{title}}</div><p>This lane keeps evidence, release pressure, and commercial framing honest.</p></div>""";

    private static string ProductDepthSection() =>
        """
        <section class="section">
          <div class="sh"><h2>Product depth</h2><div class="note">go-to-market · value architecture · proof</div></div>
          <div class="depth-grid">
            <article class="depth-card">
              <div class="eyebrow">What this product does</div>
              <h3>Diagnostic QC Evidence Router turns assay evidence into a release-confidence operating packet.</h3>
              <p>It gives lab QA, diagnostics operations, clinical quality, and release stakeholders one shared view of QC evidence before custody gaps, stale calibration, telemetry breaks, reagent variance, or missing attestations weaken result publication decisions.</p>
              <ul>
                <li><strong>For executives:</strong> shows where diagnostic release exposure is building and which assay lanes need attention before publication.</li>
                <li><strong>For operators:</strong> maps every QC evidence gap to owner, instrument or assay lane, release impact, and next repair step.</li>
                <li><strong>For technical reviewers:</strong> ships a C# analyzer, minimal API, structured JSON endpoints, static proof pages, synthetic fixtures, screenshots, and CI verification.</li>
              </ul>
            </article>
            <article class="depth-card">
              <div class="eyebrow">What these repos have in common</div>
              <h3>A repeatable Kinetic Gain control-plane pattern, not a one-off diagnostics landing page.</h3>
              <p>Each repo packages a narrow operating problem as a buyer-readable product surface with evidence, data contracts, verification routes, screenshots, and deployment metadata.</p>
              <ul>
                <li>Named business lane with a board question, operating owner, and remediation motion.</li>
                <li>Offline-safe sample data so the surface can prove value without exposing patient, assay, customer, credential, or production system data.</li>
                <li>Public page, API routes, analyzer path, README, screenshots, and CI checks that support a real diligence trail.</li>
              </ul>
            </article>
          </div>
        </section>
        """;

    private static string Metric(string value, string label, string help, string tone) =>
        $$"""<div class="kpi {{tone}}"><div class="v">{{value}}</div><div class="lbl">{{label}}</div><div class="h">{{help}}</div></div>""";

    private static string OwnerForGap(string family) => family switch
    {
        "Evidence" => "Lab QA",
        "Calibration" => "Diagnostics Operations",
        "Review" => "Clinical Quality",
        "Telemetry" => "Lab QA",
        "Variance" => "Clinical Quality",
        "Release" => "Clinical Quality",
        _ => "Diagnostics Operations"
    };

    private static string SeverityClass(string value) => value switch
    {
        "high" or "red" => "red",
        "medium" or "yellow" => "yellow",
        "green" or "low" => "green",
        _ => "info"
    };

    public static string Layout(string title, string active, string body)
    {
        var nav = new[]
        {
            Nav("/", "Overview"),
            Nav("/qc-lane", "QC Lane"),
            Nav("/evidence-routing", "Evidence Routing"),
            Nav("/release-posture", "Release Posture"),
            Nav("/verification", "Verification"),
            Nav("/docs", "Docs")
        };

        var navHtml = string.Join("", nav.Select(item =>
            item.Href == active
                ? $"""<a class="navchip active" href="{item.Href}">{item.Label}</a>"""
                : $"""<a class="navchip" href="{item.Href}">{item.Label}</a>"""));

        return $$$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>{{{title}}}</title>
          <style>
            :root{--bg:#070a0f;--panel:#0b1220;--line:rgba(120,255,170,.18);--line2:rgba(120,255,170,.10);--text:#e9f3ff;--muted:rgba(233,243,255,.72);--muted2:rgba(233,243,255,.55);--bert:#37ff8b;--bert2:#19c7ff;--warn:#ffcc66;--bad:#ff5c7a;--shadow:0 18px 60px rgba(0,0,0,.55);--mono:ui-monospace,SFMono-Regular,Menlo,Monaco,Consolas,"Liberation Mono","Courier New",monospace;--sans:ui-sans-serif,system-ui,-apple-system,Segoe UI,Roboto,Helvetica,Arial}
            *{box-sizing:border-box} body{margin:0;font-family:var(--sans);color:var(--text);background:radial-gradient(1200px 600px at 20% -10%, rgba(55,255,139,.18), transparent 60%),radial-gradient(900px 520px at 90% 0%, rgba(25,199,255,.16), transparent 55%),linear-gradient(180deg,#05070c 0%,#070a0f 35%,#05070c 100%)}
            .wrap{max-width:1280px;margin:0 auto;padding:24px 22px 80px}.topbar{display:flex;justify-content:space-between;gap:14px;border-bottom:1px solid var(--line2);padding-bottom:14px;margin-bottom:22px;font-family:var(--mono);font-size:11px;letter-spacing:.16em;color:var(--muted);text-transform:uppercase}.topbar .left{color:var(--bert)}
            .herorow{display:grid;grid-template-columns:1.5fr .9fr;gap:18px}@media (max-width:1000px){.herorow{grid-template-columns:1fr}}
            .hero,.src,.pcard,.kpi,.bluf,.corr{background:linear-gradient(180deg, rgba(11,18,32,.95), rgba(8,14,26,.92));border:1px solid var(--line);box-shadow:var(--shadow)}
            .hero{border-radius:22px;padding:28px 28px 24px;border-top:2px solid var(--bert2)} .hero h1{font-size:60px;line-height:.95;margin:0 0 18px;font-weight:800}@media (max-width:700px){.hero h1{font-size:42px}} .hero p{color:var(--muted);font-size:15px;line-height:1.55;max-width:680px;margin:0 0 18px}
            .chiprow,.navrow,.footer-links{display:flex;flex-wrap:wrap;gap:8px}.navrow{margin-top:18px}.meta-chip,.navchip,.ppri,.st,code{font-family:var(--mono)} .meta-chip,.navchip{font-size:11px;color:var(--muted);padding:7px 12px;border-radius:999px;border:1px solid var(--line);background:rgba(6,10,18,.4);text-decoration:none}.navchip.active{color:#071017;background:linear-gradient(135deg,var(--bert),var(--bert2));font-weight:700}
            .side{display:flex;flex-direction:column;gap:14px}.bluf,.corr{border-radius:14px;padding:16px 18px}.bluf{border-left:4px solid var(--warn)}.corr{border-left:4px solid var(--bert)}.lbl{font-family:var(--mono);font-size:10px;letter-spacing:.18em;text-transform:uppercase}.bluf .lbl{color:var(--warn)} .corr .lbl{color:var(--bert)} .bluf p,.corr p,.src p,.pcard .pdesc,.kpi .h{color:var(--muted);line-height:1.55}
            .section{margin-top:34px}.sh{display:flex;justify-content:space-between;gap:14px;padding-bottom:10px;border-bottom:1px solid var(--line2);margin-bottom:14px}.sh h2{margin:0;font-size:24px;font-weight:600}.sh .note{font-family:var(--mono);font-size:11px;color:var(--muted2);letter-spacing:.16em;text-transform:uppercase}
            .kpis{display:grid;grid-template-columns:repeat(6,1fr);gap:12px}@media (max-width:1100px){.kpis{grid-template-columns:repeat(3,1fr)}}@media (max-width:640px){.kpis{grid-template-columns:repeat(2,1fr)}} .kpi{border-radius:14px;padding:14px 14px 12px}.kpi .v{font-size:26px;font-weight:600}.kpi .lbl{font-size:10px;letter-spacing:.18em;text-transform:uppercase;color:var(--muted);margin-top:6px}.cyan .v{color:var(--bert2)} .green .v{color:var(--bert)} .plum .v{color:#b88cff} .amber .v,.yellow{color:var(--warn)} .red .v,.red{color:var(--bad)}
            .stack{display:grid;grid-template-columns:repeat(3,1fr);gap:12px}@media (max-width:1100px){.stack{grid-template-columns:repeat(2,1fr)}}@media (max-width:640px){.stack{grid-template-columns:1fr}} .src{border-radius:16px;padding:16px}.src-name{font-family:var(--mono);font-size:11px;color:var(--bert);letter-spacing:.2em;text-transform:uppercase}.src-tit{margin:8px 0 6px;font-size:17px;font-weight:600}
            .depth-grid{display:grid;grid-template-columns:1.05fr .95fr;gap:14px}@media (max-width:900px){.depth-grid{grid-template-columns:1fr}}.depth-card{position:relative;overflow:hidden;border-radius:18px;padding:20px 22px;border:1px solid rgba(25,199,255,.22);background:linear-gradient(140deg,rgba(14,24,42,.96),rgba(9,16,30,.92));box-shadow:0 18px 60px rgba(0,0,0,.42)}.depth-card:before{content:"";position:absolute;inset:0 0 auto;height:3px;background:linear-gradient(90deg,var(--bert),var(--bert2),#b88cff)}.depth-card .eyebrow{font-family:var(--mono);font-size:10px;letter-spacing:.2em;text-transform:uppercase;color:var(--bert);margin-bottom:10px}.depth-card h3{margin:0 0 10px;font-size:22px;line-height:1.18}.depth-card p,.depth-card li{color:var(--muted);line-height:1.6}.depth-card ul{padding-left:18px;margin:12px 0 0}.depth-card strong{color:var(--text)}
            .ttbl{width:100%;border-collapse:separate;border-spacing:0;border:1px solid var(--line);border-radius:14px;overflow:hidden}.ttbl th,.ttbl td{padding:13px 14px;text-align:left;font-size:13.5px;vertical-align:top}.ttbl thead th{font-family:var(--mono);font-size:11px;letter-spacing:.16em;text-transform:uppercase;color:var(--muted2);border-bottom:1px solid var(--line);background:rgba(11,18,32,.5)}.ttbl td,.ttbl td *{color:var(--muted)}.ttbl b{color:var(--text)}
            .board{display:grid;grid-template-columns:repeat(3,1fr);gap:14px}@media (max-width:1000px){.board{grid-template-columns:1fr}} .pcard{border-radius:16px;padding:18px 20px;display:flex;flex-direction:column}.ptop{display:flex;justify-content:space-between;align-items:center;margin-bottom:8px}.pnum{font-family:var(--mono);font-size:22px;font-weight:600;color:var(--bert)}.ppri{font-size:10px;padding:5px 10px;border-radius:999px;border:1px solid var(--line);color:var(--bert);letter-spacing:.14em;background:rgba(55,255,139,.06)}.pcard h3{margin:6px 0 8px;font-size:19px}.check{list-style:none;padding:0;margin:0 0 14px}.check li{display:grid;grid-template-columns:18px 1fr;gap:10px;padding:6px 0;font-size:13.5px;color:var(--muted)}.check li:before{content:"";width:14px;height:14px;border:1px solid var(--line);border-radius:3px;background:rgba(6,10,18,.4);margin-top:3px}
            .st{font-size:10px;padding:4px 9px;border-radius:6px;letter-spacing:.1em;text-transform:uppercase;border:1px solid currentColor;display:inline-block}.st.green{color:var(--bert)}.st.yellow{color:var(--warn)}.st.red{color:var(--bad)}.st.info{color:var(--bert2)}
            .footer{margin-top:30px;padding-top:14px;border-top:1px dashed var(--line2);display:flex;justify-content:space-between;gap:10px;flex-wrap:wrap;font-family:var(--mono);font-size:11px;color:var(--muted2);letter-spacing:.08em} code{font-size:12px;color:var(--bert2);background:rgba(25,199,255,.08);padding:1px 6px;border-radius:5px;border:1px solid rgba(25,199,255,.18)}
          </style>
        </head>
        <body>
          <div class="wrap">
            <div class="topbar">
              <div class="left">Kinetic Gain · Diagnostic QC Evidence Router</div>
              <div>synthetic assay snapshots · no patient or lab secrets</div>
            </div>
            <div class="herorow">
              <section class="hero">
                <div class="chiprow">
                  <span class="meta-chip">Wave 21 · Polyglot Language and Vertical Expansion</span>
                  <span class="meta-chip">C# · biotech / diagnostics operator proof</span>
                  <span class="meta-chip">Hosted preview planned · Embedded by engagement</span>
                </div>
                <h1>Diagnostic evidence, calibration drift, and release posture that stay operator-readable.</h1>
                <p>This control plane turns synthetic diagnostics QC exports into one review surface: evidence continuity, stale calibration artifacts, assay variance, telemetry replay, and release-packet completeness before result publication.</p>
                <div class="navrow">{{{navHtml}}}</div>
              </section>
              <aside class="side">
                <div class="bluf"><div class="lbl">Commercial front door</div><p><strong>Diagnostics QC routing, evidence posture, and release-readiness operations for biotech teams.</strong><br />The free surface is buyer-readable proof; the commercial path is an embedded evidence-routing module for regulated assay workflows.</p></div>
                <div class="corr"><div class="lbl">Proof layer</div><p><strong>.NET API + static operator shell.</strong><br />The repo models lab evidence continuity, assay release blockers, and review packets without pretending to be a live tenant integration.</p></div>
                <div class="corr"><div class="lbl">Why it matters</div><p>Biotech teams need evidence and release posture in one lane, not scattered lab exports and email threads.</p></div>
              </aside>
            </div>
            {{{body}}}
            <div class="footer">
              <div>diagnostic-qc-evidence-router · synthetic sample data only</div>
              <div class="footer-links">
                <a class="meta-chip" href="https://github.com/mizcausevic-dev/diagnostic-qc-evidence-router">Repo</a>
                <a class="meta-chip" href="https://portfolio.kineticgain.com/">Portfolio</a>
                <a class="meta-chip" href="https://suite.kineticgain.com/">Suite</a>
                <a class="meta-chip" href="https://www.linkedin.com/in/mirzacausevic/">LinkedIn</a>
                <a class="meta-chip" href="https://kineticgain.com/">Kinetic Gain</a>
              </div>
            </div>
          </div>
        </body>
        </html>
        """;
    }

    private static (string Href, string Label) Nav(string href, string label) => (href, label);
}
