using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Sprite normalSlot;
    [SerializeField] Sprite selectedSlot;
    [SerializeField] Image background;
    [SerializeField] Image itemBox;

    public string title { get; set; }
    public string description { get; set; }

    InventoryUI ui;

    private void Start()
    {
        ui = GameObject.FindWithTag("InventoryUI").GetComponent<InventoryUI>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ui.ChangeSelection(this);
    }

    public void Select()
    {
        background.sprite = selectedSlot;
    }

    public void Unselect()
    {
        background.sprite = normalSlot;
    }

    public void SetPicture(Sprite itemSpirte)
    {
        itemBox.sprite = itemSpirte;
        itemBox.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        itemBox.gameObject.SetActive(false);
    }
}
