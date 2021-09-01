using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogData : Action
{

	public List<string> dialog = new List<string>();
	public bool[] show;
	public goal[] conditionsNeeded;
	public goalIndex[] conditionsToUpdate;

	[HideInInspector] public int startIndex = 0;
	public goal UnmetCondition;

	[HideInInspector] public bool done;

	private void Start()
	{
		FindNextCondition();
	}

	private void FindNextCondition()
	{
		for (int i = 0; i < conditionsNeeded.Length; i++)
		{
			if (conditionsNeeded[i] == goal.NONE) { continue; }

			if (GlobalData.current[(int)Enum.Parse(typeof(goalIndex), conditionsNeeded[i].ToString())] < (int)conditionsNeeded[i])
			{
				UnmetCondition = conditionsNeeded[i];
				return;
			}

			UnmetCondition = goal.NONE;
		}
	}

	public void UpdateStartPosition()
	{
		for(int i = 0; i < conditionsNeeded.Length; i++)
		{
			if (conditionsNeeded[i] == UnmetCondition)
			{
				startIndex = i;
				FindNextCondition();
			}
		}
	}

	public override void RunAction()
	{
		DialogManager.Instance.StartDialog(this);
	}
}

