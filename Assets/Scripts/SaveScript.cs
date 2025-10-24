using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "save", menuName = "Scriptable Objects/CapsuleData")]
[Serializable]
public class CapsuleData : ScriptableObject
{
    public List<Capsule> _capsules = new( );
}

[Serializable]
public class Capsule
{
    public float x;
    public float y;
    public float z;
}
