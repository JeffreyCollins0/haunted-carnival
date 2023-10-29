using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecretRock : MonoBehaviour{
    [SerializeField] private TMP_Text lookText;
    private float lookTime = 0f;

    void Update(){
        // check if the player is looking at us
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, GridMove.gridSize*1.25f)){
            if(hit.collider.CompareTag("SecretRock")){
                // count time
                lookTime += Time.deltaTime;
                lookText.SetText("You have looked at the secret rock for:\n"+timeFormat(lookTime));
            }
        }
    }

    string timeFormat(float time){
        int rawSeconds = (int)Mathf.Floor(time);
        int mins = (int)Mathf.Floor(rawSeconds / 60.0f);
        int secs = rawSeconds - (60 * mins);
        return (padNumber(mins, 2)+":"+padNumber(secs, 2));
    }

    string padNumber(int number, int spaces){
        string numText = number.ToString();
        while(numText.Length < spaces){
            numText = "0"+numText;
        }
        return numText;
    }
}
