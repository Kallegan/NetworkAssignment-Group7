using UnityEngine;

public class FinishRoundGameState : GameState
{
    public override void Update()
    {
    }

    public override void Run()
    {
       WorldManager.Instance.ResetHexGrid();
    }
}
