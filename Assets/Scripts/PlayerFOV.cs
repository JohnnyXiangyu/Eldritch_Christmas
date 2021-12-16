using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    public LayerMask obstacles;
    public MeshFilter meshFilter;

    public float viewRadius = 5f;
    [Range(0,360)] public float viewAngle = 100f;
    public int rayCount = 10;
    public float startingAngle;

    float angleIncrease;

    Mesh mesh;
    Player player;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    void Start()
    {
        mesh = new Mesh();
        player = GetComponent<Player>();
        meshFilter.mesh = mesh;

        angleIncrease = viewAngle / rayCount;

        vertices = new Vector3[rayCount + 2];
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount * 3];
    }

    void Update()
    {
        Vector2 move = GetComponent<Player>().move;
        if (move != Vector2.zero)
        {
            startingAngle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90f;
        }
        float angle = startingAngle + viewAngle/2;

        vertices[0] = transform.localPosition;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, DirectionFromAngle(angle, false), viewRadius, obstacles);
            if (!hit.collider)
            {
                vertex = (Vector2)transform.position + DirectionFromAngle(angle, false) * viewRadius;
            }
            else
            {
                vertex = hit.point;
            }
            // Debug.DrawLine(transform.position, vertex, Color.green, 0.01f);
            vertices[i + 1] = vertex;

            if (i > 0)
            {
                triangles[i*3 - 3] = 0;
                triangles[i*3 - 2] = i;
                triangles[i*3 - 1] = i + 1;

                // Debug.DrawLine(vertices[0], vertices[triangles[triangleIndex + 1]], Color.red, 0.01f);
                // Debug.DrawLine(vertices[triangles[triangleIndex + 1]], vertices[triangles[triangleIndex + 2]], Color.red, 0.01f);
                // Debug.DrawLine(vertices[triangles[triangleIndex + 2]], vertices[0], Color.red, 0.01f);
            }

            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal = true)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector2(-Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
