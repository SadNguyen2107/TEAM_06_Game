using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TopDownCharacterController : MonoBehaviour
{
    Rigidbody2D playerRigidbody2D;
    public float speed;

    Animator animator;

    [Header("Controller")]
    [SerializeField] FloatingJoystick _joyStick;

    private void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 dir = Vector2.zero;
        // dir.x = Input.GetAxis("Horizontal");
        // dir.y = Input.GetAxis("Vertical");
        dir.x = _joyStick.Horizontal;
        dir.y = _joyStick.Vertical;

        if (dir.x < 0)
        {
            animator.SetInteger("Direction", 3);
        }
        else if (dir.x > 0)
        {
            animator.SetInteger("Direction", 2);
        }

        if (dir.y > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (dir.y < 0)
        {
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        playerRigidbody2D.velocity = speed * Time.fixedDeltaTime * dir;
    }
}

