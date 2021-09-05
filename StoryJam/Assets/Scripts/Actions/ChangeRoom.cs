using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeRoom : Action
{
	[SerializeField] private int roomId;

	[SerializeField] public List<goal> conditions;

    // TransitionSounds
    [SerializeField] public string soundEffect;

    public override void RunAction()
	{
		for (int i = 0; i < conditions.Count; i++)
		{
			int currentValue = GlobalData.current[(int)conditions[i]];
			int goalValue = (int)Enum.Parse(typeof(goal), conditions[i].ToString());

			if (currentValue < goalValue)
			{

				Debug.Log("Condition " + conditions[i].ToString() + " not met");
				return;
			}
		}
        RoomManager.Instance.ChangeRoom(roomId, soundEffect);
	}
}
