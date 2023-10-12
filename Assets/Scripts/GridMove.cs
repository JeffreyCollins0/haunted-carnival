using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour{
    [SerializeField] private float gridSize = 2f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private BoxCollider checkCollider;
    [SerializeField] private GameObject moveArrow;
    [SerializeField] private float lookAngleUpdateThreshold = 2f;
    [SerializeField] private bool usingDebugControls;
    [SerializeField] private Transform mainCamera;

    private static Vector3[] gridDirs;
    private static Vector3 targetPos;
    private bool moving = false;
    private Vector3 savedLookDir = Vector3.forward;

    // -- debug control variables --
    private Vector3 debugLookDir = Vector3.forward;

    //public static UnityAction GridUpdateEvent;

    void Start(){
        gridDirs = new Vector3[4]{
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };
        targetPos = Vector3.zero;
        savedLookDir = transform.forward;
        debugLookDir = transform.forward;
    }

    void Update(){
        // -- debug controls for testing --
        float debugTurnFactor = 0f;
        if(Input.GetKey("a")){
            debugTurnFactor -= (moveSpeed * 1.5f) * Mathf.Deg2Rad;
            //print("rotating left");
        }
        if(Input.GetKey("d")){
            debugTurnFactor += (moveSpeed * 1.5f) * Mathf.Deg2Rad;
            //print("rotating right");
        }

        Vector3 camDir = Camera.main.transform.forward;
        if(usingDebugControls && debugTurnFactor != 0f){
            debugLookDir = Quaternion.Euler(0f, debugTurnFactor, 0f) * debugLookDir;
            camDir = debugLookDir;
            //Debug.Log("Updated to "+camDir);
            Quaternion newRot = new Quaternion();
            newRot.SetLookRotation(debugLookDir);
            mainCamera.rotation = newRot;
        }

        if(moving){
             transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed*Time.deltaTime);
             if(Vector3.Distance(transform.position, targetPos) <= 0.01f){
                transform.position = targetPos;
                moving = false;
             }
        }else{
            if(moveArrow.activeSelf){
                moveArrow.SetActive(true);
            }

            //if(Vector3.Angle(camDir, savedLookDir) >= (Mathf.Deg2Rad * lookAngleUpdateThreshold)){
                savedLookDir = camDir;

                // update move arrow direction
                Vector3 resultMoveDir = closestGridDir(savedLookDir);
                //moveArrow.transform.rotation.SetLookRotation(resultMoveDir);
                Quaternion newArrowRot = new Quaternion();
                newArrowRot.SetLookRotation(resultMoveDir);
                moveArrow.transform.rotation = newArrowRot;
                RaycastHit collision2 = new RaycastHit();
                if(!Physics.BoxCast(transform.position, checkCollider.bounds.extents, resultMoveDir, out collision2, Quaternion.identity, gridSize)){
                    moveArrow.SetActive(true);
                }else{
                    moveArrow.SetActive(false);
                }
            //}

            if(Input.GetMouseButtonDown(0)){
                // cast camera vector to ground
                Vector3 moveDir = closestGridDir(camDir);

                // collision detection
                RaycastHit collision = new RaycastHit();
                if(Physics.BoxCast(transform.position, checkCollider.bounds.extents, moveDir, out collision, Quaternion.identity, gridSize)){
                    // check for stairs
                    if(collision.collider.CompareTag("Stairs")){
                        //Debug.Log("found the stairs");
                        Transform endpoint = collision.collider.gameObject.GetComponent<StairTransition>().getEndpoint();
                        transform.position = endpoint.position;
                    }
                }else{
                    // move
                    targetPos = transform.position + (moveDir * gridSize);
                    moving = true;
                    moveArrow.SetActive(false);
                }
            }
        }
    }

    public static Vector3 closestGridDir(Vector3 direction){
        Vector3 castedDir = new Vector3(direction.x, 0f, direction.z);

        float minAngleDiff = Mathf.Infinity;
        Vector3 closestDir = Vector3.zero;

        foreach(Vector3 gridDir in gridDirs){
            float angleDiff = Vector3.Angle(castedDir, gridDir);
            if(angleDiff < minAngleDiff){
                minAngleDiff = angleDiff;
                closestDir = gridDir;
            }
        }
        return closestDir;
    }
}
