using System;
using UnityEngine;
using System.Collections.Generic;

namespace StateFunding {
  public class ViewBox: RelativeViewComponent {
    public ViewBox () {}

    public override void paint() {
      GUI.contentColor = color;

      GUI.Box (new Rect (
        this.getTopLeftX (),
        this.getTopLeftY (),
        this.getWidth (),
        this.getHeight ()), "", HighLogic.Skin.box);
    }
  }
}