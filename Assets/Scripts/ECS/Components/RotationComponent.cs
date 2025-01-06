using System;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct RotationComponent : IComponent, ICloneable
{
    public Vector3 DirectToLook;
    [Range(0.1f,1)]
    public float SpeedRotation;
    
    public object Clone()
    {
        return MemberwiseClone();
    }
}