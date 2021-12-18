using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] List<InventorySlot> slots = new List<InventorySlot>();

    InventorySlot oldSlot = null;

    PlayerInventory inventory;
    InputActions input;

    private void Awake()
    {
        input = new InputActions();

        input.Gameplay.InventoryMenu.performed += context =>
        {
            if (inventory == null)
            {
                inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
            }

            if (gameObject.activeSelf)
            {
                Time.timeScale = 1;
                gameObject.SetActive(false);
            }
            else
            {
                int i = 0;

                foreach (var slot in slots)
                {
                    slot.Unselect();
                }

                title.text = "";
                description.text = "";

                // update items
                foreach (var item in inventory.itemList)
                {
                    slots[i].title = item.title;
                    slots[i].description = item.description;
                    slots[i].SetPicture(item.thumbnail);
                    i++;
                }

                Time.timeScale = 0;
                gameObject.SetActive(true);
            }
        };

        input.Enable();
        gameObject.SetActive(false);
    }

    public void ChangeSelection(InventorySlot newSlot)
    {
        if (oldSlot == newSlot)
            return;

        if (oldSlot)
        {
            oldSlot.Unselect();
        }

        oldSlot = newSlot;

        // update info
        title.text = newSlot.title;
        description.text = newSlot.description;
    }
}
