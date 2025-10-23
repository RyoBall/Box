using DG.Tweening;
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
        FOUR,
        FIVE,
        END
    }

    private State state;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        state = State.ONE;//暂时没有做开头
        //PlayerController.instance.transform.position = new Vector3(-4,.5f,-3);
        //cam.transform.position = new Vector3(0.54f, 7.18f, -6.64f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            MapManager.instance.Reset();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Win(null);
        }
    }
    //第一关特殊
   /* public void WinForLevel1(CPUEnterTargetEventData data) 
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
            int layer = LayerMask.NameToLayer("Level2");
            MapManager.instance.ResaveTransform(layer);
            EventManager.OnCPUEnterTarget += Win;
        }
        
    }*/

    public void Win(CPUEnterTargetEventData data)
    {
        int layer=0;
        switch (state)
        {
            case State.ONE:
                state = State.TWO;
                //data.cpu.transform.position = new Vector3(3, .5f, -4);
                //TODO:
                //唤起一个剧情，文本有待考察
                //摄像头跟随
                EventManager.OnCPUEnterTarget -= Level1SpecialManager.instance.CPUEnterSpecialEvent;
                EventManager.OnCPUEnterTarget += Win;
                PlayerController.instance.transform.position = new Vector3(9, 0, -1);
                cam.transform.DOMove(new Vector3(13.5f, 6.5f, -5.85f), Data.fixedCameraMovTime);
                layer = LayerMask.NameToLayer("Level2");
                MapManager.instance.ResaveTransformAndResetPlayer(layer);
                break;
            case State.TWO:
                state = State.THREE;
                PlayerController.instance.transform.position = new Vector3(22.5f, 0, -8);
                cam.transform.DOMove(new Vector3(27f, 7f, -7.5f), Data.fixedCameraMovTime);
                layer = LayerMask.NameToLayer("Level3");
                MapManager.instance.ResaveTransformAndResetPlayer(layer);
                //
                Level3SpecialManager.instance.Init();
                break;
            case State.THREE:
                //
                Level3SpecialManager.instance.Quit();
                EventManager.OnCPUEnterTarget -= Win;
                Level4SpecialManager.instance.Init();
                //
                state = State.FOUR;
                PlayerController.instance.transform.position = new Vector3(41f, 0, -2);
                cam.transform.DOMove(new Vector3(44.5f, 8.5f, -8f), Data.fixedCameraMovTime);
                EventManager.OnCPUEnterTarget -= Win;
                layer = LayerMask.NameToLayer("Level4");
                MapManager.instance.ResaveTransformAndResetPlayer(layer);
                break; 
            case State.FOUR:
                //
                Level4SpecialManager.instance.Quit();
                Level5SpecialManager.instance.Init();   
                //
                state = State.FIVE;
                PlayerController.instance.transform.position = new Vector3(10, .5f, -25f);
                cam.transform.DOMove(new Vector3(15, 10f, -27f), Data.fixedCameraMovTime);
                cam.transform.DORotate(new Vector3(72,0,0), Data.fixedCameraMovTime);
                EventManager.OnCPUEnterTarget -= Win;
                layer = LayerMask.NameToLayer("Level5");
                MapManager.instance.ResaveTransformAndResetPlayer(layer);
                break;
            default:
                break;
        }
        EventManager.LevelChange(new LevelChangeData(layer));
    }
}
