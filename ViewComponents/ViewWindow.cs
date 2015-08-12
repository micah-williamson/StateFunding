using System;
using UnityEngine;

namespace StateFunding {
  public class ViewWindow: ViewComponent {

    public string title;
    
    public ViewWindow (string title) {
      this.title = title;
    }

    public void setMargins(int xMargin, int yMargin) {
      this.setLeft (xMargin);
      this.setTop (yMargin);
      this.setWidth (Screen.width - xMargin * 2);
      this.setHeight (Screen.height - yMargin * 2);
    }

    public override void paint() {
      GUI.Box (new Rect (
        this.getTopLeftX (),
        this.getTopLeftY (),
        this.getWidth (),
        this.getHeight ()), title, HighLogic.Skin.window);
    }
  }
}