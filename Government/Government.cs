using System;

namespace StateFunding {
  public class Government {
		
    public float budget;
    public string description;
    public float gdp;
    public string longName;
    public String name;
    public float poModifier;
    public float poPenaltyModifier;
    public float scModifier;
    public float scPenaltyModifier;
    public int startingPO;
    public int startingSC;

    public Government () {}

    private string modifierLexicon(float val) {
      if (val <= 0.25) {
        return "Very Low";
      } else if (val <= 0.5) {
        return "Low";
      } else if (val <= 1) {
        return "Normal";
      } else if (val <= 2) {
        return "High";
      }
       
      return "Very High";
    }

    public string GetGameplayDescription() {
      return "GDP: " + gdp.ToString("#,##0") + "\n" +
        "Yearly Budget: " + (gdp*budget).ToString("#,##0") + "\n" + 
        "Starting PO: " + startingPO + "\n" +
        "Starting SC: " + startingSC + "\n" +
        "State Reward: " + modifierLexicon(scModifier) + "\n" +
        "State Penalty: " + modifierLexicon(scPenaltyModifier) + "\n" +
        "Public Reward: " + modifierLexicon(poModifier) + " \n" +
        "Public Penalty: " + modifierLexicon(poPenaltyModifier) + " \n";
    }

  }
}

