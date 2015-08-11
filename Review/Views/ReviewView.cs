using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class ReviewView: View {
    private ViewWindow Window;
    private ViewImage Image;
    private ViewLabel Label;
    private ViewTextArea ReviewText;
    private ViewButton Confirm;
    private Review Rev;

    public ReviewView (Review Rev) {
      ViewManager.addView (this);
      this.Rev = Rev;
      createWindow ();
    }

    private void createWindow() {
      Window = new ViewWindow ("Review");
      Window.setMargins (300, 100);

      Image = new ViewImage ("assets/kerbalfunding.jpg");
      Image.setRelativeTo (Window);
      Image.setPercentWidth (100);

      Label = new ViewLabel ("Could be worse.");
      Label.setRelativeTo (Image);
      Label.setPercentWidth (80);
      Label.setPercentHeight (20);
      Label.setPercentLeft (10);
      Label.setPercentTop (80);
      Label.setFontSize (18);
      Label.setColor (Color.white);

      Confirm = new ViewButton ("Ok", OnConfirm);
      Confirm.setRelativeTo (Window);
      Confirm.setWidth (100);
      Confirm.setHeight (30);
      Confirm.setRight (5);
      Confirm.setBottom (5);

      if (!Rev.pastReview) {
        Rev.touch ();
      }

      ReviewText = new ViewTextArea (Rev.GetText());
      ReviewText.setRelativeTo (Image);
      ReviewText.setPercentWidth (100);
      ReviewText.setTop (Image.getHeight() + 10);
      ReviewText.setHeight (Window.getHeight () - Image.getHeight () - Confirm.getHeight () - 20);
      ReviewText.setColor (Color.white);

      this.addComponent (Window);
      this.addComponent (Image);
      this.addComponent (Label);
      this.addComponent (Confirm);
      this.addComponent (ReviewText);
    }

    private void OnConfirm () {
      ViewManager.removeView (this);
    }
  }
}

