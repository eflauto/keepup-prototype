using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<string> toppingNames;
    public List<Texture> toppingTextures;
    public Material baseMaterial;

    private Dictionary<string, Texture> _toppingDictionary = new();
    private GameObject _toppingObject;
    private MeshRenderer _toppingMeshRenderer;
    private List<string> _toppings = new();

    private void Start()
    {
        baseMaterial.mainTexture = null;
        baseMaterial.color = new Color(255, 255, 255, 0);
        _toppingObject = GameObject.Find("Pizza/Topping_Quad");
        _toppingMeshRenderer = _toppingObject.GetComponent<MeshRenderer>();
        _toppingDictionary = toppingNames.Zip(toppingTextures, (k, v) => new { Key = k, Value = v })
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private void AddTopping(string topping)
    {
        var newMaterial = baseMaterial;
        var materialList = _toppingMeshRenderer.materials;
        
        _toppings.Add(topping);
        newMaterial.mainTexture = _toppingDictionary[topping];
        newMaterial.color = Color.white;
        materialList[_toppings.Count - 1] = newMaterial;
        _toppingMeshRenderer.materials = materialList;
    }

    public void OnButtonClick(string topping)
    {
        if (_toppings.Contains(topping)) return;
        
        AddTopping(topping);
    }
}