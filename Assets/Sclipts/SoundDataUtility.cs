using UnityEngine;

public static class SoundDataUtility
{
    public static class KeyConfig
    {
        public static class Se
        {
            public static readonly string Hi = "Hi";
            public static readonly string Kinsetu = "Kinsetu";
            public static readonly string Button = "Button";
            public static readonly string Kaminari = "Kaminari";
            public static readonly string Hit = "Hit";
            public static readonly string Koori = "Koori";
        }

        public static class Bgm
        {
            public static readonly string Hiru = "Hiru";
            public static readonly string Yoru = "Yoru";
            public static readonly string Title = "Title";
        }
    }

    public enum SoundType
    {
        Bgm = 0,
        Se = 1
    }

    public static void PrepareAudioSource(this AudioSource source, SoundData soundData)
    {
        source.playOnAwake = soundData.PlayOnAwake;
        source.loop = soundData.IsLoop;
        source.clip = soundData.Clip;
        source.volume = soundData.Volume;
    }
}
