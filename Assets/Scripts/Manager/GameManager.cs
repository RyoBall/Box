using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Camera cam;
    public GameObject OpenPanel;
    enum State
    {
        OPEN,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        END
    }

    private State state;
    public ShaderVariantCollection shaderVariantCollection;
    
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        state = State.ONE;//暂时没有做开头
        //PlayerController.instance.transform.position = new Vector3(-4,.5f,-3);
        //cam.transform.position = new Vector3(0.54f, 7.18f, -6.64f);
        OpenPanel.SetActive(true);
        if (shaderVariantCollection != null)
        {
            // 强制加载 Shader Variant Collection
            shaderVariantCollection.WarmUp();
        }
    }

    public void StartGame()
    {
        //state = State.ONE;
        OpenPanel.SetActive(false);
        Dialogue.instance.ShowText();
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
        PlayerController.instance.Reset();
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
                Dialogue.instance.ShowText();
                break;
            case State.TWO:
                state = State.THREE;
                PlayerController.instance.transform.position = new Vector3(22.5f, 0, -8);
                cam.transform.DOMove(new Vector3(27f, 7f, -7.5f), Data.fixedCameraMovTime);
                layer = LayerMask.NameToLayer("Level3");
                MapManager.instance.ResaveTransformAndResetPlayer(layer);
                //
                Level3SpecialManager.instance.Init();
                Dialogue.instance.ShowText();
                break;
            case State.THREE:
                //
                Level3SpecialManager.instance.Quit();
                Level4SpecialManager.instance.Init();
                PlayerController.instance.delaySkillUnlock = true;
                EventManager.OnCPUEnterTarget -= Win;
                //
                state = State.FOUR;
                PlayerController.instance.transform.position = new Vector3(39f, 0, -2);
                cam.transform.DOMove(new Vector3(44.5f, 8.5f, -8f), Data.fixedCameraMovTime);
                layer = LayerMask.NameToLayer("Level4");
                MapManager.instance.ResaveTransformAndResetPlayer(layer);
                Dialogue.instance.ShowTextInOtherSituation("总算恢复正常了，但讲真，你不觉得有时候卡卡的也不错吗。我给你留了个接口，按一下E就又会卡卡的了，当然再按一下会恢复正常。");
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
                layer = LayerMask.NameToLayer("Level5");
                MapManager.instance.ResaveTransformAndResetPlayer(layer);
                Dialogue.instance.ShowText();
                break;       
            case State.FIVE:
                //
                //Level5SpecialManager.instance.Quit();
                Level6SpecialManager.instance.Init();   
                //
                state = State.SIX;
                PlayerController.instance.transform.position = new Vector3(29, .5f, -27f);
                cam.transform.DOMove(new Vector3(35, 10, -27.5f), Data.fixedCameraMovTime);
                layer = LayerMask.NameToLayer("Level6");
                MapManager.instance.ResaveTransformAndResetPlayer(layer);
                break;
            case State.SIX:
                //胜利结算
                StartCoroutine(WinCoroutine());
                break;
            default:
                break;
        }
        EventManager.LevelChange(new LevelChangeData(layer));
    }
    
    private IEnumerator WinCoroutine()
    {
        // 等待文本显示完成
        yield return StartCoroutine(Dialogue.instance.ShowTextCoroutine());
    
        // 文本显示完成后关闭游戏
        QuitGame();
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}


