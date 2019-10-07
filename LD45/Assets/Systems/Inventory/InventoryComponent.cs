using System.Collections.Generic;
using SystemBase;
using UniRx;

namespace Systems.Inventory
{
    public class InventoryComponent : GameComponent
    {
        public ReactiveProperty<List<char>> CollectedKeys = new ReactiveProperty<List<char>>(
            new List<char>
        {
            
        });
    }
}
