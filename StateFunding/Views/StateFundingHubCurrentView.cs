using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubCurrentView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Current State";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      ViewTextArea TextArea = new ViewTextArea (GameInstance.ActiveReview.GetText());
      TextArea.setRelativeTo (Window);
      TextArea.setTop (40);
      TextArea.setLeft (130);
      TextArea.setWidth (Window.getWidth () - 140);
      TextArea.setHeight (Window.getHeight () - 40);
      TextArea.setColor (Color.white);

      Vw.addComponent (TextArea);
    }
  }
}

