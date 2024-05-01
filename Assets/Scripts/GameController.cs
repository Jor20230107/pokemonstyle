using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldcamera;
    GameState state;

    public void Start(){
        // subscribe event
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }
    void StartBattle(){
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldcamera.gameObject.SetActive(false);

        battleSystem.StartBattle();
    }
    void EndBattle(bool won){
        state = GameState.FreeRoam;
        worldcamera.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);        
    }
    private void Update(){
        if (state == GameState.FreeRoam){
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle){
            battleSystem.HandleUpdate();
        }
    }
}
