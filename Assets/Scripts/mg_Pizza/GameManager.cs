using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<string> toppingNames;
    public List<Texture> toppingTextures;
    public Material baseMaterial;

    private Dictionary<string, Texture> _toppingDictionary = new();
    private GameObject _toppingObject;
    private MeshRenderer _toppingMeshRenderer;
    private readonly List<string> _toppings = new();
    
    private Timer _timer;
    
    private TextMeshProUGUI _requestText;
    private List<string> _requestList;

    private GameObject _resultsObject;
    
    private ButtonSelectionManager _buttonSelectionManager;

    private void Start()
    {
        Random.InitState((int)DateTime.Now.Ticks);
        
        baseMaterial.mainTexture = null;
        baseMaterial.color = new Color(255, 255, 255, 0);
        
        _toppingObject = GameObject.Find("Pizza/Toppings");
        _toppingMeshRenderer = _toppingObject.GetComponent<MeshRenderer>();
        _toppingDictionary = toppingNames.Zip(toppingTextures, (k, v) => new { Key = k, Value = v })
            .ToDictionary(x => x.Key, x => x.Value);
        
        _timer = GetComponent<Timer>();
        _timer.StartTimer(5);
        _timer.onTimerEnd.AddListener(OnTimerEnd);
        
        _requestText = GameObject.Find("UI/txt_Request").GetComponent<TextMeshProUGUI>();

        _resultsObject = GameObject.Find("UI/Results");
        
        _buttonSelectionManager = GameObject.Find("UI").GetComponent<ButtonSelectionManager>();
        
        GenerateAndDisplayRequest();
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

    private void GenerateAndDisplayRequest()
    {
        var toppingAmount = Random.Range(1, toppingNames.Count + 1);
        _requestList = new List<string>(toppingNames);

        _requestList = _requestList.OrderBy(x => Guid.NewGuid()).ToList();
        _requestList.RemoveRange(toppingAmount, _requestList.Count - toppingAmount);
        _requestText.text = string.Join(", ", _requestList.ToArray());
    }

    public void OnButtonClick(string topping)
    {
        if (_toppings.Contains(topping)) return;
        
        AddTopping(topping);
    }

    private void OnTimerEnd()
    {
        _requestList.Sort();
        _toppings.Sort();

        for (var i = 0; i < _resultsObject.transform.childCount; i++)
        {
            _resultsObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        var resultsText = GameObject.Find("UI/Results/txt_Results").GetComponent<TextMeshProUGUI>();
        var replayButton = GameObject.Find("UI/Results/btn_Replay").GetComponent<Button>();

        _buttonSelectionManager.Delete();
        _requestText.text = "";
        resultsText.text = _requestList.SequenceEqual(_toppings) ? "You win!" : "You lose!";
        replayButton.Select();
    }
}