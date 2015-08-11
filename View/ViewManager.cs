using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFunding {
  [KSPAddon (KSPAddon.Startup.Instantly, true)]
  public class ViewManager: MonoBehaviour {

    public static List<View> Views = new List<View>();

    public void Start() {
      DontDestroyOnLoad(this);
    }

    public static void addView(View V) {
      ViewManager.Views.Add (V);
    }

    public static void removeView(View V) {
      ViewManager.Views.Remove (V);
    }

    public static void removeAll() {
      ViewManager.Views.Clear ();
    }

    public void OnGUI() {
      for (var i = 0; i < ViewManager.Views.ToArray().Length; i++) {
        View V = ViewManager.Views.ToArray() [i];
        if (V.isPainting ()) {
          V.paint ();
        }
      }
    }

  }
}

