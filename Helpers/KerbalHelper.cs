using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public static class KerbalHelper {

    public static int getAssignedKerbalCount() {
      int count = 0;

      IEnumerator Kerbals = HighLogic.CurrentGame.CrewRoster.Crew.GetEnumerator();

      while(Kerbals.MoveNext()) {
        ProtoCrewMember Kerbal = (ProtoCrewMember)Kerbals.Current;
        if (Kerbal.rosterStatus.ToString() == "Assigned") {
          count++;
        }
      }

      return count;
    }

  }
}

