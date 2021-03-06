using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public InputActions input;
    public Animator anim;
    private Rigidbody2D rb;

    public Vector2 move;
    public float speedMult = 1f;

    void Awake()
    {
        input = new InputActions();

        input.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        input.Gameplay.Movement.canceled += ctx => move = Vector2.zero;

        rb = GetComponent<Rigidbody2D>();

        // Clamp values
        if (transform.parent.localScale.x != 1f)
        {
            Debug.LogError("Change the scale of Player instead of GameController!");
            transform.parent.localScale = new Vector3Int(1,1,1);
        }
    }

    void OnEnable()
    {
        input.Enable();
    }

    void Update()
    {
        rb.velocity = move * speedMult;

        if (rb.velocity.sqrMagnitude > float.Epsilon)
        {
            anim.SetBool("moving", true);

            float angleFromUp = Vector2.SignedAngle(Vector2.up, move);
            // Debug.Log(angleFromUp);
            if (angleFromUp < -45f && angleFromUp > -135f)
                anim.SetInteger("dir", 1);
            else if (angleFromUp <= -135f || angleFromUp >= 135f)
                anim.SetInteger("dir", 2);
            else if (angleFromUp < 135f && angleFromUp > 45f)
                anim.SetInteger("dir", 3);
            else 
                anim.SetInteger("dir", 0);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    public void Die()
    {
        anim.SetTrigger("die");
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<AudioSource>().Play();
    }

    public void DeathScreen()
    {
        Transform img = GameObject.Find("DeathScreen").transform.GetChild(0);
        img.GetComponent<Image>().DOFade(1f, 1f);
        img.GetChild(0).GetComponent<Text>().DOFade(1f, 1f);
        img.GetChild(1).GetComponent<Text>().DOFade(1f, 1f);
        Invoke("Reload", 3f);
    }

    void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDisable()
    {
        input.Disable();
    }

    public bool TryThrow(Vector2 throwDirection)
    {
        float angleFromUp = Vector2.SignedAngle(Vector2.up, throwDirection);

        if (rb.velocity.sqrMagnitude > float.Epsilon)
        {
            int animDirection = anim.GetInteger("dir");

            if (angleFromUp < -45f && angleFromUp > -135f)
                return animDirection == 1;
            else if (angleFromUp <= -135f || angleFromUp >= 135f)
                return animDirection == 2;
            else if (angleFromUp < 135f && angleFromUp > 45f)
                return animDirection == 3;
            else
                return animDirection == 0;
        }
        else
        {
            if (angleFromUp < -45f && angleFromUp > -135f)
                anim.SetInteger("dir", 1);
            else if (angleFromUp <= -135f || angleFromUp >= 135f)
                anim.SetInteger("dir", 2);
            else if (angleFromUp < 135f && angleFromUp > 45f)
                anim.SetInteger("dir", 3);
            else
                anim.SetInteger("dir", 0);

            anim.SetTrigger("dryTurn");

            return true;
        }
    }
}
