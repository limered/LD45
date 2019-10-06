using System;
using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Inventory
{
    public class InventoryComponent : GameComponent
    {
        public ReactiveProperty<List<char>> CollectedKeys = new ReactiveProperty<List<char>>(
            new List<char>
        {
            'n',
            'o',
            't',
            'h',
            'i',
            'n',
            'g'
        });
    }
}
