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
		textBox = FindObjectOfType<TextMeshProUGUI>();

		Canvas tempCanvas = FindObjectOfType<Canvas>();
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
	int textindex = 0;

	public void StartDialog(DialogData data)
	{
		currentDialogData = data;
		ToggleDialog();
		textBox.text = data.dialog[0];
		textindex = 0;

		currentDialogData.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		// Only have update conditoin on index 0 for pick ups.
		if(currentDialogData.conditionsToUpdate[textindex] != goalIndex.NONE)
		{
			GlobalData.current[(int)currentDialogData.conditionsToUpdate[textindex]] += 1;
		}

		bool stop = false;
		for(int i = textindex; i<currentDialogData.conditionsNeeded.Length && !stop; i++)
		{
			if(GlobalData.current[(int)Enum.Parse(typeof(goalIndex), data.conditionsNeeded[i].ToString())] < (int)data.conditionsNeeded[i])
			{
				for(int j=i-1; j > 0 ; j--)
				{
					if(currentDialogData.conditionsNeeded[j] != goal.NONE)
					{
						textindex = j;
						stop = true;
						break;
					}
				}
			}
		}
		if(textindex == 0)
		{
			textindex++;
			/*for(int i = currentDialogData.conditionsNeeded.Length - 1 ; i > 0 ; i--)
			{
				if(currentDialogData.conditionsNeeded[i] != goal.NONE)
				{
					textindex = i;
					break;
				}
			}
			if(textindex == 0)
			{
				textindex++;
			}*/
		}
	}
	
	public void NextDialog()
	{
		if(textindex < currentDialogData.conditionsNeeded.Length)
		{
			textBox.text = currentDialogData.dialog[textindex];
		}
		else
		{
			EndDialog();
			return;
		}
		
		if(currentDialogData.conditionsToUpdate[textindex] != goalIndex.NONE)
		{
			GlobalData.current[(int)currentDialogData.conditionsToUpdate[textindex]] += 1;
		}

		if(textindex == currentDialogData.conditionsNeeded.Length)
		{
			EndDialog();
			return;
		}

		if(GlobalData.current[(int)Enum.Parse(typeof(goalIndex), currentDialogData.conditionsNeeded[textindex].ToString())] < (int)currentDialogData.conditionsNeeded[textindex])
		{
			EndDialog();
			return;
		}
		else
		{
			textindex++;
		}
	}

	private void EndDialog()
	{
		if(currentDialogData.done)
		{
			ToggleDialog();
		}
		else if(textindex == currentDialogData.conditionsNeeded.Length - 1)
		{
			//currentDialogData.done = true;
			ToggleDialog();
		}
		else
		{
			ToggleDialog();
		}

		currentDialogData.gameObject.GetComponent<BoxCollider2D>().enabled = true;
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

	# region Shitty old stuff
	/*
	private bool turnOff;

	private int currentIndex = 0;

	public void StartDialog(DialogData data)
	{
		currentDialogData = data;

		// Check if the dialog is done, show default message.
		if (data.done)
		{
			UpdateDialog();
			return;
		}

		if(currentIndex == 0)
		{
			ToggleDialog();
			UpdateText(currentDialogData.dialog[0]);
		}
		

		if (data.UnmetCondition == goal.NONE || 
			GlobalData.current[(int)Enum.Parse(typeof(goalIndex), data.UnmetCondition.ToString())] < (int)data.UnmetCondition)
		{
			UpdateDialog();
			return;
		}

		if ((int)data.UnmetCondition <= GlobalData.current[(int)Enum.Parse(typeof(goalIndex), data.UnmetCondition.ToString())])
		{
			data.UpdateStartPosition();
			UpdateDialog();
		}		
	}

	public void UpdateDialog()
	{
		ToggleDialog();

		if(currentDialogData.done)
		{
			if(currentDialogData.defaultMessage != null)
			{
				UpdateText(currentDialogData.defaultMessage);
			}
			else
			{
				UpdateText("Nothing more to do here");
			}
		}

		if(currentDialogData.currentIndex == 0)
		{
			UpdateText(currentDialogData.dialog[0]);

			
		}



	}

	public void UpdateText(string text)
	{
		textBox.text(text);
	}

	private void EndDialog()
	{
		RemovePickUp();

		currentDialogData = null;
		ToggleDialog();
		// remove current dialog
	}


	private void RemovePickUp()
	{
		if(currentDialogData.gameObject.tag == "PickUp")
		{
			Destroy(currentDialogData.gameObject);
		}
	}

	
	*/
	
	/*
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

				int currentValue = (int)Enum.Parse(typeof(current), data.conditionsNeeded[i].ToString());
				int goalValue = (int)Enum.Parse(typeof(goal), data.conditionsNeeded[i].ToString());



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
			// Debug.Log("Nothing more to do here... (End of dialog)");
		}
		else
		{
			ToggleDialog();
		}
	}
	*/
	#endregion

}
