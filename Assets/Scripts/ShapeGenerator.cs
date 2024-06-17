using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSettings _settings;
    private NoiseFilter[] _noiseFilters;

    public ShapeGenerator(ShapeSettings settings)
    {
        _settings = settings;
        _noiseFilters = new NoiseFilter[settings.noiseLayers.Length];
        for (var i = 0; i < _noiseFilters.Length; i++)
        {
            _noiseFilters[i] = new NoiseFilter(settings.noiseLayers[i].NoiseSettings);
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        var elevation = 0f;

        if (_noiseFilters.Length > 0)
        {
            firstLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_settings.noiseLayers[0].enalbed)
                elevation = firstLayerValue;
        }
        
        for (var i = 1; i < _noiseFilters.Length; i++)
        {
            if (_settings.noiseLayers[i].enalbed)
            {
                var mask = (_settings.noiseLayers[i].useFirstLayerMask) ? firstLayerValue : 1;
                elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        return pointOnUnitSphere * (_settings.planetRadius * (1 + elevation));
    }
}
