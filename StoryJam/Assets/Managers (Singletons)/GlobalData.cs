using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
	#region Singleton

	public static GlobalData Instance { get; private set; } = null;

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

}

public enum current
{
	door1 = 0,
	door2 = 1,
}

public enum goal
{
	door1 = 1,
	door2 = 1
}
