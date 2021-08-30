using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager audioManagerInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (audioManagerInstance == null)
        {
            audioManagerInstance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
