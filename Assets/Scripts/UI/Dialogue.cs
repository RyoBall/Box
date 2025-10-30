using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking; // 添加这个命名空间

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI contentText;
    public GameObject DialoguePanel;
    public List<Sprite> imagesList;//立绘列表
    private List<int> currentImages;//每句对应表情数字 // 移除了static
    public Image image;
    private int index;//当前第几句
    public float typeSpeed;
    public static Dialogue instance;

    // 移除硬编码路径，改用Resources或StreamingAssets
    private TextAsset[] textAssets; // 改用TextAsset数组
    int currentIndex = 0;
    string currentText = "";

    private bool waitingForInput;

    void Start()
    {
        instance = this;
        LoadTextFiles(); // 先加载文本文件
        currentImages = new List<int>();
        index = 0;
        ReadImage(); // 再读取图片索引
    }

    private void ReadImage() // 移除了static
    {
        try
        {
            // 使用Resources加载图片索引文件
            TextAsset imageNumFile = Resources.Load<TextAsset>("ImageNum");
            if (imageNumFile != null)
            {
                string fileContent = imageNumFile.text;
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
            else
            {
                Debug.LogError("找不到图片索引文件 ImageNum.txt");
                currentImages = new List<int> { 0 };
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取图片索引文件失败: {e.Message}");
            currentImages = new List<int> { 0 };
        }
    }

    void LoadTextFiles()
    {
        // 使用Resources加载所有文本文件
        textAssets = Resources.LoadAll<TextAsset>("Text");
        Debug.Log("找到文本文件数量：" + (textAssets != null ? textAssets.Length : 0));

        if (textAssets == null || textAssets.Length == 0)
        {
            Debug.LogError("找不到文本文件，请确保文件在Resources/Text文件夹中");
        }
    }

    public void ShowText()
    {
        if (textAssets == null || textAssets.Length == 0)
        {
            Debug.LogError("没有可用的文本文件");
            return;
        }

        if (currentIndex >= textAssets.Length)
        {
            Debug.Log("所有文本已显示完毕");
            currentIndex = 0; // 重置索引或根据需求处理
            return;
        }

        currentText = textAssets[currentIndex].text;
        currentIndex++;
        DialoguePanel.SetActive(true);
        StartCoroutine(TypeText(currentText));
    }

    public IEnumerator ShowTextCoroutine()
    {
        if (textAssets == null || textAssets.Length == 0)
        {
            yield break;
        }

        if (currentIndex >= textAssets.Length)
        {
            yield break;
        }

        currentText = textAssets[currentIndex].text;
        currentIndex++;
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
        if (currentImages == null || currentImages.Count == 0)
        {
            Debug.LogError("图片索引列表为空");
            return;
        }

        if (index >= currentImages.Count)
        {
            index = currentImages.Count - 1;
        }

        int imageIndex = currentImages[index];

        if (imageIndex >= 0 && imageIndex < imagesList.Count)
        {
            image.sprite = imagesList[imageIndex];
        }
        else
        {
            Debug.LogError($"图片索引超出范围: {imageIndex}, 图片列表长度: {imagesList.Count}");
        }
    }

    // 添加重置方法
    public void ResetDialogue()
    {
        currentIndex = 0;
        index = 0;
        if (currentImages != null) currentImages.Clear();
        ReadImage();
    }
}