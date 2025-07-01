using UnityEngine;

/// <summary>
/// 适用于Unity的泛型单例基类
/// 继承此类即可自动将一个组件转换为单例模式
/// 用法: public class MyManager : Singleton<MyManager> { ... }
/// </summary>
/// <typeparam name="T">继承自Singleton的具体类类型</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // 静态实例引用
    private static T _instance;

    // 公共访问点，确保实例存在且唯一
    public static T Instance
    {
        get
        {
            // 检查实例是否已存在
            if (_instance == null)
            {
                // 尝试在场景中查找现有实例
                _instance = FindObjectOfType<T>();

                // 如果场景中没有找到实例
                if (_instance == null)
                {
                    // 创建一个新的GameObject并添加单例组件
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();

                    // 设置为持久化对象
                    DontDestroyOnLoad(singletonObject);

                    Debug.Log($"创建了单例实例: {typeof(T).Name}");
                }
                else
                {
                    Debug.Log($"找到现有单例实例: {typeof(T).Name}");
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// 初始化方法，在实例首次创建时调用
    /// 子类可重写此方法进行初始化工作
    /// </summary>
    protected virtual void Awake()
    {
        // 检查是否已有其他实例存在
        if (_instance == null)
        {
            // 这是第一个实例 - 将其设为单例实例
            _instance = this as T;

            // 确保单例在场景切换时不被销毁
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }

            OnSingletonAwake();
        }
        else
        {
            // 如果已有实例，且当前实例不是已存在的实例
            if (this != _instance)
            {
                // 销毁重复的实例
                Debug.LogWarning($"在场景中发现重复的单例实例: {typeof(T).Name}，正在销毁当前实例");
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 当单例实例被销毁时调用
    /// </summary>
    protected virtual void OnDestroy()
    {
        // 如果当前销毁的是单例实例本身
        if (this == _instance)
        {
            // 清空引用
            _instance = null;

            OnSingletonDestroy();
        }
    }

    /// <summary>
    /// 子类可重写此方法，用于单例初始化完成后的操作
    /// 类似于Awake方法，但仅在真正的单例实例上调用
    /// </summary>
    protected virtual void OnSingletonAwake() { }

    /// <summary>
    /// 子类可重写此方法，用于单例销毁前的清理操作
    /// </summary>
    protected virtual void OnSingletonDestroy() { }

    /// <summary>
    /// 检查单例实例是否存在
    /// </summary>
    /// <returns>如果实例存在返回true，否则返回false</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }
}