using UnityEngine;

namespace Extension
{
    public static class ArrayExtension
    {
        public static T GetRandom<T>(this T[] arr)
        {
            if(arr.Length > 0)
                return arr[Random.Range(0, arr.Length)];
            return default;
        }
    }
}