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

    public override bool CheckMove(Vector2 vec)
    {
        Box box;
        if (!CheckWithTag(vec, "Box", out box))
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
                if (box.GetPush(vec)) //������Ӳ��ƶ���һ�����ƶ�����
                {
                    Move(vec);
                    return true;
                }
            }
            else if (box.tag == "Wall") 
            {
                Destroy(box.gameObject);
                Destroy(gameObject);
                return true;
            }
            return false;
        }
    }

}
