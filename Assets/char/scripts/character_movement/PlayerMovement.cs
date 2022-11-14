using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

private float timer = -1;
    public float jumpForce = 10f;
    public float wavedashForce = 700f;
    public float skid = 2f;
    public bool facingRight;
    public float moveSpeed;
    public static bool IsInputEnabled = true;
    public Animator animator;
    const float groundCheckRadius = 0.2f;
    public Rigidbody2D rb;
    public bool grounded = false;
    [SerializeField]Transform groundCheckCollider;
    [SerializeField]LayerMask groundLayer;
    private float jogSpeed = 200f;
    private float jogAirSpeed = 20f;
    private float dashSpeed = 400f;
    public bool idle;
    private float dashAirSpeed = 7.5f;
    private float drift = 300f;
    public bool jumping;
    public float xMovement;
    public float yMovement;
    public bool attacking;
    public bool crouching;
    public int jumpCount;
    public Transform transform;



    // Update is called once per frame

    private void Awake()
    {
      animator = GetComponent<Animator>();
      rb = GetComponent<Rigidbody2D>();
      //Debug.Log ("Base: " + animator.GetLayerIndex("dashing"));
      facingRight = false;
    }

    void Update()
    {

      Vector3 characterScale = transform.localScale;
      if(characterScale.x > 0)
      {
        facingRight = true;
      }
      if(timer > 0)
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = -1;
            attacking = false;
        }
    }

      if(attacking)
      {
        PlayerMovement.IsInputEnabled = false;
      }
      if(!attacking)
      {
        PlayerMovement.IsInputEnabled = true;
      }
      //****MOVEMENT
      xMovement = Input.GetAxisRaw("Horizontal");
      yMovement = Input.GetAxisRaw("Vertical");
      animator.SetBool("dashing", false);
      animator.SetBool("crouching", false);
      animator.SetBool("side_tilt", false);
//////////////////////////////////////////////
      animator.SetBool("jumping", false);

/////////////////////////////////////////////////
      animator.SetBool("idle", true);
      if(grounded == false)
      {
        animator.SetBool("idle", false);
        animator.SetBool("jumping", true);
      }

      jumping = false;
      idle = true;
      crouching = false;

      moveSpeed = 0;


      if(PlayerMovement.IsInputEnabled )
      {
      if(Input.GetAxis("Horizontal") > 0.005  && crouching == false && grounded)
      {
        if(jumping)
        {
          return;
        }
        facingRight = true;
        idle = false;
        animator.SetBool("idle", false);
        characterScale.x = 3;

      }
    }

    if(PlayerMovement.IsInputEnabled)
    {

      if(Input.GetAxis("Horizontal") < -0.005 && crouching == false && grounded)
      {
        if(jumping)
        {
          return;
        }
        facingRight = false;
        idle = false;
        animator.SetBool("idle", false);
        characterScale.x = -3;
      }
    }
      transform.localScale = characterScale;

      if(PlayerMovement.IsInputEnabled)
      {
      if(Input.GetAxisRaw("Horizontal") > 0.05 && Input.GetAxisRaw("Horizontal") < .999 && grounded
      || Input.GetAxisRaw("Horizontal") < -0.05 && Input.GetAxisRaw("Horizontal") > -.999 && grounded)
      {
        moveSpeed = jogSpeed;
        animator.SetBool("idle", false);
        idle = false;
      }
    }

    if(PlayerMovement.IsInputEnabled)
    {
      if(Input.GetAxisRaw("Horizontal") > .999 && grounded ||Input.GetAxisRaw("Horizontal") < -.999 && grounded)
      {
        moveSpeed = dashSpeed;
        animator.SetBool("idle", false);
        animator.SetBool("dashing", true);
        idle = false;
      }
    }

    if(PlayerMovement.IsInputEnabled)
    {
      if(Input.GetAxisRaw("Vertical") < -.1 && grounded)
      {
        animator.SetBool("crouching", true);
        crouching = true;

      }
    }
//***JUMPING
if(grounded)
{
  jumpCount = 1;
}

if(PlayerMovement.IsInputEnabled)
{
    if(Input.GetButtonDown("Jump"))
     {
       grounded = false;
       animator.SetBool("jumping", true);
       StartCoroutine(Waiting());
       animator.SetBool("idle", false);
       jumping = true;
       if(grounded && jumpCount > 0)
       {
         JumpUp();
         jumpCount = jumpCount - 1;
       }

       if(!grounded && jumpCount > 0)
       {
         JumpUp();
         jumpCount = jumpCount - 1;
       }

     }
   }

      if(!grounded)
      {
        jumping = true;
        moveSpeed = drift;
        animator.SetBool("grounded", false);
        animator.SetBool("dashing", false);
      }


//NEUTRALS
      if(PlayerMovement.IsInputEnabled)
      {

        if(Input.GetButtonDown("Neutral") &&Input.GetAxisRaw("Horizontal") > 0.05 && Input.GetAxisRaw("Horizontal") < .999 && grounded
        || Input.GetAxisRaw("Horizontal") < -0.05 && Input.GetAxisRaw("Horizontal") > -.999 && grounded && Input.GetButtonDown("Neutral"))
        {
          idle = false;
        //  blossom.GetComponent<PlayerMovement>().enabled = false;
          SideTilt();
          Debug.Log("Side tilt");
        }

        if(grounded && Input.GetAxisRaw("Vertical") > 0.05 && Input.GetButtonDown("Neutral"))
        {
          idle = false;

          UpTilt();
          Debug.Log("up tilt");
        }

        if(grounded && Input.GetAxisRaw("Vertical") < -0.05 && Input.GetButtonDown("Neutral"))
        {
          idle = false;

          DownTilt();
          Debug.Log("down tilt");
        }

        if(Input.GetButtonDown("Neutral") && xMovement > 0.05  && jumping && facingRight ||
        Input.GetButtonDown("Neutral") && xMovement < -0.05  && jumping && !facingRight)
        {
          idle = false;
          fAir();
          Debug.Log("fAir");
        }

        if(Input.GetButtonDown("Neutral") && xMovement > 0.05  && jumping && !facingRight ||
        Input.GetButtonDown("Neutral") && xMovement < -0.05  && jumping && facingRight)
        {
          idle = false;
          bAir();
          Debug.Log("bAir");
        }

        if(jumping && Input.GetButtonDown("Neutral") && xMovement == 0||
        jumping && Input.GetButtonDown("Neutral") && xMovement == 0)
        {
          idle = false;
          nAir();
          Debug.Log("nAir");
        }
        if(jumping && Input.GetButtonDown("Neutral") && yMovement > 0.05)
        {
          idle = false;
          uAir();
          Debug.Log("uAir");
        }

        if(jumping && Input.GetButtonDown("Neutral") && yMovement < -0.05)
        {
          idle = false;
          dAir();
          Debug.Log("dAir");
        }
        bool dashCheck = animator.GetBool("dashing");
        if(dashCheck && Input.GetButtonDown("Neutral"))
        {
          DashAttack();
                    Debug.Log("dash attack");
        }

        if(idle && Input.GetButtonDown("Neutral"))
        {
          idle = false;
          Jab();
          Debug.Log("jab");
        }

    }



      animator.SetFloat("yVelocity", rb.velocity.y);

//***WAVEDASH
      if(Input.GetButtonDown("R") && yMovement < 0)
      {
        if(!grounded)
        {
          Wavedash(xMovement, yMovement);
        }

      }
//ATTACKING


    }

      void Wavedash(float x, float y)
      {
        if(y > 0)
        {
          return;
        }
        float xVal = x * wavedashForce * Time.deltaTime;
        float yVal = y * wavedashForce * Time.deltaTime;
        Vector2 targetVelocity = new Vector2(xVal, yVal);
        rb.velocity = targetVelocity;
        //rb.AddForce(transform.up * targetVelocity);
        Debug.Log("wavedsah");
      }

      void JumpUp()
            {

                grounded = false;
                rb.velocity =  transform.up * jumpForce;

                animator.SetBool("idle", false);

                Debug.Log("grounded");




              jumping = true;
              idle = false;

              Debug.Log("jump");
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

//***FRAME DELAY

    IEnumerator Waiting()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3/60);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    void FixedUpdate()
    {
      GroundCheck();




      Move(xMovement);


    }

    void Move(float dir)
    {


      if(crouching)
      {
        return;
      }

      if(attacking && grounded)
      {
        return;
      }

      float xVal = dir * moveSpeed * Time.deltaTime;
      Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
      rb.velocity = targetVelocity;


    }
/*ATTACK*/

    /********NEUTRALS
    */
          //side tilt



          void SideTilt()
          {
            float lag = 36/60f;

            animator.Play("side_tilt");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));
            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);

          }

          void UpTilt()
          {
            float lag = 13/60f;

            animator.Play("up_tilt");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));
            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);
          }

          void DownTilt()
          {
            float lag = 16/60f;

            animator.Play("down_tilt");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));
            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);
          }

          void DashAttack()
          {

            float lag = 40/60f;
            animator.Play("dash_attack");
            if(facingRight)
            {
              rb.velocity = new Vector2(9,rb.velocity.y);
            }
            else
            {
              rb.velocity = new Vector2(-9,rb.velocity.y);
            }

            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));
            PlayerMovement.IsInputEnabled = true;
              Freeze(lag);
          }

          void fAir()
          {
            float lag = 16/60f;

            animator.Play("fair");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));
            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);

          }

          void bAir()
          {
            float lag = 15/60f;

            animator.Play("bair");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));
            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);

          }

          void nAir()
          {
            float lag = 28/60f;

            animator.Play("nair");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));

            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);

          }

          void uAir()
          {
            float lag = 45/60f;

            animator.Play("uair");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));

            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);

          }

          void dAir()
          {
            float lag = 30/60f;

            animator.Play("dair");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));

            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);

          }

          void Jab()
          {
            float lag = 8/60f;

            animator.Play("jab");
            PlayerMovement.IsInputEnabled = false;
            //StartCoroutine(Lag(lag));

            PlayerMovement.IsInputEnabled = true;

            Freeze(lag);

          }

          void Freeze(float lag)
          {

              attacking = true;
              timer = lag;
          }

    IEnumerator Lag(float endlag)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(endlag);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }


}
