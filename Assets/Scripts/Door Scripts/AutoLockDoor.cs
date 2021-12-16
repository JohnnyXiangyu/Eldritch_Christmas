using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Programmable volume that can be used to implement different sorts of doors.
/// </summary>
public class AutoLockDoor : MonoBehaviour
{    
    // component references
    [SerializeField] LockableTrigger enterLine;
    [SerializeField] LockableTrigger leaveLine;
    
    // delegations
    [SerializeField] UnityEvent beforeLocking;
    [SerializeField] UnityEvent afterLocking;
    [SerializeField] UnityEvent beforeUnlocking;
    [SerializeField] UnityEvent afterUnlocking;

    public void Lock()
    {
        beforeLocking.Invoke();
        enterLine.Lock();
        afterLocking.Invoke();
    }

    public void Unlock()
    {
        beforeUnlocking.Invoke();
        enterLine.Unlock();
        afterUnlocking.Invoke();
    }

    private void Update()
    {
        if (!enterLine.othersTags.Contains("Player") && leaveLine.othersTags.Contains("Player") && !enterLine.IsLocked())
        {
            Lock();
            Debug.Log("locked:");
            Debug.Log(gameObject);
        }
    }
}
