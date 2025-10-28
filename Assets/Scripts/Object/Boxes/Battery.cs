using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BatteryData 
{
    public Vector3 position;
    public bool inPower;
    public List<Vector2> moveVec;
    public BatteryData(Vector3 position, bool inPower, List<Vector2> moveVec)
    {
        this.position = position;
        this.inPower = inPower;
        this.moveVec=moveVec;
    }
}
public class Battery : Box,IRecord<BatteryData>
{
    public bool inPower;
    public Material material;
    private Color originColor;

    Stack<BatteryData> IRecord<BatteryData>.stack { get; set; }

    public void FindPlayer() 
    {
        
        if (PlayerController.instance.inPower&&ChecDistance()) 
        {
            PlayerController.instance.inPower = false;
            inPower = true;
            material.color = Color.red;
        }
    }
    bool ChecDistance() { return PlayerController.instance.transform.position.x - transform.position.x <= 1 && PlayerController.instance.transform.position.x - transform.position.x >= -1 && PlayerController.instance.transform.position.z - transform.position.z <= 1 && PlayerController.instance.transform.position.z - transform.position.z >= -1; }
    
    public override bool CheckMove(Vector2 vec)
    {
        Box box;
        BatteryHouse house;
        if(CheckWithTag(vec,"Box",out box)) 
        {
            if (box.TryGetComponent<BatteryHouse>(out house)) 
            {
                Move(vec);
                return true;
            }
        }
        return base.CheckMove(vec);
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

    public void ResetPower(object sender, EventArgs e)
    {
        this.inPower = false;
        material.color = originColor;
    }   
    private void ReChangePower()
    {
        if(!inPower)
        material.color = originColor;
        else
        material.color = Color.red;
    }

    protected override void Start()
    {
        base.Start();
        Renderer renderer = GetComponent<Renderer>();
        material =new Material(material);
        renderer.material = material;
        EventManager.OnPlayerOverMov += FindPlayer;
        originColor = material.color;
        MapManager.OnReset += ResetPower;
        EventManager.OnLevelChange += Init;
    }
    void Init(LevelChangeData data)
    {
        if (gameObject.layer == (int)data.layer)
        {
            ((IRecord<BatteryData>)this).Init();
        }
    }

    void IRecord<BatteryData>.Record(PlayerMovEventData data)
    {
        List<Vector2> Vecs = new List<Vector2>();
        for (int i = 0; i < moveVec.Count; i++)
        {
            Vecs.Add(moveVec[i]);
        }
        ((IRecord<BatteryData>)this).stack.Push(new BatteryData(transform.position,inPower,Vecs));
    }
    void IRecord<BatteryData>.BackEffectInClass(BatteryData data)
    {
        transform.position = data.position;
        inPower = data.inPower;
        moveVec = data.moveVec;
        SetTextCount();
        ReChangePower();
    }
}
