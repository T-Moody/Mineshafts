using Mineshafts.Interfaces;
using Mineshafts.Services;
using UnityEngine;

namespace Mineshafts.Components
{
    public class PlayerBlackbox : MonoBehaviour
    {
        private readonly IGridService _gridService = ServiceLocator.Get<IGridService>();
        public GameObject box;
        public Transform t;

        private bool active = true;

        void Awake()
        {
            t = transform;
        }

        void FixedUpdate()
        {
            if (t.position.y > _gridService.GetGridMinHeight() && t.position.y < _gridService.GetGridMaxHeight())
            {
                if (!active)
                {
                    active = true;
                    UpdateActive();
                }
            }
            else
            {
                if(active)
                {
                    active = false;
                    UpdateActive();
                }
            }
        }

        private void UpdateActive()
        {
            box.gameObject.SetActive(active);
        }
    }
}
