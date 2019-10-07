using System;
using SystemBase;
using Systems.InputHandling;
using UnityEngine;

namespace Systems.Attac
{
    public class AttackComponent : GameComponent
    {
        public AttackDefinition[] AttackPrefabs;
    }

    [Serializable]
    public class AttackDefinition
    {
        public InputWordType WordType;
        public GameObject Prefab;
    }
}