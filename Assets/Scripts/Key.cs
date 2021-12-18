using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            Invoke("Dead", 0.75f);
        }
    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }
}
