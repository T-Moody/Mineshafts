using System.Collections.Generic;
using UnityEngine;

namespace Mineshafts.Components
{
    public class UntilBuiltDisabler : MonoBehaviour
    {
        Piece piece;
        public List<GameObject> keepDisabledUntilBuilt = new List<GameObject>();

        public void Awake()
        {
            keepDisabledUntilBuilt.ForEach(go => go.SetActive(false));
        }

        public void Start()
        {
            piece = this.GetComponent<Piece>();
        }
    
        public void Update()
        {
            if (piece.m_creator != (long)0)
            {
                keepDisabledUntilBuilt.ForEach(go => go.SetActive(true));
                this.enabled = false;
            }
        }
    }
}
