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
    public int rayCount = 500;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public MeshFilter meshFilter;
    public Transform origin;

    float angleIncrease;

    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    void Start()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        angleIncrease = viewAngle / rayCount;

        vertices = new Vector3[rayCount + 2];
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount * 3];

        origin = transform;
    }

    void Update()
    {
        if (!activelySearching)
        {
            mesh.Clear();
            return;
        }

        float angle = angleOffset + viewAngle/2 + 90f;

        vertices[0] = origin.position;

        if (meshFilter.gameObject.layer == 0)
        {
            Debug.Log(vertices[0]);
        }

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(origin.position, DirectionFromAngle(angle, false), viewRadius, obstacleMask);
            if (!hit.collider)
            {
                vertex = (Vector2)origin.position + DirectionFromAngle(angle, false) * viewRadius;
            }
            else
            {
                vertex = hit.point;
            }
            // Debug.DrawLine(origin.position, vertex, Color.green, 0.01f);
            vertices[i + 1] = vertex;

            if (i > 0)
            {
                triangles[i*3 - 3] = 0;
                triangles[i*3 - 2] = i;
                triangles[i*3 - 1] = i + 1;

                // Debug.DrawLine(vertices[0], vertices[triangles[i*3 - 2]], Color.red, 0.01f);
                // Debug.DrawLine(vertices[triangles[i*3-2]], vertices[triangles[i*3-1]], Color.red, 0.01f);
                // Debug.DrawLine(vertices[triangles[i*3-1]], vertices[0], Color.red, 0.01f);
            }

            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin.position, Vector3.one * 1000f);
        meshFilter.transform.position = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (!activelySearching)
        {
            return;
        }

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
                    t.GetComponent<Player>().input.Disable();
                    t.GetComponent<Player>().anim.SetBool("moving", false);
                    t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GameObject fb = Instantiate(fireball, transform.position, Quaternion.LookRotation(transform.forward, t.position - transform.position));
                    fb.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                    fb.transform.DOMove(t.position, 0.5f).OnComplete(t.GetComponent<Player>().Die);
                    Destroy(fb, 0.5f);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.layer = 0;
            Player p = other.gameObject.GetComponent<Player>();
            p.input.Disable();
            p.anim.SetBool("moving", false);
            p.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            p.Die();
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
