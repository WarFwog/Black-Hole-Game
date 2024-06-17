using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet _planet;
    private Editor _shapeEditor, _colorEditor;
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if(check.changed)
                _planet.GeneratePlanet();
        }
        
        if(GUILayout.Button("Generate Planet"))
            _planet.GeneratePlanet();
        
        DrawSettingsEditor(_planet.shapeSettings, _planet.OnShapeSettingsUpdated, ref _planet.shapeSettingsFoldout, ref _shapeEditor);
        DrawSettingsEditor(_planet.colorSettings, _planet.OnColorSettingsUpdated, ref _planet.colorSettingsFoldout, ref _colorEditor);
    }

    private void DrawSettingsEditor(object settings, System.Action onSettingsUpdated, ref bool foldOut, ref Editor editor)
    {
        if (settings == null) return;
        foldOut = EditorGUILayout.InspectorTitlebar(foldOut, (Object)settings);
        using var check = new EditorGUI.ChangeCheckScope();

        if (!foldOut) return;
        CreateCachedEditor((Object)settings, null, ref editor);
        editor.OnInspectorGUI();

        if (!check.changed) return;
        if (onSettingsUpdated != null)
        {
            onSettingsUpdated();
        }
    }

    private void OnEnable()
    {
        _planet = (Planet)target;
    }
}
