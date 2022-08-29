using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace Br_Midi_Player.play
{
    internal class Player
    {
        private Midi? midi;

        public int playState = -1; // 播放状态 -1为停止 0为播放 1为暂停

        // 打开Midi文件并初始化
        public void Open(String path, double speed, int liftRange)
        {
            Stop(); // 结束播放防止意外

            // 读取midi
            midi = new Midi(MidiFile.Read(path), speed, liftRange);
        }

        // 开始播放
        public void Start()
        {
            if (midi == null) return;
            playState = 0;
            midi.Play();
        }

        // 暂停播放
        public void Pause()
        {
            if (midi == null) return;
            switch (playState)
            {
                case 0:
                    playState = 1;
                    midi.Stop();
                    break;
                case 1:
                    playState = 0;
                    midi.Play();
                    break;
            }
        }

        // 结束播放
        public void Stop()
        {
            if (midi == null) return;
            playState = -1;
            midi.Stop();
            midi = null;
        }

        // 获取已播放的时间
        public MetricTimeSpan? GetCurrentTime()
        {
            if (midi == null) return null;
            return midi.GetCurrentTime();
        }

        // 获取歌曲的时间
        public MetricTimeSpan? GetDuration()
        {
            if (midi == null) return null;
            return midi.GetDuration();
        }

        // 设置歌曲当前时间
        public void SetTime(long microSec)
        {
            if (midi == null) return;
            midi.SetTime(new MetricTimeSpan(microSec));
        }
    }
}
