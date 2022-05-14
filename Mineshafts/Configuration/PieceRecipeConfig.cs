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

        public void Apply()
        {
            var zns = ZNetScene.instance;
            if (zns == null) return;

            var requirements = CreateRequirements(); ;

            zns.m_namedPrefabs[piece.GetStableHashCode()].GetComponent<Piece>().m_resources = requirements;

            var hammer = zns.m_namedPrefabs["Hammer".GetStableHashCode()]?.GetComponent<ItemDrop>();
            if(hammer != null)
            {
                var pieceInHammerList = hammer.m_itemData.m_shared.m_buildPieces.m_pieces.Find(p => string.Equals(p.name, piece, StringComparison.Ordinal))?.GetComponent<Piece>();
                if (pieceInHammerList != null) pieceInHammerList.m_resources = requirements;
            }
        }
    }
}
