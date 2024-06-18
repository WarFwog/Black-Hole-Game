using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    private NoiseSettings.RigidNoiseSettings _settings;
    private Noise _noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        _settings = settings;
    }


    public float Evaluate(Vector3 point)
    {
        var noiseValue = 0f;
        var frequency = _settings.baseRoughness;
        var amplitude = 1f;
        var weight = 1f;
        for (var i = 0; i < _settings.numLayers; i++)
        {
            var v = 1 - Mathf.Abs(_noise.Evaluate(point * frequency + _settings.centre));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * _settings.weightMultiplier);
            
            noiseValue += v * amplitude;
            frequency *= _settings.roughness;
            amplitude *= _settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue);
        return noiseValue * _settings.strength;
    }
}
