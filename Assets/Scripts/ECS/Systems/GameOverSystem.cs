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
        private Array _tags;
        private Dictionary<TagTeam, int> _countUnits;

        public override void OnAwake()
        {
            _filter = World.Filter
                .With<HealthComponent>()
                .With<TagTeamComponent>()
                .Build();

            // Инициализация тэгов
            _tags = Enum.GetValues(typeof(TagTeam));
            _countUnits = new Dictionary<TagTeam, int>();
            // обнуление количества юнитов
            foreach (TagTeam tag in _tags)
            {
                _countUnits[tag] = 0;
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                _countUnits[(TagTeam) i] = 0;
            }

            foreach (var entity in _filter)
            {
                ref var tagComponent = ref entity.GetComponent<TagTeamComponent>();
                //ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                //if (healthComponent.IsLive)
                {
                    _countUnits[tagComponent.TagTeam]++;
                }
            }

            // подсчет живых игроков
            int countLivePlayers = 0;
            foreach (TagTeam tag in _tags)
            {
                if (_countUnits[tag] > 0)
                {
                    countLivePlayers++;
                }
            }

            if (countLivePlayers <= 1)
            {
                EcsLoop.Instance.GameOver();

                foreach (TagTeam tag in _tags)
                {
                    if (_countUnits[tag] > 0)
                    {
                        Debug.Log("Победила команда : " + (tag).ToString());
                        break;
                    }
                }
            }
        }
    }
}