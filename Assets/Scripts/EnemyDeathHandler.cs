using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathHandler : DeathHandler{
    [SerializeField] private Rigidbody ragdoll;
    [SerializeField] private Emote boardEmote;

    protected override void immediateOnDeath(){
        ragdoll.isKinematic = false;
        boardEmote.Hurt(0.5f, true);
    }

    protected override void delayedOnDeath(){
        Destroy(gameObject);
    }

    public override void Hurt(){
        boardEmote.Hurt(0.5f, false);
    }
}
