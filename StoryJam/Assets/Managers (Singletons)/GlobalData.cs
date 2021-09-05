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
		 2,
		 2,
		 3,
		 4,
		 5,
		 0,
		 7,
	};
}


public enum goal
{
	NONE = 0,
	door1 = 1,
	door2 = 2,
	elevator5 = 3,
	room_8 = 4,
	rickyDead = 5,
	scared = 6,
	neverDone = 7,
	finalChoice = 8,
}

public enum goalIndex
{
	NONE,
	door1,
	door2,
	elevator5,
	room_8,
	rickyDead,
	scared,
	neverDone,
	finalChoice,
}
