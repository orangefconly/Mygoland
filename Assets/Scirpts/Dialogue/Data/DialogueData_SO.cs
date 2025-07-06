using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData_SO",menuName = "DialogueData/DialogueData_SO")]

public class DialogueData_SO : ScriptableObject
{
    [SerializeField] private TextAsset dialogueCSV;
    
    // �Ի������ݽṹ
    [System.Serializable]
    public class DialogueLine
    {
        public int index;             // ������
        public string speaker;        // ˵��������
        public string imageName;      // ����ͼƬ����
        public ImagePosition position; // ͼƬλ�ã���/��/�ң�
        [TextArea] public string text; // �Ի��ı�
        public int nextIndex;         // ��һ������
        public bool isChoice;         // �Ƿ�Ϊѡ��
        public string[] choiceLabels; // ѡ���ı�����isChoiceΪtrueʱ��Ч��
        public int[] choiceNextIndices; // ѡ���Ӧ����һ������
    }

    public enum ImagePosition { Left, Center, Right }
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    // ����CSV�ļ�
    // ��DialogueData_SO���е�ParseCSV������ʵ��
    public void ParseCSV()
    {
        dialogueLines.Clear();
        if (dialogueCSV == null) return;

        string[] lines = dialogueCSV.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // ����������
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 7) continue;

            DialogueLine line = new DialogueLine
            {
                index = int.Parse(values[0]),
                speaker = values[1],
                imageName = values[2],
                position = (ImagePosition)System.Enum.Parse(typeof(ImagePosition), values[3]),
                text = values[4],
                nextIndex = int.Parse(values[5]),
                isChoice = bool.Parse(values[6])
            };

            // �����ѡ���У�����ѡ���ı���Ŀ������
            if (line.isChoice)
            {
                string[] choiceData = values[7].Split(';');
                line.choiceLabels = new string[choiceData.Length];
                line.choiceNextIndices = new int[choiceData.Length];

                for (int j = 0; j < choiceData.Length; j++)
                {
                    string[] choiceParts = choiceData[j].Split('|');
                    if (choiceParts.Length >= 2)
                    {
                        line.choiceLabels[j] = choiceParts[0];
                        line.choiceNextIndices[j] = int.Parse(choiceParts[1]);
                    }
                }
            }

            dialogueLines.Add(line);
        }
    }

    // ����������ȡ�Ի���
    public DialogueLine GetLineByIndex(int index) 
    { /* ���ض�Ӧ�����ĶԻ��� */ 
     return dialogueLines[index];
    }

}