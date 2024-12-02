using System;
using System.Collections.Generic;
using Scellecs.Morpeh;

namespace ECS
{
    public static class WorldManager
    {
        private static Dictionary<int, World> _worlds;

        public static readonly int WORLD_DEFAULT = 0;
        public static readonly int WORLD_EVENT = 1;

        public static World WorldDefault => _worlds[WORLD_DEFAULT];
        public static World WorldEvent => _worlds[WORLD_EVENT];
        
        
        public static World GetWorld(int index)
        {
            return _worlds[index];
        }

        public static void CreateWorlds()
        {
            _worlds = new Dictionary<int, World>();
            
            _worlds.Add(WORLD_DEFAULT, World.Default);
            _worlds.Add(WORLD_EVENT, World.Create("Event World"));
        }
    }
}
