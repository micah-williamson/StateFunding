using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class Review {
    public Review () {
      // Initialize coverage for Celestial Bodies (other than the Sun)

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
    public int finalPO = 0;

    [Persistent]
    public int finalSC = 0;

    [Persistent]
    public int funds = 0;

    [Persistent]
    public int kerbalDeaths = 0;

    [Persistent]
    public int vesselsDestroyed = 0;

    [Persistent]
    public int po = 0;

    [Persistent]
    public bool pastReview = false;

    [Persistent]
    public int miningRigs = 0;

    [Persistent]
    public float satelliteCoverage = 0;

    [Persistent]
    public int sc = 0;

    [Persistent]
    public int orbitalScienceStations = 0;

    [Persistent]
    public int planetaryScienceStations = 0;

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
      po = GameInstance.po;
      sc = GameInstance.sc;
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
      if (!pastReview) {
        UpdatePOSC ();
        UpdateCoverage ();
        UpdateActiveKerbals ();
        UpdateMiningRigs ();
        UpdateScienceStations ();
        UpdateFinalPO ();
        UpdateFinalSC ();
        UpdateFunds ();
        UpdateYear ();
      } else {
        Debug.LogError ("Cannot touch a past review. It's properties are already set");
      }
    }

    public void UpdateYear() {
      year = TimeHelper.Quarters(Planetarium.GetUniversalTime());
    }

    public void UpdateFinalPO() {
      int tmpPO = po;

      Instance Inst = StateFundingGlobal.fetch.GameInstance;
      Government Gov = Inst.Gov;

      // Negatives
      tmpPO -= (int)(5 * kerbalDeaths * Gov.poPenaltyModifier);
      tmpPO -= (int)(5 * strandedKerbals * Gov.poPenaltyModifier);

      // Positives
      tmpPO += (int)(5 * activeKerbals * Gov.poModifier);

      finalPO = tmpPO;
    }

    public void UpdateFinalSC() {
      int tmpSC = sc;

      Instance Inst = StateFundingGlobal.fetch.GameInstance;
      Government Gov = Inst.Gov;

      // Negatives
      tmpSC -= (int)(1 * (vesselsDestroyed) * Gov.scPenaltyModifier);
      tmpSC -= (int)(5 * contractsFailed * Gov.scPenaltyModifier);

      // Positives
      tmpSC += (int)(5 * contractsCompleted * Gov.scModifier);
      tmpSC += (int)(2 * satelliteCoverage * Gov.scModifier);
      tmpSC += (int)(2 * orbitalScienceStations * Gov.scModifier);
      tmpSC += (int)(5 * planetaryScienceStations * Gov.scModifier);
      tmpSC += (int)(5 * miningRigs * Gov.scModifier);

      finalSC = tmpSC;
    }

    private void UpdateFunds() {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;
      funds = (int)(((float)(finalPO + finalSC) / 10000 / 4) * (float)Inst.Gov.gdp * (float)Inst.Gov.budget);
    }

    public string GetText() {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      return "# Review for Quarter: " + year + "\n\n" +
             "Funding: " + funds + "\n\n" +
             "Public Opinion: " + po + "\n" +
             "State Confidence: " + sc + "\n" +
             "Public Opinion After Modifiers & Decay: " + finalPO + "\n" +
             "State Confidence After Modifiers & Decay: " + finalSC + "\n\n" +
             "Active Kerbals: " + activeKerbals + "\n" +
             "Satellite Coverage: " + Math.Round(satelliteCoverage*100) + "%\n" +
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

