using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Text gameOverText;
    [SerializeField] private GameObject pauseIcon;
    [SerializeField] private Image medalImage; // Image component to display the medal
    [SerializeField] private Sprite bronzeMedal; // Medal images for Bronze, Silver, and Gold
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private AudioSource backgroundMusic; // AudioSource for background music
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip gameOverSound;

    private int score;
    public int Score => score;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(gameObject);
            Pause();
            pauseIcon.SetActive(false);
        }
    }

    private void Update()
    {
        if (IsGameActive() && Input.GetKeyDown(KeyCode.P))
        {
            TogglePause(); // Toggle the pause state
        }
    }
    private bool IsGamePaused()
    {
        // Check if the game is currently paused
        return Time.timeScale == 0f;
    }

    private bool IsGameActive()
    {
        // Check if the game is currently active (not in game over state)
        return !gameOver.activeSelf;
    }
    private void TogglePause()
    {
        isPaused = !isPaused; // Toggle the paused state

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    

        // Pause background music
        backgroundMusic.Pause();

        // Play pause sound effect
        if (pauseSound != null)
        {
            AudioSource.PlayClipAtPoint(pauseSound, Camera.main.transform.position);
        }

        // Show pause icon
        pauseIcon.gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        player.enabled = true;

        // Resume background music
        backgroundMusic.UnPause();

        // Hide pause icon
        pauseIcon.gameObject.SetActive(false);
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();
        medalImage.gameObject.SetActive(false); // Hide the medal image at the beginning

        playButton.SetActive(false);
        gameOver.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

        // Hide the medal image at the beginning
        medalImage.gameObject.SetActive(false);

        // Play background music when the game starts
        backgroundMusic.Play();
    }

    public void GameOver()
    {
        playButton.SetActive(true);
        gameOver.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        // Play game over sound
        if (gameOverSound != null)
        {
            AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position);
        }

        // Stop background music when the game is over
        backgroundMusic.Stop();

        // Display medal based on the player's score
        DisplayMedal();

        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;

    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();

    }

    private void DisplayMedal()
    {
        medalImage.gameObject.SetActive(true); // Show the medal image

        if (score <= 5)
        {
            medalImage.sprite = bronzeMedal; // Award Bronze medal for scores 1-5
        }
        else if (score <= 10)
        {
            medalImage.sprite = silverMedal; // Award Silver medal for scores 6-10
        }
        else
        {
            medalImage.sprite = goldMedal; // Award Gold medal for scores >10
        }
    }

}
