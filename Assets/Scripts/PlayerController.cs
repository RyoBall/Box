using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    bool moving;
    // Start is called before the first frame update
    void Start()
    {

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
            ChecBox(vec, out box);
            if (box == null)//前方没有箱子
                Move(vec);
            else
            {
                if (box.ismoving)//箱子要移动
                    Move(vec);
                else if (box.pushable)//箱子可以推
                {
                    if (box.GetPush(vec))//成功推动
                        Move(vec);
                }
                else 
                {
                    box.GetPush(vec);//推箱子，但不移动
                }
            }
        }
        return 1;
    }
    int ChecBox(Vector2 vec,out Box box) 
    {
        RaycastHit[] hits=Physics.RaycastAll(transform.position,new Vector3(vec.x,0,vec.y),Data.fixedChecLength);
        foreach(RaycastHit hit in hits) 
        {
            if (hit.collider.tag == "Box") 
            {
                box= hit.collider.gameObject.GetComponent<Box>();
                return 0;
            }
        }
        box = null;
        return 0;
    }
    public void Move(Vector2 vec)
    {
        moving = true;
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 0, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(()=>moving=false);
    }
}
