namespace YaEngine.Audio
{
    public struct AudioProperties
    {
        public short NumChannels;
        public int SampleRate;
        public int ByteRate;
        public short BlockAlign;
        public short BitsPerSample;

        public override string ToString()
        {
            return
                $"{NumChannels} Channels, {SampleRate} Sample Rate, {ByteRate} Byte Rate, {BlockAlign} Block Align, {BitsPerSample} Bits per Sample";
        }
    }
}