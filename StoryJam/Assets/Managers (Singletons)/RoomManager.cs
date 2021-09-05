using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    private string[] steps = { "steps", "steps2", "steps3" };
    private string[] musicLoops = { "percs", "rythmDrums", "allDrums", "allSound", "allSound2", "allSound3" };

    private string transitionSound;
    private string currentMusic;
    private string nextMusic;

    bool playingTransitionSound = false;
    bool playingCurrentMusic = false;
    bool playNext = true;

    Queue<string> myQueue = new Queue<string>();

	private UnityEvent RoomChanged = new UnityEvent();
	public List<DialogData> dialogHolder = new List<DialogData>();

	public void Start()
	{
		RoomChanged.AddListener(RoomChange);
	}

	public void ChangeRoom(int id, string soundEffect)
	{
		// Add some type of transition animation?

		currentSceneId = id;

        //StopCoroutine(playCurrentMusic());

        if (playingTransitionSound == false)
        {
            if (soundEffect != "")
            {
                transitionSound = soundEffect;
            }
            else
            {
                // Random Foot Step
                transitionSound = steps[UnityEngine.Random.Range(0, 3)];
            }

            StartCoroutine("playTransitionSound");
        }

        if (id == 1)
        {
            AudioManager.Instance.PlayAudio(musicLoops[0], true);
        }
        else if (id == 4)
        {
            AudioManager.Instance.GetAudioSource(musicLoops[0]).Stop();
            AudioManager.Instance.PlayAudio(musicLoops[1], true);
        }
        else if (id == 9)
        {
            AudioManager.Instance.GetAudioSource(musicLoops[1]).Stop();
            AudioManager.Instance.PlayAudio(musicLoops[2], true);
        }
        else if (id == 12)
        {
            AudioManager.Instance.GetAudioSource(musicLoops[2]).Stop();
            AudioManager.Instance.PlayAudio(musicLoops[3], true);
        }
        else if (id == 15)
        {
            AudioManager.Instance.GetAudioSource(musicLoops[3]).Stop();
            AudioManager.Instance.PlayAudio(musicLoops[4], true);
        }

        // Add check if id exists
        Camera.main.transform.position = new Vector3(id * 25, 0, -10);
		RoomChanged.Invoke();
	}

	public bool stoneIsRemoved;

	private void RoomChange()
	{
		if (currentSceneId == 9)
		{
			dialogHolder[0].RunAction();
		}
		else if (currentSceneId == 11)
		{
			dialogHolder[2].RunAction();
		}
		else if (currentSceneId == 15)
		{
			dialogHolder[1].RunAction();
		}
		else if (currentSceneId == 7)
		{
			stoneIsRemoved = true;
		}
		else if (currentSceneId == 5 && GlobalData.current[4] >= 4 && stoneIsRemoved)
		{
			ChangeRoom(7, "steps");
		}
	}

	IEnumerator playTransitionSound()
    {
        playingTransitionSound = true;

        AudioManager.Instance.PlayAudio(transitionSound);
        yield return new WaitForSeconds(AudioManager.Instance.GetAudioSource(transitionSound).clip.length);

        playingTransitionSound = false;
    }


    IEnumerator playCurrentMusic()
    {
        currentMusic = myQueue.Dequeue();

        AudioManager.Instance.PlayAudio(currentMusic, true);
        yield return new WaitForSecondsRealtime(AudioManager.Instance.GetAudioSource(currentMusic).clip.length);

        if (myQueue.Count > 0)
        {
            AudioManager.Instance.GetAudioSource(currentMusic).Stop();
            AudioManager.Instance.PlayAudio(myQueue.Dequeue(), true);
        }
    }
}