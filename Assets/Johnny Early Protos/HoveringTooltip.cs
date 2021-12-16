using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoveringTooltip : MonoBehaviour
{
    [SerializeField] protected GameObject tooltip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tooltip && collision.tag == "Player")
            tooltip.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tooltip && collision.tag == "Player")
            tooltip.SetActive(false);
    }
}
