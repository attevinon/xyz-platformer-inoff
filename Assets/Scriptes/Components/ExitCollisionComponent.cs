using UnityEngine;

public class ExitCollisionComponent : MonoBehaviour
{
    [SerializeField] private string[] _tags;
    [SerializeField] private EnterEvent _action;

    private void OnCollisionExit2D(Collision2D collision)
    {
        foreach (var tag in _tags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                _action?.Invoke(collision.gameObject);
                return;
            }
        }
    }
}
