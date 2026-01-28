using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using TMPro;
public class DartGameManager : MonoBehaviour
{
    public Sprite cursorSprite;
    public GameObject dartObject;
    public int throwForce = 500;
    public float cursorSpeed = 500f; // Speed in pixels per second

    private InputAction _navigateAction;
    private GameObject _cursor;
    private RectTransform _cursorRect;
    private GameObject _resultsObject;
    private Timer _timer;
    private bool hasThrownDart = false;

    void Start()
    {
        _navigateAction = InputSystem.actions.FindAction("Navigate");
        _cursor = new GameObject("cursor");
        _cursor.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _resultsObject = GameObject.Find("UI/Results");
        var image = _cursor.AddComponent<Image>();
        image.sprite = cursorSprite;
        _cursorRect = _cursor.GetComponent<RectTransform>();
        _cursorRect.sizeDelta = new Vector2(50, 50);

        _timer = GetComponent<Timer>();
        _timer.StartTimer(5);
        _timer.onTimerEnd.AddListener(OnTimerEnd);
    }

    void Update()
    {

        Vector2 moveInput = _navigateAction.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            MoveCursor(moveInput);
        }

        if (InputSystem.actions.FindAction("Submit").WasPressedThisFrame() && !hasThrownDart)
    {
        ThrowDart();
        hasThrownDart = true;
    }
    }

    void MoveCursor(Vector2 direction)
    {
        _cursorRect.anchoredPosition += direction * cursorSpeed * Time.deltaTime;
        Vector2 pos = _cursorRect.anchoredPosition;
        _cursorRect.anchoredPosition = pos;
    }
    void ThrowDart()

    {
        Vector3 _screenPos = _cursorRect.position;
        _screenPos.z = 2.0f; 
        Vector3 _spawnPos = Camera.main.ScreenToWorldPoint(_screenPos);
        Vector3 _throwDirection = (_spawnPos - Camera.main.transform.position).normalized;
        GameObject _dart = Instantiate(dartObject, _spawnPos, Camera.main.transform.rotation);
        Rigidbody rb = _dart.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = _throwDirection * throwForce;
        }
    }
        private void OnTimerEnd()
    {
        _cursor.gameObject.SetActive(false);
        for (var i = 0; i < _resultsObject.transform.childCount; i++)
        {
            _resultsObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        var resultsText = GameObject.Find("UI/Results/txt_Results").GetComponent<TextMeshProUGUI>();
        var replayButton = GameObject.Find("UI/Results/btn_Replay").GetComponent<Button>();

        if (DartManager.hasHitDartBoard == true)
        {
            resultsText.text = "You Win!";
        }
        else 
        {
            resultsText.text = "You lose!";
        }
        DartManager.hasHitDartBoard = false;
        hasThrownDart = false;
        BoardManager.hasBeenHit = false;
        replayButton.Select();
    }
}