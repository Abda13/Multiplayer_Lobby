using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITween.Internal
{
    public class TweenManager : MonoBehaviour
    {
        public static TweenManager instance { get; private set; }


        List<ITweenEffect> Effects = new List<ITweenEffect>();


        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


        internal void Move(RectTransform rectTransform, Vector3 targetPosition, float duration, Space space, bool isSlerp)
        {
            Effects.Add(new TranslateEffect(rectTransform, targetPosition, duration,Space.Self, isSlerp));
        }

        void Update()
        {
            foreach (var effect in Effects)
            {
                if(effect.DoTween(Time.deltaTime))
                {
                    Effects.Remove(effect);
                    break;
                }
            }
        }

      
    }

    public class DoEffect
    {
        public float duration;

    }
}
