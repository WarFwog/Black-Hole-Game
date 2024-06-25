using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    private ColorSettings _settings;
    private Texture2D _texture;
    private static readonly int ElevationMinMax = Shader.PropertyToID("_elevationMinMax");
    private static readonly int Texture1 = Shader.PropertyToID("_texture");
    private const int TextureResolution = 50;

    public void UpdateSettings(ColorSettings settings)
    {
        _settings = settings;
        if(_texture == null || _texture.height != settings.biomeColorSettings.biomes.Length)
            _texture = new Texture2D(TextureResolution, settings.biomeColorSettings.biomes.Length);
    }

    public void UpdateElevation(MinMax elevationMixMax)
    {
        _settings.planetMaterial.SetVector(ElevationMinMax, new Vector4(elevationMixMax.Min, elevationMixMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        var heightPercent = (pointOnUnitSphere.y + 1) / 2;
        var biomeIndex = 0f;
        var numberOfBiomes = _settings.biomeColorSettings.biomes.Length;
        for (var i = 0; i < numberOfBiomes; i++)
        {
            if (_settings.biomeColorSettings.biomes[i].startHeight < heightPercent)
                biomeIndex = i;
            else
                break;
        }
        return biomeIndex / Mathf.Max(1, numberOfBiomes - 1);
    }
    public void UpdateColors()
    {
        Color[] colors = new Color[_texture.width * _texture.height];
        var colorIndex = 0;
        foreach (var biome in _settings.biomeColorSettings.biomes)
        {
            for (var i = 0; i < TextureResolution; i++)
            {
                var gradient = biome.gradient.Evaluate(i / (TextureResolution - 1f));
                var tintColor = biome.tint;
                colors[colorIndex] = gradient * (1 - biome.tintPercent) + tintColor * biome.tintPercent;
                colorIndex++;
            }
        }
        
        _texture.SetPixels(colors);
        _texture.Apply();
        _settings.planetMaterial.SetTexture(Texture1, _texture);
    }
}   