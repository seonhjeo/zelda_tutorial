using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    // TriggerEnter된 대상 콜라이더의 태그가 breakable이면
    //상대 오브젝트의 Pot컴포넌트에 내장된 Smash 함수 실행
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("breakable"))
        {
            if (other.GetComponent<Pot>())
                other.GetComponent<Pot>().Smash();
        }
    }
}
