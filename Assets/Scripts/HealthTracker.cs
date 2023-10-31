using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTracker : MonoBehaviour{
    [SerializeField] private int HP = 3;
    [SerializeField] private HealthBar bar;

    private int maxHP = 3;
    
    void Start(){
        maxHP = HP;
        bar.setValue(HP);
        showHealth();
        PlayerDeathHandler.LevelResetEvent += this.resetSelf;
    }

    public void damage(){
        HP -= 1;
        bar.setValue(HP);
        gameObject.GetComponent<DeathHandler>().Hurt();

        if(HP <= 0){
            gameObject.GetComponent<DeathHandler>().Die();
        }
    }

    public void heal(int amount){
        HP = Mathf.Min(HP+amount, maxHP);
        bar.setValue(HP);
    }

    public void showHealth(){
        bar.show(maxHP);
    }

    public void hideHealth(){
        bar.hide();
    }

    void resetSelf(){
        HP = maxHP;
        bar.setValue(HP);
        showHealth();
    }
}
