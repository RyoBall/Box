using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBox : Box
{
    public override bool CheckMove(Vector2 vec)
    {
        StartCoroutine(SendEvent());
        Box box;
        if (CheckWithTag(vec, "Box", out box) && box.type == Type.Wall) 
        {
            gameObject.SetActive(false);
            box.gameObject.SetActive(false);
            MapManager.instance.delObjects.Add(gameObject);
            MapManager.instance.delObjects.Add(box.gameObject);
            return true;
        }
        return base.CheckMove(vec);
    }
    IEnumerator SendEvent() 
    {
        yield return new WaitForSeconds(Data.fixedMovTime);
        EventManager.DeleteBoxGetPush();    
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

    protected override void Start()
    {
        base.Start();
    }
}
