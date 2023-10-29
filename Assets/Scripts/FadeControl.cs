using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeControl : MonoBehaviour{
    [SerializeField] private SpriteRenderer fadeScreen;
    [SerializeField] private float fadeDuration = 1.2f;

    bool fadingOut = false;
    float alpha = 0f;
    float fadeTime = 0f;
    float alphaTarget = 0f;

    void Update(){
        if(fadeTime > 0){
            fadeTime -= Time.deltaTime;

            if(fadingOut){
                alpha = (1 - (fadeTime / fadeDuration));
                alpha = Mathf.Min(alpha, alphaTarget);
            }else{
                alpha = (fadeTime / fadeDuration);
                alpha = Mathf.Max(alpha, alphaTarget);
            }
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
        }
    }

    public void fadeIn(){
        fadingOut = false;
        fadeTime = fadeDuration;
        alphaTarget = 0f;
    }

    public void fadeOut(){
        fadingOut = true;
        fadeTime = fadeDuration;
        alphaTarget = 1f;
    }

    public void fadeTo(float alpha){
        if(this.alpha > alpha){
            fadingOut = false;
        }else{
            fadingOut = true;
        }
        fadeTime = fadeDuration;
        alphaTarget = alpha;
    }
}
