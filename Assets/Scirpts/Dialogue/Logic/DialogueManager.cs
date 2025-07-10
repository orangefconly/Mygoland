using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SearchService;

public class DialogueManager : Singleton<DialogueManager>
{
    // 图片资源字典
    private Dictionary<string, Sprite> imageDictionary = new Dictionary<string, Sprite>();

    // 当前对话数据
    private DialogueData_SO currentDialogue;
    private DialogueData_SO.DialogueLine currentLine;
    private int currentIndex = 0;

    // 对话状态
    public bool IsDialogueActive { get; private set; } = false;

    protected override void Awake()
    {   
        base.Awake();
        InitializeImageDictionary();
        SubscribeEvents();
    }

    // 初始化图片字典
    private void InitializeImageDictionary()
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>("");
        foreach (Sprite sprite in allSprites)
        {
            imageDictionary[sprite.name] = sprite;
        }
    }

    // 订阅事件
    private void SubscribeEvents()
    {
        EventHandle.ChoiceSelectedEvent += HandleChoiceSelection;
    }

    // 开始对话
    public void StartDialogue(DialogueData_SO dialogue)
    {
        if (IsDialogueActive) return;

        currentDialogue = dialogue;
        currentIndex = 0;
        currentDialogue.ParseCSV();
        currentLine = currentDialogue.GetLineByIndex(currentIndex);

        if (currentLine == null)
        {
            Debug.LogError("对话起始行不存在！");
            return;
        }

        IsDialogueActive = true;
        EventHandle.CallStartDialogueEvent(currentLine);
    }

    // 继续对话
    public void ContinueDialogue()
    {
        if (!IsDialogueActive) return;

        currentIndex = currentLine.nextIndex;
        if (currentIndex == 9999)
        {
            EndDialogue();
            return;
        }
        currentLine = currentDialogue.GetLineByIndex(currentIndex);

        if (currentLine == null)
        {
            EndDialogue();
            return;
        }

        if (currentLine.isChoice)
        {
            List<DialogueData_SO.DialogueLine> choices = new List<DialogueData_SO.DialogueLine>();

            for (int i = 0; i < currentLine.choiceLabels.Length; i++)
            {
                DialogueData_SO.DialogueLine choiceLine = new DialogueData_SO.DialogueLine
                {
                    index = i,
                    text = currentLine.choiceLabels[i],
                    nextIndex = currentLine.choiceNextIndices[i]
                };

                choices.Add(choiceLine);
            }

            EventHandle.CallChoicesAvailableEvent(choices);
        }
        else
        {
            EventHandle.CallUpdateDialogueEvent(currentLine);
        }
    }

    // 处理选项选择
    private void HandleChoiceSelection(int choiceIndex)
    {
        if (!IsDialogueActive || !currentLine.isChoice) return;

        currentIndex = currentLine.choiceNextIndices[choiceIndex];
        currentLine = currentDialogue.GetLineByIndex(currentIndex);

        if (currentLine == null)
        {
            EndDialogue();
            return;
        }

        EventHandle.CallUpdateDialogueEvent(currentLine);
    }

    // 结束对话
    private void EndDialogue()
    {
        IsDialogueActive = false;
        currentDialogue = null;
        currentLine = null;
        EventHandle.CallEndDialogerEvent();
    }

    // 获取图片
    public Sprite GetImage(string imageName)
    {
        if (imageDictionary.TryGetValue(imageName, out Sprite image))
        {
            return image;
        }

        Debug.LogWarning($"未找到图片: {imageName}");
        return null;
    }
}