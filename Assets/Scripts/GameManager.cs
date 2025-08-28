using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region General
    public Transform Player;
    public GameObject[] Obstacles;
    public int ObstaclesCountAtSameTime = 3;
    public float StartingSpawnDistance = 10f;
    public float ObstaclesSeparation = 5f;
    public float GameInactiveDuration = 3f;
    #endregion
    #region UI
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestScoreText;
    public Fishy Fishy;
    #endregion
    #region Reset
    public TextMeshProUGUI CountdownText;
    #endregion
    #region MainMenu
    public InputActionReference GoToMenuAction;
    #endregion
    
    #region General
    private Queue<GameObject> _obstacles = new ();
    private float _lastZSpawnPosition;
    #endregion
    #region UI
    private int _score;
    private int _bestScore;
    #endregion
    #region Reset
    private float _gameStartTime = float.MaxValue;
    private float _countdownTime = 0f;
    #endregion
    #region NotReset
    private void Start()
    {
        Fishy.SetGameManager(this);
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
        #region MainMenu
        Reset();
        #endregion
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        #region General
        if (_obstacles.Count < ObstaclesCountAtSameTime)
        {
            SpawnObstacle();
        }
        #endregion
        #region NotReset
        // Remove obstacles that are too far behind the player
        if (Player.position.z > _obstacles.Peek().transform.position.z + 1)
        {
            #region General
            GameObject obstacle = _obstacles.Dequeue();
            Destroy(obstacle);
            #endregion
            _score++;
            ScoreText.text = "Puntaje: " + _score;
        }
        #endregion
        #region Reset
        //update countdown
        if (_countdownTime > 0)
        {
            CountdownText.text = Mathf.CeilToInt(_countdownTime).ToString();
            _countdownTime -= Time.deltaTime;
            if (_countdownTime <= 0)
            {
                CountdownText.text = "";
            }
        }
        #endregion
        #region MainMenu
        if (GoToMenuAction.action.triggered)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        #endregion
    }
    
    #region General
    private void SpawnObstacle()
    {
        GameObject obstaclePrefab = Obstacles[Random.Range(0, Obstacles.Length)];
        Vector3 spawnPosition = new Vector3(0, 0, _lastZSpawnPosition + ObstaclesSeparation);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        _obstacles.Enqueue(obstacle);
        _lastZSpawnPosition = spawnPosition.z;
    }
    #endregion

    public void OnPlayerDeath()
    {
        #region UI
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("BestScore", _bestScore);
            PlayerPrefs.Save();
        }
        #endregion
        #region Reset
        Reset();
        #endregion
    }
    #region Reset
    private void Reset()
    {
        // Reset player
        Fishy.Reset();
        
        // Clear obstacles
        while (_obstacles.Count > 0)
        {
            GameObject obstacle = _obstacles.Dequeue();
            Destroy(obstacle);
        }
        
        // Reset spawn position and score
        _lastZSpawnPosition = transform.position.z + StartingSpawnDistance;
        _score = 0;
        ScoreText.text = "Puntaje: " + _score;
        BestScoreText.text = "Mejor Puntaje: " + _bestScore;
        _gameStartTime = Time.time + GameInactiveDuration;
        _countdownTime = GameInactiveDuration;
    }
    public bool IsGameRunning()
    {
        return Time.time >= _gameStartTime;
    }
    #endregion
}
