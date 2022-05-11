using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mineshafts.Components
{
    public class SnapToGrid : MonoBehaviour
    {
        public bool resetRotation = false;

        private void Start()
        {
            Util.AlignTransformToGrid(transform);
            if (resetRotation) transform.eulerAngles = Vector3.zero;
        }
    }
}
