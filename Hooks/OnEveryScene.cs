/*
using System;
using System.Collections;
using UnityEngine;

namespace StateFunding {
  [KSPAddon (KSPAddon.Startup.EveryScene, false)]
  public class OnEveryScene : MonoBehaviour {
    public void Awake () {
      
    }

    public void Start () {
      
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
*/

