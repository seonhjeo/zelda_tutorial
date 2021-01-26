using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 자기 자신을 부수는 것을 호출하는 함수
    public void Smash()
    {
        anim.SetBool("smashed", true);
        StartCoroutine(breakco());
    }

    // 자기 자신을 부수는 코루틴
    IEnumerator breakco()
    {
        yield return new WaitForSeconds(0.25f);
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
    }
}
