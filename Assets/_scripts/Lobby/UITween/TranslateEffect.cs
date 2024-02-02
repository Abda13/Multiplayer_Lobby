using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITween.Internal
{
    public class TranslateEffect : ITweenEffect
    {
        RectTransform transform;
        Vector3 target;
        float duration;
        Space space;
        Vector3 initialPos;
        float timeElapsed;
        bool isSlerp;

        public TranslateEffect(RectTransform transform,Vector3 target, float duration, Space space, bool isSlerp)
        {
            this.transform = transform;
            this.target = target;
            this.duration = duration;

            this.space = space;
            initialPos = space == Space.Self ? transform.localPosition : transform.position;

            this.isSlerp = isSlerp;
        }


        public bool DoTween(float deltaTime)
        {
            if (timeElapsed < duration)
            {
                if (!transform.gameObject.activeInHierarchy) return true;

                Vector3 pos;

                pos = isSlerp ? Vector3.Slerp(initialPos, target, timeElapsed / duration) : Vector3.Lerp(initialPos, target, timeElapsed / duration);

                timeElapsed += deltaTime;

                if (space == Space.Self)
                {
                    transform.localPosition = pos;
                }
                else
                {
                    transform.position = pos;
                }
                return false;
            }
            else
            {
                if (space == Space.Self)
                {
                    transform.localPosition = target;
                }
                else
                {
                    transform.position = target;
                }
                return true;
            }
        }
    }
}
