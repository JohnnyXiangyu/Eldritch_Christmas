using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableTrigger : MonoBehaviour
{
    public HashSet<string> othersTags = new HashSet<string>();
    public Collider2D cd;

    private void Start()
    {
        cd = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        othersTags.Add(collision.gameObject.tag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        othersTags.Remove(collision.tag);
    }

    public void Lock()
    {
        cd.isTrigger = false;
    }

    public void Unlock()
    {
        cd.isTrigger = true;
    }

    public bool IsLocked()
    {
        return !cd.isTrigger;
    }
}
