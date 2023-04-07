using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public         GamePlay    GamePlay;
    private static GameManager Instance;
    private        int         _selectedDeckLength;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGameplayScene(int cardCount)
    {
        _selectedDeckLength = cardCount;
        SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Single)
            .completed += OnGameplaySceneLoaded;
    }

    private void OnGameplaySceneLoaded(AsyncOperation obj)
    {
        if (obj.isDone)
        {
            GamePlay = FindObjectOfType<GamePlay>();
            GamePlay.StartGame(_selectedDeckLength);
            Destroy(gameObject);
        }
    }
}