using System;
using UnityEngine;

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

    public void Update() {
      if (satCount == 0) {
        coverage = 0;
      } else {
        coverage = (float)Math.Min (satCount, satCountForFullCoverage) / (float)satCountForFullCoverage;
      }
    }
  }
}

