using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playersLife = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI LifeText;
    [SerializeField] TextMeshProUGUI ScoreText;
    void Awake()
    {
        //create our Singleton
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // 플레이어가 죽었을 때 생명하나 깎기/ 생명이 없으면 전체 게임 리스타트
    void Start()
    {
        LifeText.text = playersLife.ToString();
        ScoreText.text = score.ToString();
    }
    public void ProcessPlayerDeath()
    {
        if (playersLife > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }
    public void AddToScore(int pointToAdd)
    {
        score += pointToAdd;
        ScoreText.text = score.ToString();
    }
    void TakeLife()
    {
        playersLife--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        LifeText.text = playersLife.ToString();
    }
    void ResetGameSession()
    {
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }


}
