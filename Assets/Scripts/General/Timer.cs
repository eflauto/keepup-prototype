using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public UnityEvent onTimerEnd;
    
    private TextMeshProUGUI _timerText;
    private bool _isTimerRunning;
    private float _currentTime;
    
    private void Start()
    {
        _timerText = GameObject.Find("UI/txt_Timer").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!_isTimerRunning) return;
        
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
        {
            _isTimerRunning = false;
            _currentTime = 0;
            onTimerEnd.Invoke();

            return;
        }
        
        float secondsRemaining = Mathf.FloorToInt(_currentTime % 60);
        
        _timerText.text = secondsRemaining.ToString("00");
    }

    public void StartTimer(int seconds)
    {
        _currentTime = seconds;
        _isTimerRunning = true;
    }

    public void StopTimer(bool reset = false)
    {
        _isTimerRunning = false;
        
        if (reset) _currentTime = 0;
    }

    public float GetCurrentTime()
    {
        return Mathf.FloorToInt(_currentTime % 60);
    }
}
