using Mineshafts.Interfaces;
using System.Reflection;
using UnityEngine;

namespace Mineshafts.Services
{
    public class AssetService : IAssetService
    {
        private readonly string _mineshaftsAssetBundleName = "mineshafts";
        // Cashed mineshafts asset bundle
        private AssetBundle _mineshaftsAssetBundle;

        public AssetBundle LoadMineshaftsAssetBundle()
        {
            if (_mineshaftsAssetBundle == null)
            {
                _mineshaftsAssetBundle = LoadBundle(_mineshaftsAssetBundleName);
            }

            return _mineshaftsAssetBundle;
        }
        
        public AssetBundle LoadBundle(string bundleName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return AssetBundle.LoadFromStream(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources." + bundleName));
        }

        public GameObject LoadPrefab(string prefabName)
        {
            var bundle = LoadBundle(_mineshaftsAssetBundleName);
            return bundle.LoadAsset<GameObject>(prefabName);
        }
    }
}