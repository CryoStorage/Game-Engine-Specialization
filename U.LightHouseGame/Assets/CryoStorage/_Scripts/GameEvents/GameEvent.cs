using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    [HideInInspector]
    public List<GameEventListener> Listeners { get; } = new();


    public void Raise()
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            Listeners[i].OnEventRaised();
        }
    }
    
    public void RegisterListener(GameEventListener listener)
    {
        if (!Listeners.Contains(listener))
        {
            Listeners.Add(listener);
        }
    }
    
    public void UnregisterListener(GameEventListener listener)
    {
        if (Listeners.Contains(listener))
        {
            Listeners.Remove(listener);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get the target GameEvent
        GameEvent gameEvent = (GameEvent)target;

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("Listeners: ", EditorStyles.boldLabel);
        }

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.Space();
            
            EditorGUI.indentLevel++;
            foreach (GameEventListener listener in gameEvent.Listeners)
            {
                EditorGUILayout.LabelField("- " + listener.name, EditorStyles.miniLabel);
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();

        }
    }
}

#endif
