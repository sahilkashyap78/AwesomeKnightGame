using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE,
    WALK,
    RUN,
    PAUSE,
    GOBACK,
    ATTACK,
    DEATH
}
public class EnemyController : MonoBehaviour
{
    private float attack_Distance = 1.5f;
    private float alert_Attack_Distance = 8f;
    private float followDistace = 15f;

    private float enemyToPlayerDistance;

    [HideInInspector]
    public EnemyState enemy_CurrentState = EnemyState.IDLE;
    private EnemyState enemy_LastState = EnemyState.IDLE;

    private Transform playerTarget;
    private Vector3 initialPosition;

    private float move_Speed = 2f;
    private float walk_Speed = 1f;

    private CharacterController charController;
    private Vector3 whereTo_Move = Vector3.zero;

    //attack variables
    private float currentAttackTime;
    private float waitAttackTime = 1f;

    private Animator animator;
    private bool finished_Animation = true;
    private bool finished_Movement = true;

    private NavMeshAgent navAgent;
    private Vector3 whereTo_Navigate;

    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        whereTo_Navigate = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //IF HEALTH IS<= WE WILL SET THE STATE TO DEATH

        if(enemy_CurrentState !=EnemyState.DEATH)
        {
            enemy_CurrentState = SetEnemyState(enemy_CurrentState, enemy_LastState, enemyToPlayerDistance);
            if(finished_Movement)
            {
                GetStateControl(enemy_CurrentState);
            }
            else
            {
                if(!animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    finished_Movement = true;
                }else if(!animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsTag("Atk1") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Atk2"))
                {
                    animator.SetInteger("Atk", 0);
                }
            }
        }
        else
        {
            animator.SetBool("Death", true);
            charController.enabled = false;
            navAgent.enabled = false;

            if(!animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
            {
                Destroy(gameObject, 2f);
            }
        }
        
    }

    EnemyState SetEnemyState(EnemyState currentState, EnemyState lastState, float enemyToPlayerDis)
    {
        float initialDistance = Vector3.Distance(initialPosition, transform.position);
        enemyToPlayerDis = Vector3.Distance(transform.position, playerTarget.position);

        if(initialDistance > followDistace)
        {
            lastState = currentState;
            currentState = EnemyState.GOBACK;  // go back to petroling
        }else if (enemyToPlayerDis <= attack_Distance)
        {
            lastState = currentState;
            currentState = EnemyState.ATTACK;
        }else if (enemyToPlayerDis >= alert_Attack_Distance && lastState == EnemyState.PAUSE || lastState== EnemyState.ATTACK)
        {
            lastState = currentState;
            currentState = EnemyState.PAUSE;
        }else if (enemyToPlayerDis <= alert_Attack_Distance && enemyToPlayerDis >= attack_Distance)
        {
            if(currentState != EnemyState.GOBACK || lastState == EnemyState.WALK)
            {
                lastState = currentState;
                currentState = EnemyState.PAUSE;
            }
        }else if (enemyToPlayerDis > alert_Attack_Distance && lastState != EnemyState.GOBACK && lastState != EnemyState.PAUSE)
        {
            lastState = currentState;
            currentState = EnemyState.WALK;
        }
        return currentState;
    }

    void GetStateControl(EnemyState currentState)
    {
        if(currentState == EnemyState.RUN || currentState == EnemyState.PAUSE)
        {
            if(currentState != EnemyState.ATTACK)
            {
                Vector3 targetPosition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
                if(Vector3.Distance(transform.position, targetPosition) >= 2.1f)
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("Run", true);

                    navAgent.SetDestination(targetPosition);
                }
            }
        }

        else if (currentState == EnemyState.ATTACK)
        {
            animator.SetBool("Run", false);
            whereTo_Move.Set(0f, 0f, 0f);

            navAgent.SetDestination(transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.LookRotation(playerTarget.position - transform.position), 5f * Time.deltaTime);

            if (currentAttackTime >= waitAttackTime)
            {
                int attackRange = Random.Range(1, 3);
                // for randomizing the attack
                animator.SetInteger("Atk", attackRange);
                finished_Animation = false;
                currentAttackTime = 0f;
            }
            else
            {
                animator.SetInteger("Atk", 0);
                currentAttackTime += Time.deltaTime;
            }
        }

        else if (currentState == EnemyState.GOBACK)
        {
            animator.SetBool("Run", true);
            Vector3 targetPosition = new Vector3(initialPosition.x, transform.position.y, initialPosition.z);
            navAgent.SetDestination(targetPosition);
            if (Vector3.Distance(targetPosition, initialPosition) <= 3.5f)
            {
                enemy_LastState = currentState;
                currentState = EnemyState.WALK;
            }
        }

        else if (currentState == EnemyState.WALK)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
            if (Vector3.Distance(transform.position, whereTo_Navigate) <= 2f)
            {
                whereTo_Navigate.x = Random.Range(initialPosition.x - 5f, initialPosition.x + 5f);
                whereTo_Navigate.z = Random.Range(initialPosition.z - 5f, initialPosition.z + 5f);
            }
            else
            {
                navAgent.SetDestination(whereTo_Navigate);
            }
        }

        else
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
            whereTo_Move.Set(0f, 0f, 0f);
            navAgent.isStopped = true;
        }
    }
}
