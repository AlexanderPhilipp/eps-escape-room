using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweener : MonoBehaviour
{
    [Header("Settings")] 
    public float tweenSize = 0.9f;
    public float tweenDuration = 0.15f;
    
    public void TweenButtonIn(GameObject button)
    {
        button.LeanScale(new Vector3(tweenSize, tweenSize, tweenSize), tweenDuration);
    }

    public void TweenBUttonOut(GameObject button)
    {
        button.LeanScale(Vector3.one, tweenDuration);
    }
}
