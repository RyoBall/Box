using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    
    private Dictionary<Transform, TransformData> originalTransforms = new Dictionary<Transform, TransformData>();
    public List<GameObject> tmpObjects = new List<GameObject>();
    public List<GameObject> delObjects = new List<GameObject>();
    public class TransformData
    {
        public Vector3 position;
    }
    // Start is called before the first frame update
    void Start()//这里用Dictionary保存物体初始位置，后续需要保存其他数据可以调整
    {
        instance = this;
        int currentLayer = LayerMask.NameToLayer("Level1"); 
        ResaveTransformAndResetPlayer(currentLayer);
    }

    public void ResaveTransformAndResetPlayer(LayerMask layer)//进入新一关需要使用这个函数来保存新一关的初始位置
    {
        var allObject = FindObjectsOfType<Transform>();

        foreach (Transform t in allObject)
        {
            if (t.hideFlags == HideFlags.None && (t.gameObject.layer == layer || t.CompareTag("Player")))
            {
                var data = new TransformData()
                {
                    position = t.position,
                };
                originalTransforms[t] = data;
            }
        }

        PlayerController.instance.jumpCount = 0;
        PlayerController.instance.canJump = true;
        PlayerController.instance.onBox = false;
    }

    public void Reset()//所有物体回到原点
    {
        EventManager.LevelReset();
        foreach (var t in originalTransforms)
        {
            t.Key.position = t.Value.position;
        }
        foreach(GameObject obj in tmpObjects) 
        {
            Destroy(obj);
        }       
        foreach(GameObject obj in delObjects) 
        {
            obj.SetActive(true);
        }
        tmpObjects.Clear();
        delObjects.Clear();
        PlayerController.instance.jumpSkill = false;
        PlayerController.instance.delay = false;
        if (PlayerController.instance.jumpSkill)
        {
            PlayerController.instance.jumpCount = 0;
            PlayerController.instance.canJump = true;
            PlayerController.instance.onBox=false;
        }
    }
    
}
