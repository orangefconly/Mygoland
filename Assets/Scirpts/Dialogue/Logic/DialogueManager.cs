using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SearchService;

public class DialogueManager : Singleton<DialogueManager>
{
    // ͼƬ��Դ�ֵ�
    private Dictionary<string, Sprite> imageDictionary = new Dictionary<string, Sprite>();

    // ��ǰ�Ի�����
    private DialogueData_SO currentDialogue;
    private DialogueData_SO.DialogueLine currentLine;
    private int currentIndex = 0;

    // �Ի�״̬
    public bool IsDialogueActive { get; private set; } = false;

    protected override void Awake()
    {   
        base.Awake();
        InitializeImageDictionary();
        SubscribeEvents();
    }

    // ��ʼ��ͼƬ�ֵ�
    private void InitializeImageDictionary()
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>("");
        foreach (Sprite sprite in allSprites)
        {
            imageDictionary[sprite.name] = sprite;
        }
    }

    // �����¼�
    private void SubscribeEvents()
    {
        EventHandle.ChoiceSelectedEvent += HandleChoiceSelection;
    }

    // ��ʼ�Ի�
    public void StartDialogue(DialogueData_SO dialogue)
    {
        if (IsDialogueActive) return;

        currentDialogue = dialogue;
        currentIndex = 0;
        currentDialogue.ParseCSV();
        currentLine = currentDialogue.GetLineByIndex(currentIndex);

        if (currentLine == null)
        {
            Debug.LogError("�Ի���ʼ�в����ڣ�");
            return;
        }

        IsDialogueActive = true;
        EventHandle.CallStartDialogueEvent(currentLine);
    }

    // �����Ի�
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

    // ����ѡ��ѡ��
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

    // �����Ի�
    private void EndDialogue()
    {
        IsDialogueActive = false;
        currentDialogue = null;
        currentLine = null;
        EventHandle.CallEndDialogerEvent();
    }

    // ��ȡͼƬ
    public Sprite GetImage(string imageName)
    {
        if (imageDictionary.TryGetValue(imageName, out Sprite image))
        {
            return image;
        }

        Debug.LogWarning($"δ�ҵ�ͼƬ: {imageName}");
        return null;
    }
}