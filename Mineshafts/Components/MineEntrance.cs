using Mineshafts.Interfaces;
using Mineshafts.Services;
using UnityEngine;

namespace Mineshafts.Components
{
    public class MineEntrance : MonoBehaviour
    {
        public bool alwaysAlign = false;

        private void OnEnable()
        {
            var gridService = ServiceLocator.Get<IGridService>();

            if (transform.position.y >= gridService.GetGridMinHeight() || alwaysAlign)
            {
                Align();
            }
        }

        public void Align()
        {
            var t = transform;
            var groundHeight = ZoneSystem.instance.GetGroundHeight(transform.position);
            var pos = t.position;
            pos.y = groundHeight;
            t.position = pos;
        }
    }
}
