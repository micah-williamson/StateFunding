using System;

namespace StateFunding {
  public class CoverageReport {
    public CoverageReport () {}

    [Persistent]
    public string entity;

    [Persistent]
    public int satCount = 0;

    [Persistent]
    public int satCountForFullCoverage = 0;

    [Persistent]
    public float coverage = 0;

    public void update() {
      if (satCount == 0) {
        coverage = 0;
      } else {
        coverage = satCountForFullCoverage / Math.Min (satCount, satCountForFullCoverage);
      }
    }
  }
}

