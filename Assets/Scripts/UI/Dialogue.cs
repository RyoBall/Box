using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI contentText;
    public GameObject DialoguePanel;
    public List<Sprite> imagesList;//立绘列表
    private static List<int> currentImages;//每句对应表情数字
    public Image image;
    private int index;//当前第几句
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
        currentImages = new List<int>();
        index = 0;
        ReadImage();
    }

    private static void ReadImage()
    {
        try
        {
            string fileContent = File.ReadAllText("Assets/ImageNum.txt");

            MatchCollection matches = Regex.Matches(fileContent, @"\d+");

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Value, out int number))
                {
                    currentImages.Add(number);
                }
            }
            Debug.Log($"成功读取 {currentImages.Count} 个图片索引");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取图片索引文件失败: {e.Message}");
            currentImages = new List<int> { 0 };
        }
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
    
    public IEnumerator ShowTextCoroutine()
    {
        if (textFiles.Length == 0 || textFiles == null)
        {
            yield break;
        }
    
        string filePath = textFiles[currentIndex++];
        currentText = File.ReadAllText(filePath);
        DialoguePanel.SetActive(true);
    
        // 启动文本显示
        StartCoroutine(TypeText(currentText));
    
        // 等待对话面板关闭作为完成标志
        yield return new WaitUntil(() => !DialoguePanel.activeInHierarchy);
    }
    
    public void ShowTextInOtherSituation(string text)
    {
        currentText = text;
        DialoguePanel.SetActive(true);
        StartCoroutine(TypeText(currentText));
    }
    IEnumerator TypeText(string fullText)
    {
        bool breakDialogue = false;
        contentText.text = "";
        string currentLine = "";
        ChangeImgae();

        foreach (char c in fullText)
        {
            currentLine += c;
            contentText.text = currentLine;
            yield return new WaitForSeconds(typeSpeed);
            if (Input.GetKey(KeyCode.Escape))
            {
                DialoguePanel.SetActive(false);
                breakDialogue = true;
                contentText.text = "";
                break;
            }
            if (c == '。' || c == '.' || c == '！' || c == '？')
            {
                waitingForInput = true;
                // 等待玩家点击鼠标左键或按空格
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
                index++;
                waitingForInput = false;
                ChangeImgae();

                // 清空文字再继续下一句
                currentLine = "";
                contentText.text = "";
            }
        }
        if (!breakDialogue)
        {
            // 句子结束后等待玩家关闭
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
            DialoguePanel.SetActive(false);
        }
    }

    private void ChangeImgae()
    {
        if (index >= currentImages.Count)
        {
            index = currentImages.Count - 1;
        }
        int imageIndex = currentImages[index];
        image.sprite = imagesList[imageIndex];
    }
}
