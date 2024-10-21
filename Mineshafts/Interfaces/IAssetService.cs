using UnityEngine;

namespace Mineshafts.Interfaces
{
    public interface IAssetService
    {
        AssetBundle LoadMineshaftsAssetBundle();
        AssetBundle LoadBundle(string bundleName);
        GameObject LoadPrefab(string prefabName);
    }
}