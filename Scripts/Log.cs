using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    private Rigidbody2D myRigidbody;

    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    public Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();

        if (currentState == EnemyState.idle)
            myRigidbody.velocity = Vector2.zero;
    }

    // 플레이어와의 거리를 파악해 거리에 대한 상호작용을 하는 함수
    private void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius &&
            Vector3.Distance(target.position, transform.position) >= attackRadius)
        {
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);               
                ChangeState(EnemyState.walk);
                myAnimator.SetBool("wakeUp", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            myAnimator.SetBool("wakeUp", false);
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        myAnimator.SetFloat("moveX", setVector.x);
        myAnimator.SetFloat("moveY", setVector.y);
    }

    // 움직임에 맞춰 에니메이션 내 변수값을 변경하는 함수
    private void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                SetAnimFloat(Vector2.right);
            else if (direction.x < 0)
                SetAnimFloat(Vector2.left);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
                SetAnimFloat(Vector2.up);
            else if (direction.y < 0)
                SetAnimFloat(Vector2.down);
        }
    }

    // 스테이트 변경 함수
    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }
}
