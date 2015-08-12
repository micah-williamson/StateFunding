using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFunding {
  public class StateFunding {
    public List<Government> Governments;
    public Government USK;
    public Government USSK;
    public InstanceConfig InstanceConf;
    public ReviewManager ReviewMgr;
    public Instance GameInstance;

    private StateFundingApplicationLauncher AppLauncher;

    public StateFunding () {
      load ();
    }

    private void InitGovernments () {
      Governments = new List<Government> ();

      ConfigNode GovConfig = ConfigNode.Load ("GameData/StateFunding/data/governments.settings");
      ConfigNode[] GovItems = GovConfig.GetNode ("Governments").GetNodes ();
      for (var i = 0; i < GovItems.Length; i++) {
        ConfigNode GovItem = GovItems [i];
        Government Gov = new Government ();

        Gov.name = GovItem.GetValue ("name");
        Gov.longName = GovItem.GetValue ("longName");
        Gov.poModifier = float.Parse(GovItem.GetValue ("poModifier"));
        Gov.poPenaltyModifier = float.Parse(GovItem.GetValue ("poPenaltyModifier"));
        Gov.scModifier = float.Parse (GovItem.GetValue ("scModifier"));
        Gov.scPenaltyModifier = float.Parse (GovItem.GetValue ("scPenaltyModifier"));
        Gov.startingPO = int.Parse (GovItem.GetValue ("startingPO"));
        Gov.startingSC = int.Parse (GovItem.GetValue ("startingSC"));
        Gov.budget = float.Parse (GovItem.GetValue ("budget"));
        Gov.gdp = int.Parse (GovItem.GetValue ("gdp"));
        Gov.description = GovItem.GetValue ("description");

        Governments.Add (Gov);

        Debug.Log ("Loaded Government: " + GovItem.GetValue("name"));
      }

      Debug.Log ("Initialized Governments");
    }

    private void InitEvents() {
      GameEvents.onCrewKilled.Add(OnCrewKilled);
      GameEvents.OnCrewmemberLeftForDead.Add(OnCrewLeftForDead);
      GameEvents.onCrash.Add (OnCrash);
      GameEvents.onCrashSplashdown.Add (OnCrashSplashdown);
    }

    public void unload() {
      ViewManager.removeAll ();
      GameInstance = null;
    }

    public void load () {
      Debug.Log ("StateFunding Mod Loading");
      AppLauncher = new StateFundingApplicationLauncher ();
      InitGovernments ();
      InitEvents ();
      InstanceConf = new InstanceConfig ();
      ReviewMgr = new ReviewManager ();
      loadSave ();
      Debug.Log ("StateFunding Mod Loaded");
    }

    public void LoadIfNeeded() {
      if (GameInstance == null) {
        load ();
      }
    }

    public void loadSave () {
      if (GameInstance == null) {
        if ((GameInstance = InstanceConf.loadInstance ()) == null) {
          InstanceConf.createInstance ((Instance Inst) => {
            GameInstance = Inst;
            ReviewMgr.CompleteReview ();
            InstanceConf.saveInstance (Inst);
          });
        }

        Debug.Log ("StateFunding Save Loaded");
      }
    }

    public void tick() {
      
      if(GameInstance != null) {
        if (GameInstance.getReviews ().Length > 0) {
          int year = (int)(Planetarium.GetUniversalTime () / 60 / 60 / 6 / 426 * 4);
          if (year > ReviewMgr.LastReview ().year) {
            Debug.Log ("Happy New Year!");
            ReviewMgr.CompleteReview ();
          }
        }
      }

    }

    // Events

    public void OnCrewKilled(EventReport Evt) {
      Debug.LogWarning ("CREW KILLED");
      GameInstance.ActiveReview.kerbalDeaths++;
    }

    public void OnCrewLeftForDead(ProtoCrewMember Crew, int id) {
      Debug.LogWarning ("CREW KILLED");
      GameInstance.ActiveReview.kerbalDeaths++;
    }

    public void OnCrash(EventReport Evt) {
      if (VesselHelper.PartHasModuleAlias (Evt.origin, "Command") || VesselHelper.PartHasModuleAlias (Evt.origin, "AutonomousCommand")) {
        Debug.LogWarning ("VESSEL DESTROYED");
        GameInstance.ActiveReview.vesselsDestroyed++;
      }
    }

    public void OnCrashSplashdown(EventReport Evt) {
      if (VesselHelper.PartHasModuleAlias(Evt.origin, "Command") || VesselHelper.PartHasModuleAlias(Evt.origin, "AutonomousCommand")) {
        Debug.LogWarning ("VESSEL DESTROYED");
        GameInstance.ActiveReview.vesselsDestroyed++;
      }
    }

  }
}

