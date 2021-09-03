using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
	#region Singleton

	public static RoomManager Instance { get; private set; } = null;

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

	public int currentSceneId;
	public Room CurrentRoom;

	public void Start()
	{

	}

	public void ChangeRoom(int id)
	{
		Camera.main.transform.position = new Vector3(id * 25, 0, -10);
	}
}
