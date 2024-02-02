using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITween.Internal
{
    public class ShakeEffect : ITweenEffect
    {
        RectTransform transform;
        float duration;
        float timeElapsed;
        float intensity;
        Vector3 originalPosition;

        public ShakeEffect(RectTransform transform, float duration, float intensity)
        {
            this.transform = transform;
            this.duration = duration;
            this.intensity = intensity;
            originalPosition = transform.localPosition;
        }

        public bool DoTween(float deltaTime)
        {
            if (timeElapsed < duration)
            {
                float randomX = originalPosition.x + Random.Range(-intensity, intensity);
                float randomY = originalPosition.y + Random.Range(-intensity, intensity);
                transform.localPosition = new Vector3(randomX, randomY, originalPosition.z);

                timeElapsed += deltaTime;
                return false;
            }
            else
            {
                transform.localPosition = originalPosition;
                return true;
            }
        }
    }
}
