using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITween.Internal
{
    public class ScaleEffect : ITweenEffect
    {
        RectTransform transform;
        Vector3 targetScale;
        float duration;
        Vector3 initialScale;
        float timeElapsed;
        public ScaleEffect(RectTransform transform, Vector3 targetScale, float duration)
        {
            this.transform = transform;
            this.targetScale = targetScale;
            this.duration = duration;
            initialScale = transform.localScale;
        }
        public bool DoTween(float deltaTime)
        {
            if (timeElapsed < duration)
            {
                if (!transform.gameObject.activeInHierarchy) return true;

                Vector3 newScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);

                timeElapsed += deltaTime;

                transform.localScale = newScale;
                return false;
            }
            else
            {
                transform.localScale = targetScale;
                return true;
            }
        }
    }
}
