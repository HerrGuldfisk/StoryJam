using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

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
		// DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	private void Start()
	{
		Canvas tempCanvas = FindObjectOfType<Canvas>();
		textBox = FindObjectOfType<TextMeshProUGUI>();
		if(tempCanvas.gameObject.tag == "DialogCanvas")
		{
			dialogCanvas = tempCanvas;
			dialogCanvas.gameObject.SetActive(false);
		}
	}

	private TextMeshProUGUI textBox;
	private Canvas dialogCanvas;
	public DialogData currentDialogData;
	private bool isActive;
	private bool turnOff;

	public void StartDialog(DialogData data)
	{
		if (turnOff)
		{
			ToggleDialog();
			turnOff = false;
			return;
		}
		currentDialogData = data;
		if (!isActive)
		{
			ToggleDialog();
		}

		for(int i = 0; i<data.dialog.Length; i++)
		{
			if (data.show[i])
			{
				/*
				int currentValue = (int)Enum.Parse(typeof(current), data.conditionsNeeded[i].ToString());
				int goalValue = (int)Enum.Parse(typeof(goal), data.conditionsNeeded[i].ToString());
				*/


				if(GlobalData.current[(int)Enum.Parse(typeof(goalIndex), data.conditionsNeeded[i].ToString())] < (int)data.conditionsNeeded[i])
				{
					textBox.text = "Hmm, seems like I need something...";
					turnOff = true;
					// Return dialog hmm nothing to do here...
					Debug.Log("Am I missing something... (Conditions not met)");

					return;
				}
				else
				{
					textBox.text = data.dialog[i];
					Debug.Log(data.dialog[i]);
					data.show[i] = false;

					if(data.conditionsToUpdate[i] != goalIndex.NONE)
					{
						GlobalData.current[(int)data.conditionsToUpdate[i]] += 1;
					}
					// send dialog to dialog window.

					return;
				}
			}

			data.done = true;
		}

		if (data.done)
		{
			turnOff = true;
			textBox.text = "Nothing more to do here.";
			Debug.Log("Nothing more to do here... (End of dialog)");
		}
		else
		{
			ToggleDialog();
		}
	}

	public void ToggleDialog()
	{
		if (isActive)
		{
			dialogCanvas.gameObject.SetActive(false);
			isActive = false;
		}
		else
		{
			dialogCanvas.gameObject.SetActive(true);
			isActive = true;
		}
	}
}
