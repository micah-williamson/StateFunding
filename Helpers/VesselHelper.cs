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

    public static bool HasLiquidFuel(Vessel Vsl) {
      Debug.Log (Vsl.GetName ());
      ProtoPartSnapshot[] Parts = Vsl.protoVessel.protoPartSnapshots.ToArray();
      for (var i = 0; i < Parts.Length; i++) {
        ProtoPartSnapshot Part = Parts [i];
        ProtoPartResourceSnapshot[] Resources = Part.resources.ToArray ();
        for (var k = 0; k < Resources.Length; k++) {
          ProtoPartResourceSnapshot Resrc = Resources [k];
          if(Resrc.resourceName == "LiquidFuel" && float.Parse(Resrc.resourceValues.GetValue ("amount")) > 0) {
            return true;
          }
        }
      }

      return false;
    }

    public static bool HasCrew(Vessel Vsl) {
      return Vsl.protoVessel.GetVesselCrew().Count > 0;
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

    public static Vessel[] GetOrbitingScienceStations() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] ScienceLabs = VesselHelper.GetVesselsWithModuleAliases(new string[] {
        "Energy",
        "Communication",
        "ScienceLab"
      });

      for (var i = 0; i < ScienceLabs.Length; i++) {
        Vessel ScienceLab = ScienceLabs [i];

        if (VesselHelper.HasCrew(ScienceLab) && !ScienceLab.Landed) {
          ReturnVessels.Add (ScienceLab);
        }
      }

      return ReturnVessels.ToArray ();
    }

    public static Vessel[] GetLandedScienceStations() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] ScienceLabs = VesselHelper.GetVesselsWithModuleAliases(new string[] {
        "Energy",
        "Communication",
        "ScienceLab"
      });

      for (var i = 0; i < ScienceLabs.Length; i++) {
        Vessel ScienceLab = ScienceLabs [i];

        if (VesselHelper.HasCrew(ScienceLab) && ScienceLab.Landed) {
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
        "Energy",
        "Communication",
        "Drill"
      });

      for (var i = 0; i < MiningRigs.Length; i++) {
        Vessel MiningRig = MiningRigs [i];
        // Planetary science station

        if (MiningRig.Landed && MiningRig.landedAt != SpaceCenter.Instance.cb.GetName()) {
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

        if (!Satellite.Landed) {
          if (Satellite.GetOrbit() != null && Satellite.GetOrbit().referenceBody.GetName() != "Sun") {
            if (!VesselHelper.HasCrew (Satellite)) {
              ReturnVessels.Add (Satellite);
            }
          }
        }
      }

      return ReturnVessels.ToArray ();
    }

  }
}

