using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedMatchBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        Debug.Log("============ Entered Finished Match State ============");
        BattleLoader.GetInstance().Clear();
        SceneManager.LoadScene("GameScene");
    }
}
