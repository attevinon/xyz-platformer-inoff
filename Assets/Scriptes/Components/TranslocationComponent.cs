using System.Collections;
using UnityEngine;

public class TranslocationComponent : MonoBehaviour
{
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private float _smoothing;

    private Vector3 _startPoint;
    private Coroutine _coroutine;
    private bool _isTranslocated = false;

    void Awake()
    {
        _startPoint = this.transform.position;
    }

    public void Translocate(bool isToStartPoint)
    {
        Debug.Log("Вызван метод Translocate(bool) " + Time.time);

        StopTranslocation();

        _coroutine = StartCoroutine(
            AnimateTranslocation((isToStartPoint) ? _startPoint : _destinationPoint.position));

        Debug.Log("Корутина запущена " + Time.time);
    }

    public void Translocate()
    {
        Debug.Log("Вызван метод Translocate() " + Time.time);

        Translocate(_isTranslocated);
    }

    private IEnumerator AnimateTranslocation(Vector3 destinationPosition)
    {
        Debug.Log("Вызвали AnimateTranslocation " + Time.time);

        while(this.transform.position != destinationPosition)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, destinationPosition, _smoothing);

            yield return null;
        }

        _isTranslocated = !_isTranslocated;
        Debug.Log("Последняя строка AnimateTranslocation " + Time.time);
    }

    [ContextMenu("StopTranslocation")]
    public void StopTranslocation()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            _isTranslocated = !_isTranslocated;
            Debug.Log("Сделали корутин null " + Time.time);
        }
        else
        {
            Debug.Log("Kорутинa i tak byla null " + Time.time);
        }
    }

    [ContextMenu("Translocate")]
    public void TranslocateIn()
    {
        Translocate();
    }
}
