using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TransitionManager : Singleton<TransitionManager>
{

    // Fade �������
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    private bool isfade = false;

    private IEnumerator Fade(float targetAlpha)
    {
        fadeCanvasGroup.blocksRaycasts = true;
        isfade = true;

        //Debug.Log("111");

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        isfade = false;
        fadeCanvasGroup.blocksRaycasts = false;
    }


    [SceneName] public string sceneStart;

    private void Start()
    {
        StartCoroutine(TransitionCoroutine(string.Empty, sceneStart,true));
    }
    /// <summary>
    /// ��һ�������л�����һ������
    /// </summary>
    /// <param name="from">Դ��������</param>
    /// <param name="to">Ŀ�곡������</param>
    /// <param name="showLoadingScreen">�Ƿ���ʾ���ؽ���</param>
    public void TransitionBetweenScenes(string from, string to, bool showLoadingScreen = true)
    {
        if (!isfade)
        {
            StartCoroutine(TransitionCoroutine(from, to, showLoadingScreen));
         }
        
    }

    private IEnumerator TransitionCoroutine(string from, string to, bool showLoadingScreen)
    { 
        yield return Fade(1);
        if (from != string.Empty)
        {
            EventHandle.CallBeforeSceneUnloadEvent();
           // ��֤Դ�����Ƿ���ڲ�ж��
            if (SceneManager.GetSceneByName(from).IsValid())
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(from);

                // �ȴ�ж�����
                while (!unloadOperation.isDone)
                    yield return null;

                 Debug.Log($"��ж�س���: {from}");
            }
            else
            {
                 Debug.LogWarning($"Դ���� '{from}' �����ڻ���ж��");
            }
        }
        
        // ��ʾ���ؽ��棨�����Ҫ��
        if (showLoadingScreen)
            ShowLoadingScreen();

        // �ȴ�һ֡��ȷ�����ؽ�����ʾ
        yield return null;

        // �첽����Ŀ�곡��
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

        // �ȴ��������
        while (!loadOperation.isDone)
        {
            // ���¼��ؽ���
            UpdateLoadingProgress(loadOperation.progress);
            yield return null;
        }

        // ��ȡ�¼��صĳ���
        Scene newScene = SceneManager.GetSceneByName(to);

        // ����Ϊ�����
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
            Debug.Log($"�Ѽ����: {to}");
        }
        else
        {
            Debug.LogError($"�޷������ '{to}'��������Ч");
        }

        
        EventHandle.CallAfterSceneLoadEvent();
        yield return Fade(0);
        // ������ؽ���
        if (showLoadingScreen)
            HideLoadingScreen();
    }

    // ����Ϊ���ؽ�����ص��鷽����������������д
    protected virtual void ShowLoadingScreen()
    {
        // Ĭ��ʵ�֣�������������д
    
        Debug.Log("��ʾ���ؽ���");
    }

    protected virtual void HideLoadingScreen()
    {
        // Ĭ��ʵ�֣�������������д
        Debug.Log("���ؼ��ؽ���");
    }

    protected virtual void UpdateLoadingProgress(float progress)
    {
        // Ĭ��ʵ�֣�������������д
        Debug.Log($"���ؽ���: {progress * 100f}%");
    }
    /*
    // ��鳡���Ƿ������Build Settings��
    private bool SceneExists(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (name == sceneName)
                return true;
        }

        return false;
    }
    */
}