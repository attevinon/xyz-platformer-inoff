using System.Collections;
using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private float _timeToTranslocate = 1f;
    [SerializeField] private float _timeToAlpha = 1f;

    public void Teleport(GameObject objectToTeleport)
    {
        StartCoroutine(AnimateTeleport(objectToTeleport));
    }

    private IEnumerator AnimateTeleport(GameObject objectToTeleport)
    {
        var sprite = objectToTeleport.GetComponent<SpriteRenderer>();
        var input = objectToTeleport.GetComponent<HeroInputReader>();

        SetInputLock(input, true);
        var collider = objectToTeleport.GetComponent<Collider2D>();
        if (collider == null)
            StopAllCoroutines();

        yield return AnimateAlpha(sprite, 0f);
        objectToTeleport.SetActive(false);
        collider.enabled = false;

        yield return AnimateTranslocation(objectToTeleport);

        collider.enabled = true;
        objectToTeleport.SetActive(true);
        yield return AnimateAlpha(sprite, 1f);

        SetInputLock(input, false);
    }

    private IEnumerator AnimateAlpha(SpriteRenderer sprite, float destinationAlpha)
    {
        float timeHasPassed = 0f;
        var startAlpha = sprite.color.a;

        while (timeHasPassed < _timeToAlpha)
        {
            timeHasPassed += Time.deltaTime;

            float progress = timeHasPassed / _timeToAlpha;

            float tmpAlpha = Mathf.Lerp(startAlpha, destinationAlpha, progress);
            Color tmpColor = sprite.color;
            tmpColor.a = tmpAlpha;
            sprite.color = tmpColor;

            yield return null;
        }
    }
    
    private IEnumerator AnimateTranslocation(GameObject objectToTeleport)
    {
        float timeHasPassed = 0f;

        while (timeHasPassed < _timeToTranslocate)
        {
            timeHasPassed += Time.deltaTime;

            float progress = timeHasPassed / _timeToTranslocate;

            objectToTeleport.transform.position = Vector3.Lerp(objectToTeleport.transform.position, _destination.position, progress);

            yield return null;
        }
    }

    private void SetInputLock(HeroInputReader input, bool isLocked)
    {
        if(input != null)
            input.enabled = isLocked;
    }
}
