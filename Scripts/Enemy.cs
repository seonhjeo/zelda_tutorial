using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    public void Knock(Rigidbody2D myRigidbody, float knockTime)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
    }

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
