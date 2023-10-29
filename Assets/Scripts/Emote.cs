using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote : MonoBehaviour{
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hurtSprite;

    private float emoteDuration = -1f;
    private bool permanent = false;

    void Update(){
        if(!permanent && emoteDuration > 0f){
            emoteDuration -= Time.deltaTime;

            if(emoteDuration <= 0){
                render.sprite = normalSprite;
            }
        }
    }

    public void Hurt(float duration, bool permanent){
        emoteDuration = duration;
        this.permanent = permanent;
        render.sprite = hurtSprite;
    }
}
