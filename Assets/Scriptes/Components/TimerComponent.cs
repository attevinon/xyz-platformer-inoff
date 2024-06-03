using UnityEngine;
using UnityEngine.Events;

public class TimerComponent : MonoBehaviour
{
    [SerializeField] private float _delayTime;
    [SerializeField] private UnityEvent _onTimeIsUp;

    private float _delayTimeRunning;

    void Start()
    {
        this.enabled = false;
    }

    void Update()
    {
        _delayTimeRunning -= Time.deltaTime;

        if(_delayTimeRunning <= 0)
        {
            _onTimeIsUp?.Invoke();
            this.enabled = false;
        }
    }

    public void ActivateTimer()
    {
        _delayTimeRunning = _delayTime;
        this.enabled = true;
    }

    public void DeactivateTimer()
    {
        this.enabled = false;
    }
}
