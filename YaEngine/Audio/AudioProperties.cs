namespace YaEngine.Audio
{
    public struct AudioProperties
    {
        public short NumChannels;
        public int SampleRate;
        public short BitsPerSample;

        public override string ToString()
        {
            return
                $"{NumChannels} Channels, {SampleRate} Sample Rate, {BitsPerSample} Bits per Sample";
        }
    }
}