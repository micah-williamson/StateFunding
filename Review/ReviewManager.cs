using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class ReviewManager: MonoBehaviour {

    public void CompleteReview () {
      Instance Inst = StateFundingGlobal.fetch.GameInstance;

      Review Rev = Inst.ActiveReview;
      Rev.touch ();

      // Closed for business
      Rev.pastReview = true;

      // Move review to past review
      Inst.addReview (Rev);

      // Start a new review
      Inst.ActiveReview = new Review ();

      // Apply PO/SC decay on instance
      ApplyDecay ();

      // Apply funds from Review
      Debug.Log("Adding Funds: " + Rev.funds);
      Funding.Instance.AddFunds (Rev.funds, TransactionReasons.None);

      // Notify player that a review is available
      ReviewToastView Toast = new ReviewToastView (Rev);

      // Save the instance and game
      StateFundingGlobal.fetch.InstanceConf.saveInstance (Inst);
      GamePersistence.SaveGame ("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);

      Debug.Log ("Generated Review");
    }

    public void ApplyDecay() {
      Debug.Log ("Applying Decay");
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

