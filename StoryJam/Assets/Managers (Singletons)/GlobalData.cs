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
		// DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	public static List<int> current = new List<int>
	{
		 0,
		 0,
		 4,
	};
}


public enum goal
{
	NONE = 0,
	door1 = 1,
	door2 = 4
}

public enum goalIndex
{
	NONE,
	door1,
	door2
}
