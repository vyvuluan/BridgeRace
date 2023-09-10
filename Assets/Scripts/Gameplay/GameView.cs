using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private GameObject popUpGameOver;
    public static GameView Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void GameOver()
    {
        popUpGameOver.SetActive(true);
    }
}
