using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ButtonSelectionManager : MonoBehaviour
{
    public Sprite handSprite;
    
    private Button _selectedButton;
    private GameObject _hand;
    private Image _handImage;
    private InputAction _navigateAction;
    
    private void Start()
    {
        var userInterface = GameObject.Find("UI");
        var firstButtonObject = userInterface.transform.GetChild(0).gameObject;
        _navigateAction = InputSystem.actions.FindAction("Navigate");
        _hand = new GameObject("Hand");
        _handImage = _hand.AddComponent<Image>();
        _handImage.sprite = handSprite;
        _hand.transform.SetParent(userInterface.transform);

        try
        {
            _selectedButton = firstButtonObject.GetComponent<Button>();
            _selectedButton.Select();
            MoveCursorToButton(firstButtonObject);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
        }
    }
    
    private void Update()
    {
        if (!_navigateAction.IsPressed() || _selectedButton == null) return;
        
        var newSelectedButton = EventSystem.current.currentSelectedGameObject;
        MoveCursorToButton(newSelectedButton);
    }

    private void MoveCursorToButton(GameObject buttonObject)
    {
        var rectTransform = buttonObject.GetComponent<RectTransform>();
        var rightXPos = rectTransform.position.x + rectTransform.rect.width;
        var bottomYPos = rectTransform.position.y - rectTransform.rect.height;
        
        _hand.transform.position = new Vector2(rightXPos, bottomYPos);
        
        if (!_hand.activeInHierarchy) _hand.SetActive(true);
    }
}
