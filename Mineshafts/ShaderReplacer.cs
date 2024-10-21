using UnityEngine;

public class ShaderReplacer : MonoBehaviour
{
    [Tooltip("Use this Field For Normal Renderers")]
    [SerializeField]
    internal Renderer[] _renderers;

    private void OnEnable()
    {
        foreach (var renderer in _renderers)
        {
            foreach (var material in renderer.sharedMaterials)
            {
                string shaderName = material.shader.name;
                Shader shader = Shader.Find(shaderName);

                if (shader == null)
                {
                    Debug.LogError($"Shader '{shaderName}' not found. Ensure it is included in the build.");
                    continue;
                }

                material.shader = shader;
            }
        }
    }
}