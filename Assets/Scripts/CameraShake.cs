using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField] private Transform cameraTransform;

    private Vector3 originalPos;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        Instance = this;

        if (cameraTransform == null)
            cameraTransform = transform;

        originalPos = cameraTransform.localPosition;
    }

    public void Shake(float duration, float strength)
    {
        Debug.Log("Camera Shake!");
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    private IEnumerator ShakeRoutine(float duration, float strength)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float currentStrength = Mathf.Lerp(strength, 0f, elapsed / duration);

            Vector3 offset = Random.insideUnitSphere * currentStrength;
            offset.z = 0f;

            cameraTransform.localPosition = originalPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
}