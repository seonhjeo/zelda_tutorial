using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("breakable"))
        {
            if (other.GetComponent<Pot>())
                other.GetComponent<Pot>().Smash();
        }
    }
}
