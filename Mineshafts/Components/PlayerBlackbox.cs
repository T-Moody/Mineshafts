using UnityEngine;

namespace Mineshafts.Components
{
    public class PlayerBlackbox : MonoBehaviour
    {
        public GameObject box;
        public Transform t;

        private bool active = true;

        void Awake()
        {
            t = transform;
        }

        void FixedUpdate()
        {
            if (t.position.y > Main.gridMinHeight && t.position.y < Main.gridMaxHeight)
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
