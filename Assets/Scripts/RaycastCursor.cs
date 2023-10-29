using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCursor : MonoBehaviour{
    [SerializeField] private SpriteRenderer cursorImg;
    [SerializeField] private Sprite[] images; // move, stairs, item, enemy

    Vector3 savedLookDir = Vector3.zero;

    void Start(){
        GridMove.GridUpdateEvent += this.updateCursor;
        savedLookDir = transform.forward;
    }

    void Update(){
        Vector3 lookDir = Camera.main.transform.forward;
        if(Vector3.Angle(lookDir, savedLookDir) >= (Mathf.Deg2Rad * GridMove.lookAngleUpdateThreshold)){
            savedLookDir = Vector3.Normalize(lookDir);
            updateCursor();
        }
    }

    void updateCursor(){
        // reset cursor image
        cursorImg.sprite = images[0];

        RaycastHit hit;
        if(Physics.Raycast(transform.position, savedLookDir, out hit, GridMove.gridSize)){
            if(hit.collider.CompareTag("Enemy")){
                cursorImg.sprite = images[3];
            }else if(hit.collider.CompareTag("Item")){
                cursorImg.sprite = images[2];
            }else if(hit.collider.CompareTag("Stairs")){
                cursorImg.sprite = images[1];
            }
        }
    }
}
