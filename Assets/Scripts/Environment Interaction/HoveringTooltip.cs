using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoveringTooltip : MonoBehaviour
{
    [SerializeField] protected GameObject tooltip;

    protected bool entered = false;
    protected bool showingTooltip = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (showingTooltip && tooltip && collision.tag == "Player")
        {
            entered = true;
            tooltip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tooltip && collision.tag == "Player")
        {
            entered = false;
            tooltip.SetActive(false);
        }
    }
}
