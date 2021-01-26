using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // 특정 발판 위에 올라간 채로 상호작용을 하면 메시지를 출력해주는 함수
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && playerInRange)
        {
            if(dialogBox.activeInHierarchy)
                dialogBox.SetActive(false);
            else
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
        }
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }
}
