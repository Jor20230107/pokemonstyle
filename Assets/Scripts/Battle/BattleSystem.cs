using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currrentAction;
    int currentMove;
    public void StartBattle(){
        StartCoroutine(SetupBattle());
 
    }

    public IEnumerator SetupBattle(){
        playerUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyUnit.Setup();
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
        

        PlayerAction();
    }

    void PlayerAction(){
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove(){
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        
        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayerHitAnimation();

        var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted){
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} fainted");
            enemyUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(1f);
            OnBattleOver(true);
        }
        else{
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails){
        if (damageDetails.Critical > 1f){
            yield return dialogBox.TypeDialog("A critical hit!");
        }

        if (damageDetails.TypeEffectiveness > 1f){
            yield return dialogBox.TypeDialog("It's super effective!");
        }
        else if (damageDetails.TypeEffectiveness < 1f){
            yield return dialogBox.TypeDialog("It's not very effective");
        }
    }

    IEnumerator EnemyMove(){
        state = BattleState.EnemyMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");
        
        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerUnit.PlayerHitAnimation();

        var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted){
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} fainted");
            playerUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(1f);
            OnBattleOver(false);
        }
        else{
            PlayerAction();
        }

    }

    public void HandleUpdate() {
        if (state == BattleState.PlayerAction){
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove){
            HandleMoveSelection();
        }
    }

    void HandleActionSelection(){
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            if (currrentAction<1){
                ++currrentAction;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (currrentAction>0){
                --currrentAction;
            }
        }

        dialogBox.UpdateActionSelection(currrentAction);

        if (Input.GetKeyDown(KeyCode.Z)){
            if (currrentAction == 0){
                // Fight
                PlayerMove();
            }
            else if (currrentAction==1){
            // Run
        }
        }
        
    }

    void HandleMoveSelection(){
        
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (currentMove < playerUnit.Pokemon.Moves.Count-1){
                ++currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (currentMove > 0){
                --currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){
            if (currentMove < playerUnit.Pokemon.Moves.Count-2){
                currentMove += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (currentMove > 1){
                currentMove -= 2;
            }
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z)){
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }
}
