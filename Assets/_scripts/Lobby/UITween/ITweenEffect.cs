using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITween
{
    public interface ITweenEffect
    {
        bool DoTween(float deltaTime);
    }
}
