using System;

namespace StateFunding
{
  
[KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[] {
		GameScenes.SPACECENTER,
		GameScenes.EDITOR,
		GameScenes.FLIGHT,
		GameScenes.TRACKSTATION,
	})
]  
  public class StateFundingScenario : ScenarioModule
  {
    private static StateFundingScenario _instance;
    public StateFundingScenario Instance {
      get {
        return _instance;
      }
    }
    
    public ReviewManager ReviewMgr
    private InstanceData data;
    private bool isInit;
    private const string CONFIG_NODENAME = "STATEFUNDINGSCENARIO";
    
    
    public StateFundingScenario () {
      	_instance = this;
    }
    

	public override void OnAwake ()
	{
      	if (data == null)
      	data = new InstanceData();
      	if (ReviewMgr == null)
      	ReviewMgr = new ReviewManager();	
	}    
    
    
    public override void OnDestroy ()
	{
      _instance = null;
	} 
    
    
    //load scenario
    public override void OnLoad (ConfigNode node) {
      try {
        if (node.hasNode(CONFIG_NODENAME)) {
          //load
          ConfigNode loadNode = node.getNode(CONFIG_NODENAME);
          ConfigNode.LoadObjectFromConfig(data, loadNode);
          isInit = true;
        }
        else {
          //default init
          //...
          isInit = true;
        }
      }
      catch {
        
      }
    }
    
    
    //save scenario
    public override void OnSave (ConfigNode node) {
      if (!isInit)
        return;
     
      ConfigNode saveNode = ConfigNode.CreateConfigFromObject(data);
      node.AddNode(saveNode, CONFIG_NODENAME);
    }
    
    
  }
}

