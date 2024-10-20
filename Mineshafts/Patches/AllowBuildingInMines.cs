using HarmonyLib;

namespace Mineshafts.Patches
{
	[HarmonyPatch(typeof(Player), nameof(Player.UpdatePlacementGhost))]
    public static class AllowBuildingInMines
    {
		public static void Prefix(Player __instance)
        {
            var ghost = __instance.m_placementGhost;
            if (ghost == null) return;
            var piece = ghost.GetComponent<Piece>();
            var pos = ghost.transform.position.y;

            if (pos > Main.gridMinHeight && pos < Main.gridMaxHeight) piece.m_allowedInDungeons = true;
        }
    }
}