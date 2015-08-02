using System;

namespace StateFunding
{
	public class USK : Government
	{
		public USK () {
			budget = 0.2;
			kspBudget = 0.005;
			gdp = 80000000;
			longName = "United States of Kirba";
			name = "USK";
			poModifier = 2;
			poPenaltyModifier = 3;
			scModifier = 1;
			scPenaltyModifier = 1;
		}
	}
}

