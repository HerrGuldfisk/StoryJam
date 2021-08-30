using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Action
{

	public goalIndex[] conditionsToUpdate;

	public override void RunAction()
	{
		UpdateConditions();

		Debug.Log("Journal entry?");

		Destroy(this.gameObject);
	}

	private void UpdateConditions()
	{
		for(int i = 0; i<conditionsToUpdate.Length; i++)
		{
			GlobalData.current[(int)conditionsToUpdate[i]] += 1;
		}
	}
}
