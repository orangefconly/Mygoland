using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TransitionManager : Singleton<TransitionManager>
{

    // Fade 所需变量
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
    /// 从一个场景切换到另一个场景
    /// </summary>
    /// <param name="from">源场景名称</param>
    /// <param name="to">目标场景名称</param>
    /// <param name="showLoadingScreen">是否显示加载界面</param>
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
           // 验证源场景是否存在并卸载
            if (SceneManager.GetSceneByName(from).IsValid())
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(from);

                // 等待卸载完成
                while (!unloadOperation.isDone)
                    yield return null;

                 Debug.Log($"已卸载场景: {from}");
            }
            else
            {
                 Debug.LogWarning($"源场景 '{from}' 不存在或已卸载");
            }
        }
        
        // 显示加载界面（如果需要）
        if (showLoadingScreen)
            ShowLoadingScreen();

        // 等待一帧，确保加载界面显示
        yield return null;

        // 异步加载目标场景
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

        // 等待加载完成
        while (!loadOperation.isDone)
        {
            // 更新加载进度
            UpdateLoadingProgress(loadOperation.progress);
            yield return null;
        }

        // 获取新加载的场景
        Scene newScene = SceneManager.GetSceneByName(to);

        // 设置为激活场景
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
            Debug.Log($"已激活场景: {to}");
        }
        else
        {
            Debug.LogError($"无法激活场景 '{to}'，场景无效");
        }

        
        EventHandle.CallAfterSceneLoadEvent();
        yield return Fade(0);
        // 清理加载界面
        if (showLoadingScreen)
            HideLoadingScreen();
    }

    // 以下为加载界面相关的虚方法，可在子类中重写
    protected virtual void ShowLoadingScreen()
    {
        // 默认实现，可在子类中重写
    
        Debug.Log("显示加载界面");
    }

    protected virtual void HideLoadingScreen()
    {
        // 默认实现，可在子类中重写
        Debug.Log("隐藏加载界面");
    }

    protected virtual void UpdateLoadingProgress(float progress)
    {
        // 默认实现，可在子类中重写
        Debug.Log($"加载进度: {progress * 100f}%");
    }
    /*
    // 检查场景是否存在于Build Settings中
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