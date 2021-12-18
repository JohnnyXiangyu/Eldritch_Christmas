using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    PlayerMag magazine;
    public List<MetaInfo> itemList { get; private set; }  = new List<MetaInfo>();

    public class MetaInfo
    {
        public MetaInfo(string intitle, string indescription, Sprite inthumbnail)
        {
            title = intitle;
            description = indescription;
            thumbnail = inthumbnail;
        }

        public string title;
        public string description;
        public Sprite thumbnail;
    }

    private void Start()
    {
        magazine = GetComponent<PlayerMag>();
    }

    public bool AddItem(Pickup newItem)
    {
        if (itemList.Count < 9)
        {
            itemList.Add(new MetaInfo(newItem.itemName, newItem.itemDescription, newItem.thumbnail));
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveOne(string name)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].title == name)
            {
                itemList.RemoveAt(i);
                return;
            }
        }
    }
}
