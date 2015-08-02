using System;

namespace StateFunding
{
	public class Government : IGovernment
	{
		
		public float budget;
		public float kspBudget;
		public float gdp;
		public string longName;
		public float maxPO;
		public float maxSC;
		public String name;
		public float poModifier;
		public float poPenaltyModifier;
		public float scModifier;
		public float scPenaltyModifier;

		public Government () {
			maxPO = 1000;
			maxSC = 1000;
		}

		public float getKSPBudget () {
			return (float)gdp * budget * kspBudget;
		}
	}
}

