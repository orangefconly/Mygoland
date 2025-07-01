using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SceneName] public string sceneForm;

    [SceneName] public string sceneGoto;

    public void TeleportToScene()
    {
        TransitionManager.Instance.TransitionBetweenScenes(sceneForm, sceneGoto);
    }
}