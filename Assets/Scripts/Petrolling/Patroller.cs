using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Patroller : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] protected List<GameObject> nodes; // child objects should be able to get it

    State state = State.WAITING;
    Vector3 dest;

    private enum State
    {
        WAITING,
        MOVING,
        CHASING
    }

    protected abstract Vector3 GetNextDest();

    /// <summary>
    /// Return true to switch to chasing mode.
    /// </summary>
    protected abstract bool DetectTarget();

    /// <summary>
    /// Return true to continue chasing, return false to stop chasing.
    /// </summary>
    protected abstract bool ChaseBehavior();

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
                    if (DetectTarget())
                    {
                        state = State.CHASING;
                    }
                    else if (((Vector2) dest - (Vector2) transform.position).magnitude > 0.01f)
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
            case State.CHASING:
                {
                    if (!ChaseBehavior())
                    {
                        state = State.MOVING;
                    }
                    break;
                }
        }
    }
}
