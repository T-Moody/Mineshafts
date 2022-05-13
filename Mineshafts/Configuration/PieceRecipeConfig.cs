using System;
using System.Collections.Generic;

namespace Mineshafts.Configuration
{
    public class PieceRecipeConfig
    {
        public string piece { get; set; } = string.Empty;
        public List<string> items { get; set; } = new List<string>();
        public bool recover { get; set; } = false;

        public Piece.Requirement[] CreateRequirements()
        {
            var zns = ZNetScene.instance;
            if (zns == null) return Array.Empty<Piece.Requirement>();

            var reqs = new List<Piece.Requirement>();
            for (int i = 0; i < items.Count; i+=2)
            {
                reqs.Add(new Piece.Requirement()
                {
                    m_resItem = zns.GetPrefab(items[i])?.GetComponent<ItemDrop>(),
                    m_amount = int.Parse(items[i+1]),
                    m_recover = recover
                });
            }
            return reqs.ToArray();
        }

        public void ApplyConfig()
        {
            var zns = ZNetScene.instance;
            if (zns == null) return;

            var pieceObject = zns.GetPrefab(piece)?.GetComponent<Piece>();
            pieceObject.m_resources = CreateRequirements();
        }
    }
}
