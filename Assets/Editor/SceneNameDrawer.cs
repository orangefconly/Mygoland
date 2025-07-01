using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// �Զ����������ΪSceneNameAttribute�����ṩ����������ѡ��
[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    private List<string> sceneNames; // �洢���г�����

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // �״ε���ʱ��ȡ���г���������Build Settings�У�
        if (sceneNames == null)
        {
            sceneNames = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                {
                    // ��·������ȡ����������"Assets/Scenes/TestScene.unity" �� "TestScene"��
                    string path = EditorBuildSettings.scenes[i].path;
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                    sceneNames.Add(sceneName);
                }
            }
        }

        // ��������Ƿ�Ϊ�ַ�������
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "��֧���ַ�������");
            return;
        }

        // ��ȡ��ǰֵ���б��е�����
        int currentIndex = sceneNames.IndexOf(property.stringValue);
        if (currentIndex < 0) currentIndex = 0; // Ĭ��Ϊ��һ������

        // ��ʾ����ѡ���
        currentIndex = EditorGUI.Popup(position, label.text, currentIndex, sceneNames.ToArray());
        property.stringValue = sceneNames[currentIndex]; // ��������ֵ
    }
}