using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core.UI.Editor
{
    [CustomEditor(typeof(BaseWindow<>), true)]
    public class BaseWindowEditor : UnityEditor.Editor
    {
        private static readonly string[] ManuallyDrawnFields =
        {
            "id", "startHidden", "animated", "animationEase", "animationDuration", "fade", "scalableContent", "useDoTween"
        };
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("startHidden"));

            var animatedProp = serializedObject.FindProperty("animated");
            EditorGUILayout.PropertyField(animatedProp);

            if (animatedProp.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useDoTween"));
#if DOTWEEN
                EditorGUILayout.PropertyField(serializedObject.FindProperty("animationEase"));
#endif
                EditorGUILayout.PropertyField(serializedObject.FindProperty("animationDuration"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fade"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("scalableContent"));
            }

            var baseType = target.GetType().BaseType;
            if (baseType != null)
            {
                var baseFields = baseType
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(f =>
                        (f.IsPublic || f.GetCustomAttribute<SerializeField>() != null) &&
                        !ManuallyDrawnFields.Contains(f.Name)
                    );

                var fieldInfos = baseFields as FieldInfo[] ?? baseFields.ToArray();
                foreach (var field in fieldInfos)
                {
                    var property = serializedObject.FindProperty(field.Name);
                    if (property != null)
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }
                }

                var exclude = ManuallyDrawnFields
                    .Concat(fieldInfos.Select(f => f.Name))
                    .Append("m_Script")
                    .ToArray();

                DrawPropertiesExcluding(serializedObject, exclude);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}