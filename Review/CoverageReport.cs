using System;
using UnityEngine;

namespace StateFunding {
  public class CoverageReport {
    public CoverageReport () {}

    [KSPField (isPersistant=true)]
    public string entity;

    [KSPField (isPersistant=true)]
    public int satCount = 0;

    [KSPField (isPersistant=true)]
    public int satCountForFullCoverage = 0;

    [KSPField (isPersistant=true)]
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

