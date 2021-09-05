using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastButton : MonoBehaviour
{
	public Canvas canvas;
	public Canvas lasthoiceScreen;

    // Start is called before the first frame update
    public void OnPress()
	{
		lasthoiceScreen.gameObject.SetActive(false);
		canvas.gameObject.SetActive(true);
	}
}
