using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("UI���")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Image backgroundPanel; // ��������
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Text speakerText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image centerImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private GameObject clickArea;

    [Header("��������")]
    [SerializeField] private float fadeDuration = 0.5f; // ���뵭��ʱ��
    [SerializeField] private Color fadeColor = new Color(0, 0, 0, 150); // ������ɫ

    private float originalTimeScale; // ����ԭʼʱ������
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
    // ������򱻵��ʱ����
    private void OnClickAreaClicked()
    {
        if (isTyping)
        {
            // ������ڴ��֣�������ʾȫ���ı�
            StopAllCoroutines();
            dialogueText.text = fullText;
            isTyping = false;
        }
        else if (choicesPanel.activeSelf)
        {
            // ���ѡ�����ɼ���������������ֹ�󴥣�
            return;
        }
        else
        {
            // ��������Ի�
            DialogueManager.Instance.ContinueDialogue();
        }
    }
    private void Update()
    {
        if (choicesPanel.activeSelf) return;

        // ����������
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

    // ��ʾ�Ի����������䰵����ͣ��
    private void ShowDialogue(DialogueData_SO.DialogueLine line)
    {
        // ���浱ǰʱ�����Ų���ͣ��Ϸ
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // ��ʾ�������ֲ�����
        dialogueCanvas.enabled = true;
        backgroundPanel.color = new Color(0, 0, 0, 0);
        StartCoroutine(FadeInBackground());

        // ���¶Ի�����
        UpdateDialogueUI(line);
    }

    // ��������
    private IEnumerator FadeInBackground()
    {
        float elapsedTime = 0.5f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, fadeColor.a, elapsedTime / fadeDuration);
            backgroundPanel.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            elapsedTime += Time.unscaledDeltaTime; // ʹ��unscaledDeltaTime����ʹtimeScaleΪ0Ҳ����������
            yield return null;
        }

        backgroundPanel.color = fadeColor;
        dialogueBox.SetActive(true);
    }

    // ��������
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

        // �ָ���Ϸʱ��
        Time.timeScale = originalTimeScale;
    }

    // ���¶Ի�����
    private void UpdateDialogue(DialogueData_SO.DialogueLine line)
    {
        choicesPanel.SetActive(false);
        clickArea.SetActive(true);
        UpdateDialogueUI(line);
    }

    // ���ضԻ�
    private void HideDialogue()
    {
        StartCoroutine(FadeOutBackground());
    }

    // ���¶Ի�UI
    private void UpdateDialogueUI(DialogueData_SO.DialogueLine line)
    {
        speakerText.text = line.speaker;
        fullText = line.text;

        // ��������ͼƬ
        leftImage.enabled = false;
        centerImage.enabled = false;
        rightImage.enabled = false;

        // ���õ�ǰͼƬ
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

        // ��ʼ���ֻ�Ч��
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    // ���ֻ�Ч��
    private IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        System.Globalization.StringInfo stringInfo = new System.Globalization.StringInfo(fullText);
        int charCount = stringInfo.LengthInTextElements;

        for (int i = 0; i < charCount; i++)
        {
            // ��ȡ�����ַ��������ɶ��Unicode�����ɣ�
            string nextChar = stringInfo.SubstringByTextElements(i, 1);
            dialogueText.text += nextChar;

            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    // ��ʾѡ��
    private void ShowChoices(List<DialogueData_SO.DialogueLine> choices)
    {
        Debug.Log("����ѡ��");
        // �����ѡ��
        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }
        clickArea.gameObject.SetActive(false);
        // ������ѡ��
        foreach (DialogueData_SO.DialogueLine choice in choices)
        {
            GameObject button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            button.GetComponentInChildren<Text>().text = choice.text;

            // ע�ᰴť����¼�
            int choiceIndex = choice.index;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                EventHandle.CallChoiceSelectedEvent(choiceIndex);
            });
        }

        choicesPanel.SetActive(true);
    }
    // ���ضԻ�UI
    private void HideDialogueUI()
    {
        dialogueCanvas.enabled = false;
        choicesPanel.SetActive(false);
    }

    /*
    // �����Ի���������ѡ��ʱ��"����"��ť��
    public void ContinueButtonClicked()
    {
        if (!isTyping)
        {
            DialogueManager.Instance.ContinueDialogue();
        }
        else
        {
            // ������ʾȫ���ı�
            StopAllCoroutines();
            dialogueText.text = fullText;
            isTyping = false;
        }
    }
    */
}
