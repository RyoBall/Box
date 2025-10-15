using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public bool CheckWithTag<T>(Vector2 vec, String tag, out T obj)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, new Vector3(vec.x, 0, vec.y), Data.fixedChecLength);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == tag)
            {
                obj = hit.collider.gameObject.GetComponent<T>();
                return true;
            }
        }
        obj = default(T);
        return false;
    }
    public bool CheckWithTag(Vector2 vec, String tag)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, new Vector3(vec.x, 0, vec.y), Data.fixedChecLength);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == tag)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckWithTag(Vector3 posOffset,int downVec, Vector2 vec, String tag)//对射线初始位置进行偏移以及允许向下发射射线
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position+posOffset, new Vector3(vec.x, -downVec, vec.y), Data.fixedChecLength+.01f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == tag)
            {
                return true;
            }
        }
        return false;
    }
    public virtual void Move(Vector2 vec)
    {
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 0, vec.y * Data.fixedLength), Data.fixedMovTime);
    }
}
