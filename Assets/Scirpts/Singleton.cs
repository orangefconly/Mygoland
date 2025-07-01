using UnityEngine;

/// <summary>
/// ������Unity�ķ��͵�������
/// �̳д��༴���Զ���һ�����ת��Ϊ����ģʽ
/// �÷�: public class MyManager : Singleton<MyManager> { ... }
/// </summary>
/// <typeparam name="T">�̳���Singleton�ľ���������</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // ��̬ʵ������
    private static T _instance;

    // �������ʵ㣬ȷ��ʵ��������Ψһ
    public static T Instance
    {
        get
        {
            // ���ʵ���Ƿ��Ѵ���
            if (_instance == null)
            {
                // �����ڳ����в�������ʵ��
                _instance = FindObjectOfType<T>();

                // ���������û���ҵ�ʵ��
                if (_instance == null)
                {
                    // ����һ���µ�GameObject����ӵ������
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();

                    // ����Ϊ�־û�����
                    DontDestroyOnLoad(singletonObject);

                    Debug.Log($"�����˵���ʵ��: {typeof(T).Name}");
                }
                else
                {
                    Debug.Log($"�ҵ����е���ʵ��: {typeof(T).Name}");
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// ��ʼ����������ʵ���״δ���ʱ����
    /// �������д�˷������г�ʼ������
    /// </summary>
    protected virtual void Awake()
    {
        // ����Ƿ���������ʵ������
        if (_instance == null)
        {
            // ���ǵ�һ��ʵ�� - ������Ϊ����ʵ��
            _instance = this as T;

            // ȷ�������ڳ����л�ʱ��������
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }

            OnSingletonAwake();
        }
        else
        {
            // �������ʵ�����ҵ�ǰʵ�������Ѵ��ڵ�ʵ��
            if (this != _instance)
            {
                // �����ظ���ʵ��
                Debug.LogWarning($"�ڳ����з����ظ��ĵ���ʵ��: {typeof(T).Name}���������ٵ�ǰʵ��");
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// ������ʵ��������ʱ����
    /// </summary>
    protected virtual void OnDestroy()
    {
        // �����ǰ���ٵ��ǵ���ʵ������
        if (this == _instance)
        {
            // �������
            _instance = null;

            OnSingletonDestroy();
        }
    }

    /// <summary>
    /// �������д�˷��������ڵ�����ʼ����ɺ�Ĳ���
    /// ������Awake�����������������ĵ���ʵ���ϵ���
    /// </summary>
    protected virtual void OnSingletonAwake() { }

    /// <summary>
    /// �������д�˷��������ڵ�������ǰ���������
    /// </summary>
    protected virtual void OnSingletonDestroy() { }

    /// <summary>
    /// ��鵥��ʵ���Ƿ����
    /// </summary>
    /// <returns>���ʵ�����ڷ���true�����򷵻�false</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }
}