using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class VesselHelper {

    public static bool HasLiquidFuel(Vessel Vsl) {
      ProtoPartModuleSnapshot[] LiquidFuelModules = VesselHelper.GetModules (Vsl, "LiquidFuel");
      for (var i = 0; i < LiquidFuelModules.Length; i++) {
        ProtoPartModuleSnapshot LiquidFuelModule = LiquidFuelModules [i];
        if (int.Parse(LiquidFuelModule.moduleValues.GetValue ("amount")) > 0) {
          return true;
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

    public static Vessel[] GetVesselsWithModules(string[] modules) {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] Vessels = (Vessel[])FlightGlobals.Vessels.ToArray();
      for (var i = 0; i < Vessels.Length; i++) {
        Vessel Vsl = Vessels [i];
        bool hasAllModules = true;

        for (int k = 0; k < modules.Length; k++) {
          if(!VesselHasModule(Vsl, modules[k])) {
            hasAllModules = false;
          }
        }

        if (hasAllModules) {
          ReturnVessels.Add (Vsl);
        }
      }
        
      return ReturnVessels.ToArray();
    }

    public static bool VesselHasModule(Vessel Vsl, string module) {
      ProtoPartSnapshot[] Parts = (ProtoPartSnapshot[])Vsl.protoVessel.protoPartSnapshots.ToArray();
      for (int k = 0; k < Parts.Length; k++) {
        ProtoPartSnapshot Prt = Parts [k];
        ProtoPartModuleSnapshot[] Modules = Prt.modules.ToArray ();
        for (int j = 0; j < Modules.Length; j++) {
          ProtoPartModuleSnapshot Module = Modules [j];

          if (Module.moduleValues.GetValue ("name") == module) {
            return true;
          }
        }
      }

      return false;
    }

    public static Vessel[] GetOrbitingScienceStations() {
      List<Vessel> ReturnVessels = new List<Vessel>();

      Vessel[] ScienceLabs = VesselHelper.GetVesselsWithModules(new string[] {
        "ModuleDeployableSolarPanel",
        "ModuleDataTransmitter",
        "ModuleScienceLab"
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

      Vessel[] ScienceLabs = VesselHelper.GetVesselsWithModules(new string[] {
        "ModuleDeployableSolarPanel",
        "ModuleDataTransmitter",
        "ModuleScienceLab"
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

      Vessel[] MiningRigs = VesselHelper.GetVesselsWithModules(new string[] {
        "ModuleDeployableSolarPanel",
        "ModuleDataTransmitter",
        "ModuleResourceHarvester"
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

      Vessel[] Satellites = VesselHelper.GetVesselsWithModules(new string[] {
        "ModuleDeployableSolarPanel",
        "ModuleDataTransmitter",
        "ModuleSAS"
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

