using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyFOV : MonoBehaviour
{
    public GameObject fireball;

    public float viewRadius = 3;
    [Range(0,360)] public float viewAngle = 100;
    public float angleOffset = 0f;
    public bool activelySearching = true;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();

    void FixedUpdate()
    {
        if (!activelySearching)
        {
            return;
        }

        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        foreach (Collider2D target in targetsInViewRadius)
        {
            Transform t = target.transform;

            // Ignore self
            if(t == this.transform)
            {
                continue;
            }

            Vector2 dirToTarget = (t.position - transform.position).normalized;

            if (Vector2.Angle(Quaternion.Euler(0, 0, angleOffset) * transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, t.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    activelySearching = false;
                    gameObject.layer = 0;
                    // t.GetComponent<Player>().input.Disable();
                    // t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GameObject fb = Instantiate(fireball, transform.position, Quaternion.LookRotation(transform.forward, t.position - transform.position));
                    fb.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                    fb.transform.DOMove(t.position, 0.5f).OnComplete(t.GetComponent<Player>().Die);
                    Destroy(fb, 0.5f);
                }
            }
        }
    }

    public Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal = true)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }

    // Called by animation events
    public void Enable()
    {
        activelySearching = true;
    }
    public void Disable()
    {
        activelySearching = false;
    }
}
