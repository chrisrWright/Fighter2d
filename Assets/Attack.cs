using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{


  public float moveSpeed;
  public Animator animator;
  const float groundCheckRadius = 0.2f;
  public Rigidbody2D rb;
  public bool grounded = false;
  [SerializeField]Transform groundCheckCollider;
  [SerializeField]LayerMask groundLayer;
  private float jogSpeed = 200f;
  private float dashSpeed = 400f;
  public bool idle;
  private float drift = 300f;
  public bool jumping;
  public float xMovement;
  public float yMovement;
  private bool crouching;
  public bool attacking;
  public  GameObject blossom;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        blossom = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
      xMovement = Input.GetAxisRaw("Horizontal");
      yMovement = Input.GetAxisRaw("Vertical");
      animator.SetBool("side_tilt", false);

      jumping = false;
      idle = true;
      crouching = false;
      attacking = false;
/********NEUTRALS
*/
      //side tilt
      if(Input.GetButtonDown("Neutral"))
      {
        idle = false;

        if(Input.GetAxisRaw("Horizontal") > 0.05 && Input.GetAxisRaw("Horizontal") < .999 && grounded
        || Input.GetAxisRaw("Horizontal") < -0.05 && Input.GetAxisRaw("Horizontal") > -.999 && grounded)
        {
          blossom.GetComponent<PlayerMovement>().enabled = false;
          SideTilt();
        }
        Debug.Log("Side tilt");
      }

    }

    void SideTilt()
    {
      attacking = true;
      animator.Play("side_tilt");
      attacking = true;
    


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
