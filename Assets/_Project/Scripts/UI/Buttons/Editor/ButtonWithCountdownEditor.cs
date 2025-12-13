using UnityEditor;
using Asteroids.Scripts.UI.Buttons;

[CustomEditor(typeof(ButtonWithCountdown))]
public class ButtonWithCountdownEditor : UnityEditor.UI.ButtonEditor
{
    private SerializedProperty _delaySecondsProp;
    private SerializedProperty _delayImageProp;

    protected override void OnEnable()
    {
        base.OnEnable();
        _delaySecondsProp = serializedObject.FindProperty("_delaySeconds");
        _delayImageProp = serializedObject.FindProperty("_delayImage");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_delaySecondsProp);
        EditorGUILayout.PropertyField(_delayImageProp);

        serializedObject.ApplyModifiedProperties();
    }
}