using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
public class PlayerActionData 
{
    public Vector3 position;
    public bool canJump;
    public bool delay;
    public bool onBox;//在箱子上前进
    public int jumpCount;
    public bool inPower;
    public bool jumpSkill;
    public bool delaySkillUnlock;
    public PlayerActionData(Vector3 position,bool canJump,bool delay,bool onBox,int jumpCount,bool inPower,bool jumpSkill,bool delaySkillUnlock) 
    {
        this.position = position;
        this.canJump = canJump;
        this.delay = delay;
        this.onBox = onBox;
        this.jumpCount = jumpCount;
        this.inPower = inPower;
        this.jumpSkill = jumpSkill;
        this.delaySkillUnlock = delaySkillUnlock;
    }
}
public interface IRecord<T> 
{
    public Stack<T> stack { get; set; }
    public void Init() 
    {
        stack = new Stack<T>();
        EventManager.OnPlayerMov +=Record;
        EventManager.OnBack +=BackEffect;
    }
    public void BackEffect()//框架 
    {
        if (stack.Count != 0) 
        {
            T data= stack.Pop();
            BackEffectInClass(data);
        }
    }
    public void Record(PlayerMovEventData data);
    public void BackEffectInClass(T data);
}

public class PlayerController : CheckObject,ICode,IRecord<PlayerActionData>
{
    public static PlayerController instance;
    public bool delaySkillUnlock = false;
    bool moving;
    Animator playerAnimator;
    [FormerlySerializedAs("jump")] public bool jumpSkill;
    public bool canJump;
    public bool delay;
    public bool onBox;//在箱子上前进
    Vector2 faceVec = new Vector2(1, 0);
    //[SerializeField] float accelaration;
    //[SerializeField] float first;
    public int jumpCount;
    public bool inPower;

    ICode.CodeType ICode.codeType { get; set; }
    string ICode.name { get; set; }
    Stack<PlayerActionData> IRecord<PlayerActionData>.stack { get; set; }

    void Start()
    {
        name = "Bug";
        instance = this;
        jumpSkill = true;
        playerAnimator = GetComponent<Animator>();
        playerAnimator.speed = 3;
        ((IRecord<PlayerActionData>)this).Init();
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
    public void Reset()
    {
        onBox = false;
        jumpCount = 0;
    }
    int Control()
    {
        Vector2 vec;
        if (!moving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                vec = new Vector2(0, 1);
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                vec = new Vector2(0, -1);
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                vec = new Vector2(-1, 0);
                this.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                vec = new Vector2(1, 0);
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                EventManager.Back();
                return 0;
            }
            else if (Input.GetKeyDown(KeyCode.E)&&delaySkillUnlock)
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
            else if (Input.GetKeyDown(KeyCode.Space) && canJump&& jumpSkill)
            {
                if (CheckWithTag(faceVec, "Box",out Box box))
                {
                    playerAnimator.SetBool("Jump", true);
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
            if (!CheckWithTag(new Vector3(vec.x * (Data.fixedChecLength + .01f), 0, vec.y * (Data.fixedChecLength + .01f)), 1,
                    Vector2.zero, "Wall"))
            {
                //如果前下方没有box
                if (!CheckWithTag(
                        new Vector3(vec.x * (Data.fixedChecLength + .01f), 0, vec.y * (Data.fixedChecLength + .01f)), 1,
                        Vector2.zero, "Box"))
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
        }
        else if (delay)
        {
            if (CheckWithTag(vec, "Box", out box))
            {
                box.GetDelayPush(vec);
                EventManager.PlayerOverMov();
            }
            else if (!CheckWithTag(vec, "Wall"))
            {
                Move(vec);
                playerAnimator.SetBool("Walk", true);
            }
            else 
            {
                EventManager.PlayerOverMov();
            }
        }
        //在地面上前进
        else
        {
            if (!CheckWithTag(faceVec, "Wall"))
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
                    if (box.isMoving) //�˺�����һ��
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
                        EventManager.PlayerOverMov();
                    }
                }
            }
        }
        EventManager.PlayerMov(new PlayerMovEventData());
    }
   
    public override void Move(Vector2 vec)
    {
        moving = true;
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 0, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(() => { moving = false; EventManager.PlayerOverMov(); FinishAction();});
    }

    //Jump和JumpDown都是弧线轨迹，等动画出了修改
    public void Jump(Vector2 vec)
    {
        moving = true;
        onBox = true;
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 1, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(() => { moving = false; EventManager.PlayerOverMov(); FinishAction();});
    }
    public void JumpDown(Vector2 vec)
    {
        moving = true;
        onBox = false;
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, -1, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(() => { moving = false; EventManager.PlayerOverMov(); FinishAction();});
    }
    /*IEnumerator JumpInOneFrame(float vel) 
    {
       yield return new WaitForSeconds(0);
        transform.position += Vector3.up*vel*Time.deltaTime;
        if (transform.position.y > 1.5f || vel > 0) 
        {
            JumpInOneFrame(vel-=accelaration);   
        }

    }*///试图通过直接对transform进行修改实现弧线轨迹，具体实现等动画出来了再说吧
    public void FinishAction()
    {
        playerAnimator.SetBool("Walk", false);
        playerAnimator.SetBool("Push", false);
        playerAnimator.SetBool("Jump", false);
    }
    void IRecord<PlayerActionData>.BackEffectInClass(PlayerActionData data)
    {
        transform.position = data.position;
        canJump = data.canJump;
        delay = data.delay;
        onBox = data.onBox;
        jumpCount = data.jumpCount;
        inPower = data.inPower;
        jumpSkill = data.jumpSkill;
        delaySkillUnlock = data.delaySkillUnlock;
    }

    void IRecord<PlayerActionData>.Record(PlayerMovEventData data)
    {
        ((IRecord<PlayerActionData>)this).stack.Push(new PlayerActionData(transform.position, canJump, delay, onBox, jumpCount, inPower, jumpSkill, delaySkillUnlock));
    }
}
