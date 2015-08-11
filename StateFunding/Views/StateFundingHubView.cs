using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class StateFundingHubView: View {
    private ViewWindow Window;
    private ViewButton PastReviews;
    private ViewButton SFGuide;
    private ViewLabel CurrentView;

    public StateFundingHubView () {
      createWindow ();
    }

    private void createWindow() {
      reloadBase ();
    }

    private void reloadBase() {
      this.removeAll ();

      Window = new ViewWindow ("");
      Window.setMargins (300, 100);

      PastReviews = new ViewButton ("Past Reviews", LoadPastReviews);
      PastReviews.setRelativeTo (Window);
      PastReviews.setLeft (10);
      PastReviews.setTop (10);
      PastReviews.setWidth (120);
      PastReviews.setHeight (35);

      SFGuide = new ViewButton ("SF Guide", LoadGuide);
      SFGuide.setRelativeTo (Window);
      SFGuide.setLeft (10);
      SFGuide.setTop (55);
      SFGuide.setWidth (120);
      SFGuide.setHeight (35);

      CurrentView = new ViewLabel ("");
      CurrentView.setRelativeTo (Window);
      CurrentView.setLeft (140);
      CurrentView.setWidth (Window.getWidth () - 140);
      CurrentView.setHeight (30);
      CurrentView.setColor (Color.white);

      this.addComponent (Window);
      this.addComponent (PastReviews);
      this.addComponent (SFGuide);
      this.addComponent (CurrentView);
    }

    private void LoadPastReviews() {
      reloadBase ();

      CurrentView.label = "Past Reviews";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;

      int buttonWidth = 60;
      int buttonHeight = 20;
      int buttonMargin = 10;
      int xOffset = 0;
      int yOffset = 0;

      for (int i = GameInstance.getReviews ().Length - 1; i >= 0; i--) {
        Review Rev = GameInstance.getReviews () [i];

        ViewReviewButton Btn = new ViewReviewButton (Rev, OnReviewClick);
        Btn.setRelativeTo (Window);

        int left = 140 + xOffset * buttonMargin + xOffset * buttonWidth;
        int top = 40 + yOffset * buttonMargin + yOffset * buttonHeight;
        int absoluteRight = Window.getTopLeftX() + left + buttonWidth + 10;

        if (absoluteRight > Window.getBottomRightX()) {
          xOffset = 0;
          yOffset++;

          left = 140 + xOffset * buttonMargin + xOffset * buttonWidth;
          top = 40 + yOffset * buttonMargin + yOffset * buttonHeight;
        }


        Btn.setLeft (left);
        Btn.setTop (top);
        Btn.setWidth (buttonWidth);
        Btn.setHeight (buttonHeight);

        xOffset++;

        this.addComponent (Btn);
      }
    }

    private void LoadGuide() {
      reloadBase ();

      CurrentView.label = "StateFunding Guide";

      string guideLabel = "StateFunding Version 0.1.0\n" +
                          "--------------------------\n\n" +
                          "[*] Overview\n" +
                          "StateFunding offers another path to funding your space program. When you chose your government " +
                          "you chose based on the strengths and weaknesses of that government. If you government cares more " +
                          "more about public opinion, using Kerbals will have a greater inpact on your funding but you will also " +
                          "be penalized for Kerbal death more. If your government cares more about their own confidence in your " +
                          "program, using autonomous rockets is a safer, cheaper, and more affective. Each year you will recieve " +
                          "a year review showing the achievements and failures and you will be paid by your total PO/SC scores.\n\n" +
                          "[*] Public Opinion (PO)\n" +
                          "To raise public opinion, have more Kerbals in active flights. The amount of science generated from " +
                          "science stations will also increase public opinion. Kerbal death will negatively impact public opinion\n\n" +
                          "[*] State Confidence (SC)\n" +
                          "To raise state confidence, have satelites with antenas in orbit of celestial bodies other than the Sun. " +
                          "You can have 10 satelites in orbit of each celestial body affect your SC score. Resource mining will " +
                          "also increase your SC score. Destroyed vessels will have a negative impact on the SC score.";

      ViewTextArea TextArea = new ViewTextArea (guideLabel);
      TextArea.setRelativeTo (Window);
      TextArea.setTop (40);
      TextArea.setLeft (130);
      TextArea.setWidth (Window.getWidth () - 140);
      TextArea.setHeight (Window.getHeight () - 40);
      TextArea.setColor (Color.white);

      this.addComponent (TextArea);
    }

    public void OnReviewClick(Review Rev) {
      StateFundingGlobal.fetch.ReviewMgr.OpenReview(Rev);
    }
  }
}

