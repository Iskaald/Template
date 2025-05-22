namespace Core.UI.Editor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.UI;
    using System.Reflection;

    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        private bool showCustomFields = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space(10);
            var headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12,
                normal = { textColor = new Color(0.2f, 0.6f, 1.0f) }
            };
            EditorGUILayout.LabelField("Custom Button Properties", headerStyle);

            showCustomFields = EditorGUILayout.Foldout(showCustomFields, "Show Custom Fields", true, EditorStyles.foldoutHeader);
            if (showCustomFields)
            {
                EditorGUI.indentLevel++;

                var targetType = serializedObject.targetObject.GetType();
                while (targetType != typeof(CustomButton).BaseType && targetType != null)
                {
                    var fields = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                    foreach (var field in fields)
                    {
                        if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                        {
                            var property = serializedObject.FindProperty(field.Name);
                            if (property != null)
                            {
                                EditorGUILayout.PropertyField(property, true);
                            }
                        }
                    }

                    targetType = targetType.BaseType;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space(5);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

