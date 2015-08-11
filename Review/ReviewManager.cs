using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class ReviewManager: MonoBehaviour {

    public void GenerateReview () {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      Review Rev = Inst.ActiveReview;
      Rev.year = (int)(Planetarium.GetUniversalTime()/60/60/6/426);
      Rev.touch ();

      ReviewToastView Toast = new ReviewToastView (Rev);

      ApplyFunding (Rev);

      Debug.Log ("Generated Review");
    }

    public void ApplyFunding(Review Rev) {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      Funding.Instance.AddFunds (Rev.funds, TransactionReasons.None);

      Inst.addReview (Inst.ActiveReview);
      Inst.ActiveReview = new Review ();
      ApplyDecay ();

      StateFundingGlobal.fetch.InstanceConf.saveInstance (Inst);
      GamePersistence.SaveGame ("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
    }

    public void ApplyDecay() {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;
      if (Inst.po > 0) {
        int newPO = Inst.po - (int)Math.Ceiling (Inst.po * 0.2);
        newPO = Math.Max (0, newPO);

        Inst.po = newPO;
      } else {
        int newPO = Inst.po += (int)Math.Ceiling (Inst.po * -0.2);
        newPO = Math.Min (0, newPO);

        Inst.po = newPO;
      }

      if (Inst.sc > 0) {
        int newSC = Inst.sc - (int)Math.Ceiling (Inst.sc * 0.2);
        newSC = Math.Max (0, newSC);

        Inst.sc = newSC;
      } else {
        int newSC = Inst.sc += (int)Math.Ceiling (Inst.sc * -0.2);
        newSC = Math.Min (0, newSC);

        Inst.sc = newSC;
      }
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

