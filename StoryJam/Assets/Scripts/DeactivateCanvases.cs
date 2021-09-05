using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCanvases : MonoBehaviour
{
	public Canvas[] canvases;

	private void Awake()
	{
		for (int i = 0; i < canvases.Length; i++)
		{
			canvases[i].gameObject.SetActive(false);
		}
	}
}
