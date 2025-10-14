using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerController : CheckObject
{
    public static PlayerController instance;
    public static bool delaySkillUnlock = false;
    bool moving;
    Animator playerAnimator;
    [FormerlySerializedAs("jump")] public bool jumpSkill;
    public bool canJump;
    public bool delay;
    private bool onBox;//在箱子上前进
    Vector2 faceVec = new Vector2(1,0);
    public int jumpCount;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerAnimator = GetComponent<Animator>();
        if (jumpSkill)
        {
            canJump = true;
        }
    }   

    // Update is called once per frame
    void Update()
    {
        Control();
        //时刻检查，进入新一关重置，在jump第一次开启后，才有可能计数
        if (jumpCount > 2)
        {
            canJump = false;
        }
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
                vec = new Vector2(1, 0);
                EventManager.PlayerMov(new PlayerMovEventData());
            }
            else if (Input.GetKeyDown(KeyCode.E)) 
            {
                if (delay) 
                {
                    EventManager.PlayerExitDelay(new PlayerExitDelayEventData());
                    delay = false;
                }
                else 
                {
                    EventManager.PlayerEnterDelay();
                    delay = true;
                }
                return 0;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && canJump) 
            {
                if (CheckWithTag(faceVec, "Box")) 
                {
                    Jump(faceVec);
                }
                return 0;
            }
            else
            {
                return 0;
            }
            PlayerCheckMove(vec);
        }
        return 1;
    }

    private void PlayerCheckMove(Vector2 vec)
    {
        Box box;
        faceVec = vec;
        if (onBox)
        {
            //如果前下方没有box
            if (!CheckWithTag(new Vector3(vec.x*(Data.fixedChecLength+.01f),0,vec.y*(Data.fixedChecLength+.01f)),1,Vector2.zero, "Box"))
            {
                playerAnimator.SetBool("Jump", true);
                //飞下去的轨迹
                JumpDown(vec);
                jumpCount++;
            }
            else
            {
                //如果前下方是box，继续走
                Move(vec);
                playerAnimator.SetBool("Walk", true);
            }
        }
        else if (delay) 
        {
            if(CheckWithTag(vec,"Box",out box)) 
            {
                box.GetDelayPush(vec);
            }
            else if(!CheckWithTag(vec,"Wall"))
            {
                Move(vec);   
            }
        }
        //在地面上前进
        else
        {
            if (!CheckWithTag(vec, "Box", out box)) //ǰ��û������
            {
                Move(vec);
                playerAnimator.SetBool("Walk", true);
                /*if (!jump)
                {
                    Move(vec);
                    playerAnimator.SetBool("Walk", true);
                }
                else
                {
                    //第一次jump
                    playerAnimator.SetBool("Jump", true);
                    //这里会设置jump的移动，但是移动形式待定，根据要求写
                    onBox = true;
                }*/
            }
            else
            {
                if (box.ismoving) //�˺�����һ��
                {
                    Move(vec);
                    playerAnimator.SetBool("Push", true);
                }
                else if (box.pushable) //���ӿ�����
                {
                    if (box.GetPush(vec)) //�ɹ��ƶ�
                    {
                        Move(vec);
                        playerAnimator.SetBool("Push", true);
                    }
                }
                //这一段效果是如果箱子不可推动，那么箱子被推，估计用于relybox计数？但是以后有更多不可推动箱子要注意
                else
                {
                    box.GetPush(vec); //�����ӣ������ƶ�
                }
            }
        }
    }

    public override void Move(Vector2 vec)
    {
        moving = true;
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 0, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(() => moving = false);
    }
    
    //Jump和JumpDown都是弧线轨迹，等动画出了修改
    public void Jump(Vector2 vec) 
    {
        moving = true;
        onBox = true;
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 1, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(() => moving = false);
    } 
    public void JumpDown(Vector2 vec) 
    {
        moving = true;
        onBox = false;
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, -1, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(() => moving = false);
    }
    public void FinishAction()
    {
        playerAnimator.SetBool("Walk", false);
        playerAnimator.SetBool("Push", false);
        playerAnimator.SetBool("Jump", false);
    }
}
