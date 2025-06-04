using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CarVisualManager : MonoBehaviour
{
    [Serializable]
    public class MaterialSet
    {
        public string groupName;
        public List<Material> materials = new();
    }

    [Header("Material Sets")]
    [SerializeField]
    private List<MaterialSet> _materialSets = new();

    [Header("Car Body Renderers")]
    [SerializeField]
    private List<Renderer> _bodyRenderers = new();

    private const string SelectedSetKeyFormat = "{0}_SelectedMaterialSet";

    public void PreviewMaterialSet(int setIndex)
    {
        if (!ValidateSets()) return;
        if (setIndex < 0 || setIndex >= _materialSets.Count)
        {
            Debug.LogErrorFormat("[CarVisualManager] PreviewMaterialSet: Index {0} out of range.", setIndex);
            return;
        }

        ApplyMaterials(_materialSets[setIndex].materials);
        Debug.LogFormat("[CarVisualManager] Previewing: {0}", _materialSets[setIndex].groupName);
    }

    public void ApplyMaterialSet(int setIndex, string carName)
    {
        if (string.IsNullOrEmpty(carName))
        {
            Debug.LogError("[CarVisualManager] ApplyMaterialSet: 'carName' is null or empty.");
            return;
        }
        if (!ValidateSets()) return;
        if (setIndex < 0 || setIndex >= _materialSets.Count)
        {
            Debug.LogErrorFormat("[CarVisualManager] ApplyMaterialSet: Index {0} out of range.", setIndex);
            return;
        }

        var set = _materialSets[setIndex];
        ApplyMaterials(set.materials);

        string key = string.Format(SelectedSetKeyFormat, carName);
        PlayerPrefs.SetInt(key, setIndex);
        PlayerPrefs.Save();

        Debug.LogFormat("[CarVisualManager] Applied: {0}", set.groupName);
    }

    public IReadOnlyList<MaterialSet> GetMaterialSets()
    {
        return _materialSets;
    }

    private bool ValidateSets()
    {
        if (_materialSets == null || _materialSets.Count == 0)
        {
            Debug.LogError("[CarVisualManager] No material sets assigned.");
            return false;
        }
        return true;
    }

    private void ApplyMaterials(IReadOnlyList<Material> materials)
    {
        if (_bodyRenderers == null || _bodyRenderers.Count == 0)
        {
            Debug.LogWarning("[CarVisualManager] No body renderers assigned.");
            return;
        }

        for (int i = 0; i < _bodyRenderers.Count; i++)
        {
            var rend = _bodyRenderers[i];
            if (rend == null) continue;
            if (i < materials.Count && materials[i] != null)
            {
                rend.material = materials[i];
            }
        }
    }
}
