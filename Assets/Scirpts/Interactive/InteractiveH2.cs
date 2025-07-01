using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveH2 : Interactive
{
    public DialogueData_SO dialogueData1;
    public DialogueData_SO dialogueData2;

    protected override void OnClickedAction()
    {
        DialogueManager.Instance.StartDialogue(dialogueData1);
    }

    public override void EmptyClicked()
    {
        base.EmptyClicked();
        DialogueManager.Instance.StartDialogue(dialogueData2);
    }
}

