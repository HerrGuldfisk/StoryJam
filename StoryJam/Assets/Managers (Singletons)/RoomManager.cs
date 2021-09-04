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

    private string[] steps = { "steps", "steps2", "steps3" };
    private string[] musicLoops = { "percs", "rythmDrums", "allDrums", "allSound", "allSound2", "allSound3" };

    private string transitionSound;
    private string currentMusic;
    private string nextMusic;

    bool playingTransitionSound = false;
    bool playingCurrentMusic = false;
    bool playNext;

    public void Start()
	{

	}

	public void ChangeRoom(int id, string soundEffect)
	{
        // Add some type of transition animation?

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

        if (playingCurrentMusic == false)
        {
            if (id < 3)
            {
                currentMusic = musicLoops[0];
                if (id == 2)
                {
                    nextMusic = musicLoops[1];
                    playNext = true;
                }
            }
            else if (id < 5)
            {
                currentMusic = musicLoops[1];
                if (id == 4)
                {
                    nextMusic = musicLoops[2];
                    playNext = true;
                }
            }
            else if (id < 10)
            {
                currentMusic = musicLoops[2];
                if (id == 9)
                {
                    nextMusic = musicLoops[3];
                    playNext = true;
                }
            }

            StartCoroutine(playCurrentMusic(playNext));
        }

        // Add check if id exists
        Camera.main.transform.position = new Vector3(id * 25, 0, -10);
	}
    IEnumerator playTransitionSound()
    {
        playingTransitionSound = true;

        AudioManager.Instance.PlayAudio(transitionSound);
        yield return new WaitForSeconds(AudioManager.Instance.GetAudioSource(transitionSound).clip.length);

        playingTransitionSound = false;
    }

    IEnumerator playCurrentMusic(bool playNext)
    {
        playingCurrentMusic = true;

        AudioManager.Instance.PlayAudio(currentMusic, true);
        yield return new WaitForSecondsRealtime(AudioManager.Instance.GetAudioSource(currentMusic).clip.length);
        if (playNext == true)
        {
            AudioManager.Instance.GetAudioSource(currentMusic).Stop();
            AudioManager.Instance.PlayAudio(nextMusic, true);
            playNext = false;
        }

        playingCurrentMusic = false;
    }
}
