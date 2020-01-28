using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Awake()
    {
        StreamVideo[] sv = FindObjectsOfType<StreamVideo>();

        if (sv.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.1f);
            break;
        }

        videoPlayer.Play();
    }


}
