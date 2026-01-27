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
        _navigateAction = InputSystem.actions.FindAction("Navigate");
        _hand = new GameObject("Hand");
        _handImage = _hand.AddComponent<Image>();
        _handImage.sprite = handSprite;
        _hand.transform.SetParent(userInterface.transform);

        for (var i = 0; i < userInterface.transform.childCount; i++)
        {
            if (!userInterface.transform.GetChild(i).name.Contains("btn_")) continue;

            var potentialButtonObject = userInterface.transform.GetChild(i).gameObject;

            try
            {
                _selectedButton = potentialButtonObject.GetComponent<Button>();
                _selectedButton.Select();
                MoveCursorToButton(potentialButtonObject);
                
                break;
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e.Message);
            }
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

    public void Delete()
    {
        Destroy(_hand);
        Destroy(this);
    }
}
