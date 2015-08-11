using System;
using System.Collections;
using UnityEngine;

namespace StateFunding {
  public class StateFunding {
    public ArrayList Governments;
    public Government USK;
    public Government USSK;
    public InstanceConfig InstanceConf;
    public ReviewManager ReviewMgr;
    public Instance GameInstance;

    private StateFundingApplicationLauncher AppLauncher;

    public StateFunding () {
      AppLauncher = new StateFundingApplicationLauncher ();
      InitGovernments ();
      InitEvents ();
    }

    private void InitGovernments () {
      Governments = new ArrayList ();
      USK = new Government ();
      USSK = new Government ();

      USK.budget = 0.2f;
      USK.gdp = 80000000;
      USK.longName = "United States of Kirba";
      USK.description = "For the people, by the people. At least that's what they say. With the USK the peoples opinions have " +
                        "a greater impact on your funding. Their hearts pour out for Kerbals, not robots and machines. Because of " +
                        "this, the more Kerbals on missions the more interested the people of USK will be in funding the your " +
                        "space program. But there's an equal- well not really equal- reaction to every action. While having Kerbals " +
                        "on missions is crucial to funding, Kerbal death will be devastating in performance reviews. Every life is " +
                        "special and there isn't much room for failure.";
      USK.gameplayDescription = "GDP: 80,000,000\n" +
                                "Yearly Budget: 16,000,000\n" + 
                                "Starting KSP Budget: 80,000\n" +
                                "Starting PO: 40\n" +
                                "Starting SC: 10\n" +
                                "State Reward: Low\n" +
                                "State Penalty: Low\n" +
                                "Public Reward: High\n" +
                                "Public Penalty: Very High\n";
      USK.name = "USK";
      USK.poModifier = 2;
      USK.poPenaltyModifier = 3;
      USK.scModifier = 1;
      USK.scPenaltyModifier = 1;
      USK.startingPO = 20;
      USK.startingSC = 30;

      USSK.budget = 0.2f;
      USSK.gdp = 10000000;
      USSK.longName = "The Union of Soviet Socialist Kerbals";
      USSK.name = "USSK";
      USSK.description = "The needs of the many outweigh the needs of the few. The USSK are more interested in results and it's " +
                         "not as important how you get those results. While the citizens care greatly about their comrades in " +
                         "space, the governemnt isn't all too revealing about their operations and furthermore don't care too much " +
                         "about the peoples opinions anyway. Kerbal achievement and death will not reach the morning newspaper of many " +
                         "individuals in this country and as a result the impact of such achievements and failures will be muted. A " +
                         "greater reliance on unmaned vehicles and surveilance will be your best bet for continued funding.";
      USSK.gameplayDescription = "GDP: 10,000,000\n" +
                                "Yearly Budget: 2,000,000\n" + 
                                "Starting KSP Budget: 60,000\n" +
                                "Starting PO: 50\n" +
                                "Starting SC: 250\n" +
                                "State Reward: High\n" +
                                "State Penalty: Very High\n" +
                                "Public Reward: Low\n" +
                                "Public Penalty: Low\n";
      USSK.poModifier = 0.5f;
      USSK.poPenaltyModifier = 0.5f;
      USSK.scModifier = 1;
      USSK.scPenaltyModifier = 2;
      USSK.startingPO = 50;
      USSK.startingSC = 250;

      Governments.Add (USK);
      Governments.Add (USSK);

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
            ReviewMgr.GenerateReview ();
            InstanceConf.saveInstance (Inst);
          });
        }

        Debug.Log ("StateFunding Save Loaded");
      }
    }

    public void tick() {
      
      if(GameInstance != null) {
        if (GameInstance.getReviews ().Length > 0) {
          int year = (int)(Planetarium.GetUniversalTime () / 60 / 60 / 6 / 426);
          if (year > ReviewMgr.LastReview ().year) {
            Debug.Log ("Happy New Year!");
            ReviewMgr.GenerateReview ();
          }
        }
      }

    }

    // Events

    public void OnCrewKilled(EventReport Evt) {
      GameInstance.ActiveReview.kerbalDeaths++;
    }

    public void OnCrewLeftForDead(ProtoCrewMember Crew, int id) {
      GameInstance.ActiveReview.kerbalDeaths++;
    }

    public void OnCrash(EventReport Evt) {
      GameInstance.ActiveReview.vesselsDestroyed++;
    }

    public void OnCrashSplashdown(EventReport Evt) {
      GameInstance.ActiveReview.vesselsDestroyed++;
    }

  }
}

