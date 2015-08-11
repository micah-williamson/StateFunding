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

    public void Update () {
      if (StateFundingGlobal.fetch != null) {
        StateFundingGlobal.fetch.tick ();
      }
    }

    public void FixedUpdate () {

    }

    public void OnDestroy () {

    }
  }
}

