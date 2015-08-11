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
      Coverages = new CoverageReport [Bodies.Length-1];

      int k = 0;
      for (int i = 0; i < Bodies.Length; i++) {
        CelestialBody Body = Bodies [i];

        // Don't need to survey the sun
        if (Body.GetName () != "Sun") {
          CoverageReport Report = new CoverageReport ();
          Report.entity = Body.GetName ();

          // Benchmark: Kerbin
          // 10 sats till full coverage on Kerbin
          Report.satCountForFullCoverage = (int)Math.Ceiling (Body.Radius / 60000);

          Coverages [k] = Report;
          k++;
        }
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
    public float satelliteCoverage = 0;

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

      for (int i = 0; i < Coverages.Length; i++) {
        Coverages [i].satCount = 0;
      }

      Vessel[] Satellites = VesselHelper.GetSatellites ();

      for (int i = 0; i < Satellites.Length; i++) {
        Vessel Satellite = Satellites [i];

        CelestialBody Body = Satellite.GetOrbit ().referenceBody;
        CoverageReport Report = GetReport (Body.GetName ());
        Report.satCount++;
        Report.Update ();
      }

      float totalCoverage = 0;
      for (int i = 0; i < Coverages.Length; i++) {
        totalCoverage += Coverages [i].coverage;
      }

      satelliteCoverage = (float)totalCoverage/(float)Coverages.Length;
    }

    private void UpdateActiveKerbals() {
      activeKerbals = KerbalHelper.GetActiveKerbals ().Length;
      strandedKerbals = KerbalHelper.GetStrandedKerbals ().Length;
    }

    private void UpdateMiningRigs() {
      miningRigs = VesselHelper.GetMiningRigs ().Length;
    }

    private void UpdateScienceStations() {
      orbitalScienceStations = VesselHelper.GetOrbitingScienceStations ().Length;
      planetaryScienceStations = VesselHelper.GetLandedScienceStations ().Length;
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

      touch ();

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

