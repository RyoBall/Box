using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI contentText;
    public GameObject DialoguePanel;
    public float typeSpeed;
    public static Dialogue instance;

    private string folderPath;
    private string[] textFiles;
    int currentIndex = 0;
    string currentText = "";

    private bool waitingForInput;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        folderPath = "Assets/Text";
        LoadTextFiles();
    }
    
    void LoadTextFiles()
    {
        // 获取所有 txt 文件路径
        if (Directory.Exists(folderPath))
        {
            textFiles = Directory.GetFiles(folderPath, "*.txt");
            Debug.Log("找到文本文件数量：" + textFiles.Length);
        }
        else
        {
            Debug.LogError("找不到路径：" + folderPath);
        }
    }

    public void ShowText()
    {
        if (textFiles.Length == 0 || textFiles == null)
        {
            return;
        }
        else
        {
            string filePath = textFiles[currentIndex++];
            currentText = File.ReadAllText(filePath);
            DialoguePanel.SetActive(true);
            StartCoroutine(TypeText(currentText));
        }
    }
    
    IEnumerator TypeText(string fullText)
    {
        contentText.text = "";
        string currentLine = "";

        foreach (char c in fullText)
        {
            currentLine += c;
            contentText.text = currentLine;
            yield return new WaitForSeconds(typeSpeed);
            
            if (c == '。' || c == '.' || c == '！' || c == '？')
            {
                waitingForInput = true;
                // 等待玩家点击鼠标左键或按空格
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
                waitingForInput = false;

                // 清空文字再继续下一句
                currentLine = "";
                contentText.text = "";
            }
        }

        // 句子结束后等待玩家关闭
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
        DialoguePanel.SetActive(false);
    }
}
