using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour{
    [SerializeField] private Sprite filledSection;
    [SerializeField] private Sprite emptySection;
    [SerializeField] private SpriteRenderer[] barSections;

    int displaySections = 3;
    int value = 3;

    public void setValue(int value){
        this.value = value;
        updateSections();

        if(value == 0){
            hide();
        }
    }

    public void show(int sections){
        displaySections = sections;
        gameObject.SetActive(true);
        updateSections();
    }

    public void hide(){
        foreach(SpriteRenderer render in barSections){
            render.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    void updateSections(){
        for(int segmentId=0; segmentId<barSections.Length; segmentId++){
            if(segmentId < displaySections){
                if(segmentId < value){
                    barSections[segmentId].sprite = filledSection;
                }else{
                    barSections[segmentId].sprite = emptySection;
                }
                barSections[segmentId].gameObject.SetActive(true);
            }else{
                barSections[segmentId].gameObject.SetActive(false);
            }
        }
    }
}
