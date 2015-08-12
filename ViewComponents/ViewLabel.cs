using System;
using UnityEngine;
using System.IO;

namespace StateFunding {
  public class ViewLabel: RelativeViewComponent {

    public string label;

    public ViewLabel (string label) {
      this.label = label;
    }

    public override void paint() {
      GUI.contentColor = color;

      GUI.Label (new Rect (
        this.getTopLeftX (),
        this.getTopLeftY (),
        this.getWidth (),
        this.getHeight ()), label, HighLogic.Skin.label);
    }
  }
}