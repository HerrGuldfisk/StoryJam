using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffAtCondition : MonoBehaviour
{

	public int conditionIndex;

	private void FixedUpdate()
	{
		if(GlobalData.current[conditionIndex] >= conditionIndex)
		{
			Destroy(this.gameObject);
		}
	}
}
