using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
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
        background.gameObject.SetActive(true);
        background.DOFade(1f, 2f);
        background.transform.GetChild(0).GetComponent<Text>().DOFade(1f, 2f);
        background.transform.GetChild(1).GetComponent<Image>().DOFade(1f, 2f);
        background.transform.GetChild(2).GetComponent<Image>().DOFade(1f, 2f);

        if (Gamepad.current != null || Joystick.current != null)
        {
            Debug.Log(Gamepad.current.displayName);
            GameObject.FindObjectOfType<EventSystem>().SetSelectedGameObject(background.transform.GetChild(1).gameObject);
        }
        else
        {
            GameObject.FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
