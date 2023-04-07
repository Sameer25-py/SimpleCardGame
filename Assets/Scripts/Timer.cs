using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float    countDownTime = 45;
    public TMP_Text CountDownText;

    private bool  _isCountDownInProgress;
    private float _elapsedTime;
    private float _remainingTime;

    public GamePlay GamePlay;

    private void DecrementTimer()
    {
        _remainingTime -= 1f;
        int seconds = Mathf.FloorToInt(_remainingTime % 60f);
        CountDownText.text = $"{0:00} : {seconds:00}";

        if (_remainingTime == 0f)
        {
            _isCountDownInProgress = false;
            GamePlay.OnTimerEnd();
        }
    }

    private void OnEnable()
    {
        _remainingTime         = countDownTime;
        CountDownText.text     = $"{0:00} : {_remainingTime:00}";
        _isCountDownInProgress = true;
    }

    private void OnDisable()
    {
        _isCountDownInProgress = false;
    }

    private void Update()
    {
        if (!_isCountDownInProgress) return;
        _elapsedTime += Time.deltaTime;
        if (!(_elapsedTime >= 1f)) return;
        _elapsedTime = 0f;
        DecrementTimer();
    }
}