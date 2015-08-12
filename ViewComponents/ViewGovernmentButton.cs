using System;
using UnityEngine;
using System.IO;

namespace StateFunding {
  public class ViewGovernmentButton: RelativeViewComponent {

    protected Government Gov;
    protected Action <Government>callback;

    public ViewGovernmentButton (Government Gov, Action <Government>callback) {
      this.Gov = Gov;
      this.callback = callback;
    }

    public override void paint() {
      if (GUI.Button (new Rect (
        this.getTopLeftX (),
        this.getTopLeftY (),
        this.getWidth (),
        this.getHeight ()), Gov.name, HighLogic.Skin.button)) {
        callback (Gov);
      }
    }

  }
}