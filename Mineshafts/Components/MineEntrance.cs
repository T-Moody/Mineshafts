using UnityEngine;

namespace Mineshafts.Components
{
    public class MineEntrance : MonoBehaviour
    {
        public void OnEnable()
        {
            var t = transform;
            if (t.position.y >= Main.gridMinHeight)
            {
                var groundHeight = ZoneSystem.instance.GetGroundHeight(transform.position);
                var pos = t.position;
                pos.y = groundHeight;
                t.position = pos;
            }
        }
    }
}
