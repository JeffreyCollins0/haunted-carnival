using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
    [SerializeField] private GameObject[] levels;

    private int levelId = 0;

    void Start(){
        GridMove.LevelUpdateEvent += this.nextLevel;
        PlayerDeathHandler.LevelResetEvent += this.resetLevel;
    }

    void nextLevel(){
        levelId += 1;
        if(levelId >= levels.Length){
            resetLevel();
            return;
        }
        enableDisable();
    }

    void resetLevel(){
        levelId = 0;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        enableDisable();
    }

    void enableDisable(){
        for(int id=0; id<levels.Length; id++){
            if(id == levelId){
                levels[id].SetActive(true);
            }else{
                levels[id].SetActive(false);
            }
        }
    }
}
