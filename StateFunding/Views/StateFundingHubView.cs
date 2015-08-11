using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class StateFundingHubView: View {
    private ViewWindow Window;
    private ArrayList SideMenu;

    public StateFundingHubView () {
      createWindow ();
    }

    private void createWindow() {
      reloadBase ();
    }

    private void reloadBase() {
      this.removeAll ();

      SideMenu = new ArrayList ();

      Window = new ViewWindow ("");
      Window.setMargins (300, 100);

      this.addComponent (Window);

      SideMenu.Add(new ViewButton("PastReviews", LoadPastReviews));
      SideMenu.Add(new ViewButton ("SF Guide", LoadGuide));

      for (var i = 0; i < SideMenu.ToArray ().Length; i++) {
        ViewButton Btn = (ViewButton)SideMenu.ToArray () [i];
        Btn.setRelativeTo (Window);
        Btn.setLeft (10);
        Btn.setTop (10 + i * 45);
        Btn.setWidth (120);
        Btn.setHeight (35);
        this.addComponent (Btn);
      }
    }

    private void LoadPastReviews() {
      reloadBase ();

      Window.title = "Past Reviews";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;

      int buttonWidth = 60;
      int buttonHeight = 20;
      int buttonMargin = 10;
      int xOffset = 0;
      int yOffset = 0;

      Debug.Log ("X");

      Debug.Log (GameInstance);
      Debug.Log(GameInstance.getReviews ());

      Debug.Log ("Y");

      for (int i = GameInstance.getReviews ().Length - 1; i >= 0; i--) {
        Debug.Log ("V");
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
          Debug.Log ("V1");
        }


        Btn.setLeft (left);
        Btn.setTop (top);
        Btn.setWidth (buttonWidth);
        Btn.setHeight (buttonHeight);

        xOffset++;

        Debug.Log ("Y");

        this.addComponent (Btn);
      }
    }

    private void LoadGuide() {
      reloadBase ();

      Window.title = "StateFunding Guide";

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

