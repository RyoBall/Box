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
    public void Record<T>(Stack<T> stack,T data) 
    {
        stack.Push(data);
    }
    public virtual void RecordEffect() 
    {
        ;
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
    public bool CheckWithTag(Vector3 posOffset,int downVec, Vector2 vec, String tag)//�����߳�ʼλ�ý���ƫ���Լ��������·�������
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
    
    public bool CheckWithTag<T>(Vector3 posOffset,int downVec, Vector2 vec, String tag,out T obj)//�����߳�ʼλ�ý���ƫ���Լ��������·�������
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position+posOffset, new Vector3(vec.x, -downVec, vec.y), Data.fixedChecLength+.01f);
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
    public virtual void Move(Vector2 vec)
    {
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 0, vec.y * Data.fixedLength), Data.fixedMovTime);
    }
}
