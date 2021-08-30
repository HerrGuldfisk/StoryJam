using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogData : Action
{
	public string[] dialog;
	public bool[] show;
	public goal[] conditionsNeeded;
	public goalIndex[] conditionsToUpdate;

	[HideInInspector] public bool done;

	public override void RunAction()
	{
		DialogManager.Instance.StartDialog(this);
	}
}
