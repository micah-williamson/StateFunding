using System;
using UnityEngine;
using System.IO;

namespace StateFunding {
  public class ViewTextArea: RelativeViewComponent {

    public string label;

    public ViewTextArea (string label) {
      this.label = label;
    }

    public override void paint() {
      GUI.contentColor = color;

      GUI.TextArea (new Rect (
        this.getTopLeftX (),
        this.getTopLeftY (),
        this.getWidth (),
        this.getHeight ()), label, HighLogic.Skin.textArea);
    }
  }
}