using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class Review {
    public Review () {
      Init (StateFundingGlobal.fetch.GameInstance);
    }

    public Review (Instance Inst) {
      Init (Inst);
    }

    private void Init(Instance Inst) {
      CelestialBody[] Bodies = FlightGlobals.Bodies.ToArray ();
      Coverages = new CoverageReport [Bodies.Length];

      for (int i = 0; i < Bodies.Length; i++) {
        CelestialBody Body = Bodies [i];

        CoverageReport Report = new CoverageReport ();
        Report.entity = Body.GetName ();

        // Benchmark: Kerbin
        // 10 sats till full coverage on Kerbin
        Report.satCountForFullCoverage = (int)Math.Ceiling (Body.Radius / 60000);

        Coverages [i] = Report;
      }
    }

    [Persistent]
    public int activeKerbals = 0;

    // TODO
    [Persistent]
    public int contractsCompleted = 0;

    // TODO
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

    // TODO
    [Persistent]
    public int poChange = 0;

    [Persistent]
    public int miningRigs = 0;

    [Persistent]
    public int satelliteCoverage = 0;

    [Persistent]
    public int sc = 0;

    // TODO
    [Persistent]
    public int scChange = 0;

    [Persistent]
    public int orbitalScienceStations = 0;

    [Persistent]
    public int planetaryScienceStations = 0;

    // TODO
    [Persistent]
    public int strandedKerbals = 0;

    [Persistent]
    public int year = 0;

    public CoverageReport GetReport(string bodyName) {
      for (var i = 0; i < Coverages.Length; i++) {
        CoverageReport Report = Coverages [i];
        if (Report.entity == bodyName) {
          return Report;
        }
      }

      CoverageReport CReport = new CoverageReport();
      CReport.entity = bodyName;

      return CReport;
    }

    private void UpdatePOSC() {
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;
      po = GameInstance.Gov.startingPO;
      sc = GameInstance.Gov.startingSC;
    }

    private void UpdateCoverage() {
      
      Vessel[] Satellites = VesselHelper.GetVesselsWithModules(new string[] {
        "ModuleDeployableSolarPanel",
        "ModuleDataTransmitter",
        "ModuleSAS"
      });

      for (int i = 0; i < Satellites.Length; i++) {
        Vessel Satellite = Satellites [i];

        if (!Satellite.Landed) {
          if (Satellite.GetOrbit() != null) {
            CelestialBody Body = Satellite.GetOrbit ().referenceBody;
            CoverageReport Report = GetReport (Body.GetName ());
            Report.satCount++;
            Report.update ();
          }
        }
      }

      float totalCoverage = 0;
      for (int i = 0; i < Coverages.Length; i++) {
        totalCoverage += Coverages [i].coverage;
      }

      satelliteCoverage = (int)totalCoverage / Coverages.Length;
    }

    private void UpdateActiveKerbals() {
      activeKerbals = KerbalHelper.getAssignedKerbalCount ();

    }

    private void UpdateMiningRigs() {
      Vessel[] MiningRigs = VesselHelper.GetVesselsWithModules(new string[] {
        "ModuleDeployableSolarPanel",
        "ModuleDataTransmitter",
        "ModuleResourceHarvester"
      });

      for (var i = 0; i < MiningRigs.Length; i++) {
        Vessel MiningRig = MiningRigs [i];
        Debug.LogWarning ("Found a science lab with crew");
        // Planetary science station

        if (MiningRig.Landed && MiningRig.landedAt != SpaceCenter.Instance.cb.GetName()) {
          Debug.Log ("It's Landed");
          orbitalScienceStations++;
        }

      }
    }

    private void UpdateScienceStations() {
      Vessel[] ScienceLabs = VesselHelper.GetVesselsWithModules(new string[] {
        "ModuleDeployableSolarPanel",
        "ModuleDataTransmitter",
        "ModuleScienceLab"
      });

      for (var i = 0; i < ScienceLabs.Length; i++) {
        Vessel ScienceLab = ScienceLabs [i];

        if (ScienceLab.GetCrewCount () > 0) {
          Debug.LogWarning ("Found a science lab with crew");

          if (ScienceLab.Landed && ScienceLab.landedAt != SpaceCenter.Instance.cb.GetName()) {
            Debug.Log ("It's Landed");
            orbitalScienceStations++;
          } else if (!ScienceLab.Landed) {
            Debug.Log ("It's Orbital");
            planetaryScienceStations++;
          }
        }
      }
    }

    public void touch() {
      UpdatePOSC ();
      UpdateCoverage ();
      UpdateActiveKerbals ();
      UpdateMiningRigs ();
      UpdateScienceStations ();
    }

    public int CalcPO() {
      int tmpPo = po;

      Instance Inst = StateFundingGlobal.fetch.GameInstance;
      Government Gov = Inst.Gov;

      touch ();

      tmpPo -= (int)(5 * kerbalDeaths * Gov.poPenaltyModifier);
      tmpPo += (int)(3 * activeKerbals * Gov.poModifier);
      tmpPo += (int)(3 * activeKerbals * Gov.poModifier);

      return tmpPo;
    }

    public int CalcSC() {
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

    public float CalcFunds() {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      return (float)((float)(CalcPO() + CalcSC()) / 10000)*(float)Inst.Gov.gdp*(float)Inst.Gov.budget;
    }

    public string GetText() {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      float funds = CalcFunds ();
      po = CalcPO ();
      sc = CalcSC ();

      return "Yearly Review:\n" +
             "--------------\n\n" +
             "Funding: " + funds + "\n\n" +
             "Public Opinion: " + po + " (" + poChange + " Change)\n" +
             "State Confidence: " + sc + " (" + scChange + " Change)\n\n" +
             "Active Kerbals: " + activeKerbals + "\n" +
             "Satellite Coverage: " + satelliteCoverage + "\n" +
             "Active Mining Rigs: " + miningRigs + "\n" +
             "Obital Science Stations: " + orbitalScienceStations + "\n" +
             "Planetary Science Stations: " + planetaryScienceStations + "\n" +
             "Govt. Contracts Completed: " + contractsCompleted + "\n" +
             "Govt. Contracts Failed: " + contractsFailed + "\n" +
             "Kerbal \"Accidents\": " + kerbalDeaths + "\n" +
             "Stranded Kerbals: " + strandedKerbals + "\n" +
             "Vessels Destroyed: " + vesselsDestroyed + "\n";
          
    }

  }
}

