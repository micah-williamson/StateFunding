using System;
using UnityEngine;

namespace StateFunding {
  public class SpaceStationReport {
    public SpaceStationReport () {}

    [KSPField (isPersistant=true)]
    public string name = "";

    [KSPField (isPersistant=true)]
    public string entity = "";

    [KSPField (isPersistant=true)]
    public int fuel = 0;

    [KSPField (isPersistant=true)]
    public int ore = 0;

    [KSPField (isPersistant=true)]
    public int crew = 0;

      [KSPField (isPersistant=true)]
    public int crewCapacity = 0;

      [KSPField (isPersistant=true)]
    public int dockedVessels = 0;

      [KSPField (isPersistant=true)]
    public int dockingPorts = 0;

      [KSPField (isPersistant=true)]
    public bool drill = false;

      [KSPField (isPersistant=true)]
    public bool scienceLab = false;

      [KSPField (isPersistant=true)]
    public bool onAstroid = false;

      [KSPField (isPersistant=true)]
    public int po = 0;

      [KSPField (isPersistant=true)]
    public int sc = 0;
  }
}

