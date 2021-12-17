using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableAmmo : Pickup
{
    public PlayerMag magazine = null;
    public GameObject throwablePrefab = null;

    private void Start()
    {
        if (magazine == null)
        {
            magazine = GameObject.FindWithTag("Player").GetComponent<PlayerMag>();
        }
    }

    protected override void PickedUp()
    {
        magazine.AddItem(this);
    }
}
