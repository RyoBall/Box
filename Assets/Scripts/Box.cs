using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Box : MonoBehaviour
{
    public bool pushable;
    public int timeCount;
    public TimeText text;
    public bool ismoving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual bool GetPush(Vector2 vec) //返回是否成功移动
    {
        return TryMove(vec);
    }
    public virtual bool TryMove(Vector2 vec)//返回是否成功移动
    {
        Box box;
        ChecBox(vec, out box);
        if (box == null) 
        {
            Move(vec);
            return true;
        }
        else
        {
            if (box.ismoving) 
            {
                Move(vec);
                return true;
            }
            else if (box.pushable)
            {
                if (box.GetPush(vec)) //如果箱子不移动，一定会推动箱子
                {
                    Move(vec);
                    return true;
                }
            }
            return false;
        }
    }
    public virtual void Move(Vector2 vec) 
    {
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 0, vec.y * Data.fixedLength), Data.fixedMovTime);
    }
	public int ChecBox(Vector2 vec,out Box box)
	{
		RaycastHit[] hits = Physics.RaycastAll(transform.position, new Vector3(vec.x, 0, vec.y), Data.fixedChecLength);
		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.tag == "Box")
			{
                box = hit.collider.gameObject.GetComponent<Box>();
                return 0;
			}
		}
        box = null;
        return 0;
	}
}
