

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ------------")]
    public AudioClip background;
    public AudioClip catchperson ;
    public AudioClip gameover ;
    public AudioClip coin;
        

    [Header("---------- UI Buttons ------------")]
    public Button musicToggleButton;
    public Button sfxToggleButton;
    //public Button vibrationToggleButton;  // New button for vibration toggle

    [Header("---------- UI Button Images ------------")]
    public Sprite musicOnImage;
    public Sprite musicOffImage;
    public Sprite sfxOnImage;
    public Sprite sfxOffImage;
    // public Sprite vibrationOnImage;       // New image for vibration on
    // public Sprite vibrationOffImage;      // New image for vibration off

    private bool isMusicMuted;
    private bool isSFXMuted;
    //private bool isVibrationEnabled;

    public static AudioManager instance;

    private void Start()
    {
        LoadAudioSettings();

        musicSource.clip = background;
        if (!isMusicMuted)
        {
            musicSource.Play();
        }

        // Set up button listeners
        musicToggleButton.onClick.AddListener(ToggleMusic);
        sfxToggleButton.onClick.AddListener(ToggleSFX);
        //vibrationToggleButton.onClick.AddListener(ToggleVibration);  // Add listener for vibration button

        // Update button visuals based on settings
        UpdateMusicButtonVisual();
        UpdateSFXButtonVisual();
        //UpdateVibrationButtonVisual();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!isSFXMuted)
        {
            SFXSource.PlayOneShot(clip);
        }
        //Vibrate();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (!isMusicMuted)
        {
            musicSource.PlayOneShot(clip);
        }
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;

        if (isMusicMuted)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.Play();
        }

        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
        PlayerPrefs.Save();
        UpdateMusicButtonVisual();
    }

    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;
        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSFXButtonVisual();
    }

    //Toggle vibration on/off
    // public void ToggleVibration()
    // {
    //     isVibrationEnabled = !isVibrationEnabled;
    //     PlayerPrefs.SetInt("VibrationEnabled", isVibrationEnabled ? 1 : 0);
    //     PlayerPrefs.Save();
    //     UpdateVibrationButtonVisual();

    //     // Optional: Give short vibration to indicate toggle if enabled
    //     if (isVibrationEnabled)
    //     {
    //         Vibrate();
    //     }
    // }

    // Trigger a vibration
//     public void Vibrate()
//     {
//         if (isVibrationEnabled)
//         {
// #if UNITY_ANDROID && !UNITY_EDITOR
//         Handheld.Vibrate();
// #else
//             Debug.Log("Vibration triggered (not supported in editor).");
// #endif
//         }
//     }


    private void UpdateMusicButtonVisual()
    {
        Image buttonImage = musicToggleButton.GetComponent<Image>();
        buttonImage.sprite = isMusicMuted ? musicOffImage : musicOnImage;
    }

    private void UpdateSFXButtonVisual()
    {
        Image buttonImage = sfxToggleButton.GetComponent<Image>();
        buttonImage.sprite = isSFXMuted ? sfxOffImage : sfxOnImage;
    }

    // private void UpdateVibrationButtonVisual()
    // {
    //     Image buttonImage = vibrationToggleButton.GetComponent<Image>();
    //     buttonImage.sprite = isVibrationEnabled ? vibrationOnImage : vibrationOffImage;
    // }

    private void LoadAudioSettings()
    {
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        //isVibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;

        if (isMusicMuted)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.Play();
        }
    }
}
