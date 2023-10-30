using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCursor : MonoBehaviour{
    [SerializeField] private SpriteRenderer cursorImg;
    [SerializeField] private Sprite[] images; // move, stairs, item, enemy
    [SerializeField] private float baseCursorScale = 5f;

    Vector3 savedLookDir = Vector3.zero;
    Vector3 baseCursorOffset = new Vector3(0f, -0.015f, 0f);
    float minCursorDistance = 0.05f;

    void Start(){
        GridMove.GridUpdateEvent += this.updateCursor;
        savedLookDir = transform.forward;

        Vector3 toCursorImg = (cursorImg.gameObject.transform.position - transform.position);
        toCursorImg = new Vector3(toCursorImg.x, 0f, toCursorImg.z);
        minCursorDistance = Mathf.Max(toCursorImg.magnitude, Camera.main.nearClipPlane*1.5f);
        updateCursor();
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

        // update image
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

        // set position/scale of image (to offset double-vision effect)
        if(Physics.Raycast(transform.position, savedLookDir, out hit)){
            float castDistance = Mathf.Max(((hit.point - transform.position).magnitude * 0.5f), minCursorDistance);
            scaleByDistance(castDistance);
        }else{
            // reset position / scale
            scaleByDistance(minCursorDistance);
        }
    }

    void scaleByDistance(float distance){
        // set scale based on FOV and distance from the camera (to keep a uniform view size across distance)
        float scaleFactor = ((distance * minCursorDistance) * -Mathf.Tan(Camera.main.fieldOfView / 2f) * 1.6f * baseCursorScale);
        cursorImg.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        cursorImg.transform.position = ( transform.position + (Camera.main.transform.forward * distance) + (baseCursorOffset * scaleFactor) );
    }
}
