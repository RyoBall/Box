using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBox : Box
{
    public override bool GetPush(Vector2 vec)
    {
        return base.GetPush(vec);
    }

    public override void Move(Vector2 vec)
    {
        base.Move(vec);
    }

    public override bool TryMove(Vector2 vec)
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
            else if (box.GetComponent<Wall>() != null) 
            {
                Destroy(box.gameObject);
                Destroy(gameObject);
                return true;
            }
            return false;
        }
    }

}
