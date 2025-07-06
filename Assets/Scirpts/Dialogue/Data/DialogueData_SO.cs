using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData_SO",menuName = "DialogueData/DialogueData_SO")]

public class DialogueData_SO : ScriptableObject
{
    [SerializeField] private TextAsset dialogueCSV;
    
    // 对话行数据结构
    [System.Serializable]
    public class DialogueLine
    {
        public int index;             // 行索引
        public string speaker;        // 说话人姓名
        public string imageName;      // 表现图片名称
        public ImagePosition position; // 图片位置（左/中/右）
        [TextArea] public string text; // 对话文本
        public int nextIndex;         // 下一句索引
        public bool isChoice;         // 是否为选项
        public string[] choiceLabels; // 选项文本（仅isChoice为true时有效）
        public int[] choiceNextIndices; // 选项对应的下一句索引
    }

    public enum ImagePosition { Left, Center, Right }
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    // 解析CSV文件
    // 在DialogueData_SO类中的ParseCSV方法简化实现
    public void ParseCSV()
    {
        dialogueLines.Clear();
        if (dialogueCSV == null) return;

        string[] lines = dialogueCSV.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 跳过标题行
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

            // 如果是选项行，解析选项文本和目标索引
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

    // 根据索引获取对话行
    public DialogueLine GetLineByIndex(int index) 
    { /* 返回对应索引的对话行 */ 
     return dialogueLines[index];
    }

}