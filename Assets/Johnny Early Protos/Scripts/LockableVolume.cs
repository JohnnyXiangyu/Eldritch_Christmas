using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Programmable volume that can be used to implement different sorts of doors.
/// </summary>
public class LockableVolume : MonoBehaviour
{
    // delegations
    [SerializeField]
    UnityEvent beforeLocking;
    [SerializeField]
    UnityEvent afterLocking;
    [SerializeField]
    UnityEvent beforeUnlocking;
    [SerializeField]
    UnityEvent afterUnlocking;

    public void Lock()
    {
        beforeLocking.Invoke();
        afterLocking.Invoke();
    }

    public void Unlock()
    {
        beforeUnlocking.Invoke();
        afterUnlocking.Invoke();
    }
}
