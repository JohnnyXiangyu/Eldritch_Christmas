using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

public class TitleScreen : MonoBehaviour
{
    public VideoPlayer video;
    public Image background;

    void Start()
    {
        video.Play();
        video.loopPointReached += ctx => ShowTitleScreen();
    }

    void ShowTitleScreen()
    {
        video.gameObject.SetActive(false);
        background.DOFade(1f, 2f);
    }
}
