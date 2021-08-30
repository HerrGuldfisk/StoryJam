using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogButton : Action
{
    public override void RunAction()
	{
		DialogManager.Instance.StartDialog(DialogManager.Instance.currentDialogData);
	}
}
