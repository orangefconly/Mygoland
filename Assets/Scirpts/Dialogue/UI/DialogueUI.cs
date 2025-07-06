using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("UI组件")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Image backgroundPanel; // 背景遮罩
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Text speakerText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image centerImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private GameObject clickArea;

    [Header("动画参数")]
    [SerializeField] private float fadeDuration = 0.5f; // 淡入淡出时间
    [SerializeField] private Color fadeColor = new Color(0, 0, 0, 150); // 遮罩颜色

    private float originalTimeScale; // 保存原始时间缩放
    private string fullText;
    private float typingSpeed = 0.05f;
    private bool isTyping = false;

    private void Start()
    {
        HideDialogueUI();
        SubscribeEvents();
        Button clickButton = clickArea.GetComponent<Button>();
        if (clickButton != null)
        {
            clickButton.onClick.AddListener(OnClickAreaClicked);
        }
    }
    // 点击区域被点击时调用
    private void OnClickAreaClicked()
    {
        if (isTyping)
        {
            // 如果正在打字，加速显示全部文本
            StopAllCoroutines();
            dialogueText.text = fullText;
            isTyping = false;
        }
        else if (choicesPanel.activeSelf)
        {
            // 如果选项面板可见，不处理点击（防止误触）
            return;
        }
        else
        {
            // 否则继续对话
            DialogueManager.Instance.ContinueDialogue();
        }
    }
    private void Update()
    {
        if (choicesPanel.activeSelf) return;

        // 检测继续按键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!choicesPanel.activeSelf)
            {
                OnClickAreaClicked();
            }
        }
    }

    private void SubscribeEvents()
    {
        EventHandle.StartDialogueEvent += ShowDialogue;
        EventHandle.UpdateDialogueEvent += UpdateDialogue;
        EventHandle.EndDialogerEvent += HideDialogue;
        EventHandle.ChoicesAvailableEvent += ShowChoices;
    }

    // 显示对话（带场景变暗和暂停）
    private void ShowDialogue(DialogueData_SO.DialogueLine line)
    {
        // 保存当前时间缩放并暂停游戏
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // 显示背景遮罩并淡入
        dialogueCanvas.enabled = true;
        backgroundPanel.color = new Color(0, 0, 0, 0);
        StartCoroutine(FadeInBackground());

        // 更新对话内容
        UpdateDialogueUI(line);
    }

    // 背景淡入
    private IEnumerator FadeInBackground()
    {
        float elapsedTime = 0.5f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, fadeColor.a, elapsedTime / fadeDuration);
            backgroundPanel.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            elapsedTime += Time.unscaledDeltaTime; // 使用unscaledDeltaTime，即使timeScale为0也能正常工作
            yield return null;
        }

        backgroundPanel.color = fadeColor;
        dialogueBox.SetActive(true);
    }

    // 背景淡出
    private IEnumerator FadeOutBackground()
    {
        dialogueBox.SetActive(false);

        float elapsedTime = 0.5f;
        Color startColor = backgroundPanel.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);
            backgroundPanel.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        backgroundPanel.color = new Color(0, 0, 0, 0);
        dialogueCanvas.enabled = false;

        // 恢复游戏时间
        Time.timeScale = originalTimeScale;
    }

    // 更新对话内容
    private void UpdateDialogue(DialogueData_SO.DialogueLine line)
    {
        choicesPanel.SetActive(false);
        clickArea.SetActive(true);
        UpdateDialogueUI(line);
    }

    // 隐藏对话
    private void HideDialogue()
    {
        StartCoroutine(FadeOutBackground());
    }

    // 更新对话UI
    private void UpdateDialogueUI(DialogueData_SO.DialogueLine line)
    {
        speakerText.text = line.speaker;
        fullText = line.text;

        // 重置所有图片
        leftImage.enabled = false;
        centerImage.enabled = false;
        rightImage.enabled = false;

        // 设置当前图片
        Sprite image = DialogueManager.Instance.GetImage(line.imageName);
        if (image != null)
        {
            switch (line.position)
            {
                case DialogueData_SO.ImagePosition.Left:
                    leftImage.sprite = image;
                    leftImage.enabled = true;
                    break;
                case DialogueData_SO.ImagePosition.Center:
                    centerImage.sprite = image;
                    centerImage.enabled = true;
                    break;
                case DialogueData_SO.ImagePosition.Right:
                    rightImage.sprite = image;
                    rightImage.enabled = true;
                    break;
            }
        }

        // 开始打字机效果
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    // 打字机效果
    private IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        System.Globalization.StringInfo stringInfo = new System.Globalization.StringInfo(fullText);
        int charCount = stringInfo.LengthInTextElements;

        for (int i = 0; i < charCount; i++)
        {
            // 获取完整字符（可能由多个Unicode码点组成）
            string nextChar = stringInfo.SubstringByTextElements(i, 1);
            dialogueText.text += nextChar;

            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    // 显示选项
    private void ShowChoices(List<DialogueData_SO.DialogueLine> choices)
    {
        Debug.Log("触发选项");
        // 清除旧选项
        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }
        clickArea.gameObject.SetActive(false);
        // 创建新选项
        foreach (DialogueData_SO.DialogueLine choice in choices)
        {
            GameObject button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            button.GetComponentInChildren<Text>().text = choice.text;

            // 注册按钮点击事件
            int choiceIndex = choice.index;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                EventHandle.CallChoiceSelectedEvent(choiceIndex);
            });
        }

        choicesPanel.SetActive(true);
    }
    // 隐藏对话UI
    private void HideDialogueUI()
    {
        dialogueCanvas.enabled = false;
        choicesPanel.SetActive(false);
    }

    /*
    // 继续对话（用于无选项时的"继续"按钮）
    public void ContinueButtonClicked()
    {
        if (!isTyping)
        {
            DialogueManager.Instance.ContinueDialogue();
        }
        else
        {
            // 快速显示全部文本
            StopAllCoroutines();
            dialogueText.text = fullText;
            isTyping = false;
        }
    }
    */
}
