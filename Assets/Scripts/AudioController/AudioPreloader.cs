using UnityEngine;

public class AudioPreloader : IAudioController
{
    public void PlaySFX(string SFX)
    {
        Resources.Load(SFX);
    }

    public void PlaySong(string songName)
    {
        Resources.Load(songName);
    }

    public void StopSong()
    {
    }
}
