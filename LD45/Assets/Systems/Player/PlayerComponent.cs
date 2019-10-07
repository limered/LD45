using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerComponent : GameComponent
    {
        public BoolReactiveProperty IsMoving;
    }
}