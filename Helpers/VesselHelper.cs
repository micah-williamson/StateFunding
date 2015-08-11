using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class VesselHelper {

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

  }
}

