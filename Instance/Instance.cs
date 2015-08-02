using System;

namespace StateFunding
{
	public class Instance : IInstance
	{
		[Persistent]
		Government Gov;

		[Persistent]
		int po;

		[Persistent]
		int sc;

		public Instance () {}
	}
}

