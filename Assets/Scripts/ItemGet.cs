using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGet : MonoBehaviour{
    [SerializeField] private ItemDisplay itemUI;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private BoxCollider pickupCollider;
    [SerializeField] private string itemName;
    [SerializeField] private string[] descriptions;
    [SerializeField] private int healAmount = 2;

    /*
    void Start(){
        GridMove.GridUpdateEvent += this.pickupCheck;
    }

    void OnDestroy(){
        GridMove.GridUpdateEvent -= this.pickupCheck;
    }
    */

    void Update(){
        pickupCheck();
    }

    void pickupCheck(){
        Collider[] collisions = Physics.OverlapBox(transform.position, pickupCollider.bounds.extents, Quaternion.identity);
        if(collisions.Length > 1){
            bool playerCollision = false;
            Collider playerObj = null;
            foreach(Collider obj in collisions){
                if(obj.CompareTag("Player")){
                    playerCollision = true;
                    playerObj = obj;
                }
            }

            if(playerCollision){
                playerObj.gameObject.GetComponent<HealthTracker>().heal(healAmount);
                int descId = (int)(Random.Range(0, descriptions.Length-1));
                itemUI.showItem(render.sprite, itemName, descriptions[descId]);
                Destroy(gameObject);
            }
        }
    }
}
