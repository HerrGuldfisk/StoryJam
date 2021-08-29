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
		DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	public int currentSceneId;
	public Room CurrentRoom;

	public void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void ChangeRoom(int id)
	{
		SceneManager.LoadSceneAsync(id);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		currentSceneId = scene.buildIndex;
		CurrentRoom = FindObjectOfType<Room>();
		Debug.Log("Current scene is: " + scene.name);
	}

}
