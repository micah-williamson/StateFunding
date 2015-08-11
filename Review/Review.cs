using System;
using UnityEngine;

namespace StateFunding {
  public class Review {
    public Review () {}

    [Persistent]
    public int activeKerbals = 0;

    [Persistent]
    public int activeKerbalsChange = 0;

    [Persistent]
    public int contractsCompleted = 0;

    [Persistent]
    public int contractsFailed = 0;

    [Persistent]
    public CoverageReport[] Coverages;

    [Persistent]
    public int kerbalDeaths = 0;

    [Persistent]
    public int vesselsDestroyed = 0;

    [Persistent]
    public int po = 0;

    [Persistent]
    public int poChange = 0;

    [Persistent]
    public int resourcesMined = 0;

    [Persistent]
    public int resourcesMinedChange = 0;

    [Persistent]
    public int sateliteCoverage = 0;

    [Persistent]
    public int sateliteCoverageChange = 0;

    [Persistent]
    public int sc = 0;

    [Persistent]
    public int scChange = 0;

    [Persistent]
    public int stationScience = 0;

    [Persistent]
    public int stationScienceChange = 0;

    [Persistent]
    public int strandedKerbals = 0;

    [Persistent]
    public int year = 0;

    public int calcPO() {
      int tmpPo = po;

      Instance Inst = StateFundingGlobal.fetch.GameInstance;
      Government Gov = Inst.Gov;

      Vessel[] Vessels = (Vessel[])FlightGlobals.Vessels.ToArray();
      for (var i = 0; i < Vessels.Length; i++) {
        Vessel Vsl = Vessels [i];
        // Check for parts
        bool hasAntenna = false;
        bool hasLab = false;
        bool hasSolar = false;
        ProtoPartSnapshot[] Parts = (ProtoPartSnapshot[])Vsl.protoVessel.protoPartSnapshots.ToArray();

        for (int k = 0; k < Parts.Length; k++) {
          ProtoPartSnapshot Prt = Parts [k];
          ProtoPartModuleSnapshot[] Modules = Prt.modules.ToArray ();
          for (int j = 0; j < Modules.Length; j++) {
            ProtoPartModuleSnapshot Module = Modules [j];

            if (Module.moduleValues.GetValue ("name") == "ModuleDeployableSolarPanel") {
              hasSolar = true;
            }

            if(Module.moduleValues.GetValue("name") == "ModuleDataTransmitter") {
              hasAntenna = true;
            }

            if (Module.moduleValues.GetValue ("name") == "ModuleScienceLab") {
              hasLab = true;
            }
          }
        }

        // If it can generate power and communicate continue
        if (hasSolar && hasAntenna) {
          Debug.Log ("Found vessel with power and an antenna");

          // If it has a drill and is landed somewhere else, have some points
          // TODO: Remove comments in condition
          if (Vsl.Landed && hasLab/* && Vsl.landedAt != FlightGlobals.GetHomeBodyName*/) {
            Debug.Log ("Found a vessel with a landed lab");
            tmpPo += (int)(6 * Gov.poModifier);
          } else if (hasLab) {
            Debug.Log ("Found a vessel with an orbiting lab");
            tmpPo += (int)(3 * Gov.poModifier);
          }
        }

      }

      tmpPo -= (int)(5 * kerbalDeaths * Gov.poPenaltyModifier);
      tmpPo += (int)(3 * activeKerbals * Gov.poModifier);
      tmpPo += (int)(3 * activeKerbals * Gov.poModifier);

      return tmpPo;
    }

    public int calcSC() {
      int tmpSc = sc;

      Instance Inst = StateFundingGlobal.fetch.GameInstance;
      Government Gov = Inst.Gov;

      Vessel[] Vessels = (Vessel[])FlightGlobals.Vessels.ToArray();
      for (var i = 0; i < Vessels.Length; i++) {
        Vessel Vsl = Vessels [i];

        // Check for parts
        bool hasAntenna = false;
        bool hasDrill = false;
        bool hasSolar = false;
        ProtoPartSnapshot[] Parts = (ProtoPartSnapshot[])Vsl.protoVessel.protoPartSnapshots.ToArray();
        for (int k = 0; k < Parts.Length; k++) {
          ProtoPartSnapshot Prt = Parts [k];
          ProtoPartModuleSnapshot[] Modules = Prt.modules.ToArray ();
          for (int j = 0; j < Modules.Length; j++) {
            ProtoPartModuleSnapshot Module = Modules [j];

            if (Module.moduleValues.GetValue ("name") == "ModuleDeployableSolarPanel") {
              hasSolar = true;
            }

            if(Module.moduleValues.GetValue("name") == "ModuleDataTransmitter") {
              hasAntenna = true;
            }

            if (Module.moduleValues.GetValue ("name") == "ModuleResourceHarvester") {
              hasDrill = true;
            }
          }
        }

        if (hasSolar) {
          Debug.LogWarning ("Found vessel with power");
        }

        if (hasAntenna) {
          Debug.LogWarning ("Found vessel with an antenna");
        }

        if (hasDrill) {
          Debug.LogWarning ("Found a vessel with a drill");
        }

        // If it can generate power and communicate continue
        if (hasSolar && hasAntenna) {
          Debug.Log ("Found vessel with power and an antenna");

          // If it has a drill and is landed somewhere else, have some points
          // TODO: Remove after testing
          if (Vsl.Landed && hasDrill/* && Vsl.landedAt != FlightGlobals.GetHomeBodyName*/) {
            Debug.Log ("Found a vessel with a drill");
            tmpSc += (int)(20 * Gov.scModifier);
          }

          tmpSc += (int)(3 * Gov.scModifier);
        }

      }

      tmpSc -= (int)(3 * (vesselsDestroyed / 3) * Gov.scPenaltyModifier);
      tmpSc -= (int)(5 * contractsFailed * Gov.scPenaltyModifier);
      tmpSc += (int)(5 * contractsCompleted * Gov.scModifier);

      return tmpSc;
    }

    public float calcFunds() {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      return (float)((float)(calcPO() + calcSC()) / 10000)*(float)Inst.Gov.gdp*(float)Inst.Gov.budget;
    }

    public string getText() {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      float funds = calcFunds ();
      po = calcPO ();
      sc = calcSC ();

      return "Yearly Review:\n" +
             "--------------\n\n" +
             "Funding: " + funds + "\n\n" +
             "Public Opinion: " + po + " (" + poChange + " Change)\n" +
             "State Confidence: " + sc + " (" + scChange + " Change)\n\n" +
             "Active Kerbals: " + activeKerbals + " (" + activeKerbalsChange + " Change)\n" +
             "Satelite Coverage: " + sateliteCoverage + " (" + sateliteCoverageChange + " Change)\n" +
             "Resources Mined: " + resourcesMined + " (" + resourcesMinedChange + " Change)\n" +
             "Station Science Accumulated: " + stationScience + " (" + stationScienceChange + " Change)\n" +
             "Govt. Contracts Completed: " + contractsCompleted + "\n" +
             "Govt. Contracts Failed: " + contractsFailed + "\n" +
             "Kerbal \"Accidents\": " + kerbalDeaths + "\n" +
             "Stranded Kerbals: " + strandedKerbals + "\n" +
             "Vessels Destroyed: " + vesselsDestroyed + "\n";
          
    }

  }
}

