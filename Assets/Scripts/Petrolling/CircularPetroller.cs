using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPetroller : Petroller
{
    Queue<GameObject> nodeQueue = new Queue<GameObject>();

    private void Start()
    {
        foreach (GameObject node in nodes)
        {
            nodeQueue.Enqueue(node);    
        }
    }

    protected override Vector3 GetNextDest()
    {
        var node = nodeQueue.Dequeue();
        nodeQueue.Enqueue(node);
        return node.transform.position;
    }
}
