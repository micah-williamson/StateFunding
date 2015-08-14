using System;
using System.Collections;
using UnityEngine;

namespace StateFunding {
  [KSPAddon (KSPAddon.Startup.MainMenu, false)]
  public class OnMainMenu : MonoBehaviour {

    public void Awake () {
      if (StateFundingGlobal.fetch != null) {
        StateFundingGlobal.fetch.unload ();
      }

      StateFundingGlobal.fetch = new StateFunding ();
    }

    public void Start () {
      
    }

    public void Update () {

    }

    public void FixedUpdate () {

    }

    public void OnDestroy () {

    }
  }
}

