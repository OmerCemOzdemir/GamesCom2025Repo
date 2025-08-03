using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Background Music")] 
    public AudioClip mainMenuMusic;
    public AudioClip clickerMusic;
    public AudioClip mainHubMusic;
    public AudioClip platformerMusic;

    [Header("Sound Effects - General")]
    public AudioClip menuButtonClick;
    public AudioClip gameOver;

    [Header("Sound Effects - Platformer")]
    public AudioClip buttonMoneyClick;
    public AudioClip passiveSale;
    public AudioClip criticalSale;
    public AudioClip upgradeButtonClick;
    public AudioClip upgradeButtonSuccess;
    public AudioClip upgradeButtonFail;

    [Header("Sound Effects - Platformer")]
    public AudioClip playerWalk;
    public AudioClip playerJump;
    public AudioClip playerSprint;
    public AudioClip interactWithObject;
    public AudioClip npcInteract;

    private Dictionary<string, AudioClip> sfxClips;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Build SFX dictionary
        sfxClips = new Dictionary<string, AudioClip>
        {
            // General
            { "menuClick", menuButtonClick },
            { "gameOver", gameOver },

            // Clicker / Cookie Game
            { "buttonMoney", buttonMoneyClick },
            { "passiveSale", passiveSale },
            { "criticalSale", criticalSale },
            { "upgradeClick", upgradeButtonClick },
            { "upgradeSuccess", upgradeButtonSuccess },
            { "upgradeFail", upgradeButtonFail },

            // Platformer
            { "walk", playerWalk },
            { "jump", playerJump },
            { "sprint", playerSprint },
            { "interact", interactWithObject },
            { "npcInteract", npcInteract }
        };
    }

    private void OnEnable()
    {
        // Subscribe to player action events
        PlayerControler.onPlayerJump += HandlePlayerJump;
        PlayerControler.onPlayerMove += HandlePlayerMove;
        PlayerControler.onPlayerClimb += HandlePlayerClimb;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        PlayerControler.onPlayerJump -= HandlePlayerJump;
        PlayerControler.onPlayerMove -= HandlePlayerMove;
        PlayerControler.onPlayerClimb -= HandlePlayerClimb;
    }

    void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (bgmSource == null) return;

        AudioClip bgmPlaying;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0)
            bgmPlaying = mainMenuMusic;
        else if (sceneIndex == 1)
            bgmPlaying = clickerMusic;
        else if (sceneIndex == 2)
            bgmPlaying = mainHubMusic;
        else if (sceneIndex >= 3 && sceneIndex <= 7)
            bgmPlaying = platformerMusic;
        else
            bgmPlaying = mainHubMusic;

        if (bgmSource.clip == bgmPlaying) return; // prevent replaying same track

        bgmSource.clip = bgmPlaying;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(string key)
    {
        if (sfxClips.ContainsKey(key) && sfxClips[key] != null)
        {
            sfxSource.PlayOneShot(sfxClips[key]);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        bgmSource.Stop();
    }
    
    /* public void ReloadScene()
    {
        Destroy(AudioManager.Instance.gameObject); // Destroy before reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } */

    // Event Handlers
    private void HandlePlayerJump() => PlaySFX("jump");
    private void HandlePlayerMove() => PlaySFX("walk");
    private void HandlePlayerClimb() => PlaySFX("sprint");
}
