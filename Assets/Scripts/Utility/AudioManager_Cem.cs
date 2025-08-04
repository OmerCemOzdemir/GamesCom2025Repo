using UnityEngine;

public class AudioManager_Cem : MonoBehaviour
{
    private GameObject musicContainer;
    private GameObject SFXContainer;

    private AudioSource mainMenuMusicSource;
    private AudioSource clickerMusicSource;
    private AudioSource platformMusicSource;

    private AudioSource activeSFXSource;
    private AudioSource idleSFXSource;
    private AudioSource genericSFXSource;
    private AudioSource outOfMoneySFXSource;
    private AudioSource itemPickUpSFXSource;
    private AudioSource itemPurchaseSFXSource;


    private void Awake()
    {
        musicContainer = transform.GetChild(0).GetChild(0).gameObject;
        SFXContainer = transform.GetChild(0).GetChild(1).gameObject;

        //Music Audio Source
        mainMenuMusicSource = musicContainer.transform.GetChild(0).GetComponent<AudioSource>();
        clickerMusicSource = musicContainer.transform.GetChild(1).GetComponent<AudioSource>();
        platformMusicSource = musicContainer.transform.GetChild(2).GetComponent<AudioSource>();

        //SFX Audio Source
        activeSFXSource = SFXContainer.transform.GetChild(0).GetComponent<AudioSource>();
        idleSFXSource = SFXContainer.transform.GetChild(1).GetComponent<AudioSource>();
        genericSFXSource = SFXContainer.transform.GetChild(2).GetComponent<AudioSource>();
        outOfMoneySFXSource = SFXContainer.transform.GetChild(3).GetComponent<AudioSource>();
        itemPickUpSFXSource = SFXContainer.transform.GetChild(4).GetComponent<AudioSource>();
        itemPurchaseSFXSource = SFXContainer.transform.GetChild(5).GetComponent<AudioSource>();

    }

    private void Start()
    {
        Debug.Log("Testing audio play...");
        PlayAudio(mainMenuMusicSource); // Force play
    }

    // Disabled to prevent namespace conflict with new AudioManager script
    /* private void OnEnable()
    {
        MainMenu.onPlayMusic += SetUpMusic;
        MainMenu.onPlaySFX += SetUpSFX;

        ClickerManager.onPlayMusic += SetUpMusic;
        ClickerManager.onPlaySFX += SetUpSFX;

        PlayerControler.onPlayMusic += SetUpMusic;
        PlayerControler.onPlaySFX += SetUpSFX;
    }

    private void OnDisable()
    {
        MainMenu.onPlayMusic -= SetUpMusic;
        MainMenu.onPlaySFX -= SetUpSFX;

        ClickerManager.onPlayMusic -= SetUpMusic;
        ClickerManager.onPlaySFX -= SetUpSFX;

        PlayerControler.onPlayMusic -= SetUpMusic;
        PlayerControler.onPlaySFX -= SetUpSFX;
    } */

    private void SetUpMusic(Music music)
    {
        switch (music)
        {
            case Music.MainMenu:
                PlayAudio(mainMenuMusicSource);
                break;
            case Music.Platformer:
                PlayAudio(platformMusicSource);
                break;
            case Music.Clicker:
                PlayAudio(clickerMusicSource);
                break;
        }
    }

    private void SetUpSFX(SFX_Cem sfx)
    {
        switch (sfx)
        {
            case SFX_Cem.Active:
                PlayAudio(activeSFXSource);
                break;
            case SFX_Cem.Idle:
                PlayAudio(idleSFXSource);
                break;
            case SFX_Cem.Generic:
                PlayAudio(genericSFXSource);
                break;
            case SFX_Cem.OutOfMoney:
                PlayAudio(outOfMoneySFXSource);
                break;
            case SFX_Cem.ItemPickUp:
                PlayAudio(itemPickUpSFXSource);
                break;
            case SFX_Cem.ItemPurchase:
                PlayAudio(itemPurchaseSFXSource);
                break;
        }
    }

    private void PlayAudio(AudioSource audio)
    {
        audio.Play();
    }


}

public enum Music
{
    MainMenu,
    Clicker,
    Platformer
}

public enum SFX_Cem
{
    Active,
    Idle,
    Generic,
    OutOfMoney,
    ItemPickUp,
    ItemPurchase
}
