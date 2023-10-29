using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridMove : MonoBehaviour{
    [SerializeField] public static float gridSize = 1f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private BoxCollider checkCollider;
    [SerializeField] private GameObject moveArrow;
    [SerializeField] public static float lookAngleUpdateThreshold = 2f;
    [SerializeField] private bool usingDebugControls;
    [SerializeField] private Transform mainCamera;

    private static Vector3[] gridDirs;
    private static Vector3 targetPos;
    private bool moving = false;
    private Vector3 savedLookDir = Vector3.forward;
    private int steps = 0;

    // -- debug control variables --
    private Vector3 debugLookDir = Vector3.forward;

    public static UnityAction GridUpdateEvent;
    public static UnityAction LevelUpdateEvent;

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

        GridUpdateEvent += this.countStep;
    }

    void Update(){
        // -- debug controls for testing --
        float debugTurnFactor = 0f;
        if(Input.GetKey("a")){
            debugTurnFactor -= (moveSpeed * 1.5f) * Mathf.Deg2Rad;
        }
        if(Input.GetKey("d")){
            debugTurnFactor += (moveSpeed * 1.5f) * Mathf.Deg2Rad;
        }

        Vector3 camDir = Camera.main.transform.forward;
        if(usingDebugControls && debugTurnFactor != 0f){
            debugLookDir = Quaternion.Euler(0f, debugTurnFactor, 0f) * debugLookDir;
            camDir = debugLookDir;
            Quaternion newRot = new Quaternion();
            newRot.SetLookRotation(debugLookDir);
            mainCamera.rotation = newRot;
        }

        if(moving){
             transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed*Time.deltaTime);
             if(Vector3.Distance(transform.position, targetPos) <= 0.01f){
                transform.position = targetPos;
                moving = false;
                GridUpdateEvent();
             }
        }else{
            if(moveArrow.activeSelf){
                moveArrow.SetActive(true);
            }

            if(Vector3.Angle(camDir, savedLookDir) >= (Mathf.Deg2Rad * lookAngleUpdateThreshold)){
                savedLookDir = camDir;

                // update move arrow direction
                Vector3 resultMoveDir = closestGridDir(savedLookDir);
                Quaternion newArrowRot = new Quaternion();
                newArrowRot.SetLookRotation(resultMoveDir);
                moveArrow.transform.rotation = newArrowRot;
                RaycastHit collision2 = new RaycastHit();
                if(!Physics.BoxCast(transform.position, checkCollider.bounds.extents, resultMoveDir, out collision2, Quaternion.identity, gridSize)){
                    moveArrow.SetActive(true);
                }else{
                    moveArrow.SetActive(false);
                }
            }

            if(Input.GetMouseButtonDown(0)){
                // cast camera vector to ground
                Vector3 moveDir = closestGridDir(camDir);

                // collision detection
                RaycastHit collision;
                RaycastHit collision2;
                bool moveCheck = Physics.BoxCast(transform.position, checkCollider.bounds.extents, moveDir, out collision, Quaternion.identity, gridSize);
                bool floorCheck = Physics.BoxCast(transform.position+moveDir, checkCollider.bounds.extents, Vector3.down, out collision2, Quaternion.identity, gridSize/2f);
                if(moveCheck && !collision.collider.CompareTag("Item")){
                    // check for stairs
                    if(collision.collider.CompareTag("Stairs")){
                        Transform endpoint = collision.collider.gameObject.GetComponent<StairTransition>().getEndpoint();
                        transform.position = endpoint.position;
                        LevelUpdateEvent();
                    }

                    // check for enemy
                    if(collision.collider.CompareTag("Enemy")){
                        collision.collider.gameObject.GetComponent<HealthTracker>().damage();
                    }

                    GridUpdateEvent();
                }else if(floorCheck){
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

    private void countStep(){
        steps += 1;
    }
}
