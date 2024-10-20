using System.Collections.Generic;
using UnityEngine;

namespace Mineshafts.Components
{
    public class PieceOnBuiltSwitch : MonoBehaviour
    {
        private Piece piece;
        public List<GameObject> keepDisabledUntilBuilt = new List<GameObject>();
        public List<GameObject> disableAfterBuilt = new List<GameObject>();

        public void Awake()
        {
            keepDisabledUntilBuilt.ForEach(go => go.SetActive(false));
            disableAfterBuilt.ForEach(go => go.SetActive(true));
        }

        public void Start()
        {
            piece = this.GetComponent<Piece>();
        }
    
        public void FixedUpdate()
        {
            if(this.GetComponent<ZNetView>() != null)
            //if (piece.m_creator != (long)0)
            {
                keepDisabledUntilBuilt.ForEach(go => go.SetActive(true));
                disableAfterBuilt.ForEach(go => go.SetActive(false));
                this.enabled = false;
            }
        }
    }
}
