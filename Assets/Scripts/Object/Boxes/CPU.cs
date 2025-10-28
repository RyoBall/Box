using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUData
{
    public Vector3 position;
    public List<Vector2> moveVec;
    public CPUData(Vector3 position, List<Vector2> moveVec)
    {
        this.position = position;
        this.moveVec = moveVec;
    }
}
public class CPU : Box,IRecord<CPUData>
{
    Stack<CPUData> IRecord<CPUData>.stack { get; set; }
    public void FindPlayer()
    {
        if (PlayerController.instance.inPower && ChecDistance())
        {
            PlayerController.instance.inPower = false;
            BurnOut();
        }
    }
    bool ChecDistance() { return PlayerController.instance.transform.position.x - transform.position.x <= 1 && PlayerController.instance.transform.position.x - transform.position.x >= -1 && PlayerController.instance.transform.position.z - transform.position.z <= 1 && PlayerController.instance.transform.position.z - transform.position.z >= -1; }
    public override bool CheckMove(Vector2 vec)
    {
        Box box;
        if (CheckWithTag(vec, "Box", out box))
        {
            if (box.GetComponent<Box>().type == Box.Type.Target)
            {
                Move(vec);
                Debug.Log("Enter Target");
                StartCoroutine(CPUEnterTarget());
                return true;
            }
            else if (box.type == Type.Unreal)
            {
                return false;
            }
        }
        return base.CheckMove(vec);
    }
    public void BurnOut() 
    {
        pushable = false;//ʧ���¼�
    }
    IEnumerator CPUEnterTarget() //�������Ӧ�����ƶ��������������
    {
        yield return new WaitForSeconds(Data.fixedMovTime);
        EventManager.CPUEnterTarget(new CPUEnterTargetEventData(this));
    }
    public override void GetDelayPush(Vector2 vec)
    {
        base.GetDelayPush(vec);
    }

    public override bool GetPush(Vector2 vec)
    {
        return base.GetPush(vec);
    }

    public override void Move(Vector2 vec)
    {
        base.Move(vec);
    }

    void IRecord<CPUData>.Record(PlayerMovEventData data)
    {
        List<Vector2> Vecs=new List<Vector2>();
        for(int i = 0; i < moveVec.Count; i++) 
        {
            Vecs.Add(moveVec[i]);
        }
        ((IRecord<CPUData>)this).stack.Push(new CPUData(transform.position,Vecs));
    }

    void IRecord<CPUData>.BackEffectInClass(CPUData data)
    {
        transform.position = data.position;
        moveVec = data.moveVec;
        SetTextCount();
    }
    void Init(LevelChangeData data) 
    {
        if (gameObject.layer == (int)data.layer) 
        {
            ((IRecord<CPUData>)this).Init();
            EventManager.OnPlayerOverMov += FindPlayer;
        }
    }
    protected override void Start()
    {
        base.Start();
        EventManager.OnLevelChange += Init;
    }
}
