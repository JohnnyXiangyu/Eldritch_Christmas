using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : HoveringTooltip
{
    public string itemName;
    public string itemDescription;
    public Sprite thumbnail;

    [SerializeField] UnityEvent onPickUp;

    InputActions input;

    private void Awake()
    {
        input = new InputActions();

        input.Gameplay.Interact.performed += context =>
        {
            if (entered && GameObject.FindWithTag("Player").GetComponent<PlayerInventory>().AddItem(this))
            {
                onPickUp.Invoke();
                PickedUp();
                GetComponent<SpriteRenderer>().enabled = false;
                this.enabled = false;

                for (int i = 0; i < transform.childCount; i++)
                    Destroy(transform.GetChild(i).gameObject);
            }       
        };
    }

    protected virtual void PickedUp()
    {

    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
