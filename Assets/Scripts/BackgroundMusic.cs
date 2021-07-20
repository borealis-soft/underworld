using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static GameObject Instance;
    public List<AudioClip> MusicList;
    public int DelayPlaing = 1;

    private AudioSource myAudio;
    private int current;
    private float time = 0;
    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = gameObject;

        myAudio = GetComponent<AudioSource>();
        MixList();

        DontDestroyOnLoad(gameObject);
    }

    private void MixList()
    {
        current = 0;
        MusicList.Sort((clip1, clip2) => Random.Range(-1, 1));
    }

    private void FixedUpdate()
    {
        if (!myAudio.isPlaying && current < MusicList.Count)
        {
            if (time > 0) time -= Time.deltaTime;
            else
            {
                myAudio.PlayOneShot(MusicList[current++]);
                time = DelayPlaing;
            }
        }
        else if (current == MusicList.Count) MixList();
    }
}