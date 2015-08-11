using System;
using System.Collections;
using UnityEngine;

namespace StateFunding {
  [KSPAddon (KSPAddon.Startup.Instantly, true)]
  public class ViewManager: MonoBehaviour {

    public static View[] views = new View[0];

    public void Start() {
      DontDestroyOnLoad(this);
    }

    public static void addView(View V) {
      View[] newViews = new View[ViewManager.views.Length+1];
      for(int i = 0; i < ViewManager.views.Length; i++) {
        newViews[i] = ViewManager.views[i];
      }
      newViews[ViewManager.views.Length] = V;

      ViewManager.views = newViews;
    }

    public static void removeView(View V) {
      View[] newViews = new View[ViewManager.views.Length-1];
      int k = 0;
      for(int i = 0; i < ViewManager.views.Length; i++) {
        // TODO: Why is this always true? If you ever remove a view everything blows up
        if (V != ViewManager.views [i]) {
          newViews [k] = ViewManager.views [i];
          k++;
        }
      }

      ViewManager.views = newViews;
    }

    public static View[] GetViews() {
      return ViewManager.views;
    }

    public void OnGUI() {
      for (var i = 0; i < ViewManager.GetViews().Length; i++) {
        View V = ViewManager.GetViews() [i];
        if (V.isPainting ()) {
          V.paint ();
        }
      }
    }

    public static void removeAll() {
      ViewManager.views = new View[0];
    }

  }
}

