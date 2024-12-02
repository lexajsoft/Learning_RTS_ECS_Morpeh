using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using ECS;
using ECS.Factories;
using ECS.Factories.Data;
using ECS.ScriptableObjects;
using ECS.Systems;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Logging;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-1000)]
public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private TagTeamColorsScriptableObject _tagTeamColorsScriptableObject;
    [SerializeField] private VisualEffectCollectionScriptableObject _visualEffectCollectionScriptableObject;
    [SerializeField] private HealthBarScriptableObject _healthBarScriptableObject;
    
    private void Awake()
    {
        Registration();

        Initing();

        InitingECS();
        
        _game.Install();
    }

    private void Registration()
    {
        ServiceLocator.Clear();
        ServiceLocator.Registy<TagTeamColorsScriptableObject>(_tagTeamColorsScriptableObject);
        ServiceLocator.Registy<VisualEffectCollectionScriptableObject>(_visualEffectCollectionScriptableObject);
        ServiceLocator.Registy<HealthBarScriptableObject>(_healthBarScriptableObject);
        
    }


    private void Initing()
    {
        VisualEffectFactory.Init(null);
        HealthBarFactory.Init();
    }

    private void InitingECS()
    {
        var gameLoopGameObject = new GameObject();
        gameLoopGameObject.name = "EcsLoop";
        var ecsLoop = gameLoopGameObject.AddComponent<EcsLoop>();
        ecsLoop.Initialize();
        DontDestroyOnLoad(gameLoopGameObject);
    }
}
