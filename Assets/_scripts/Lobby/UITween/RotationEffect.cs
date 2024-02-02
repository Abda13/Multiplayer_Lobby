using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITween.Internal
{
    public class RotationEffect : ITweenEffect
    {
        RectTransform transform;
        float duration;
        float timeElapsed;
        Quaternion initialRotation;
        Quaternion targetRotation;

        public RotationEffect(RectTransform transform, Quaternion targetRotation, float duration)
        {
            this.transform = transform;
            this.duration = duration;
            this.targetRotation = targetRotation;
            initialRotation = transform.localRotation;
        }

        public bool DoTween(float deltaTime)
        {
            if (timeElapsed < duration)
            {
                transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed / duration);
                timeElapsed += deltaTime;
                return false;
            }
            else
            {
                transform.localRotation = targetRotation;
                return true;
            }
        }
    }
}
