using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioController : MonoBehaviour, IAudioController
{
    [SerializeField] private NarrativeGameState _narrativeGameState;

    /// <summary>
    /// One day this will come from the "Settings," but for now it lives on a field
    /// </summary>
    [Tooltip("Music Volume level set by player")]
    [Range(0f, 1f)]
    [SerializeField] private float _settingsMusicVolume = 0.5f;

    /// <summary>
    /// One day this will come from the "Settings," but for now it lives on a field
    /// </summary>
    [Tooltip("SFX Volume level set by player")]
    [Range(0f, 1f)]
    [SerializeField] private float _settingsSfxVolume = 0.5f;

    [Tooltip("Total duration of fade out + fade in")]
    [Range(0f, 4f)]
    [SerializeField] private float _transitionDuration = 2f;
    private AudioSource _musicAudioSource;
    private AudioSource _sfxAudioSource;
    private Coroutine _currentFadeCoroutine;
    private MusicFader _musicFader;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Awake()
    {
        _musicFader = new MusicFader();
        _musicAudioSource = CreateAudioSource("Music Player");
        _sfxAudioSource = CreateAudioSource("SFX Player");
        _musicAudioSource.loop = true;
    }

    /// <summary>
    /// Called every rendered frame
    /// </summary>
    private void Update()
    {
        if (_musicAudioSource != null && _musicFader != null)
            _musicAudioSource.volume = _musicFader.NormalizedVolume * _settingsMusicVolume;

        if (_sfxAudioSource != null)
            _sfxAudioSource.volume = _settingsSfxVolume;
    }

    /// <summary>
    /// Overload for PlaySong which allows songs to be played using a direct reference.
    /// </summary>
    /// <param name="song">The song to play.</param>
    public void PlaySong(AudioClip song)
    {
        if (_currentFadeCoroutine != null)
        {
            StopCoroutine(_currentFadeCoroutine);
        }

        _currentFadeCoroutine = StartCoroutine(FadeToNewSong(song));
    }

    /// <summary>
    /// Creates a child object that has an AudioSource component
    /// </summary>
    /// <returns>The AudioSource of the new child object</returns>
    private AudioSource CreateAudioSource(string gameObjectName)
    {
        var newGameObject = new GameObject(gameObjectName);
        newGameObject.transform.parent = transform;
        return newGameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Plays sound effect of desired name.
    /// </summary>
    /// <param name="sfx">Name of sound effect asset, must be in `Resources/Audio/SFX`</param>
    public void PlaySfx(string sfx)
    {
        AudioClip soundEffectClip = _narrativeGameState.ObjectStorage.GetObject<AudioClip>(sfx);
        PlaySfx(soundEffectClip);
    }

    /// <summary>
    /// Play given audio clip immediately
    /// </summary>
    /// <param name="soundEffectClip">Clip to play</param>
    public void PlaySfx(AudioClip soundEffectClip)
    {
        _sfxAudioSource.PlayOneShot(soundEffectClip);
    }

    /// <summary>
    /// Coroutine to fade to a new song.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    public IEnumerator FadeToNewSong(AudioClip song)
    {
        if (IsCurrentlyPlayingMusic())
        {
            yield return _musicFader.FadeOut(_transitionDuration / 2f);
        }

        SetCurrentTrack(song);
        yield return _musicFader.FadeIn(_transitionDuration / 2f);
    }

    /// <summary>
    /// If music is currently playing, stop it!
    /// </summary>
    public void StopSong()
    {
        if (_musicAudioSource != null)
        {
            _musicAudioSource.Stop();
        }
    }

    /// <summary>
    /// Are we playing music?
    /// </summary>
    /// <returns>True if we're playing a music track with a volume above zero</returns>
    private bool IsCurrentlyPlayingMusic()
    {
        return _musicAudioSource.clip != null && _settingsMusicVolume != 0;
    }

    /// <summary>
    /// Assigns the music player to a given song with 0 volume, intending to fade it in.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    private void SetCurrentTrack(AudioClip song)
    {
        _musicAudioSource.clip = song;
        _musicAudioSource.volume = 0f; // Always set volume to 0 BEFORE playing the audio source
        _musicAudioSource.Play();
    }
}
