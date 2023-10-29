using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDisplay : MonoBehaviour{
    [SerializeField] private FadeControl screenFade;
    [SerializeField] private SpriteRenderer itemSlot;
    [SerializeField] private TMP_Text describeText;
    [SerializeField] private float squashDuration;
    [SerializeField] private AnimationCurve squashCurve;

    [SerializeField] private Sprite testSpr;

    //private Sprite itemSprite;
    private Vector3 initItemScale;
    private float squashTime;
    private bool showing = false;

    void Start(){
        initItemScale = itemSlot.gameObject.transform.localScale;
    }

    void Update(){
        /*
        if(Input.GetMouseButtonDown(0)){
            if(!showing){
                showItem(testSpr, "Conk Bepis Uwu");
            }else{
                hideItem();
            }
        }
        */
        if(Input.GetMouseButtonDown(0)){
            if(showing){
                hideItem();
            }
        }

        if(squashTime > 0){
            squashTime -= Time.deltaTime;
            float curveSample = 0f;
            if(showing){
                curveSample = (1 - (squashTime / squashDuration));
            }else{
                curveSample = (squashTime / squashDuration);
            }
            float scaleFactor = squashCurve.Evaluate(curveSample);
            itemSlot.gameObject.transform.localScale = (initItemScale * scaleFactor);

            if(squashTime <= 0){
                itemSlot.gameObject.transform.localScale = initItemScale;
                if(scaleFactor <= 0){
                    itemSlot.gameObject.SetActive(false);
                }

                if(!showing){
                    itemSlot.gameObject.SetActive(false);
                    describeText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void showItem(Sprite item, string itemName, string description){
        showing = true;
        squashTime = squashDuration;

        describeText.SetText("You ate a "+itemName.ToUpper()+"!\n"+description);
        describeText.gameObject.SetActive(true);

        itemSlot.sprite = item;
        itemSlot.gameObject.SetActive(true);

        screenFade.fadeTo(0.5f);
    }

    public void hideItem(){
        showing = false;
        squashTime = squashDuration;
        screenFade.fadeTo(0f);
    }
}
