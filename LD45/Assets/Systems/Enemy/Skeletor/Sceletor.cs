﻿using SystemBase;
using Systems.InputHandling;
using Systems.Movement;
using UniRx;
using UnityEngine;

namespace Systems.Enemy.Skeletor
{
    [RequireComponent(typeof(MovementComponent))]
    public class Sceletor : GameComponent
    {
        public string WordToDrop;
    }
}