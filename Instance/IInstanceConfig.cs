using System;
using UnityEngine;

namespace StateFunding
{
  public interface IInstanceConfig
	{
		Instance createInstance();
		Instance loadInstance();
		bool saveExists();
		void saveInstance(Instance Inst);
	}
}

