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
		choiceScreen.gameObject.SetActive(false);

		for (int i = 0; i < endingScreens.Length; i++)
		{
			endingScreens[i].gameObject.SetActive(false);
		}

		Canvas tempCanvas = FindObjectOfType<Canvas>();
		if(tempCanvas.gameObject.tag == "DialogCanvas")
		{
			dialogCanvas = tempCanvas;
			dialogCanvas.gameObject.SetActive(false);
		}
	}
	public TextMeshProUGUI textBox;
    //public TMP_Text text;

	public Canvas choiceScreen;
	public Canvas[] endingScreens;

	private Canvas dialogCanvas;
	public DialogData currentDialogData;
	[HideInInspector] public bool isActive;
	int textindex = 0;

    private bool playing;
    private string voice;

	private bool beenScared;

    bool typing;
    const string kAlphaCode = "<color=#00000000>";
    const float kMaxTextTime = 0.1f;
    public static int textSpeed = 2;
    public static int textSpeedFast = 30;
    int currentTextSpeed;
    private string currentText = "";

    public void StartDialog(DialogData data)
	{
		Close();
        currentDialogData = data;
		ToggleDialog();
		//textBox.text = data.dialog[0];
        currentText = data.dialog[0];
        StartCoroutine(DisplayText());
        textindex = 0;

        if (playing == false)
        {
            StartCoroutine("playVoice");
        }

        currentDialogData.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		// Only have update conditoin on index 0 for pick ups.
		if(currentDialogData.conditionsToUpdate[textindex] != goalIndex.NONE)
		{
			GlobalData.current[(int)currentDialogData.conditionsToUpdate[textindex]] += 1;
		}

		bool stop = false;
		for(int i = textindex; i<currentDialogData.conditionsNeeded.Length && !stop; i++)
		{
			if (stop)
			{
				stop = false;
				return;
			}

			if(GlobalData.current[(int)Enum.Parse(typeof(goalIndex), data.conditionsNeeded[i].ToString())] < (int)data.conditionsNeeded[i])
			{
				stop = true;

				for (int j=i-1; j > 0 ; j--)
				{
					if(currentDialogData.conditionsNeeded[j] != goal.NONE)
					{
						textindex = j;
						return;
					}
				}
			}
		}

		if (textindex == 0)
		{
			textindex++;
		}
	}

	public void NextDialog()
	{
        currentTextSpeed = textSpeedFast;
        if (typing == false)
        {
            Close();
            if (textindex < currentDialogData.conditionsNeeded.Length)
            {
                //textBox.text = currentDialogData.dialog[textindex];
                currentText = currentDialogData.dialog[textindex];
                StartCoroutine(DisplayText());
            }
            else
            {
                EndDialog();
                return;
            }

            if (currentDialogData.conditionsToUpdate[textindex] != goalIndex.NONE)
            {
                GlobalData.current[(int)currentDialogData.conditionsToUpdate[textindex]] += 1;
            }

            if (textindex == currentDialogData.conditionsNeeded.Length)
            {
                EndDialog();
                return;
            }

            if (GlobalData.current[(int)Enum.Parse(typeof(goalIndex), currentDialogData.conditionsNeeded[textindex].ToString())] < (int)currentDialogData.conditionsNeeded[textindex])
            {
                EndDialog();
                return;
            }
            else
            {
                if (playing == false)
                {
                    StartCoroutine("playVoice");
                }
                textindex++;
            }
        }
	}

	private bool madeChoice;


	private void EndDialog()
	{
		if(currentDialogData.done)
		{
			ToggleDialog();
		}
		else if(textindex == currentDialogData.conditionsNeeded.Length - 1)
		{
			ToggleDialog();
		}
		else
		{
			ToggleDialog();
		}

		if (GlobalData.current[6] >= 6 && beenScared == false)
		{
			beenScared = true;
			RoomManager.Instance.ChangeRoom(11, "steps2");
		}

		if (GlobalData.current[8] >= 8 && madeChoice == false)
		{
			madeChoice = true;
			choiceScreen.gameObject.SetActive(true);
			// choiceScreen.GetComponent<DeactivateCanvases>().canvases[0].gameObject.SetActive(true);
		}

		currentDialogData.gameObject.GetComponent<BoxCollider2D>().enabled = true;

		if (currentDialogData.gameObject.GetComponent<PickUp>() && currentDialogData.gameObject.GetComponent<PickUp>().destroyAfter == true)
		{
			Destroy(currentDialogData.gameObject);
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

    public void Show(string text)
    {
        currentText = text;
        StartCoroutine(DisplayText());
    }

    public void Close()
    {
        StopAllCoroutines();
    }


    IEnumerator playVoice()
    {
        playing = true;

        voice = currentDialogData.voices[UnityEngine.Random.Range(0, currentDialogData.voices.Length)];

        AudioManager.Instance.PlayAudio(voice);
        yield return new WaitForSeconds(AudioManager.Instance.GetAudioSource(voice).clip.length);

        playing = false;
    }

    private IEnumerator DisplayText()
    {
        currentTextSpeed = textSpeed;

        string originalText = currentText;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in currentText.ToCharArray())
        {
            typing = true;

            alphaIndex++;
            textBox.text = originalText;
            displayedText = textBox.text.Insert(alphaIndex, kAlphaCode);
            textBox.text = displayedText;
            textBox.text += c;

            yield return new WaitForSecondsRealtime(kMaxTextTime / currentTextSpeed);

            typing = false;
        }

        yield return null;
    }

    #region Shitty old stuff
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
