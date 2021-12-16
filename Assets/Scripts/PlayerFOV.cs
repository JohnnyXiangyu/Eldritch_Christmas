using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    public LayerMask obstacles;

    public float viewRadius = 5f;
    [Range(0,360)] public float viewAngle = 100f;
    public int rayCount = 10;

    float angleIncrease;

    Mesh mesh;
    Player player;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    void Start()
    {
        mesh = new Mesh();
        player = transform.parent.GetComponent<Player>();
        GetComponent<MeshFilter>().mesh = mesh;

        angleIncrease = viewAngle / rayCount;

        vertices = new Vector3[rayCount + 2];
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount * 3];
    }

    void Update()
    {
        vertices[0] = Vector2.zero;
        float angle = transform.eulerAngles.z + viewAngle/2;
        if (angle >= 360f)
        {
            angle -= 360f;
        }

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, DirectionFromAngle(angle, false), viewRadius, obstacles);
            Debug.DrawRay(transform.position, DirectionFromAngle(i*angleIncrease - viewAngle/2) * viewRadius, Color.green, 0.01f);
            if (!hit.collider)
            {
                vertex = Vector2.zero + DirectionFromAngle(angle, false) * viewRadius;
            }
            else
            {
                vertex = hit.point;
                Debug.Log(vertex + " " + i);
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;

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
