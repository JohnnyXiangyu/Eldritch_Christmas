using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            int nextSceneNo = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneNo);
        }
    }
}
