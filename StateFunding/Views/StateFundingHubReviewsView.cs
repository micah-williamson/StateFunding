using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubReviewsView {

    private static View curView;
    private static ViewWindow curWindow;

    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Past Reviews";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;

      curView = Vw;
      curWindow = Window;

      int buttonWidth = 180;
      int buttonHeight = 30;
      int buttonMargin = 10;
      int xOffset = 0;
      int yOffset = 0;

      ViewScroll PastReviewsScroll = new ViewScroll ();
      PastReviewsScroll.setRelativeTo (Window);
      PastReviewsScroll.setWidth (Window.getWidth () - 140);
      PastReviewsScroll.setHeight (Window.getHeight () - 50);
      PastReviewsScroll.setLeft (140);
      PastReviewsScroll.setTop (40);

      Vw.addComponent (PastReviewsScroll);

      for (int i = GameInstance.getReviews ().Length - 1; i >= 0; i--) {
        Review Rev = GameInstance.Reviews[i];

        ViewReviewButton Btn = new ViewReviewButton (Rev, OnReviewClick);
        Btn.setRelativeTo (PastReviewsScroll);

        int left = 0;
        int top = yOffset * buttonMargin + yOffset * buttonHeight;

        Btn.setLeft (left);
        Btn.setTop (top);
        Btn.setWidth (buttonWidth);
        Btn.setHeight (buttonHeight);
        Btn.setColor (Color.white);

        yOffset++;

        PastReviewsScroll.Components.Add (Btn);
      }
    }
      

    public static void OnReviewClick(Review Rev) {
      StateFundingGlobal.fetch.ReviewMgr.OpenReview(Rev);
    }
  }
}

