using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class Review {
    public Review () {
      // Initialize coverage for Celestial Bodies (other than the Sun)

      CelestialBody[] Bodies = FlightGlobals.Bodies.ToArray ();
      Coverages = new CoverageReport [Bodies.Length-1];

      CelestialBody home = Planetarium.fetch.Home;
      if (home == null) home = FlightGlobals.Bodies.Find(body => body.isHomeWorld == true);
      if (home != null) refRadius = home.Radius / 10;

      int k = 0;
      for (int i = 0; i < Bodies.Length; i++) {
        CelestialBody Body = Bodies [i];

        // Don't need to survey the sun
        if (Body.GetName () != "Sun") {
          CoverageReport Report = new CoverageReport ();
          Report.entity = Body.GetName ();

          // Benchmark: Kerbin
          // 10 sats till full coverage on Kerbin
          Report.satCountForFullCoverage = (int)Math.Ceiling (Body.Radius / refRadius);

          Coverages [k] = Report;
          k++;
        }
      }
    }
    
    public double refRadius = 60000.0;

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
    public int rovers = 0;

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
    public SpaceStationReport[] SpaceStations;

    [Persistent]
    public BaseReport[] Bases;

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
      Debug.Log ("Updating POSC");
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      po = GameInstance.po;
      sc = GameInstance.sc;
    }

    private void UpdateCoverage() {
      Debug.Log ("Updating Coverage");

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
      Debug.Log ("Updating Active Kerbals");
      activeKerbals = KerbalHelper.GetActiveKerbals ().Length;
      strandedKerbals = KerbalHelper.GetStrandedKerbals ().Length;
    }

    private void UpdateMiningRigs() {
      Debug.Log ("Updating Mining Rigs");
      miningRigs = VesselHelper.GetMiningRigs ().Length;
    }

    private void UpdateScienceStations() {
      Debug.Log ("Updating Science Stations");
      orbitalScienceStations = VesselHelper.GetOrbitingScienceStations ().Length;
      planetaryScienceStations = VesselHelper.GetLandedScienceStations ().Length;
    }

    private void UpdateRovers() {
      Debug.Log ("Updating Rovers");
      rovers = VesselHelper.GetRovers ().Length;
    }

    private void UpdateSpaceStations() {
      Debug.Log ("Updating Space Stations");

      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Vessel[] SpcStations = VesselHelper.GetSpaceStations ();
      SpaceStations = new SpaceStationReport[SpcStations.Length];

      for (int i = 0; i < SpcStations.Length; i++) {
        Vessel SpcStation = SpcStations [i];

        SpaceStationReport SpcStationReport = new SpaceStationReport ();
        SpcStationReport.name = SpcStation.vesselName;
        SpcStationReport.crew = VesselHelper.GetCrew (SpcStation).Length;
        SpcStationReport.crewCapacity = VesselHelper.GetCrewCapactiy (SpcStation);
        SpcStationReport.dockedVessels = VesselHelper.GetDockedVesselsCount (SpcStation);
        SpcStationReport.dockingPorts = VesselHelper.GetDockingPorts (SpcStation).Length;
        SpcStationReport.drill = VesselHelper.VesselHasModuleAlias (SpcStation, "Drill");
        SpcStationReport.scienceLab = VesselHelper.VesselHasModuleAlias (SpcStation, "ScienceLab");
        SpcStationReport.fuel = VesselHelper.GetResourceCount (SpcStation, "LiquidFuel");
        SpcStationReport.ore = VesselHelper.GetResourceCount (SpcStation, "Ore");
        SpcStationReport.onAstroid = VesselHelper.OnAstroid (SpcStation);

        if (SpcStation.Landed) {
          SpcStationReport.entity = SpcStation.landedAt;
        } else {
          SpcStationReport.entity = SpcStation.GetOrbit ().referenceBody.GetName ();
        }

        SpcStationReport.po = 0;
        SpcStationReport.sc = 0;

        SpcStationReport.po += (int)(5 * SpcStationReport.crew * GameInstance.Gov.poModifier);
        SpcStationReport.po += (int)(5 * SpcStationReport.dockedVessels * GameInstance.Gov.poModifier);

        if (SpcStationReport.onAstroid) {
          SpcStationReport.po += (int)(30 * GameInstance.Gov.poModifier);

          if (SpcStationReport.drill) {
            SpcStationReport.po += (int)(10 * GameInstance.Gov.poModifier);
            SpcStationReport.sc += (int)(10 * GameInstance.Gov.poModifier);
          }
        }

        SpcStationReport.sc += (int)(2 * SpcStationReport.crewCapacity * GameInstance.Gov.scModifier);
        SpcStationReport.sc += (int)(SpcStationReport.fuel / 200f * GameInstance.Gov.scModifier);
        SpcStationReport.sc += (int)(SpcStationReport.ore / 200f * GameInstance.Gov.scModifier);
        SpcStationReport.sc += (int)(2 * SpcStationReport.dockingPorts * GameInstance.Gov.scModifier);
        SpcStationReport.sc += (int)(2 * SpcStationReport.crewCapacity * GameInstance.Gov.scModifier);

        if (SpcStationReport.scienceLab) {
          SpcStationReport.po += (int)(10 * GameInstance.Gov.poModifier);
          SpcStationReport.sc += (int)(10 * GameInstance.Gov.poModifier);
        }

        SpaceStations [i] = SpcStationReport;
      }
    }

    private void UpdateBases() {
      Debug.Log ("Updating Bases");

      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Vessel[] _Bases = VesselHelper.GetBases ();
      Bases = new BaseReport[_Bases.Length];

      for (int i = 0; i < _Bases.Length; i++) {
        Vessel Base = _Bases [i];
        BaseReport _BaseReport = new BaseReport ();
        _BaseReport.name = Base.vesselName;
        _BaseReport.crew = VesselHelper.GetCrew (Base).Length;
        _BaseReport.crewCapacity = VesselHelper.GetCrewCapactiy (Base);
        _BaseReport.dockedVessels = VesselHelper.GetDockedVesselsCount (Base);
        _BaseReport.dockingPorts = VesselHelper.GetDockingPorts (Base).Length;
        _BaseReport.drill = VesselHelper.VesselHasModuleAlias (Base, "Drill");
        _BaseReport.scienceLab = VesselHelper.VesselHasModuleAlias (Base, "ScienceLab");
        _BaseReport.fuel = VesselHelper.GetResourceCount (Base, "LiquidFuel");
        _BaseReport.ore = VesselHelper.GetResourceCount (Base, "Ore");
        _BaseReport.entity = Base.mainBody.name;

        _BaseReport.po = 0;
        _BaseReport.sc = 0;

        _BaseReport.po += (int)(5 * _BaseReport.crew * GameInstance.Gov.poModifier);
        _BaseReport.po += (int)(5 * _BaseReport.dockedVessels * GameInstance.Gov.poModifier);
        _BaseReport.po += (int)((Base.mainBody.Radius / refRadius) * (_BaseReport.dockedVessels + 1) * GameInstance.Gov.poModifier);

        _BaseReport.sc += (int)(2 * _BaseReport.crewCapacity * GameInstance.Gov.scModifier);
        _BaseReport.sc += (int)(_BaseReport.fuel / 200f * GameInstance.Gov.scModifier);
        _BaseReport.sc += (int)(_BaseReport.ore / 200f * GameInstance.Gov.scModifier);
        _BaseReport.sc += (int)(2 * _BaseReport.dockingPorts * GameInstance.Gov.scModifier);
        _BaseReport.sc += (int)(2 * _BaseReport.crewCapacity * GameInstance.Gov.scModifier);

        if (_BaseReport.scienceLab) {
          _BaseReport.po += (int)(10 * GameInstance.Gov.poModifier);
          _BaseReport.sc += (int)(10 * GameInstance.Gov.poModifier);
        }

        if (_BaseReport.drill) {
          _BaseReport.po += (int)(10 * GameInstance.Gov.poModifier);
          _BaseReport.sc += (int)(10 * GameInstance.Gov.poModifier);
        }

        Bases [i] = _BaseReport;
      }
    }

    public void touch() {
      if (!pastReview) {
        UpdatePOSC ();
        UpdateCoverage ();
        UpdateActiveKerbals ();
        UpdateMiningRigs ();
        UpdateScienceStations ();
        UpdateSpaceStations ();
        UpdateBases ();
        UpdateRovers ();
        UpdateFinalPO ();
        UpdateFinalSC ();
        UpdateFunds ();
        UpdateYear ();
      } else {
        Debug.LogError ("Cannot touch a past review. It's properties are already set");
      }
    }

    public void UpdateYear() {
      Debug.Log ("Updating Year");
      year = TimeHelper.Quarters(Planetarium.GetUniversalTime());
    }

    public void UpdateFinalPO() {
      Debug.Log ("Updating Final PO");
      int tmpPO = po;

      InstanceData Inst = StateFundingGlobal.fetch.GameInstance;
      Government Gov = Inst.Gov;

      // Negatives
      tmpPO -= (int)(5 * kerbalDeaths * Gov.poPenaltyModifier);
      tmpPO -= (int)(5 * strandedKerbals * Gov.poPenaltyModifier);

      // Positives
      tmpPO += (int)(5 * activeKerbals * Gov.poModifier);
      tmpPO += (int)(5 * rovers * Gov.poModifier);

      for (int i = 0; i < SpaceStations.Length; i++) {
        tmpPO += SpaceStations [i].po;
      }

      for (int i = 0; i < Bases.Length; i++) {
        tmpPO += Bases [i].po;
      }

      finalPO = tmpPO;
    }

    public void UpdateFinalSC() {
      Debug.Log ("Updating Final SC");
      int tmpSC = sc;

      InstanceData Inst = StateFundingGlobal.fetch.GameInstance;
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

      for (int i = 0; i < SpaceStations.Length; i++) {
        tmpSC += SpaceStations [i].sc;
      }

      for (int i = 0; i < Bases.Length; i++) {
        tmpSC += Bases [i].sc;
      }

      finalSC = tmpSC;
    }

    private void UpdateFunds() {
      Debug.Log ("Updating Funds");
      InstanceData Inst = StateFundingGlobal.fetch.GameInstance;
      funds = (int)(((float)(finalPO + finalSC) / 10000 / 4) * (float)Inst.Gov.gdp * (float)Inst.Gov.budget);
    }

    public string GetText() {
      //InstanceData Inst = StateFundingGlobal.fetch.GameInstance;

      string returnText = "# Review for Quarter: " + year + "\n\n" +
                          "Funding: " + funds + "\n\n" +
                          "Public Opinion: " + po + "\n" +
                          "State Confidence: " + sc + "\n" +
                          "Public Opinion After Modifiers & Decay: " + finalPO + "\n" +
                          "State Confidence After Modifiers & Decay: " + finalSC + "\n\n" +
                          "Active Kerbals: " + activeKerbals + "\n" +
                          "Satellite Coverage: " + Math.Round(satelliteCoverage*100) + "%\n" +
                          "Active Mining Rigs: " + miningRigs + "\n" +
                          "Rovers: " + rovers + "\n" +
                          "Obital Science Stations: " + orbitalScienceStations + "\n" +
                          "Planetary Science Stations: " + planetaryScienceStations + "\n" +
                          "Govt. Contracts Completed: " + contractsCompleted + "\n" +
                          "Govt. Contracts Failed: " + contractsFailed + "\n" +
                          "Kerbal \"Accidents\": " + kerbalDeaths + "\n" +
                          "Stranded Kerbals: " + strandedKerbals + "\n" +
                          "Vessels Destroyed: " + vesselsDestroyed;

      if ((SpaceStations != null) && (SpaceStations.Length > 0)) {
        returnText += "\n\n== Space Stations ==\n\n";
        for (int i = 0; i < SpaceStations.Length; i++) {
          SpaceStationReport StationReport = SpaceStations [i];
          returnText += "[" + StationReport.name + " Orbiting " + StationReport.entity + "]\n";
          returnText += "Fuel: " + StationReport.fuel + "\n";
          returnText += "Ore: " + StationReport.ore + "\n";
          returnText += "Crew: " + StationReport.crew + "\n";
          returnText += "Crew Capacity: " + StationReport.crewCapacity + "\n";
          returnText += "Docked Vessels: " + StationReport.dockedVessels + "\n";
          returnText += "Docking Ports: " + StationReport.dockingPorts + "\n";
          returnText += "Has Drill: " + StationReport.drill + "\n";
          returnText += "Science Lab: " + StationReport.scienceLab + "\n";
          returnText += "On Astroid: " + StationReport.onAstroid + "\n";
          returnText += "PO: " + StationReport.po + "\n";
          returnText += "SC: " + StationReport.sc + "\n\n";
        }
      }

      if ((Bases != null) && (Bases.Length > 0)) {
        returnText += "\n\n== Bases ==\n\n";
        for (int i = 0; i < Bases.Length; i++) {
          BaseReport Base = Bases [i];
          returnText += "[" + Base.name + " Landed At " + Base.entity + "]\n";
          returnText += "Fuel: " + Base.fuel + "\n";
          returnText += "Ore: " + Base.ore + "\n";
          returnText += "Crew: " + Base.crew + "\n";
          returnText += "Crew Capacity: " + Base.crewCapacity + "\n";
          returnText += "Docked Vessels: " + Base.dockedVessels + "\n";
          returnText += "Docking Ports: " + Base.dockingPorts + "\n";
          returnText += "Has Drill: " + Base.drill + "\n";
          returnText += "Science Lab: " + Base.scienceLab + "\n";
          returnText += "PO: " + Base.po + "\n";
          returnText += "SC: " + Base.sc + "\n\n";
        }
      }

      return returnText;
          
    }

  }
}

