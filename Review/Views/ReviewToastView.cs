using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class ReviewToastView: View {
    private Review Rev;
    private ViewWindow Toast;
    private ViewLabel ToastLabel;
    private ViewButton Dismiss;
    private ViewButton OpenReview;

    public ReviewToastView (Review Rev) {
      this.Rev = Rev;
      ViewManager.addView (this);
      createWindow ();
    }

    private void createWindow() {
      Toast = new ViewWindow ("");
      Toast.setWidth (300);
      Toast.setHeight (100);
      Toast.setBottom (10);
      Toast.setRight (10);

      ToastLabel = new ViewLabel ("New Review Report Avaialble");
      ToastLabel.setRelativeTo (Toast);
      ToastLabel.setWidth (290);
      ToastLabel.setHeight (90);
      ToastLabel.setLeft (10);
      ToastLabel.setTop (10);
      ToastLabel.setColor (Color.white);

      OpenReview = new ViewButton ("View Review", OnOpenReview);
      OpenReview.setRelativeTo (Toast);
      OpenReview.setWidth (90);
      OpenReview.setHeight (40);
      OpenReview.setRight (10);
      OpenReview.setTop (10);

      Dismiss = new ViewButton ("Dismiss", OnDismiss);
      Dismiss.setRelativeTo (Toast);
      Dismiss.setWidth (90);
      Dismiss.setHeight (40);
      Dismiss.setRight (10);
      Dismiss.setBottom (10);

      this.addComponent (Toast);
      this.addComponent (ToastLabel);
      this.addComponent (OpenReview);
      this.addComponent (Dismiss);
    }

    private void OnOpenReview() {
      ViewManager.removeView (this);
      StateFundingGlobal.fetch.ReviewMgr.OpenReview (this.Rev);
    }

    private void OnDismiss() {
      ViewManager.removeView (this);
    }
  }
}

