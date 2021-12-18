using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchableAmmo : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;
    [SerializeField] float stunDuration = 2;

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
            StartCoroutine(StunRoutine(collision.gameObject.GetComponent<EnemyFOV>()));
        }

        Destroy(gameObject);
    }

    private IEnumerator StunRoutine(EnemyFOV enemy)
    {
        enemy.enabled = false;

        float time = 0;
        while (time < stunDuration)
        {
            time += Time.deltaTime;
            yield return null;  
        }

        enemy.enabled = true;
    }
}
