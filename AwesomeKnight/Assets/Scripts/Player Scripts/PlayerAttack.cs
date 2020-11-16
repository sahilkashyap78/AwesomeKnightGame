using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAttack : MonoBehaviour
{
    public Image fillWaitImage_1;
    public Image fillWaitImage_2;
    public Image fillWaitImage_3;
    public Image fillWaitImage_4;
    public Image fillWaitImage_5;
    public Image fillWaitImage_6;

    private PlayerMove playerMove;
    private Animator animator;
    bool canAttack = true;
    public int[] fadeImage = new int[] { 0, 0, 0, 0, 0, 0 };

    void Awake()
    {
        playerMove = GetComponent<PlayerMove> ();
        animator = GetComponent<Animator> ();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(!animator.IsInTransition (0) && animator.GetCurrentAnimatorStateInfo (0).IsName("Stand"))
        {
            //here if animation is not the attack animation then we can attack
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        CheckToFade ();
        CheckInput ();
        
    }

    void CheckInput ()
    {
        //first we check that our attack animation is not playing
        if(animator.GetInteger ("Atk") == 0)
        {
            //we need to set our finished movement to false which we get from playermove script
            // we need to set the atk valuse form 1 to 6 in order to attack
            playerMove.finishedMovement = false; // as player doesnt finihed the movement

            if(!animator.IsInTransition (0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
            {
                playerMove.finishedMovement = true;
            }

        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerMove.targetPosition = transform.position; // we dont want to move
            if(playerMove.finishedMovement && fadeImage [0] != 1 && canAttack)
            {
                fadeImage[0] = 1;
                animator.SetInteger("Atk", 1);
            }
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerMove.targetPosition = transform.position; // we dont want to move
            if (playerMove.finishedMovement && fadeImage[1] != 1 && canAttack)
            {
                fadeImage[1] = 1;
                animator.SetInteger("Atk", 2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerMove.targetPosition = transform.position; // we dont want to move
            if (playerMove.finishedMovement && fadeImage[2] != 1 && canAttack)
            {
                fadeImage[2] = 1;
                animator.SetInteger("Atk", 3);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerMove.targetPosition = transform.position; // we dont want to move
            if (playerMove.finishedMovement && fadeImage[3] != 1 && canAttack)
            {
                fadeImage[3] = 1;
                animator.SetInteger("Atk", 4);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerMove.targetPosition = transform.position; // we dont want to move
            if (playerMove.finishedMovement && fadeImage[4] != 1 && canAttack)
            {
                fadeImage[4] = 1;
                animator.SetInteger("Atk", 5);
            }
        }
        else if (Input.GetMouseButtonDown (1))
        {
            playerMove.targetPosition = transform.position; // we dont want to move
            if (playerMove.finishedMovement && fadeImage[5] != 1 && canAttack)
            {
                fadeImage[5] = 1;
                animator.SetInteger("Atk", 6);
            }
        }
        else
        {
            animator.SetInteger("Atk", 0);
        }

        // rotating the player by pressing the space toward the mouse
        if(Input.GetKey(KeyCode.Space))
        {
            Vector3 targetPos = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), 15f * Time.deltaTime);

            }
        }
    }

    void CheckToFade ()
    {
        if(fadeImage [0] == 1)
        {
            if(FadeAndWait (fillWaitImage_1 ,1.0f))
            {
                fadeImage[0] = 0;
            }
        }

        if (fadeImage[1] == 1)
        {
            if (FadeAndWait(fillWaitImage_2, 0.7f))
            {
                fadeImage[1] = 0;
            }
        }

        if (fadeImage[2] == 1)
        {
            if (FadeAndWait(fillWaitImage_3, 0.1f))
            {
                fadeImage[2] = 0;
            }
        }

        if (fadeImage[3] == 1)
        {
            if (FadeAndWait(fillWaitImage_4, 0.3f))
            {
                fadeImage[3] = 0;
            }
        }

        if (fadeImage[4] == 1)
        {
            if (FadeAndWait(fillWaitImage_5, 0.3f))
            {
                fadeImage[4] = 0;
            }
        }

        if (fadeImage[5] == 1)
        {
            if (FadeAndWait(fillWaitImage_6, 0.08f))
            {
                fadeImage[5] = 0;
            }
        }
    }

    bool FadeAndWait (Image fadeImg, float fadeTime)
    {
        bool faded = false;

        if(fadeImage ==null)
        {
            return faded;
        }

        if (!fadeImg.gameObject.activeInHierarchy)
        {
            fadeImg.gameObject.SetActive(true);
            fadeImg.fillAmount = 1f;
        }
        fadeImg.fillAmount -= fadeTime * Time.deltaTime;

        if (fadeImg.fillAmount <= 0.0f)
        {
            fadeImg.gameObject.SetActive(false);
            faded = true;
        }

        return faded;
    }
}
