using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Action
{
	public bool destroyAfter;

	public override void RunAction()
	{
		GetComponent<DialogData>().RunAction();
	}
}
