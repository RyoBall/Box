using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : CheckObject
{
    bool moving;
    Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }
    int Control()
    {
        Vector2 vec;
        if (!moving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                vec = new Vector2(0, 1);
                EventManager.PlayerMov(new PlayerMovEventData());
            }     
            else if (Input.GetKeyDown(KeyCode.S))
            {
                vec = new Vector2(0, -1);
                EventManager.PlayerMov(new PlayerMovEventData());
            }      
            else if (Input.GetKeyDown(KeyCode.A))
            {
                vec = new Vector2(-1, 0);
                EventManager.PlayerMov(new PlayerMovEventData());
            }        
            else if (Input.GetKeyDown(KeyCode.D))
            {
                vec = new Vector2(1,0);
                EventManager.PlayerMov(new PlayerMovEventData());
            }
            else 
            {
                return 0;
            }
            Box box;
            
            if (!CheckWithTag(vec, "Box",out box))//ǰ��û������
            {
                Move(vec);
                playerAnimator.SetBool("Walk", true);
            }
            else
            {
                if (box.ismoving)//�˺�����һ��
                {
                    Move(vec);
                    playerAnimator.SetBool("Push", true);
                }
                else if (box.pushable)//���ӿ�����
                {
                    if (box.GetPush(vec))//�ɹ��ƶ�
                    {
                        Move(vec);
                        playerAnimator.SetBool("Push", true);
                    }
                }
                else
                {
                    box.GetPush(vec);//�����ӣ������ƶ�
                }
            }
        }
        return 1;
    }
    public void Move(Vector2 vec)
    {
        moving = true;
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 0, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(()=>moving=false);
    }

    public void FinishAction()
    {
        playerAnimator.SetBool("Walk",false);
        playerAnimator.SetBool("Push",false);
    }
}
