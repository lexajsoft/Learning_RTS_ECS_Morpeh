using System.Collections;
using System.Collections.Generic;
using ECS;
using ECS.Components;
using ECS.Factories;
using ECS.ScriptableObjects;
using Scellecs.Morpeh;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform _gameContainer;
    // пресет базы
    [SerializeField] private EntityDescriptionScriptableObject castelEntityDescriptionScriptableObject;
    
    [SerializeField] private EntityDescriptionScriptableObject testUnitEntityDescriptionScriptableObject;
    
    
    [SerializeField] private GameObject _player1PointSpawnCastle;     
    [SerializeField] private GameObject _player2PointSpawnCastle;
    [SerializeField] private GameObject _player3PointSpawnCastle;
    [SerializeField] private GameObject _player4PointSpawnCastle;

    [SerializeField] private bool is4Spawns;
    [SerializeField] private bool is4Player;

    [SerializeField] private PlayerGameplayController _playerController;
    private List<Entity> _players;
    
    public void Install()
    {
        _players = new List<Entity>();
        // управление устанавливается строго для красного игрока
        // CreateBase
        if (is4Player)
        {
            _players.Add(EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                _gameContainer, TagTeam.Red, _player1PointSpawnCastle.transform.position,
                _player1PointSpawnCastle.transform.rotation));

            _players.Add(EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                _gameContainer, TagTeam.Blue, _player2PointSpawnCastle.transform.position,
                _player2PointSpawnCastle.transform.rotation));

            _players.Add(EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                _gameContainer, TagTeam.Green, _player3PointSpawnCastle.transform.position,
                _player3PointSpawnCastle.transform.rotation));

            _players.Add(EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                _gameContainer, TagTeam.Purple, _player4PointSpawnCastle.transform.position,
                _player4PointSpawnCastle.transform.rotation));
        }
        else
        {
            if (is4Spawns)
            {
                _players.Add(EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                    castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                    _gameContainer, TagTeam.Red, _player1PointSpawnCastle.transform.position,
                    _player1PointSpawnCastle.transform.rotation));

                _players.Add(EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                    castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                    _gameContainer, TagTeam.Blue, _player2PointSpawnCastle.transform.position,
                    _player2PointSpawnCastle.transform.rotation));

                EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                    castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                    _gameContainer, TagTeam.Red, _player3PointSpawnCastle.transform.position,
                    _player3PointSpawnCastle.transform.rotation);

                EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                    castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                    _gameContainer, TagTeam.Blue, _player4PointSpawnCastle.transform.position,
                    _player4PointSpawnCastle.transform.rotation);
            }
            else
            {
                _players.Add(EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                    castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                    _gameContainer, TagTeam.Red, _player1PointSpawnCastle.transform.position,
                    _player1PointSpawnCastle.transform.rotation));

                _players.Add(EntityFactory.CreateEntity(castelEntityDescriptionScriptableObject.GetEntityDescriptionData(),
                    castelEntityDescriptionScriptableObject.GetEntityComponentsData(),
                    _gameContainer, TagTeam.Blue, _player2PointSpawnCastle.transform.position,
                    _player2PointSpawnCastle.transform.rotation));
            }
        }
        
        _playerController.SetEntity(_players[0]);
        _playerController.Init();

        for (int i = 1; i < _players.Count; i++)
        {
            var aiGameObject = new GameObject();
            var aiGameplayController = aiGameObject.AddComponent<AIGameplayController>();
            aiGameplayController.name = "[BOT-AI_CONTROLLER]:" + i;
            aiGameplayController.SetEntity(_players[i]);
            aiGameplayController.Init();
        }
    }
    
    [Button]
    public void Spawn()
    {
        // test create npc
        // CreateBase
        EntityFactory.CreateEntity(testUnitEntityDescriptionScriptableObject.GetEntityDescriptionData(),
            testUnitEntityDescriptionScriptableObject.GetEntityComponentsData(),
            _gameContainer, TagTeam.Red, _player1PointSpawnCastle.transform.position, _player1PointSpawnCastle.transform.rotation);
        
        EntityFactory.CreateEntity(testUnitEntityDescriptionScriptableObject.GetEntityDescriptionData(), 
            testUnitEntityDescriptionScriptableObject.GetEntityComponentsData(),
            _gameContainer, TagTeam.Blue, _player2PointSpawnCastle.transform.position, _player2PointSpawnCastle.transform.rotation);
        
        EntityFactory.CreateEntity(testUnitEntityDescriptionScriptableObject.GetEntityDescriptionData(), 
            testUnitEntityDescriptionScriptableObject.GetEntityComponentsData(),
            _gameContainer, TagTeam.Green, _player3PointSpawnCastle.transform.position, _player3PointSpawnCastle.transform.rotation);
        
        EntityFactory.CreateEntity(testUnitEntityDescriptionScriptableObject.GetEntityDescriptionData(), 
            testUnitEntityDescriptionScriptableObject.GetEntityComponentsData(),
            _gameContainer, TagTeam.Purple, _player4PointSpawnCastle.transform.position, _player4PointSpawnCastle.transform.rotation);
        
    }
}