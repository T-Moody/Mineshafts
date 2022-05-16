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
            var reqs = new List<Piece.Requirement>();
            var zns = ZNetScene.instance;
            if (zns == null) return reqs.ToArray();
            
            try
            {
                for (int i = 0; i < items.Count; i += 2)
                {
                    var resItem = zns.GetPrefab(items[i])?.GetComponent<ItemDrop>();
                    if (resItem == null) continue;

                    var amount = int.Parse(items[i + 1]);

                    var req = new Piece.Requirement()
                    {
                        m_resItem = resItem,
                        m_amount = amount,
                        m_recover = recover
                    };

                    reqs.Add(req);
                }
                return reqs.ToArray();
            }
            catch(Exception e)
            {
                Main.log.LogError($"error caught while attemptint to create recipe for piece {piece}, the recipe will be empty");
                Main.log.LogError(e.Message);
            }

            return reqs.ToArray();
        }

        public void Apply()
        {
            var zns = ZNetScene.instance;
            if (zns == null) return;

            var pieceThing = zns.GetPrefab(piece)?.GetComponent<Piece>();
            if (pieceThing == null) return;

            var requirements = CreateRequirements();
            pieceThing.m_resources = requirements;

            var hammer = zns.GetPrefab("Hammer")?.GetComponent<ItemDrop>(); //zns.m_namedPrefabs["Hammer".GetStableHashCode()]?.GetComponent<ItemDrop>();
            if(hammer != null)
            {
                var pieceInHammerList = hammer.m_itemData.m_shared.m_buildPieces?.m_pieces?.Find(p => string.Equals(p.name, piece, StringComparison.Ordinal))?.GetComponent<Piece>();
                if (pieceInHammerList != null) pieceInHammerList.m_resources = requirements;
            }
        }
    }
}
