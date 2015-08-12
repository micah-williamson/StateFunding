using System;
using UnityEngine;
using System.IO;

namespace StateFunding {
  public class ViewReviewButton: RelativeViewComponent {

    protected Review Rev;
    protected Action <Review>callback;

    public ViewReviewButton (Review Rev, Action <Review>callback) {
      this.Rev = Rev;
      this.callback = callback;
    }

    public override void paint() {
      if (GUI.Button (new Rect (
        this.getTopLeftX (),
        this.getTopLeftY (),
        this.getWidth (),
        this.getHeight ()), "Quarter " + Rev.year, HighLogic.Skin.button)) {
        callback (Rev);
      }
    }

  }
}