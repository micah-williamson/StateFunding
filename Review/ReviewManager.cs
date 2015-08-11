using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class ReviewManager: MonoBehaviour {

    private ReviewApplicationLauncher AppLauncher;

    public ReviewManager () {
      AppLauncher = new ReviewApplicationLauncher ();
    }

    public void GenerateReview () {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      Review Rev = Inst.ActiveReview;
      Rev.po = Inst.po;
      Rev.sc = Inst.sc;
      Rev.year = (int)(Planetarium.GetUniversalTime()/60/60/6/426);

      IEnumerator Kerbals = HighLogic.CurrentGame.CrewRoster.Crew.GetEnumerator();

      while(Kerbals.MoveNext()) {
        ProtoCrewMember Kerbal = (ProtoCrewMember)Kerbals.Current;
        if (Kerbal.rosterStatus.ToString() == "Assigned") {
          Rev.activeKerbals++;
        }
      }

      ReviewToastView Toast = new ReviewToastView (Rev);

      ApplyFunding (Rev);

      Debug.Log ("Generated Review");
    }

    public void ApplyFunding(Review Rev) {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      Funding.Instance.AddFunds (Rev.calcFunds (), TransactionReasons.None);

      Inst.addReview (Inst.ActiveReview);
      Inst.ActiveReview = new Review ();

      StateFundingGlobal.fetch.InstanceConf.saveInstance (Inst);
      GamePersistence.SaveGame ("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
    }

    public void OpenReview(Review Rev) {
      Debug.Log ("Viewing Review");
      ReviewView RevView = new ReviewView (Rev);
    }

    public Review LastReview() {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;
      Review[] Reviews = Inst.getReviews ();
      return Reviews [Reviews.Length - 1];
    }

  }
}

