using NAudio.Wave;

public class MazeSoundPlayer : IMazeSoundPlayer
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public void PlayMusic(string fileName, float volume = 0.3f, bool isNeedToLoop = false)
    {
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", fileName);

        outputDevice = new WaveOutEvent();
        audioFile = new AudioFileReader(fullPath);

        audioFile.Volume = Math.Clamp(volume, 0f, 1f);

        if (isNeedToLoop)
        {
            var loopStream = new LoopStream(audioFile);
            outputDevice.Init(loopStream);
        }
        else
        {
            outputDevice.Init(audioFile);
        }

        outputDevice.Play();

    }

    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;
        public override long Length => sourceStream.Length;

        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = sourceStream.Read(buffer, offset, count);

            if (bytesRead == 0)
            {
                sourceStream.Position = 0;
                bytesRead = sourceStream.Read(buffer, offset, count);
            }

            return bytesRead;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sourceStream?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}