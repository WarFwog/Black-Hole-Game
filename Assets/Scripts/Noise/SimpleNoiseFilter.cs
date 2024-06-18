using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private NoiseSettings.SimpleNoiseSettings _settings;
    private Noise _noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        _settings = settings;
    }


    public float Evaluate(Vector3 point)
    {
        var noiseValue = 0f;
        var frequency = _settings.baseRoughness;
        var amplitude = 1f;
        for (var i = 0; i < _settings.numLayers; i++)
        {
            var v = _noise.Evaluate(point * frequency + _settings.centre);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= _settings.roughness;
            amplitude *= _settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue);
        return noiseValue * _settings.strength;
    }
}
