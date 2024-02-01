using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UITween.Internal;


namespace UITween
{
    public static class Core 
    {
        public static void LocalMove(this RectTransform rectTransform,Vector3 targetPosition, float duration)
        {
            TweenManager.Move(rectTransform, targetPosition, duration,Space.Self,false);
        }
        public static void LocalSphericalMove(this RectTransform rectTransform, Vector3 targetPosition, float duration)
        {
            TweenManager.Move(rectTransform, targetPosition, duration, Space.Self, true);
        }
        public static void Move(this RectTransform rectTransform, Vector3 targetPosition, float duration)
        {
            TweenManager.Move(rectTransform, targetPosition, duration, Space.World, false);
        }
        public static void SphericalMove(this RectTransform rectTransform, Vector3 targetPosition, float duration)
        {
            TweenManager.Move(rectTransform, targetPosition, duration, Space.World, true);
        }

        public static void LocalRotate(this RectTransform rectTransform, Vector3 targetRotation, float duration)
        {
           
        }
        public static void Rotate(this RectTransform rectTransform, Vector3 targetRotation, float duration)
        {
            
        }

        public static void Scale(this RectTransform rectTransform, Vector3 targetPosition, float duration)
        {
           
        }


        static TweenManager TweenManager
        {
            get
            {
                if (TweenManager.instance == null)
                {
                    GameObject gameObject = new GameObject("TweenManager");
                    gameObject.AddComponent<TweenManager>();
                }
                return TweenManager.instance;
            }
        }

    }
}
