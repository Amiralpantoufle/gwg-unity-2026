using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GWG_NavigationHelper))]
public class GWG_NavigationHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);
        GWG_NavigationHelper helper = (GWG_NavigationHelper)target;

        if (GUILayout.Button("Reset to Playable State"))
        {
            helper.ResetToPlayState();
        }

    }
}
