using System;

namespace StateFunding {
  public class CoverageReport {
    public CoverageReport () {}

    [Persistent]
    string entity;

    [Persistent]
    float coverage;
  }
}

