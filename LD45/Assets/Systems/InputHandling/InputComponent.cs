using SystemBase;
using UniRx;

namespace Assets.Systems
{
    public class InputComponent : GameComponent
    {
        public StringReactiveProperty CurrentWord = new StringReactiveProperty("");
        public float MaxTime = 5;
        public FloatReactiveProperty TimeLeft = new FloatReactiveProperty(5);
        public BoolReactiveProperty StartedTyping = new BoolReactiveProperty(false);
    }
}
