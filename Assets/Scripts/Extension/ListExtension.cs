﻿using System.Collections.Generic;
using UnityEngine;

namespace Extension
{
    public static class ListExtension
    {
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return list[Random.Range(0, list.Count)];
            }

            return default;
        }
    }
}