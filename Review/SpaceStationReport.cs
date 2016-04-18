using System;
using UnityEngine;

namespace StateFunding {
  public class SpaceStationReport {
    public SpaceStationReport () {}

    [Persistent]
    public string name = "";

    [Persistent]
    public string entity = "";

    [Persistent]
    public int fuel = 0;

    [Persistent]
    public int ore = 0;

    [Persistent]
    public int crew = 0;

      [Persistent]
    public int crewCapacity = 0;

      [Persistent]
    public int dockedVessels = 0;

      [Persistent]
    public int dockingPorts = 0;

      [Persistent]
    public bool drill = false;

      [Persistent]
    public bool scienceLab = false;

      [Persistent]
    public bool onAstroid = false;

      [Persistent]
    public int po = 0;

      [Persistent]
    public int sc = 0;
  }
}

