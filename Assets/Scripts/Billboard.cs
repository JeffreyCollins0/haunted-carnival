using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour{
    //void Start(){}

    //private Vector3 savedLookDir = Vector3.zero;

    void Update(){
        //Vector3 newLookDir = Camera.main.transform.forward; //(Camera.main.transform.position - transform.position);
        //if(Vector3.Angle(newLookDir, savedLookDir) >= (Mathf.Deg2Rad * GridMove.lookAngleUpdateThreshold)){
            //savedLookDir = newLookDir;
        //Vector3 camDir = Camera.main.transform.forward;

        //transform.rotation = Quaternion.LookRotation(-camDir, )
            transform.LookAt(Camera.main.transform);
            Quaternion rotate = Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = (transform.rotation * rotate);
        //}
    }
}
