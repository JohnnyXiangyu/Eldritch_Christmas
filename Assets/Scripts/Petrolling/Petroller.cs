using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Petroller : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] protected List<GameObject> nodes; // child objects should be able to get it

    State state = State.WAITING;
    Vector3 dest;

    private enum State
    {
        WAITING,
        MOVING
    }

    protected abstract Vector3 GetNextDest();

    private void Update()
    {
        if (nodes.Count == 0)
            return;

        switch (state)
        {
            case State.WAITING:
                {
                    dest = GetNextDest();
                    state = State.MOVING;
                    break;
                }
            case State.MOVING:
                {
                    if (((Vector2) dest - (Vector2) transform.position).magnitude > 0.01f)
                    {
                        Vector3 direction = dest - transform.position;
                        direction.z = 0;
                        float maxMovement = direction.magnitude;
                        direction.Normalize();

                        transform.position += direction * Mathf.Min(speed * Time.deltaTime, maxMovement);
                    }
                    else
                    {
                        state = State.WAITING;
                    }
                    break;
                }
        }
    }
}
