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

        private readonly IDictionary<int, String> musics = new Dictionary<int, String>(); // �ļ�����
        private readonly Player player = new(); // ������
        private int playIndex = -1; // �����е���������
        private bool isMoving; // �Ƿ������ƶ�������
        public static bool isFinished; // �Ƿ񲥷����

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadMusic();
        }

        // ���������б�
        public void ReloadMusic()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + "music\\";

            //�ж��ļ����Ƿ���ڣ���������ھʹ����ļ���
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                return;
            }
            // ���������ļ�
            var files = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories);

            // ��ͣ���ֲ���
            PlayerStop();

            // �������
            musics.Clear();
            uiListBox1.Items.Clear();
            // ����б�����
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

        // ����midi
        public void PlayerPlay(String path)
        {
            // �ж��Ƿ�������
            if (uiSwitch2.Active)
            {
                AllocConsole(); // ����
            }
            // �ж��Ƿ���ת����
            if (uiSwitch1.Active)
            {
                WindowHelper.EnsureGameOnTop(uiTextBox1.Text);
            }

            pictureBox1.Image = Properties.Resources.��ͣ;
            // ��midi������
            player.Open(path, (double)numericUpDown1.Value, (int)numericUpDown2.Value);
            player.Start();

            label9.Text = Path.GetFileNameWithoutExtension(path); // ������
            timer1.Enabled = true; // ��������ʱ����ʾ
            timer2.Enabled = true; // �����Զ��л���һ�׸�
            timer3.Enabled = true; // ������ⴰ�ڽ���
        }

        // ��ͣ����
        public void PlayerPause()
        {
            switch (player.playState)
            {
                case -1:
                    PlayerNext(); // ��һ�׸�
                    pictureBox1.Image = Properties.Resources.��ͣ;
                    break;
                case 0:
                    pictureBox1.Image = Properties.Resources.����;
                    player.Pause(); // ��ͣ����
                    break;
                case 1:
                    pictureBox1.Image = Properties.Resources.��ͣ;
                    player.Pause(); // ��������
                    break;
            }
        }

        // ������һ��
        public void PlayerLast()
        {
            playIndex -= 1; // ����
            if (playIndex < 0 || musics.Count == 0)
            {
                playIndex = -1;
                uiListBox1.SelectedIndex = 0; // ѡ���б�
                return;
            }
            uiListBox1.SelectedIndex = playIndex; // ѡ���б�
            PlayerPlay(musics[playIndex]); // ����Midi
        }

        // ������һ��
        public void PlayerNext()
        {
            playIndex += 1; // ����
            if (playIndex > musics.Count)
            {
                playIndex = musics.Count - 1;
                uiListBox1.SelectedIndex = playIndex; // ѡ���б�
                return;
            }
            uiListBox1.SelectedIndex = playIndex; // ѡ���б�
            PlayerPlay(musics[playIndex]); // ����Midi
        }

        // ���Ž���
        public void PlayerStop()
        {
            pictureBox1.Image = Properties.Resources.����;

            label9.Text = "��δ����"; // ���ø�����
            timer1.Enabled = false; // ��������ʱ����ʾ
            timer2.Enabled = false; // �����Զ��л���һ�׸�
            timer3.Enabled = false; // ������ⴰ�ڽ���

            player.Stop(); // ��������

            playIndex = -1; // ��������
            uiListBox1.SelectedIndex = -1;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0); // ǿ���˳�����
        }

        private void label12_Click(object sender, EventArgs e)
        {
            ReloadMusic(); // ˢ���б�
        }

        private void uiListBox1_ItemDoubleClick(object sender, EventArgs e)
        {
            playIndex = uiListBox1.SelectedIndex;
            PlayerPlay(musics[playIndex]); // ����Midi
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PlayerPause(); // ��ͣ����
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PlayerLast(); // ��һ�׸�
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PlayerNext(); // ��һ�׸�
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var currentTime = player.GetCurrentTime(); // ��ǰʱ��
            var durationTime = player.GetDuration(); // ��ʱ��

            if (currentTime == null || durationTime == null) return;

            // ��ʾ����ʱ��
            label10.Text = currentTime.Minutes + ":" + currentTime.Seconds + " / " + durationTime.Minutes + ":" + durationTime.Seconds;
            
            // ��������ı��������ֵ
            if (!isMoving)
            {
                // ��ʾ����������
                double currentSec = currentTime.Minutes * 60 + currentTime.Seconds;
                double durationSec = durationTime.Minutes * 60 + durationTime.Seconds;
                uiTrackBar1.Value = (int)(currentSec / durationSec * 10000);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // �Զ�������һ�׸�
            if (isFinished)
            {
                isFinished = false;
                PlayerNext(); // ��һ�׸�
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (!uiSwitch3.Active) return;
            // ��Ϸ����û�л�ý���ֹͣ����
            var processName = uiTextBox1.Text;
            if (!WindowHelper.IsGameFocused(processName) && player.playState == 0)
            {
                PlayerPause(); // ��ͣ����
            }
            else if (WindowHelper.IsGameFocused(processName) && player.playState == 1)
            {
                PlayerPause(); // ��������
            }
        }

        private void uiTrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            isMoving = true; // ��ֹ�����ı��������ֵ
        }

        private void uiTrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            // ���Ĳ���ʱ��
            var durationTime = player.GetDuration(); // ��ʱ��
            if (durationTime == null) return;

            int durationSec = durationTime.Minutes * 60 + durationTime.Seconds;
            player.SetTime((long)(durationSec * (double)(uiTrackBar1.Value) / 10000 * 1000000));
            isMoving = false; // ��������ı��������ֵ
        }
    }
}