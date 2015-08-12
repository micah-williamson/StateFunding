using System;
using UnityEngine;
using System.IO;

namespace StateFunding {
  public class ViewButton: RelativeViewComponent {

    public string text;
    protected Action callback;

    public ViewButton (string text, Action callback) {
      this.text = text;
      this.callback = callback;
    }

    public override void paint() {
      if (GUI.Button (new Rect (
        this.getTopLeftX (),
        this.getTopLeftY (),
        this.getWidth (),
        this.getHeight ()), text, HighLogic.Skin.button)) {
        callback ();
      }
    }

  }
}