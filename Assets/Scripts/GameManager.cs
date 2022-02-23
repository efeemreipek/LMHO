using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static int score = 0;
    public static float scoreMultiplier = 1f;
    [Header("Score")]
    public int baseScore = 10;
    public bool canScore = false;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScoreText;
    [Header("People/Spawn")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<GameObject> peoplePrefabs;
    [Space]
    public Slider deathSlider;
    [Header("Timer")]
    public TextMeshProUGUI timerText;
    [SerializeField] private int timerDuration;
    private int currentTime;
    [Space]
    public GameObject endGamePanel;
    public bool isGameOver = false;
    [Header("Restart Necessaries")]
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Vector3 playerRotation;
    [SerializeField] private GameObject player;

    private AudioSource _audioSource;

    

    private void Start()
    {
        Cursor.visible = false;

        SpawnPeople();

        scoreText.text = $"SCORE : {score}";

        currentTime = timerDuration;
        timerText.text = $"TIME LEFT : {currentTime}";

        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(LowerTime());
    }

    private void Update()
    {
        if (currentTime == -1)
        {
            StartCoroutine(EndGame());
        }
        if(isGameOver && Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }

    public IEnumerator AddScore()
    {
        while (canScore)
        {
            score += (int)(baseScore * scoreMultiplier);
            _audioSource.PlayOneShot(AudioManager.Instance.scoreEffect);

            yield return new WaitForSeconds(0.5f);
            scoreMultiplier += 0.05f;

            scoreText.text = $"SCORE : {score}";
        }
    }

    private void SpawnPeople()
    {
        int index = 0;

        for (int i = 0; i < peoplePrefabs.Count; i++)
        {
            index = Random.Range(0, spawnPoints.Count);
            Instantiate(peoplePrefabs[i], spawnPoints[index].position, Quaternion.identity);
            spawnPoints.RemoveAt(index);
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 0f;
        isGameOver = true;
        endGamePanel.SetActive(true);
        endScoreText.text = $"YOUR SCORE: {score}";
    }

    private IEnumerator LowerTime()
    {
        while(currentTime >= 0)
        {
            timerText.text = $"TIME LEFT : {currentTime}";
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
    }

    //private void RestartGame()
    //{
    //    isGameOver = false;
    //    endGamePanel.SetActive(false);
    //    Time.timeScale = 1f;

    //    //Set Player transform

    //    player.transform.position = playerPosition;
    //    player.transform.rotation = Quaternion.Euler(playerRotation);
    //    //SpawnPeople

    //    //spawnPoints = spawnPointsEmpty;
    //    for (int i = 0; i < spawnPointsReplica.Count; i++)
    //    {
    //        if(spawnPoints[i] == null)
    //            spawnPoints[i] = spawnPointsReplica[i];
    //    }
    //    Destroy(peopleParent);
    
    //    SpawnPeople();

    //    //Reset score,lives and time
    //    score = 0;
    //    currentTime = timerDuration;
    //    PlayerInteraction.deathCount = 0;
    //}

    private void QuitGame()
    {
        Application.Quit();
    }
}
