using System;
using System.Collections;
using UnityEngine;

namespace StateFunding
{
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	public class StateFunding
	{
    ArrayList Governments;
    InstanceConfig InstanceConf;
    Instance GameInstance;

    public StateFunding () {
			InstanceConf = new InstanceConfig();
      GameEvents.onGameStateLoad.Add(load);
		}

    public void initGovernments () {
		  Governments = new ArrayList ();
			Government USK = new Government ();
			Government USSK = new Government ();

			USK.budget = 0.2f;
			USK.kspBudget = 0.005f;
			USK.gdp = 80000000;
			USK.longName = "United States of Kirba";
			USK.name = "USK";
			USK.poModifier = 2;
			USK.poPenaltyModifier = 3;
			USK.scModifier = 1;
			USK.scPenaltyModifier = 1;

			USSK.budget = 0.2f;
			USSK.kspBudget = 0.03f;
			USSK.gdp = 10000000;
			USSK.longName = "The Union of Soviet Socialist Kerbals";
			USSK.name = "USSK";
			USSK.poModifier = 0.5f;
			USSK.poPenaltyModifier = 0.5f;
			USSK.scModifier = 1;
			USSK.scPenaltyModifier = 2;

			Governments.Add(USK);
			Governments.Add(USSK);

			Debug.Log ("Initialized Governments");
		}

    public void load (ConfigNode N) {
			Debug.Log ("StateFunding Mod Loading");
			initGovernments ();
			loadSave ();
			Debug.Log ("StateFunding Mod Loaded");
		}

    public void loadSave () {
			if((GameInstance = InstanceConf.loadInstance ()) == null) {
				GameInstance = InstanceConf.createInstance ();
			}

			Debug.Log ("StateFunding Save Loaded");
		}
	}
}

