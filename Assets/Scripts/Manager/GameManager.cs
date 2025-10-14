using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Camera cam;
    enum State
    {
        OPEN,
        ONE,
        TWO,
        THREE,
        END
    }

    private State state;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        state = State.ONE;//暂时没有做开头
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            MapManager.instance.Reset();
        }
    }
    public void Win(CPUEnterTargetEventData data) 
    {
        //后续改为switch
        if (state == State.ONE)
        {
            state = State.TWO;
            data.cpu.transform.position = new Vector3(3,.5f,-4);
            //TODO:
            //唤起一个剧情，文本有待考察
            //摄像头跟随
            cam.transform.position = new Vector3(13.22f,6.1f,-7.2f);
            PlayerController.instance.transform.position = new Vector3(9, .5f, -1);
        }
    }
}
