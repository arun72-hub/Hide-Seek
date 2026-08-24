

using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ------------")]
    public AudioClip background;
    public AudioClip catchperson;
    public AudioClip gameover;
    public AudioClip coin;

    [Header("---------- UI Buttons ------------")]
    public Button musicToggleButton;
    public Button sfxToggleButton;

    [Header("---------- UI Button Images ------------")]
    public Sprite musicOnImage;
    public Sprite musicOffImage;
    public Sprite sfxOnImage;
    public Sprite sfxOffImage;

    private bool isMusicMuted;
    private bool isSFXMuted;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        InitializeAudio();
        SetupButtonListeners();
        UpdateAllVisuals();
    }

    private void OnEnable()
    {
        SetupButtonListeners();
        UpdateAllVisuals();
    }

    private void InitializeAudio()
    {
        LoadAudioSettings();

        if (musicSource != null)
        {
            musicSource.loop = true;
            if (background != null)
            {
                musicSource.clip = background;
            }

            musicSource.mute = isMusicMuted;
            if (!isMusicMuted && musicSource.clip != null)
            {
                if (!musicSource.isPlaying)
                {
                    musicSource.Play();
                }
            }
            else
            {
                musicSource.Pause();
            }
        }

        if (SFXSource != null)
        {
            SFXSource.mute = isSFXMuted;
        }
    }

    public void SetupButtonListeners()
    {
        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.RemoveListener(ToggleMusic);
            musicToggleButton.onClick.AddListener(ToggleMusic);
        }

        if (sfxToggleButton != null)
        {
            sfxToggleButton.onClick.RemoveListener(ToggleSFX);
            sfxToggleButton.onClick.AddListener(ToggleSFX);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (isSFXMuted || clip == null || SFXSource == null) return;
        SFXSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null) return;

        if (clip != null)
        {
            musicSource.clip = clip;
        }

        if (!isMusicMuted && musicSource.clip != null)
        {
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }

    public void ToggleMusic()
    {
        SetMusicMuted(!isMusicMuted);
    }

    public void SetMusicMuted(bool mute)
    {
        isMusicMuted = mute;

        if (musicSource != null)
        {
            musicSource.mute = isMusicMuted;
            if (isMusicMuted)
            {
                musicSource.Pause();
            }
            else
            {
                if (!musicSource.isPlaying && musicSource.clip != null)
                {
                    musicSource.Play();
                }
            }
        }

        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
        PlayerPrefs.Save();
        UpdateMusicButtonVisual();
    }

    public void ToggleSFX()
    {
        SetSFXMuted(!isSFXMuted);
    }

    public void SetSFXMuted(bool mute)
    {
        isSFXMuted = mute;

        if (SFXSource != null)
        {
            SFXSource.mute = isSFXMuted;
        }

        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSFXButtonVisual();
    }

    public void UpdateAllVisuals()
    {
        UpdateMusicButtonVisual();
        UpdateSFXButtonVisual();
    }

    public void UpdateMusicButtonVisual()
    {
        if (musicToggleButton == null) return;
        Image buttonImage = musicToggleButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (isMusicMuted && musicOffImage != null)
            {
                buttonImage.sprite = musicOffImage;
            }
            else if (!isMusicMuted && musicOnImage != null)
            {
                buttonImage.sprite = musicOnImage;
            }
        }
    }

    public void UpdateSFXButtonVisual()
    {
        if (sfxToggleButton == null) return;
        Image buttonImage = sfxToggleButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (isSFXMuted && sfxOffImage != null)
            {
                buttonImage.sprite = sfxOffImage;
            }
            else if (!isSFXMuted && sfxOnImage != null)
            {
                buttonImage.sprite = sfxOnImage;
            }
        }
    }

    private void LoadAudioSettings()
    {
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }

    public bool IsMusicMuted => isMusicMuted;
    public bool IsSFXMuted => isSFXMuted;
}

