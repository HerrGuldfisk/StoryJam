using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastButton : MonoBehaviour
{
	public Canvas canvas;
	public Canvas lasthoiceScreen;
	public CanvasGroup canvasGroup;

	Coroutine fade;

    // Start is called before the first frame update
    public void OnPress()
	{
		canvas.gameObject.SetActive(true);
		canvasGroup.alpha = 0;

		fade = StartCoroutine(FadeToBlack());

		lasthoiceScreen.GetComponent<CanvasGroup>().alpha = 0;
	}

	IEnumerator FadeToBlack()
	{
		while(canvasGroup.alpha <= 1)
		{
			if (canvasGroup.alpha == 1)
			{
				canvasGroup.alpha = 1;
				StopCoroutine(fade);
			}
			else
			{
				canvasGroup.alpha += 0.01f;
			}
			yield return new WaitForSecondsRealtime(0.01f);
		}




	}
}
