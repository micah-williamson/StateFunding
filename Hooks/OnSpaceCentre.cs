using System;
using System.Collections;
using UnityEngine;

namespace StateFunding {
  [KSPAddon (KSPAddon.Startup.SpaceCentre, true)]
  public class OnSpaceCentre : MonoBehaviour {
    public void Awake () {
      
    }

    public void Start () {
      ViewManager.removeAll ();
      StateFundingGlobal.fetch.LoadIfNeeded ();
    }


    private int curTicks;
    private const int INTERVAL_TICKS = 50;

    public void Update () {
      // Update once every 50 fixedupdates
      curTicks++;
      if (curTicks > INTERVAL_TICKS) {
        curTicks = 0;
        if (StateFundingGlobal.fetch != null) {
          StateFundingGlobal.fetch.tick ();
        }
      }
    }

    public void FixedUpdate () {

    }

    public void OnDestroy () {

    }
  }
}

