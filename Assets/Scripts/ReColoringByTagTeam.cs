using System;
using System.Collections;
using Core;
using ECS.Components;
using ECS.Factories.Data;
using ECS.ScriptableObjects;
using UnityEngine;

public class ReColoringByTagTeam : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    
    // TODO возможно уже не используется по причине того что ранее использовался на примитивных моделях
    public void ReColor(TagTeam tagTeam)
    {
        if(_meshRenderer == null)
            return;
        
        var material = _meshRenderer.material;
        
        var tagTeamColorsScriptableObject = ServiceLocator.Get<TagTeamColorsScriptableObject>();
        var color = tagTeamColorsScriptableObject.GetColorByTagTeam(tagTeam);
        material.color = color;
    }
}
