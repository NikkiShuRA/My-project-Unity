using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveWorld", menuName = "Scriptable Objects/SaveWorld")]
[Serializable]
public class SaveWorld : ScriptableObject
{
    public List<GameObject> GameObjects = new();
}
