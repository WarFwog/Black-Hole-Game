using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public bool autoUpdate = true;

    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };

    public FaceRenderMask faceRenderMask;

    [HideInInspector] public bool shapeSettingsFoldout, colorSettingsFoldout;
    private ShapeGenerator _shapeGenerator = new ShapeGenerator();
    private ColorGenerator _colorGenerator = new ColorGenerator();
    
    [Range(2, 256)] public int resolution = 10;
    [SerializeField, HideInInspector] private MeshFilter[] meshFilters;
    private TerrainFace[] _terrainFaces;

    private void Initialize()
    {
        _shapeGenerator.UpdateSettings(shapeSettings);
        _colorGenerator.UpdateSettings(colorSettings);
        if (meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];
        _terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (var i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                var meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
            
            _terrainFaces[i] = new TerrainFace(_shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            var renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (!autoUpdate) return;
        Initialize();
        GenerateMesh();
    }

    public void OnColorSettingsUpdated()
    {
        if (!autoUpdate) return;
        Initialize();
        GenerateColors();
    }

    private void GenerateMesh()
    {
        for (var i = 0; i < 6; i++)
        {
            if(meshFilters[i].gameObject.activeSelf)
                _terrainFaces[i].ConstructMesh();
        }
        _colorGenerator.UpdateElevation(_shapeGenerator.ElevationMinMax);
    }

    private void GenerateColors()
    {  
        _colorGenerator.UpdateColors();
    }
}