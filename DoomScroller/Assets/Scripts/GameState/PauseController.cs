using UnityEngine;
 
public class PauseController : MonoBehaviour
{
    public GameState GameState;
    public GameStateManager GameStateManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused
                : GameState.Gameplay;
 
            GameStateManager.Instance.SetState(newGameState);
        }
    }
}