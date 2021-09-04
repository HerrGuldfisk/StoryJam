using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogData : Action
{

	public List<string> dialog = new List<string>();

	public string defaultMessage = null;

	public goal[] conditionsNeeded;
	public goalIndex[] conditionsToUpdate;
	public goal UnmetCondition;

    public string[] voices;

	[HideInInspector] public int startIndex = 0;
	[HideInInspector] public int currentIndex = 0;
	[HideInInspector] public bool done;
	[HideInInspector] public bool[] show;

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

