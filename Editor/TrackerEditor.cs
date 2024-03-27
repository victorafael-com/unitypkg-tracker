



using UnityEditor;
using UnityEngine;

namespace com.victorafael.tracking
{

    [CustomEditor(typeof(Tracker))]
    public class TrackerEditor : Editor
    {

        void DrawLine(SerializedProperty events, string toggleName, string propertyName)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(events.FindPropertyRelative(toggleName), GUIContent.none, GUILayout.Width(20));
            if (events.FindPropertyRelative(toggleName).boolValue)
            {
                EditorGUILayout.PropertyField(events.FindPropertyRelative(propertyName));
            }
            else
            {
                EditorGUILayout.LabelField(events.FindPropertyRelative(propertyName).displayName);
            }
            EditorGUILayout.EndHorizontal();
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tracking"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("anchorOverride"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("camera"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("trackedObject"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("updateMode"));

            var events = serializedObject.FindProperty("events");
            DrawLine(events, "useDistance", "distanceUpdate");
            DrawLine(events, "useScreenPosition", "screenPositionUpdate");
            DrawLine(events, "useWorldPosition", "worldPositionUpdate");
            EditorGUILayout.PropertyField(events.FindPropertyRelative("onTrackingStateChange"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}