using System;
using UnityEngine;

namespace StateFunding {
  public class InstanceConfig : MonoBehaviour, IInstanceConfig {
    private String saveName = "statefunding.conf";
    private String saveFilePath;

    public InstanceConfig () {
      saveFilePath = HighLogic.fetch.GameSaveFolder + "/" + saveName;
    }

    public Instance createInstance () {
      RenderingManager.AddToPostDrawQueue(0, OnDraw);
      return new Instance ();
    }

    public void createInstanceWindow (int windowId) {
      GUILayout.BeginHorizontal(GUILayout.Width(250f));
      GUILayout.Label("This is a label");
      GUILayout.EndHorizontal();

      GUI.DragWindow();
    }

    public Instance loadInstance () {
      ConfigNode CnfNode = ConfigNode.Load (saveFilePath);
      if (CnfNode == null) {
        return null;
      } else {
        Instance Inst = new Instance ();
        ConfigNode.LoadObjectFromConfig (Inst, CnfNode);

        return Inst;  
      }
    }

    void OnDraw() {
      int horzMargin = 100;
      int vertMargin = 60;
      Rect windowRect = new Rect (horzMargin, vertMargin, Screen.width - horzMargin*2, Screen.height - vertMargin*2);
      GUILayout.Window (0, windowRect, createInstanceWindow, "State Funding");
    }

    public bool saveExists () {
      return KSP.IO.File.Exists<String> (saveFilePath);
    }

    public void saveInstance (Instance Inst) {
      ConfigNode CnfNode = ConfigNode.CreateConfigFromObject (Inst);
      CnfNode.Save ("test.statefunding.conf");
    }
  }
}

