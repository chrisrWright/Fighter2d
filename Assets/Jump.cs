using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

    public float jumpForce = 1000f;
    public Animator animator;
    const float groundCheckRadius = 0.2f;
    public Rigidbody2D rb;
    public bool grounded = false;
    [SerializeField]Transform groundCheckCollider;
    [SerializeField]LayerMask groundLayer;
    public bool jumping;
    public bool idle;
    // Start is called before the first frame update
    void Start()
    {
      animator = GetComponent<Animator>();
      rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

      if(Input.GetButtonDown("Jump"))
     {
       JumpUp();
     }

  else if(Input.GetButtonUp("Jump"))
     {
       jumping = false;
     }

    }

    void GroundCheck()
    {
      grounded = false;
      Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
      if(colliders.Length > 0)
      {
        grounded = true;
        animator.SetBool("grounded", true);
      }

      if(grounded == false)
      {
        animator.SetBool("dashing", false);
        animator.SetBool("grounded", false);
      }

    }


    void JumpUp()
        {
          if(grounded)
          {
              grounded = false;
          rb.velocity =  transform.up * jumpForce;


          animator.SetBool("idle", false);
          animator.SetBool("jumping", true);
          Debug.Log("grounded");
          }

          jumping = true;
          idle = false;

          Debug.Log("jump");
        }

        void FixedUpdate()
        {
          GroundCheck();
        }
}
