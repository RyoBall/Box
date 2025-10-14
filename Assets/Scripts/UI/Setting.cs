using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public GameObject SettingPanel;
    [SerializeField]private AudioSource audio;
    public UnityEngine.UI.Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        SettingPanel.SetActive(false);
        audio.volume = 1;
        slider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSettingPanel()
    {
        SettingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        SettingPanel.SetActive(false);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
    
    

    public void ChangeAudio()
    {
        audio.volume = slider.value;
    }
}
