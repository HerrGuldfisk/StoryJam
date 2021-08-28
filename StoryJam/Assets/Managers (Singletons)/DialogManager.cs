using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogManager : MonoBehaviour
{
	#region Singleton

	public static DialogManager Instance { get; private set; } = null;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	#endregion



	public void StartDialog(DialogData data)
	{
		for(int i = 0; i<data.dialog.Length; i++)
		{
			if (data.show[i])
			{
				int currentValue = (int)Enum.Parse(typeof(current), data.conditions[i].ToString());
				int goalValue = (int)Enum.Parse(typeof(goal), data.conditions[i].ToString());

				if(currentValue < goalValue)
				{

					// Return dialog hmm nothing to do here...
					Debug.Log("Am I missing something... (Conditions not met)");
					return;
				}
				else
				{
					Debug.Log(data.dialog[i]);
					data.show[i] = false;
					// send dialog to dialog window.

					return;
				}

			}
		}

		Debug.Log("Nothing more to do here... (End of dialog)");
	}
}
