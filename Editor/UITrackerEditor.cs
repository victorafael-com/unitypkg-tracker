



using UnityEditor;
using UnityEngine;

namespace com.victorafael.tracking
{
    [CustomEditor(typeof(UITracker))]
    public class UITrackerEditor : TrackerEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("trackingRoot"));
            base.OnInspectorGUI();
        }
    }
}