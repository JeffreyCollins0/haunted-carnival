using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathHandler : DeathHandler{
    [SerializeField] private Rigidbody ragdoll;
    [SerializeField] private Emote boardEmote;

    Vector3 ragdollInitPos = Vector3.zero;
    Quaternion ragdollInitRot = Quaternion.identity;

    protected override void immediateOnDeath(){
        ragdoll.isKinematic = false;
        boardEmote.Hurt(0.5f, true);
    }

    protected override void delayedOnDeath(){
        // disable the object
        ragdoll.gameObject.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public override void Hurt(){
        boardEmote.Hurt(0.5f, false);
    }

    void Start(){
        PlayerDeathHandler.LevelResetEvent += this.resetSelf;
        ragdollInitPos = ragdoll.transform.localPosition;
        ragdollInitRot = ragdoll.transform.rotation;
    }

    void resetSelf(){
        dead = false;
        ragdoll.gameObject.SetActive(true);
        ragdoll.isKinematic = true;
        boardEmote.Hurt(0.01f, false);
        ragdoll.transform.localPosition = ragdollInitPos;
        ragdoll.transform.rotation = ragdollInitRot;
    }
}
