using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 自定义绘制器：为SceneNameAttribute属性提供场景名下拉选择
[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    private List<string> sceneNames; // 存储所有场景名

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 首次调用时获取所有场景名（从Build Settings中）
        if (sceneNames == null)
        {
            sceneNames = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                {
                    // 从路径中提取场景名（如"Assets/Scenes/TestScene.unity" → "TestScene"）
                    string path = EditorBuildSettings.scenes[i].path;
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                    sceneNames.Add(sceneName);
                }
            }
        }

        // 检查属性是否为字符串类型
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "仅支持字符串类型");
            return;
        }

        // 获取当前值在列表中的索引
        int currentIndex = sceneNames.IndexOf(property.stringValue);
        if (currentIndex < 0) currentIndex = 0; // 默认为第一个场景

        // 显示下拉选择框
        currentIndex = EditorGUI.Popup(position, label.text, currentIndex, sceneNames.ToArray());
        property.stringValue = sceneNames[currentIndex]; // 更新属性值
    }
}