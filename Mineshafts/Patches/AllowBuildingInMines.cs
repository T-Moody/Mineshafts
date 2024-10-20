using HarmonyLib;
using Mineshafts.Interfaces;
using Mineshafts.Services;

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

            var gridService = ServiceLocator.Get<IGridService>();

            if (pos > gridService.GetGridMinHeight() && pos < gridService.GetGridMaxHeight()) piece.m_allowedInDungeons = true;
        }
    }
}