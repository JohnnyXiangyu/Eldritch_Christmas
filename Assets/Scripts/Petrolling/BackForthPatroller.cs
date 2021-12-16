using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackForthPatroller : Petroller
{
    Queue<GameObject> activeNodes = new Queue<GameObject>();
    Stack<GameObject> usedNodes = new Stack<GameObject>();

    private void Start()
    {
        foreach (GameObject node in nodes)
        {
            if (node != null)
            {
                activeNodes.Enqueue(node);
            }
        }
    }

    protected override Vector3 GetNextDest()
    {
        if (activeNodes.Count <= 0)
        {
            while (usedNodes.Count > 0)
            {
                activeNodes.Enqueue(usedNodes.Pop());
            }
        }

        GameObject node = activeNodes.Dequeue();
        usedNodes.Push(node);
        return node.transform.position;
    }
}
