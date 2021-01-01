using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<Pot>())
                other.GetComponent<Pot>().Smash();
        }

        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (other.gameObject.CompareTag("enemy"))
                {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockTime);
                }
                if (other.gameObject.CompareTag("Player"))
                {
                    hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                    other.GetComponent<PlayerMovement>().Knock(knockTime);
                }               
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody2D hit)
    {
        float knockt = 0;
        Vector2 vel;
        if (hit != null)
        {
            vel = hit.velocity;
            while (knockt < knockTime)
            {
                hit.velocity = Vector2.Lerp(vel, Vector2.zero, (knockt / knockTime));
                knockt += Time.deltaTime;
                yield return null;
            }
            hit.velocity = Vector2.zero;
            hit.GetComponent<Enemy>().currentState = EnemyState.idle;
            hit.velocity = Vector2.zero;
        }
    }
}
