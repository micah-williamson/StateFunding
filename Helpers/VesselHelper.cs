using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class VesselHelper {

    public static ConfigNode[] ModuleAliases;

    public static void LoadAliases() {
      ConfigNode AliasConfig = ConfigNode.Load ("GameData/StateFunding/data/modulealiases.settings");
      ModuleAliases = AliasConfig.GetNode ("AliasConfig").GetNodes ();
    }

    public static bool HasEnergy(Vessel Vsl) {
      return VesselHelper.HasResource (Vsl, "ElectricCharge");
    }

    public static bool HasLiquidFuel(Vessel Vsl) {
      return VesselHelper.HasResource (Vsl, "LiquidFuel");
    }

    public static bool GeneratesEnergy(Vessel Vsl) {
      return VesselHelper.VesselHasModuleAlias (Vsl, "Energy");
    }

    public static bool HasCommunication(Vessel Vsl) {
      return VesselHelper.VesselHasModuleAlias (Vsl, "Communication");
    }

    public static bool WorkingWheels(Vessel Vsl) {
      ProtoPartModuleSnapshot[] Wheels = VesselHelper.GetModulesWithAlias (Vsl, "Wheel");
      if (Wheels.Length >= 4) {
        return true;
      }

      return false;

      // TODO: Check to see if they're borken
      /*
      for (int i = 0; i < Wheels.Length; i++) {
        ProtoPartSnapshot Wheel = Wheels [i];
      }*/
    }

    public static bool HasCrew(Vessel Vsl) {
      return Vsl.protoVessel.GetVesselCrew().Count > 0;
    }

    public static int GetCrewCapactiy(Vessel Vsl) {
      ProtoPartSnapshot[] Parts = Vsl.protoVessel.protoPartSnapshots.ToArray();
      int crewCapacity = 0;

      for (int i = 0; i < Parts.Length; i++) {
        ProtoPartSnapshot Part = Parts [i];
        crewCapacity += Part.protoModuleCrew.Capacity;
      }

      return crewCapacity;
    }

    public static ProtoPartSnapshot[] GetDockingPorts(Vessel Vsl) {
      return VesselHelper.GetPartsWithAlias (Vsl, "DockingPort");
    }

    public static int GetDockedVesselsCount(Vessel Vsl) {
      List<uint> MissionIds = new List<uint> ();
      ProtoPartSnapshot[] Parts = Vsl.protoVessel.protoPartSnapshots.ToArray();

      for (int i = 0; i < Parts.Length; i++) {
        uint missionId = Parts [i].missionID;
        bool found = false;
        for (int k = 0; k < MissionIds.ToArray ().Length; k++) {
          uint _missionId = MissionIds.ToArray () [k];
          if (missionId == _missionId) {
            found = true;
          }
        }

        if (!found) {
          MissionIds.Add (missionId);
        }
      }

      int missionCount = MissionIds.Count - 1;

      if (OnAstroid (Vsl)) {
        missionCount--;
      }

      return missionCount;
    }

    public static bool HasResource(Vessel Vsl, string resource) {
      ProtoPartSnapshot[] Parts = Vsl.protoVessel.protoPartSnapshots.ToArray();
      for (var i = 0; i < Parts.Length; i++) {
        ProtoPartSnapshot Part = Parts [i];
        ProtoPartResourceSnapshot[] Resources = Part.resources.ToArray ();
        for (var k = 0; k < Resources.Length; k++) {
          ProtoPartResourceSnapshot Resrc = Resources [k];
          if(Resrc.resourceName == resource && float.Parse(Resrc.resourceValues.GetValue ("amount")) > 0) {
            return true;
          }
        }
      }

      return false;
    }

    public static int GetResourceCount(Vessel Vsl, string resource) {
      ProtoPartSnapshot[] Parts = Vsl.protoVessel.protoPartSnapshots.ToArray();
      int resourceCount = 0;

      for (var i = 0; i < Parts.Length; i++) {
        ProtoPartSnapshot Part = Parts [i];
        ProtoPartResourceSnapshot[] Resources = Part.resources.ToArray ();
        for (var k = 0; k < Resources.Length; k++) {
          ProtoPartResourceSnapshot Resrc = Resources [k];
          if(Resrc.resourceName == resource) {
            resourceCount += (int)float.Parse (Resrc.resourceValues.GetValue ("amount"));
          }
        }
      }

      return resourceCount;
    }

    public static ProtoCrewMember[] GetCrew(Vessel Vsl) {
      return Vsl.protoVessel.GetVesselCrew ().ToArray();
    }

    public static ProtoPartModuleSnapshot[] GetModules(Vessel Vsl, string module) {
      List<ProtoPartModuleSnapshot> ReturnModules = new List<ProtoPartModuleSnapshot>();

      ProtoPartSnapshot[] Parts = (ProtoPartSnapshot[])Vsl.protoVessel.protoPartSnapshots.ToArray();
      for (int k = 0; k < Parts.Length; k++) {
        ProtoPartSnapshot Prt = Parts [k];
        ProtoPartModuleSnapshot[] Modules = Prt.modules.ToArray ();
        for (int j = 0; j < Modules.Length; j++) {
          ProtoPartModuleSnapshot Module = Modules [j];

          if (Module.moduleValues.GetValue ("name") == module) {
            ReturnModules.Add(Module);
          }
        }
      }

      return ReturnModules.ToArray ();
    }

    public static ProtoPartModuleSnapshot[] GetModulesWithAlias(Vessel Vsl, string alias) {
      List<ProtoPartModuleSnapshot> ReturnModules = new List<ProtoPartModuleSnapshot>();

      ProtoPartSnapshot[] Parts = (ProtoPartSnapshot[])Vsl.protoVessel.protoPartSnapshots.ToArray();
      for (int k = 0; k < Parts.Length; k++) {
        ProtoPartSnapshot Prt = Parts [k];
        ProtoPartModuleSnapshot[] Modules = Prt.modules.ToArray ();
        for (int j = 0; j < Modules.Length; j++) {
          ProtoPartModuleSnapshot Module = Modules [j];

          if (ModuleInAlias(Module, alias)) {
            ReturnModules.Add(Module);
          }
        }
      }

      return ReturnModules.ToArray ();
    }

    public static ProtoPartSnapshot[] GetPartsWithAlias(Vessel Vsl, string alias) {
      List<ProtoPartSnapshot> ReturnParts = new List<ProtoPartSnapshot>();

      ProtoPartSnapshot[] Parts = (ProtoPartSnapshot[])Vsl.protoVessel.protoPartSnapshots.ToArray();
      for (int k = 0; k < Parts.Length; k++) {
        ProtoPartSnapshot Prt = Parts [k];
        ProtoPartModuleSnapshot[] Modules = Prt.modules.ToArray ();
        for (int j = 0; j < Modules.Length; j++) {
          ProtoPartModuleSnapshot Mod = Modules [j];
          if (ModuleInAlias(Mod, alias)) {
            ReturnParts.Add(Prt);
            break;
          }
        }
      }

      return ReturnParts.ToArray ();
    }

    public static Vessel[] GetVesselsWithModuleAliases(string[] aliases) {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] Vessels = (Vessel[])FlightGlobals.Vessels.ToArray();
      for (var i = 0; i < Vessels.Length; i++) {
        Vessel Vsl = Vessels [i];
        bool hasAllModules = true;

        for (int k = 0; k < aliases.Length; k++) {
          if(!VesselHasModuleAlias(Vsl, aliases[k])) {
            hasAllModules = false;
          }
        }

        if (hasAllModules) {
          ReturnVessels.Add (Vsl);
        }
      }
        
      return ReturnVessels.ToArray();
    }

    public static bool VesselHasModuleAlias(Vessel Vsl, string alias) {
      ProtoPartSnapshot[] Parts = (ProtoPartSnapshot[])Vsl.protoVessel.protoPartSnapshots.ToArray();
      for (int k = 0; k < Parts.Length; k++) {
        ProtoPartSnapshot Prt = Parts [k];
        ProtoPartModuleSnapshot[] Modules = Prt.modules.ToArray ();
        for (int j = 0; j < Modules.Length; j++) {
          ProtoPartModuleSnapshot Module = Modules [j];

          if (ModuleInAlias(Module, alias)) {
            return true;
          }
        }
      }

      return false;
    }

    public static bool PartHasModuleAlias(Part Prt, string alias) {
      ProtoPartModuleSnapshot[] Modules = Prt.protoPartSnapshot.modules.ToArray ();

      for (int j = 0; j < Modules.Length; j++) {
        ProtoPartModuleSnapshot Module = Modules [j];

        if (ModuleInAlias (Module, alias)) {
          return true;
        }
      }

      return false;
    }

    public static bool ModuleInAlias(ProtoPartModuleSnapshot Module, string alias) {
      for (int i = 0; i < ModuleAliases.Length; i++) {
        ConfigNode ModuleAlias = ModuleAliases [i];

        if (ModuleAlias.GetValue ("name") == alias) {
          
          ConfigNode[] Modules = ModuleAlias.GetNode("Modules").GetNodes ();

          for (int k = 0; k < Modules.Length; k++) {
            ConfigNode Mod = Modules [k];
            if (Mod.GetValue ("name") == Module.moduleValues.GetValue("name")) {
              return true;
            }
          }
        }
      }

      return false;
    }

    public static Vessel[] GetBases() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] Bases = (Vessel[])FlightGlobals.Vessels.ToArray();

      for (var i = 0; i < Bases.Length; i++) {
        Vessel Base = Bases [i];
        if (Base.vesselType == VesselType.Base) {
          if (Base.Landed && Base.landedAt != SpaceCenter.Instance.cb.GetName()
            && VesselHelper.HasEnergy(Base)
            && VesselHelper.GeneratesEnergy(Base)
            && VesselHelper.HasCommunication(Base)) {
            ReturnVessels.Add (Base);
          }
        }
      }

      return ReturnVessels.ToArray ();
    }

    public static Vessel[] GetRovers() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] Rovers = (Vessel[])FlightGlobals.Vessels.ToArray();

      for (var i = 0; i < Rovers.Length; i++) {
        Vessel Rover = Rovers [i];
        if (Rover.vesselType == VesselType.Rover) {
          if (Rover.Landed && Rover.landedAt != SpaceCenter.Instance.cb.GetName()
            && VesselHelper.HasEnergy(Rover)
            && VesselHelper.WorkingWheels(Rover)) {
            ReturnVessels.Add (Rover);
          }
        }
      }

      return ReturnVessels.ToArray ();
    }

    public static Vessel[] GetSpaceStations() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] SpaceStations = (Vessel[])FlightGlobals.Vessels.ToArray();

      for (var i = 0; i < SpaceStations.Length; i++) {
        Vessel SpaceStation = SpaceStations [i];
        if (SpaceStation.vesselType == VesselType.Station) {
          /**
           * Make sure it's not landed. If it is landed check to see if it's a celestial body-
           * If it's not, then you landed on an astroid. good job!
           */

          if (!SpaceStation.Landed || !BodyHelper.ACelestialBody(SpaceStation.landedAt)) {
            ReturnVessels.Add (SpaceStation);
          }
        }
      }

      return ReturnVessels.ToArray ();
    }

    public static Vessel[] GetOrbitingScienceStations() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] ScienceLabs = VesselHelper.GetVesselsWithModuleAliases(new string[] {
        "ScienceLab"
      });

      for (var i = 0; i < ScienceLabs.Length; i++) {
        Vessel ScienceLab = ScienceLabs [i];

        if (VesselHelper.HasCrew(ScienceLab)
          && !ScienceLab.Landed
          && ScienceLab.vesselType != VesselType.Station
          && ScienceLab.vesselType != VesselType.Base
          && VesselHelper.HasEnergy(ScienceLab)
          && VesselHelper.GeneratesEnergy(ScienceLab)
          && VesselHelper.HasCommunication(ScienceLab)) {
          ReturnVessels.Add (ScienceLab);
        }
      }

      return ReturnVessels.ToArray ();
    }

    public static Vessel[] GetLandedScienceStations() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] ScienceLabs = VesselHelper.GetVesselsWithModuleAliases(new string[] {
        "ScienceLab"
      });

      for (var i = 0; i < ScienceLabs.Length; i++) {
        Vessel ScienceLab = ScienceLabs [i];

        if (VesselHelper.HasCrew(ScienceLab)
          && ScienceLab.Landed
          && ScienceLab.vesselType != VesselType.Station
          && ScienceLab.vesselType != VesselType.Base
          && ScienceLab.landedAt != SpaceCenter.Instance.cb.GetName()
          && VesselHelper.HasEnergy(ScienceLab)
          && VesselHelper.GeneratesEnergy(ScienceLab)
          && VesselHelper.HasCommunication(ScienceLab)) {
          ReturnVessels.Add (ScienceLab);
        }
      }

      return ReturnVessels.ToArray ();
    }

    public static Vessel[] GetScienceStations() {
      List<Vessel> ReturnVessels = new List<Vessel>();
      Vessel[] LandedStations = GetLandedScienceStations();
      Vessel[] OrbitingStations = GetOrbitingScienceStations();

      for(int i = 0; i < OrbitingStations.Length; i++) {
        ReturnVessels.Add(OrbitingStations[i]);
      }

      for(int i = 0; i < LandedStations.Length; i++) {
        ReturnVessels.Add(LandedStations[i]);
      }

      return ReturnVessels.ToArray();
    }

    public static Vessel[] GetMiningRigs() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] MiningRigs = VesselHelper.GetVesselsWithModuleAliases(new string[] {
        "Drill"
      });

      for (var i = 0; i < MiningRigs.Length; i++) {
        Vessel MiningRig = MiningRigs [i];
        if ((MiningRig.Landed || OnAstroid(MiningRig))
          && MiningRig.vesselType != VesselType.Station
          && MiningRig.vesselType != VesselType.Base
          && MiningRig.landedAt != SpaceCenter.Instance.cb.GetName()
          && VesselHelper.HasEnergy(MiningRig)
          && VesselHelper.GeneratesEnergy(MiningRig)
          && VesselHelper.HasCommunication(MiningRig)) {
          ReturnVessels.Add (MiningRig);
        }

      }

      return ReturnVessels.ToArray();
    }

    public static Vessel[] GetSatellites() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] Satellites = VesselHelper.GetVesselsWithModuleAliases(new string[] {
        "Energy",
        "Communication",
        "AutonomousCommand"
      });

      for (int i = 0; i < Satellites.Length; i++) {
        Vessel Satellite = Satellites [i];

        if (!Satellite.Landed
          && Satellite.vesselType == VesselType.Probe
          && Satellite.GetOrbit() != null
          && Satellite.GetOrbit().referenceBody.GetName() != "Sun"
          && !VesselHelper.HasCrew (Satellite)) {
          ReturnVessels.Add (Satellite);
        }
      }

      return ReturnVessels.ToArray ();
    }

    public static bool OnAstroid(Vessel Vsl) {
      return VesselHasModuleAlias(Vsl, "Astroid");
    }

  }
}

