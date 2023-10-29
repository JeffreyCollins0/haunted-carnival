using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathHandler : DeathHandler{
    [SerializeField] private FadeControl fade;
    [SerializeField] private Transform startPoint;

    public static UnityAction LevelResetEvent;

    protected override void immediateOnDeath(){
        fade.fadeOut();
    }

    protected override void delayedOnDeath(){
        gameObject.transform.position = startPoint.position;
        LevelResetEvent();
        fade.fadeIn();
    }

    public override void Hurt(){}
}
