using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator animator;
    private CharacterController charController;
    private CollisionFlags collisionFlags = CollisionFlags.None;

    private float moveSpeed = 5f;
    private bool canMove;
    private bool finished_Movement = false;

    private Vector3 player_Move =  Vector3.zero;
    private Vector3 target_Position = Vector3.zero;

    private float gravity = 9.8f;

    private float player_ToPointDistance;
    private float height;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator> ();
        charController = GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update ()
    {
        CalculateHeight ();
        CheckIfFinishedMovement ();
    
    }

    bool IsGrounded ()
    {
        return collisionFlags == CollisionFlags.CollidedBelow ? true : false;
    }

    void CalculateHeight ()
    {
        if(IsGrounded ())
        {
            height = 0;
        }
        else
        {
            height -= gravity * Time.deltaTime;
        }
    }

    void CheckIfFinishedMovement ()
    {
        if (!finished_Movement)
        {
            if(!animator.IsInTransition (0) 
               && !animator.GetCurrentAnimatorStateInfo (0).IsName ("Stand") 
               && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                finished_Movement = true;
            }
            else
            {
                MoveThePlayer();
                player_Move.y = height * Time.deltaTime;
                collisionFlags = charController.Move(player_Move);
                
            }
        }
    }

    void MoveThePlayer()
    {
        if(Input.GetMouseButtonDown (0))
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast (ray, out hit))
            {
                if(hit.collider is TerrainCollider)
                {
                    player_ToPointDistance = Vector3.Distance (transform.position,hit.point);

                    if(player_ToPointDistance >= 1.0f)
                    {
                        canMove = true;
                        target_Position = hit.point;
                    }

                }
            }
        }

        if (canMove)
        {
            animator.SetFloat("Walk", 1.0f);
            Vector3 target_Temp = new Vector3(target_Position.x,
                                              transform.position.y, 
                                              target_Position.z);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(target_Temp - transform.position), 
                                                  15.0f * Time.deltaTime);
            player_Move = transform.transform.TransformDirection(Vector3.forward) * moveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, target_Position) <= 0.5f)
            {
                canMove = false;
            }
        }
        else
        {
            player_Move.Set(0f, 0f, 0f);
            animator.SetFloat("Walk", 0f);
        }
    }

    public bool finishedMovement
    {
        get
        {
            return finished_Movement;
        }
        set
        {
            finished_Movement = value;
        }
    }

    public Vector3 targetPosition
    {

        get
        {
            return target_Position;
        }
        set
        {
            target_Position = value;
        }
    }
}
