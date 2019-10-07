using UnityEngine;

namespace Utils.Unity
{
    public static class VectorExtensions
    {
        public static Vector2 XZ(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.z);
        }
    }
}
