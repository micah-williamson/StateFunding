using System;

namespace StateFunding
{
  public class StateFundingScenario : ScenarioModule
  {
    private static StateFundingScenario _instance;
    public StateFundingScenario Instance {
      get {
        return _instance;
      }
    }
    
    private static InstanceData data;
    private static bool isInit;
    
    public StateFundingScenario () {
      if (_instance != null)
        return;
        
      _instance = this;
      data = new InstanceData();
    }
    
    
    //load scenario
    public override void OnLoad (ConfigNode node) {
      
    }
    
    
    //save scenario
    public override void OnSave (ConfigNode node) {
      if (!isInit)
        return;
      
    }
    
    
  }
}

