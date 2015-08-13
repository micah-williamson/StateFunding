using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace StateFunding {
  public class ViewScroll: RelativeViewComponent {

    public List<ViewComponent> Components = new List<ViewComponent>();
    private Vector2 ScrollVector = Vector2.zero;

    public ViewScroll () {}

    public override void paint() {
      GUI.contentColor = color;

      int bottomX = 0;
      int bottomY = 0;

      for (var i = 0; i < Components.ToArray ().Length; i++) {
        ViewComponent Component = Components.ToArray () [i];
        if (Component.getBottomRightX () > bottomX) {
          bottomX = Component.getBottomRightX ();
        }

        if (Component.getBottomRightY () > bottomY) {
          bottomY = Component.getBottomRightY ();
        }
      }

      ScrollVector = GUI.BeginScrollView (
        new Rect (this.getTopLeftX (), this.getTopLeftY (), this.getWidth (), this.getHeight ()),
        ScrollVector,
        new Rect (0, 0, bottomX, bottomY),
        HighLogic.Skin.horizontalScrollbar,
        HighLogic.Skin.verticalScrollbar
      );

      for (var i = 0; i < Components.ToArray ().Length; i++) {
        ViewComponent Component = Components.ToArray () [i];
        Component.paint ();
      }

      GUI.EndScrollView ();
    }
  }
}