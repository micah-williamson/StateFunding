using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class KerbalHelper {

    public static Vessel GetVessel(ProtoCrewMember Kerb) {
      Vessel[] Vessels = (Vessel[])FlightGlobals.Vessels.ToArray();
      for(int i = 0; i < Vessels.Length; i++) {
        Vessel Vsl = Vessels [i];
        ProtoCrewMember[] Crew = Vsl.GetVesselCrew ().ToArray();

        for(int k = 0; k < Crew.Length; k++) {
          ProtoCrewMember CrewMember = Crew [k];
          if (CrewMember.name == Kerb.name) {
            return Vsl;
          }
        }
      }

      return null;
    }

    public static ProtoCrewMember[] GetActiveKerbals() {
      List<ProtoCrewMember> ReturnKerbals = new List<ProtoCrewMember> ();

      IEnumerator Kerbals = HighLogic.CurrentGame.CrewRoster.Crew.GetEnumerator();

      while(Kerbals.MoveNext()) {
        ProtoCrewMember Kerbal = (ProtoCrewMember)Kerbals.Current;
        if (Kerbal.rosterStatus.ToString() == "Assigned") {
          if (!KerbalHelper.IsStranded (Kerbal)) {
            ReturnKerbals.Add (Kerbal);
          }
        }
      }

      return ReturnKerbals.ToArray ();
    }

    public static bool IsStranded(ProtoCrewMember Kerb) {
      Vessel Vsl = GetVessel (Kerb);
      if (Vsl != null
        && KerbalHelper.QualifiedStranded(Kerb)
        && Vsl.missionTime > TimeHelper.ToYears(2)) {

        return true;
      }

      return false;
    }

    public static int TimeToStranded(ProtoCrewMember Kerb) {
      Vessel Vsl = GetVessel (Kerb);
      if (Vsl != null) {
        return TimeHelper.Days (TimeHelper.ToYears (2) - Vsl.missionTime);
      }

      return 0;
    }

    public static bool QualifiedStranded(ProtoCrewMember Kerb) {
      Vessel Vsl = GetVessel (Kerb);
      if (Vsl != null
        && Vsl.protoVessel.vesselType != VesselType.Base
        && Vsl.protoVessel.vesselType != VesselType.Rover
        && Vsl.protoVessel.vesselType != VesselType.Station) {
        if (!VesselHelper.HasLiquidFuel (Vsl) || !VesselHelper.HasEnergy (Vsl)) {
          if(!VesselHelper.VesselHasModuleAlias(Vsl, "ScienceLab")) {
            if (!VesselHelper.VesselHasModuleAlias (Vsl, "Drill")) {
              return true;
            }
          }
        }
      }

      return false;
    }

    public static ProtoCrewMember[] GetStrandedKerbals() {
      List<ProtoCrewMember> ReturnKerbals = new List<ProtoCrewMember> ();

      IEnumerator Kerbals = HighLogic.CurrentGame.CrewRoster.Crew.GetEnumerator();

      while(Kerbals.MoveNext()) {
        ProtoCrewMember Kerbal = (ProtoCrewMember)Kerbals.Current;
        if (KerbalHelper.IsStranded (Kerbal)) {
          ReturnKerbals.Add (Kerbal);
        }
      }

      return ReturnKerbals.ToArray ();
    }

    public static ProtoCrewMember[] GetKerbals() {
      List<ProtoCrewMember> ReturnKerbals = new List<ProtoCrewMember> ();

      IEnumerator Kerbals = HighLogic.CurrentGame.CrewRoster.Crew.GetEnumerator();

      while(Kerbals.MoveNext()) {
        ProtoCrewMember Kerbal = (ProtoCrewMember)Kerbals.Current;
        if (Kerbal.rosterStatus.ToString() == "Assigned") {
          ReturnKerbals.Add (Kerbal);
        }
      }

      return ReturnKerbals.ToArray ();
    }

  }
}

