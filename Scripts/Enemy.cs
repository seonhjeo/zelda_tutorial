using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy의 state
public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    // 자기 자신이 데미지를 입는 함수
    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    // 자기 자신이 넉백당하는 것을 호출하는 함수
    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    // 자기 자신이 넉백당하는 코루틴
    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
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
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}
