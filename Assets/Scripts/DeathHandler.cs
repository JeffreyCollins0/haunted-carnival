using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour{
    [SerializeField] private float duration = 1.2f;
    protected bool dead = false;
    private float timeRemaining = -1f;

    void Update(){
        if(timeRemaining > 0f){
            timeRemaining -= Time.deltaTime;

            if(timeRemaining <= 0f){
                delayedOnDeath();
            }
        }
    }

    public void Die(){
        if(!dead){
            dead = true;
            timeRemaining = duration;
            immediateOnDeath();
        }
    }

    // called immediately when Die() is called
    protected virtual void immediateOnDeath(){}

    // called after the death duration elapses
    protected virtual void delayedOnDeath(){}

    public virtual void Hurt(){}

    public bool isDead(){
        return dead;
    }
}
