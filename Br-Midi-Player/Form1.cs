namespace Br_Midi_Player
{
    using Br_Midi_Player.lib;
    using Br_Midi_Player.play;
    using System.Runtime.InteropServices;

    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        private readonly IDictionary<int, String> musics = new Dictionary<int, String>(); // 文件索引
        private readonly Player player = new(); // 播放器
        private int playIndex = -1; // 播放中的音乐索引
        private bool isMoving; // 是否正在移动进度条
        public static bool isFinished; // 是否播放完毕

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadMusic();
        }

        // 重载音乐列表
        public void ReloadMusic()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + "music\\";

            //判断文件夹是否存在，如果不存在就创建文件夹
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                return;
            }
            // 遍历所有文件
            var files = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories);

            // 暂停音乐播放
            PlayerStop();

            // 清空数据
            musics.Clear();
            uiListBox1.Items.Clear();
            // 添加列表数据
            for (var i = 0; i < files.Count(); i++)
            {
                var path = files[i];
                var fileName = Path.GetFileNameWithoutExtension(path);
                if (fileName.Length > 30)
                {
                    fileName = fileName.Substring(0, 30) + "...";
                }
                musics[i] = path;
                uiListBox1.Items.Add(fileName);
            }
        }

        // 播放midi
        public void PlayerPlay(String path)
        {
            // 判断是否开启调试
            if (uiSwitch2.Active)
            {
                AllocConsole(); // 调试
            }
            // 判断是否跳转窗口
            if (uiSwitch1.Active)
            {
                WindowHelper.EnsureGameOnTop(uiTextBox1.Text);
            }

            pictureBox1.Image = Properties.Resources.暂停;
            // 打开midi并播放
            player.Open(path, (double)numericUpDown1.Value, (int)numericUpDown2.Value);
            player.Start();

            label9.Text = Path.GetFileNameWithoutExtension(path); // 歌曲名
            timer1.Enabled = true; // 开启歌曲时间显示
            timer2.Enabled = true; // 开启自动切换下一首歌
            timer3.Enabled = true; // 开启检测窗口焦点
        }

        // 暂停播放
        public void PlayerPause()
        {
            switch (player.playState)
            {
                case -1:
                    PlayerNext(); // 下一首歌
                    pictureBox1.Image = Properties.Resources.暂停;
                    break;
                case 0:
                    pictureBox1.Image = Properties.Resources.播放;
                    player.Pause(); // 暂停音乐
                    break;
                case 1:
                    pictureBox1.Image = Properties.Resources.暂停;
                    player.Pause(); // 播放音乐
                    break;
            }
        }

        // 播放上一个
        public void PlayerLast()
        {
            playIndex -= 1; // 索引
            if (playIndex < 0 || musics.Count == 0)
            {
                playIndex = -1;
                uiListBox1.SelectedIndex = 0; // 选择列表
                return;
            }
            uiListBox1.SelectedIndex = playIndex; // 选择列表
            PlayerPlay(musics[playIndex]); // 播放Midi
        }

        // 播放下一个
        public void PlayerNext()
        {
            playIndex += 1; // 索引
            if (playIndex > musics.Count)
            {
                playIndex = musics.Count - 1;
                uiListBox1.SelectedIndex = playIndex; // 选择列表
                return;
            }
            uiListBox1.SelectedIndex = playIndex; // 选择列表
            PlayerPlay(musics[playIndex]); // 播放Midi
        }

        // 播放结束
        public void PlayerStop()
        {
            pictureBox1.Image = Properties.Resources.播放;

            label9.Text = "暂未播放"; // 重置歌曲名
            timer1.Enabled = false; // 开启歌曲时间显示
            timer2.Enabled = false; // 开启自动切换下一首歌
            timer3.Enabled = false; // 开启检测窗口焦点

            player.Stop(); // 结束播放

            playIndex = -1; // 重置索引
            uiListBox1.SelectedIndex = -1;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0); // 强制退出进程
        }

        private void label12_Click(object sender, EventArgs e)
        {
            ReloadMusic(); // 刷新列表
        }

        private void uiListBox1_ItemDoubleClick(object sender, EventArgs e)
        {
            playIndex = uiListBox1.SelectedIndex;
            PlayerPlay(musics[playIndex]); // 播放Midi
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PlayerPause(); // 暂停歌曲
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PlayerLast(); // 上一首歌
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PlayerNext(); // 下一首歌
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var currentTime = player.GetCurrentTime(); // 当前时长
            var durationTime = player.GetDuration(); // 总时长

            if (currentTime == null || durationTime == null) return;

            // 显示播放时间
            label10.Text = currentTime.Minutes + ":" + currentTime.Seconds + " / " + durationTime.Minutes + ":" + durationTime.Seconds;
            
            // 允许歌曲改变进度条的值
            if (!isMoving)
            {
                // 显示进度条进度
                double currentSec = currentTime.Minutes * 60 + currentTime.Seconds;
                double durationSec = durationTime.Minutes * 60 + durationTime.Seconds;
                uiTrackBar1.Value = (int)(currentSec / durationSec * 10000);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // 自动播放下一首歌
            if (isFinished)
            {
                isFinished = false;
                PlayerNext(); // 下一首歌
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (!uiSwitch3.Active) return;
            // 游戏窗口没有获得焦点停止播放
            var processName = uiTextBox1.Text;
            if (!WindowHelper.IsGameFocused(processName) && player.playState == 0)
            {
                PlayerPause(); // 暂停播放
            }
            else if (WindowHelper.IsGameFocused(processName) && player.playState == 1)
            {
                PlayerPause(); // 继续播放
            }
        }

        private void uiTrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            isMoving = true; // 禁止歌曲改变进度条的值
        }

        private void uiTrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            // 更改播放时间
            var durationTime = player.GetDuration(); // 总时长
            if (durationTime == null) return;

            int durationSec = durationTime.Minutes * 60 + durationTime.Seconds;
            player.SetTime((long)(durationSec * (double)(uiTrackBar1.Value) / 10000 * 1000000));
            isMoving = false; // 允许歌曲改变进度条的值
        }
    }
}