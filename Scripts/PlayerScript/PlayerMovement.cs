using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 상태
public enum PlayerState
{
    idle,
    walk,
    attack,
    interact,
    stagger
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public  float       playerSpeed;

    private Rigidbody2D myRigidbody;
    private Animator    animator;

    private Vector3     change;

    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        { StartCoroutine(AttackCo()); }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        { UpdateAnimationAndMove(); }
    }

    // 공격 코루틴
    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.3f);
        currentState = PlayerState.walk;
    }

    // 움직임에 따른 애니메이션 변수 변경
    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
            animator.SetBool("moving", false);
    }

    // 입력에 따라서 캐릭터를 움직임
    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(transform.position + change * playerSpeed * Time.deltaTime);
    }

    // 캐릭터의 넉백을 호출하는 함수
    public void Knock(float knockTime, float damage)
    {
        currentHealth.runtimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.runtimeValue > 0)
        {   
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }

    // 캐릭터 넉백 코루틴
    private IEnumerator KnockCo(float knockTime)
    {
        float knockt = 0;
        Vector2 vel;
        if (myRigidbody != null)
        {
            vel = myRigidbody.velocity;
            while (knockt < knockTime)
            {
                myRigidbody.velocity = Vector2.Lerp(vel, Vector2.zero, (knockt / knockTime));
                knockt += Time.deltaTime;
                yield return null;
            }
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}
