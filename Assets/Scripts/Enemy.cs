using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour{
    [SerializeField] private BoxCollider check;
    [SerializeField] private int turnsPerAttack = 2;

    DeathHandler handler;

    static Vector3[] checkDirections = {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };
    public static UnityAction EnemyHurtEvent;

    int turnsTaken = 0;

    void Start(){
        GridMove.GridUpdateEvent += this.takeAction;
        PlayerDeathHandler.LevelResetEvent += this.resetSelf;
        gameObject.GetComponent<HealthTracker>().hideHealth();
        handler = gameObject.GetComponent<DeathHandler>();
    }

    void OnDestroy(){
        GridMove.GridUpdateEvent -= this.takeAction;
        PlayerDeathHandler.LevelResetEvent -= this.resetSelf;
    }

    void resetSelf(){
        check.enabled = true;
        turnsTaken = 0;
    }

    private void takeAction(){
        if(!handler.isDead()){

        bool playerTest = false;
        foreach(Vector3 dir in checkDirections){
            RaycastHit collisionCheck = new RaycastHit();
            if(Physics.BoxCast(transform.position, check.bounds.extents, dir, out collisionCheck, Quaternion.identity, GridMove.gridSize)){
                if(collisionCheck.collider.CompareTag("Player")){
                    if(turnsTaken < turnsPerAttack){
                        turnsTaken += 1;
                    }else{
                        turnsTaken = 0;
                        collisionCheck.collider.GetComponent<HealthTracker>().damage();
                    }
                }

                // show the healthbar
                gameObject.GetComponent<HealthTracker>().showHealth();
                playerTest = true;
            }
        }

        }
    }
}
