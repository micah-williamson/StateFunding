using System;
using UnityEngine;
using System.Collections;
using System.IO;

namespace StateFunding {
  public class ReviewApplicationLauncher: MonoBehaviour {
    private ReviewHubView View;

    public ReviewApplicationLauncher () {
      View = new ReviewHubView ();
      Texture2D Image = new Texture2D (2, 2);
      Image.LoadImage (File.ReadAllBytes ("GameData/StateFunding/assets/cashmoney.png"));
      ApplicationLauncherButton Button = ApplicationLauncher.Instance.AddModApplication (onTrue, onFalse, onHover, onHoverOut, onEnable, onDisable, ApplicationLauncher.AppScenes.SPACECENTER, Image);
    }

    public void onTrue() {
      Debug.Log ("Opened State Funding Hub");
      ViewManager.addView (View);
    }

    public void onFalse() {
      Debug.Log ("Closed State Funding Hub");
      ViewManager.removeView (View);
    }

    public void onHover() {

    }

    public void onHoverOut() {
      
    }

    public void onEnable() {
      
    }

    public void onDisable() {
    }

  }
}

