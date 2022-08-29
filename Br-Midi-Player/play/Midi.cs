using Br_Midi_Player.lib;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using WindowsInput.Native;

namespace Br_Midi_Player.play
{
    internal class Midi
    {
        private Playback playback;

        // 设置项
        public double speed; // 播放速度
        public int liftRange; // 升降调范围

        public Midi(MidiFile midiFile, double speed, int liftRange)
        { 
            this.speed = speed;
            this.liftRange = liftRange;
            // 初始化
            playback = midiFile.GetPlayback();
            playback.Speed = speed;
            playback.InterruptNotesOnStop = true;
            playback.Finished += (_, _) => { Form1.isFinished = true; };
            playback.EventPlayed += OnNoteEvent;
        }

        // 开始播放
        public void Play()
        {
            new Thread(() => {
                Console.WriteLine(GetCurrentTime() + " - " + "播放已开始"); // 调试输出
                playback.Play();
                Console.WriteLine(GetCurrentTime() + " - " + "播放已结束"); // 调试输出
            }).Start();
        }

        // 暂停播放
        public void Stop()
        {
            playback.Stop();
        }

        // 获取已播放的时间
        public MetricTimeSpan GetCurrentTime()
        {
            return (MetricTimeSpan)playback.GetCurrentTime(TimeSpanType.Metric);
        }

        // 获取歌曲的时间
        public MetricTimeSpan GetDuration()
        {
            return (MetricTimeSpan)playback.GetDuration(TimeSpanType.Metric);
        }

        // 设置歌曲当前时间
        public void SetTime(ITimeSpan time)
        {
            playback.MoveToTime(time);
        }

        // 当note被触发的事件
        private void OnNoteEvent(object? sender, MidiEventPlayedEventArgs e)
        {
            if (e.Event is not NoteEvent noteEvent)
                return;

            PlayNote(noteEvent);
        }

        // 模拟按键
        private void PlayNote(NoteEvent note)
        {
            var key = LiftNote(note.NoteNumber); // 升降调音符寻找键位
            if (key == null)
            {
                Console.WriteLine(GetCurrentTime().ToString() + " - " + note.ToString() + " :无法找到对应键位"); // 调试输出
                return;
            }
            // 根据事件类型执行
            switch (note.EventType)
            {
                case MidiEventType.NoteOn:
                    Press.KeyDown((VirtualKeyCode)key);
                    break;
                case MidiEventType.NoteOff:
                    Press.KeyUp((VirtualKeyCode)key);
                    break;
            }
            Console.WriteLine(GetCurrentTime().ToString() + " - " + note.ToString()); // 调试输出
        }

        // 升降调某个音符后返回相应键位 若无法找到返回null
        private VirtualKeyCode? LiftNote(int note)
        {
            var keys = Key.keys; // 音符键位

            // 判断键位表是否存在此音符键位
            if (!keys.ContainsKey(note))
            {
                for (var i = 0; i < liftRange; i++)
                {
                    var note1 = note - 12 * i; // 优先级第一的音符
                    var note2 = note + 12 * i; // 优先级第二的音符
                    // 判断附近音符是否合法
                    if (note1 < 0 || note1 > 127 || note2 < 0 || note2 > 127)
                    {
                        return null;
                    }
                    // 判断附近音符是否存在于键位表
                    if (keys.ContainsKey(note1))
                    {
                        return keys[note1];
                    }
                    else if (keys.ContainsKey(note2))
                    {
                        return keys[note2];
                    }
                }
            }
            else
            {
                // 存在该音符的键位就直接返回就好啦
                return keys[note];
            }
            return null;
        }
    }
}
