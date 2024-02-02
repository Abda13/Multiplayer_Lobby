using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITween.Internal
{
    public class PopEffect : ITweenEffect
    {
        RectTransform transform;
        float duration;
        float timeElapsed;
        Vector3 originalScale;
        Vector3 targetScale;

        public PopEffect(RectTransform transform, Vector3 targetScale, float duration)
        {
            this.transform = transform;
            this.duration = duration;
            this.targetScale = targetScale;
            originalScale = transform.localScale;
        }

        public bool DoTween(float deltaTime)
        {
            if (timeElapsed < duration)
            {
                float midDuration = duration / 2f;
                Vector3 scale;

                if (timeElapsed <= midDuration)
                {
                    // Scale up
                    scale = Vector3.Lerp(originalScale, targetScale, timeElapsed / midDuration);
                }
                else
                {
                    // Scale down
                    scale = Vector3.Lerp(targetScale, originalScale, (timeElapsed - midDuration) / midDuration);
                }

                transform.localScale = scale;
                timeElapsed += deltaTime;
                return false;
            }
            else
            {
                transform.localScale = originalScale;
                return true;
            }
        }
    }
}
