using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoFixWeb : MonoBehaviour
{
    private void Start()
    {
        var videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Intro Scene with Audio Fade.mp4");
        videoPlayer.Play();
    }
}
