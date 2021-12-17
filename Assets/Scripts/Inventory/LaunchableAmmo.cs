using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchableAmmo : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;
    [SerializeField] float maxDistance = 10;
    [SerializeField] string targetType = "";

    Vector3 direction = Vector3.zero;
    Vector3 oldLocation = Vector3.zero;

    public void Launch(Vector3 inDirection)
    {
        inDirection.z = 0;
        direction = inDirection;

        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            StunnableEnemy enemy = collision.collider.GetComponent<StunnableEnemy>();
            if (enemy.GetEnemyType() == targetType)
            {
                // TODO: implement whatever boss should do
            }
        }

        Destroy(gameObject);
    }
}
