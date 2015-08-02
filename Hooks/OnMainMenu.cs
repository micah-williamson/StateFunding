using System;
using System.Collections;
using UnityEngine;

namespace StateFunding
{
  [KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class OnMainMenu : MonoBehaviour
	{
		public void Awake() {
      StateFundingGlobal.fetch = new StateFunding ();
		}

		public void Start() {
      
		}

		public void Update() {

		}

		public void FixedUpdate() {

		}

		public void OnDestroy() {

		}
	}
}

