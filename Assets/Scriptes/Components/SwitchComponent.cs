using UnityEngine;

public class SwitchComponent : MonoBehaviour
{
    [SerializeField] private string _animationKey;
    
    //пока что не нужны, но может понядобятся
    //[SerializeField] private UnityEvent _onActivate;
    //[SerializeField] private UnityEvent _onDeactivate;

    private bool _isActive;
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Switch()
    {
        _isActive = !_isActive;
        _animator.SetBool(_animationKey, _isActive);
    }

    public void Activate()
    {
        if (_isActive)
            return;

        //_onActivate?.Invoke();
        Switch();
    }

    public void Deactivate()
    {
        if (!_isActive)
            return;

        //_onDeactivate?.Invoke();
        Switch();

    }

    [ContextMenu("Switch")]
    public void SwitchIn()
    {
        Switch();
    }
}
