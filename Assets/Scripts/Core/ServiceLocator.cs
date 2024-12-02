using System;
using System.Collections.Generic;

namespace Core
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, System.Object> _objects = new Dictionary<Type, object>();
    
        public static T Get<T>() where T : class
        {
            if (_objects.ContainsKey(typeof(T)))
            {
                if (_objects.TryGetValue(typeof(T), out object obj))
                {
                    return obj as T;
                }
            }

            return null;
        }
    
        public static object Get(Type type)
        {
            if (_objects.ContainsKey(type))
            {
                if (_objects.TryGetValue(type, out object obj))
                {
                    return obj;
                }
            }

            return null;
        }

        /// <summary>
        /// Регистрируется тип, указывается объект, и если надо то есть возможность перезаписывать если hardRegistry - true
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="hardRegistry"></param>
        /// <returns></returns>
        public static bool Registy(Type type, System.Object obj, bool hardRegistry = true)
        {
            if (_objects.ContainsKey(type) && hardRegistry)
            {
                UnRegistry(type);
            }

            if (_objects.ContainsKey(type) == false)
            {
                _objects.Add(type,obj);
                return true;
            }

            return false;
        }
    
        /// <summary>
        /// Регистрируется тип, указывается объект, и если надо то есть возможность перезаписывать если hardRegistry - true
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="hardRegistry"></param>
        /// <returns></returns>
        public static bool Registy<T>(Object obj, bool hardRegistry = true)
        {
            Type type = typeof(T);
            if (_objects.ContainsKey(type) && hardRegistry)
            {
                UnRegistry(type);
            }

            if (_objects.ContainsKey(type) == false)
            {
                _objects.Add(type,obj);
                return true;
            }

            return false;
        }

        public static void UnRegistry(Type type)
        {
            _objects.Remove(type);
        }
        public static void UnRegistry<T>()
        {
            _objects.Remove(typeof(T));
        }

        public static void Clear()
        {
            _objects.Clear();
        }
    }
}