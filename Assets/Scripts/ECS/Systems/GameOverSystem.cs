using System;
using System.Collections.Generic;
using ECS.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(AnimationSystem))]
    public class GameOverSystem : UpdateSystem
    {
        private Filter _filter;
        public override void OnAwake()
        {
            _filter = World.Filter
                .With<HealthComponent>()
                .With<TagTeamComponent>()
                .Build();
            _countUnits = new Dictionary<TagTeam, int>();
            _countUnits[TagTeam.Red] = 0;
            _countUnits[TagTeam.Blue] = 0;
            _countUnits[TagTeam.Green] = 0;
            _countUnits[TagTeam.Purple] = 0;
            
        }

        private Dictionary<TagTeam, int> _countUnits;

        public override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < Enum.GetValues(typeof(TagTeam)).Length; i++)
            {
                _countUnits[(TagTeam)i] = 0;
            }
            
            foreach (var entity in _filter)
            {
                ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                ref var tagComponent = ref entity.GetComponent<TagTeamComponent>();
                //if (healthComponent.IsLive)
                {
                    _countUnits[tagComponent.TagTeam]++;
                }
            }

            int countLivePlayers = 0;
            for (int i = 0; i < Enum.GetValues(typeof(TagTeam)).Length; i++)
            {
                if (_countUnits[(TagTeam)i] > 0)
                {
                    countLivePlayers++;
                }    
            }
            // if (_countUnits[TagTeam.Red] > 0)
            // {
            //     countLivePlayers++;
            // }
            // if (_countUnits[TagTeam.Blue] > 0)
            // {
            //     countLivePlayers++;
            // }
            // if (_countUnits[TagTeam.Green] > 0)
            // {
            //     countLivePlayers++;
            // }
            // if (_countUnits[TagTeam.Purple] > 0)
            // {
            //     countLivePlayers++;
            // }

            if (countLivePlayers <= 1)
            {
                EcsLoop.Instance.GameOver();
                
                for (int i = 0; i < Enum.GetValues(typeof(TagTeam)).Length; i++)
                {
                    if (_countUnits[(TagTeam)i] > 0)
                    {
                        Debug.Log("Победила команда : " + ((TagTeam)i).ToString());
                        break;
                    }    
                }
            }
        }
    }
}