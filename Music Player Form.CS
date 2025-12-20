using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicPlayer
{
    public partial class MusicPlayerForm : Form
    {
        private System.Windows.Forms.Timer timer;
        private WMPLib.WindowsMediaPlayer player;
        private bool isDraggingTrackbar = false;
        private string currentSongName = "";
        private string currentArtist = "";
        private List<SongInfo> playlist = new List<SongInfo>();
        private static readonly HttpClient httpClient = new HttpClient();
        private Label titleLabel;
        private Panel albumPanel;
        private Label albumArt;
        private Label nowPlayingLabel;
        private Label trackNameLabel;
        private Label artistLabel;
        private TrackBar progressBar;
        private Label currentTimeLabel;
        private Label totalTimeLabel;
        private Panel controlPanel;
        private Button prevBtn;
        private Button playBtn;
        private Button nextBtn;
        private Label volumeLabel;
        private TrackBar volumeBar;
        private Label volumePercentLabel;
        private Label playlistLabel;
        private ListBox playlistBox;
        private Button addSongsBtn;
        private Button clearPlaylistBtn;
        private Button recommendBtn;
        private Label recommendLabel;
        private Button exitBTN;
        private const string LASTFM_API_KEY = "bf454f6e44e9be86e8ab92e33730d22c";

        public class SongInfo
        {
            public string FilePath { get; set; }
            public string SongName { get; set; }
            public string Artist { get; set; }
            public string DisplayName => $"{Artist} - {SongName}";
        }

        public MusicPlayerForm()
        {
            InitializeComponent();
            InitializePlayer();
        }

        private void InitializeComponent()
        {
            titleLabel = new Label();
            albumPanel = new Panel();
            albumArt = new Label();
            nowPlayingLabel = new Label();
            trackNameLabel = new Label();
            artistLabel = new Label();
            progressBar = new TrackBar();
            currentTimeLabel = new Label();
            totalTimeLabel = new Label();
            controlPanel = new Panel();
            prevBtn = new Button();
            playBtn = new Button();
            nextBtn = new Button();
            volumeLabel = new Label();
            volumeBar = new TrackBar();
            volumePercentLabel = new Label();
            playlistLabel = new Label();
            playlistBox = new ListBox();
            addSongsBtn = new Button();
            clearPlaylistBtn = new Button();
            recommendBtn = new Button();
            recommendLabel = new Label();
            exitBTN = new Button();
            albumPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)progressBar).BeginInit();
            controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volumeBar).BeginInit();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(29, 185, 84);
            titleLabel.Location = new Point(30, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(246, 45);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "MUSIC PLAYER";
            // 
            // albumPanel
            // 
            albumPanel.BackColor = Color.FromArgb(40, 40, 40);
            albumPanel.Controls.Add(albumArt);
            albumPanel.Location = new Point(30, 80);
            albumPanel.Name = "albumPanel";
            albumPanel.Size = new Size(200, 200);
            albumPanel.TabIndex = 1;
            // 
            // albumArt
            // 
            albumArt.Font = new Font("Segoe UI", 70F);
            albumArt.ForeColor = Color.FromArgb(29, 185, 84);
            albumArt.Location = new Point(0, 0);
            albumArt.Name = "albumArt";
            albumArt.Size = new Size(200, 200);
            albumArt.TabIndex = 0;
            albumArt.Text = "♪";
            albumArt.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // nowPlayingLabel
            // 
            nowPlayingLabel.AutoSize = true;
            nowPlayingLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            nowPlayingLabel.ForeColor = Color.White;
            nowPlayingLabel.Location = new Point(260, 80);
            nowPlayingLabel.Name = "nowPlayingLabel";
            nowPlayingLabel.Size = new Size(108, 21);
            nowPlayingLabel.TabIndex = 2;
            nowPlayingLabel.Text = "Now Playing";
            // 
            // trackNameLabel
            // 
            trackNameLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            trackNameLabel.ForeColor = Color.White;
            trackNameLabel.Location = new Point(260, 110);
            trackNameLabel.Name = "trackNameLabel";
            trackNameLabel.Size = new Size(380, 30);
            trackNameLabel.TabIndex = 3;
            trackNameLabel.Text = "No track loaded";
            // 
            // artistLabel
            // 
            artistLabel.Font = new Font("Segoe UI", 11F);
            artistLabel.ForeColor = Color.FromArgb(180, 180, 180);
            artistLabel.Location = new Point(260, 145);
            artistLabel.Name = "artistLabel";
            artistLabel.Size = new Size(380, 25);
            artistLabel.TabIndex = 4;
            artistLabel.Text = "Select a song from your playlist";
            // 
            // progressBar
            // 
            progressBar.BackColor = Color.FromArgb(18, 18, 18);
            progressBar.Location = new Point(260, 180);
            progressBar.Maximum = 100;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(380, 45);
            progressBar.TabIndex = 5;
            progressBar.TickStyle = TickStyle.None;
            progressBar.MouseDown += ProgressBar_MouseDown;
            progressBar.MouseUp += ProgressBar_MouseUp;
            // 
            // currentTimeLabel
            // 
            currentTimeLabel.AutoSize = true;
            currentTimeLabel.Font = new Font("Segoe UI", 9F);
            currentTimeLabel.ForeColor = Color.FromArgb(180, 180, 180);
            currentTimeLabel.Location = new Point(260, 215);
            currentTimeLabel.Name = "currentTimeLabel";
            currentTimeLabel.Size = new Size(28, 15);
            currentTimeLabel.TabIndex = 6;
            currentTimeLabel.Text = "0:00";
            // 
            // totalTimeLabel
            // 
            totalTimeLabel.AutoSize = true;
            totalTimeLabel.Font = new Font("Segoe UI", 9F);
            totalTimeLabel.ForeColor = Color.FromArgb(180, 180, 180);
            totalTimeLabel.Location = new Point(605, 215);
            totalTimeLabel.Name = "totalTimeLabel";
            totalTimeLabel.Size = new Size(28, 15);
            totalTimeLabel.TabIndex = 7;
            totalTimeLabel.Text = "0:00";
            // 
            // controlPanel
            // 
            controlPanel.BackColor = Color.Transparent;
            controlPanel.Controls.Add(prevBtn);
            controlPanel.Controls.Add(playBtn);
            controlPanel.Controls.Add(nextBtn);
            controlPanel.Location = new Point(260, 240);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(380, 60);
            controlPanel.TabIndex = 8;
            // 
            // prevBtn
            // 
            prevBtn.BackColor = Color.FromArgb(40, 40, 40);
            prevBtn.Cursor = Cursors.Hand;
            prevBtn.FlatAppearance.BorderSize = 0;
            prevBtn.FlatStyle = FlatStyle.Flat;
            prevBtn.Font = new Font("Segoe UI", 16F);
            prevBtn.ForeColor = Color.White;
            prevBtn.Location = new Point(80, 0);
            prevBtn.Name = "prevBtn";
            prevBtn.Size = new Size(55, 55);
            prevBtn.TabIndex = 0;
            prevBtn.Text = "⏪";
            prevBtn.UseVisualStyleBackColor = false;
            prevBtn.Click += PrevBtn_Click;
            // 
            // playBtn
            // 
            playBtn.BackColor = Color.FromArgb(29, 185, 84);
            playBtn.Cursor = Cursors.Hand;
            playBtn.FlatAppearance.BorderSize = 0;
            playBtn.FlatStyle = FlatStyle.Flat;
            playBtn.Font = new Font("Segoe UI", 20F);
            playBtn.ForeColor = Color.White;
            playBtn.Location = new Point(145, -5);
            playBtn.Name = "playBtn";
            playBtn.Size = new Size(65, 65);
            playBtn.TabIndex = 1;
            playBtn.Tag = "play";
            playBtn.Text = "▶";
            playBtn.UseVisualStyleBackColor = false;
            playBtn.Click += PlayBtn_Click;
            // 
            // nextBtn
            // 
            nextBtn.BackColor = Color.FromArgb(40, 40, 40);
            nextBtn.Cursor = Cursors.Hand;
            nextBtn.FlatAppearance.BorderSize = 0;
            nextBtn.FlatStyle = FlatStyle.Flat;
            nextBtn.Font = new Font("Segoe UI", 16F);
            nextBtn.ForeColor = Color.White;
            nextBtn.Location = new Point(220, 0);
            nextBtn.Name = "nextBtn";
            nextBtn.Size = new Size(55, 55);
            nextBtn.TabIndex = 2;
            nextBtn.Text = "⏩";
            nextBtn.UseVisualStyleBackColor = false;
            nextBtn.Click += NextBtn_Click;
            // 
            // volumeLabel
            // 
            volumeLabel.AutoSize = true;
            volumeLabel.Font = new Font("Segoe UI", 12F);
            volumeLabel.ForeColor = Color.White;
            volumeLabel.Location = new Point(30, 310);
            volumeLabel.Name = "volumeLabel";
            volumeLabel.Size = new Size(32, 21);
            volumeLabel.TabIndex = 9;
            volumeLabel.Text = "🔊";
            // 
            // volumeBar
            // 
            volumeBar.BackColor = Color.FromArgb(18, 18, 18);
            volumeBar.Location = new Point(65, 305);
            volumeBar.Maximum = 100;
            volumeBar.Name = "volumeBar";
            volumeBar.Size = new Size(150, 45);
            volumeBar.TabIndex = 10;
            volumeBar.TickStyle = TickStyle.None;
            volumeBar.Value = 50;
            volumeBar.ValueChanged += VolumeBar_ValueChanged;
            // 
            // volumePercentLabel
            // 
            volumePercentLabel.AutoSize = true;
            volumePercentLabel.Font = new Font("Segoe UI", 9F);
            volumePercentLabel.ForeColor = Color.FromArgb(180, 180, 180);
            volumePercentLabel.Location = new Point(220, 310);
            volumePercentLabel.Name = "volumePercentLabel";
            volumePercentLabel.Size = new Size(29, 15);
            volumePercentLabel.TabIndex = 11;
            volumePercentLabel.Text = "50%";
            // 
            // playlistLabel
            // 
            playlistLabel.AutoSize = true;
            playlistLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            playlistLabel.ForeColor = Color.FromArgb(29, 185, 84);
            playlistLabel.Location = new Point(670, 20);
            playlistLabel.Name = "playlistLabel";
            playlistLabel.Size = new Size(131, 25);
            playlistLabel.TabIndex = 12;
            playlistLabel.Text = "MY PLAYLIST";
            // 
            // playlistBox
            // 
            playlistBox.BackColor = Color.FromArgb(30, 30, 30);
            playlistBox.BorderStyle = BorderStyle.None;
            playlistBox.Font = new Font("Segoe UI", 10F);
            playlistBox.ForeColor = Color.White;
            playlistBox.Location = new Point(670, 60);
            playlistBox.Name = "playlistBox";
            playlistBox.Size = new Size(300, 357);
            playlistBox.TabIndex = 13;
            playlistBox.DoubleClick += PlaylistBox_DoubleClick;
            // 
            // addSongsBtn
            // 
            addSongsBtn.BackColor = Color.FromArgb(29, 185, 84);
            addSongsBtn.Cursor = Cursors.Hand;
            addSongsBtn.FlatAppearance.BorderColor = Color.FromArgb(29, 185, 84);
            addSongsBtn.FlatAppearance.BorderSize = 2;
            addSongsBtn.FlatStyle = FlatStyle.Flat;
            addSongsBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            addSongsBtn.ForeColor = Color.White;
            addSongsBtn.Location = new Point(670, 430);
            addSongsBtn.Name = "addSongsBtn";
            addSongsBtn.Size = new Size(145, 40);
            addSongsBtn.TabIndex = 14;
            addSongsBtn.Text = "➕ Add Songs";
            addSongsBtn.UseVisualStyleBackColor = false;
            addSongsBtn.Click += AddSongsBtn_Click;
            // 
            // clearPlaylistBtn
            // 
            clearPlaylistBtn.BackColor = Color.FromArgb(60, 30, 30);
            clearPlaylistBtn.Cursor = Cursors.Hand;
            clearPlaylistBtn.FlatAppearance.BorderColor = Color.FromArgb(200, 50, 50);
            clearPlaylistBtn.FlatAppearance.BorderSize = 2;
            clearPlaylistBtn.FlatStyle = FlatStyle.Flat;
            clearPlaylistBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            clearPlaylistBtn.ForeColor = Color.White;
            clearPlaylistBtn.Location = new Point(825, 430);
            clearPlaylistBtn.Name = "clearPlaylistBtn";
            clearPlaylistBtn.Size = new Size(145, 40);
            clearPlaylistBtn.TabIndex = 15;
            clearPlaylistBtn.Text = "🗑️ Clear";
            clearPlaylistBtn.UseVisualStyleBackColor = false;
            clearPlaylistBtn.Click += ClearPlaylistBtn_Click;
            // 
            // recommendBtn
            // 
            recommendBtn.BackColor = Color.FromArgb(138, 43, 226);
            recommendBtn.Cursor = Cursors.Hand;
            recommendBtn.FlatAppearance.BorderColor = Color.FromArgb(138, 43, 226);
            recommendBtn.FlatAppearance.BorderSize = 2;
            recommendBtn.FlatStyle = FlatStyle.Flat;
            recommendBtn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            recommendBtn.ForeColor = Color.White;
            recommendBtn.Location = new Point(670, 480);
            recommendBtn.Name = "recommendBtn";
            recommendBtn.Size = new Size(300, 45);
            recommendBtn.TabIndex = 16;
            recommendBtn.Text = "🎵 AI: Suggest Next Song";
            recommendBtn.UseVisualStyleBackColor = false;
            recommendBtn.Click += RecommendBtn_Click;
            // 
            // recommendLabel
            // 
            recommendLabel.Font = new Font("Segoe UI", 10F);
            recommendLabel.ForeColor = Color.FromArgb(138, 43, 226);
            recommendLabel.Location = new Point(30, 380);
            recommendLabel.Name = "recommendLabel";
            recommendLabel.Size = new Size(620, 160);
            recommendLabel.TabIndex = 17;
            recommendLabel.Visible = false;
            // 
            // exitBTN
            // 
            exitBTN.BackColor = Color.FromArgb(18, 18, 18);
            exitBTN.FlatStyle = FlatStyle.Flat;
            exitBTN.ForeColor = Color.FromArgb(29, 185, 84);
            exitBTN.Location = new Point(565, 24);
            exitBTN.Name = "exitBTN";
            exitBTN.Size = new Size(75, 23);
            exitBTN.TabIndex = 18;
            exitBTN.Text = "Exit";
            exitBTN.UseVisualStyleBackColor = false;
            exitBTN.Click += exitBTN_Click;
            // 
            // MusicPlayerForm
            // 
            BackColor = Color.FromArgb(18, 18, 18);
            ClientSize = new Size(984, 561);
            Controls.Add(exitBTN);
            Controls.Add(titleLabel);
            Controls.Add(albumPanel);
            Controls.Add(nowPlayingLabel);
            Controls.Add(trackNameLabel);
            Controls.Add(artistLabel);
            Controls.Add(progressBar);
            Controls.Add(currentTimeLabel);
            Controls.Add(totalTimeLabel);
            Controls.Add(controlPanel);
            Controls.Add(volumeLabel);
            Controls.Add(volumeBar);
            Controls.Add(volumePercentLabel);
            Controls.Add(playlistLabel);
            Controls.Add(playlistBox);
            Controls.Add(addSongsBtn);
            Controls.Add(clearPlaylistBtn);
            Controls.Add(recommendBtn);
            Controls.Add(recommendLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MusicPlayerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Music Player";
            albumPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)progressBar).EndInit();
            controlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void InitializePlayer()
        {
            player = new WMPLib.WindowsMediaPlayer();
            player.settings.volume = 50;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void AddSongsBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.mp3;*.wav;*.wma;*.m4a;*.aac|All Files|*.*";
            openFileDialog.Title = "Select Music Files";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string artist = "Unknown Artist";
                    string songName = fileName;

                    // Try to parse "Artist - Song" format
                    string[] parts = fileName.Split(new[] { " - ", "-" }, StringSplitOptions.None);
                    if (parts.Length >= 2)
                    {
                        artist = parts[0].Trim();
                        songName = parts[1].Trim();
                    }

                    var songInfo = new SongInfo
                    {
                        FilePath = file,
                        Artist = artist,
                        SongName = songName
                    };

                    playlist.Add(songInfo);
                }

                UpdatePlaylistDisplay();
            }
        }

        private void UpdatePlaylistDisplay()
        {
            var playlistBoxControls = this.Controls.Find("playlistBox", true);
            if (playlistBoxControls.Length > 0)
            {
                ListBox playlistBox = (ListBox)playlistBoxControls[0];
                playlistBox.Items.Clear();

                foreach (var song in playlist)
                {
                    playlistBox.Items.Add(song.DisplayName);
                }
            }
        }

        private void PlaylistBox_DoubleClick(object sender, EventArgs e)
        {
            var playlistBox = (ListBox)sender;
            if (playlistBox.SelectedIndex >= 0 && playlistBox.SelectedIndex < playlist.Count)
            {
                PlaySong(playlist[playlistBox.SelectedIndex]);
            }
        }

        private void PlaySong(SongInfo song)
        {
            try
            {
                player.URL = song.FilePath;
                currentSongName = song.SongName;
                currentArtist = song.Artist;

                var trackNameControls = this.Controls.Find("trackNameLabel", true);
                if (trackNameControls.Length > 0)
                {
                    Label trackNameLabel = (Label)trackNameControls[0];
                    trackNameLabel.Text = currentSongName;
                }

                var artistControls = this.Controls.Find("artistLabel", true);
                if (artistControls.Length > 0)
                {
                    Label artistLabel = (Label)artistControls[0];
                    artistLabel.Text = currentArtist;
                }

                var playBtnControls = this.Controls.Find("playBtn", true);
                if (playBtnControls.Length > 0)
                {
                    Button playBtn = (Button)playBtnControls[0];
                    playBtn.Text = "⏸";
                    playBtn.Tag = "pause";
                }

                // Hide recommendation
                var recommendLabelControls = this.Controls.Find("recommendLabel", true);
                if (recommendLabelControls.Length > 0)
                {
                    recommendLabelControls[0].Visible = false;
                }

                player.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing song: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearPlaylistBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to clear the entire playlist?",
                "Clear Playlist", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                playlist.Clear();
                UpdatePlaylistDisplay();

                player.controls.stop();

                var trackNameControls = this.Controls.Find("trackNameLabel", true);
                if (trackNameControls.Length > 0)
                {
                    ((Label)trackNameControls[0]).Text = "No track loaded";
                }

                var artistControls = this.Controls.Find("artistLabel", true);
                if (artistControls.Length > 0)
                {
                    ((Label)artistControls[0]).Text = "Select a song from your playlist";
                }
            }
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            Button playBtn = (Button)sender;

            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                player.controls.pause();
                playBtn.Text = "▶";
                playBtn.Tag = "play";
            }
            else
            {
                player.controls.play();
                playBtn.Text = "⏸";
                playBtn.Tag = "pause";
            }
        }

        private void PrevBtn_Click(object sender, EventArgs e)
        {
            if (player.controls.currentPosition > 3)
            {
                player.controls.currentPosition = 0;
            }
            else
            {
                // Play previous song in playlist
                var playlistBoxControls = this.Controls.Find("playlistBox", true);
                if (playlistBoxControls.Length > 0)
                {
                    ListBox playlistBox = (ListBox)playlistBoxControls[0];
                    int currentIndex = playlist.FindIndex(s => s.SongName == currentSongName);

                    if (currentIndex > 0)
                    {
                        playlistBox.SelectedIndex = currentIndex - 1;
                        PlaySong(playlist[currentIndex - 1]);
                    }
                }
            }
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            // Play next song in playlist
            var playlistBoxControls = this.Controls.Find("playlistBox", true);
            if (playlistBoxControls.Length > 0)
            {
                ListBox playlistBox = (ListBox)playlistBoxControls[0];
                int currentIndex = playlist.FindIndex(s => s.SongName == currentSongName);

                if (currentIndex >= 0 && currentIndex < playlist.Count - 1)
                {
                    playlistBox.SelectedIndex = currentIndex + 1;
                    PlaySong(playlist[currentIndex + 1]);
                }
            }
        }

        private void VolumeBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar volumeBar = (TrackBar)sender;
            player.settings.volume = volumeBar.Value;

            var volumePercentControls = this.Controls.Find("volumePercentLabel", true);
            if (volumePercentControls.Length > 0)
            {
                Label volumePercentLabel = (Label)volumePercentControls[0];
                volumePercentLabel.Text = volumeBar.Value + "%";
            }
        }

        private void ProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            isDraggingTrackbar = true;
        }

        private void ProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            isDraggingTrackbar = false;
            TrackBar progressBar = (TrackBar)sender;

            if (player.currentMedia != null && player.currentMedia.duration > 0)
            {
                double newPosition = (progressBar.Value / 100.0) * player.currentMedia.duration;
                player.controls.currentPosition = newPosition;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (player.currentMedia != null && !isDraggingTrackbar)
            {
                try
                {
                    double duration = player.currentMedia.duration;
                    double position = player.controls.currentPosition;

                    if (duration > 0)
                    {
                        var progressControls = this.Controls.Find("progressBar", true);
                        if (progressControls.Length > 0)
                        {
                            TrackBar progressBar = (TrackBar)progressControls[0];
                            progressBar.Value = (int)((position / duration) * 100);
                        }

                        var currentTimeControls = this.Controls.Find("currentTimeLabel", true);
                        if (currentTimeControls.Length > 0)
                        {
                            Label currentTimeLabel = (Label)currentTimeControls[0];
                            currentTimeLabel.Text = FormatTime(position);
                        }

                        var totalTimeControls = this.Controls.Find("totalTimeLabel", true);
                        if (totalTimeControls.Length > 0)
                        {
                            Label totalTimeLabel = (Label)totalTimeControls[0];
                            totalTimeLabel.Text = FormatTime(duration);
                        }
                    }
                }
                catch { }
            }
        }

        private string FormatTime(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return string.Format("{0}:{1:D2}", (int)time.TotalMinutes, time.Seconds);
        }

        private async void RecommendBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentSongName))
            {
                MessageBox.Show("Please play a song first!", "No Song Playing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (playlist.Count < 2)
            {
                MessageBox.Show("Please add more songs to your playlist for better recommendations!", "Playlist Too Small", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Button btn = (Button)sender;
            string originalText = btn.Text;
            btn.Enabled = false;
            btn.Text = "⏳ Analyzing...";

            try
            {
                string recommendation = await GetLastFMRecommendation();

                var recommendLabelControls = this.Controls.Find("recommendLabel", true);
                if (recommendLabelControls.Length > 0)
                {
                    Label recommendLabel = (Label)recommendLabelControls[0];
                    recommendLabel.Text = recommendation;
                    recommendLabel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                var recommendLabelControls = this.Controls.Find("recommendLabel", true);
                if (recommendLabelControls.Length > 0)
                {
                    Label recommendLabel = (Label)recommendLabelControls[0];
                    recommendLabel.Text = $"⚠️ Error getting recommendations:\n{ex.Message}\n\nTip: Make sure your Last.fm API key is set correctly.";
                    recommendLabel.Visible = true;
                }
            }
            finally
            {
                btn.Enabled = true;
                btn.Text = originalText;
            }
        }

        private string GetDemoRecommendation()
        {
            // Find songs similar to current song
            var currentSong = playlist.FirstOrDefault(s => s.SongName == currentSongName);
            if (currentSong != null)
            {
                // Recommend songs from same artist or with similar names
                var recommendations = playlist
                    .Where(s => s.SongName != currentSongName)
                    .Where(s => s.Artist == currentArtist ||
                               s.SongName.Split(' ').Intersect(currentSongName.Split(' ')).Any())
                    .Take(3)
                    .ToList();

                if (recommendations.Any())
                {
                    string result = $"🎵 AI Recommendation based on '{currentSongName}' by {currentArtist}:\n\n";
                    result += "From your playlist, you might enjoy:\n";
                    foreach (var rec in recommendations)
                    {
                        result += $"• {rec.DisplayName}\n";
                    }
                    result += "\nDouble-click to play!";
                    return result;
                }
            }

            // Random recommendation if no similar songs
            var random = playlist.Where(s => s.SongName != currentSongName).OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            if (random != null)
            {
                return $"🎵 AI Suggestion: Try '{random.DisplayName}' next!\n\nNote: Connect Claude API for smarter recommendations based on music analysis.";
            }

            return "Add more songs to get personalized recommendations!";
        }

        private async Task<string> GetLastFMRecommendation()
        {
            if (LASTFM_API_KEY == "YOUR_LASTFM_API_KEY_HERE")
            {
                return "⚠️ Last.fm API key not configured!\n\n" +
                       "Get your free API key at:\nhttps://www.last.fm/api/account/create\n\n" +
                       "Then replace YOUR_LASTFM_API_KEY_HERE in the code.";
            }

            try
            {
                // Get similar tracks from Last.fm
                string artistEncoded = Uri.EscapeDataString(currentArtist);
                string trackEncoded = Uri.EscapeDataString(currentSongName);
                string url = $"http://ws.audioscrobbler.com/2.0/?method=track.getsimilar&artist={artistEncoded}&track={trackEncoded}&api_key={LASTFM_API_KEY}&format=json&limit=20";

                var response = await httpClient.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"❌ Last.fm API Error: {response.StatusCode}\n\nPlease check your API key.";
                }

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

                // Check for errors
                if (jsonResponse.error != null)
                {
                    return $"❌ Last.fm Error: {jsonResponse.message}\n\nTry a different song or check your API key.";
                }

                // Get similar tracks
                if (jsonResponse.similartracks?.track != null)
                {
                    List<string> similarTracks = new List<string>();

                    foreach (var track in jsonResponse.similartracks.track)
                    {
                        string artist = track.artist?.name?.ToString() ?? "";
                        string name = track.name?.ToString() ?? "";

                        if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(name))
                        {
                            similarTracks.Add($"{artist} - {name}");
                        }
                    }

                    if (similarTracks.Count == 0)
                    {
                        return $"🎵 No similar tracks found for '{currentSongName}' by {currentArtist}\n\nTry another song!";
                    }

                    // Find matches in playlist
                    var matchesInPlaylist = new List<SongInfo>();
                    foreach (var song in playlist)
                    {
                        if (song.SongName == currentSongName && song.Artist == currentArtist)
                            continue; // Skip current song

                        string songDisplay = $"{song.Artist} - {song.SongName}";

                        // Check if song is in similar tracks
                        foreach (var similar in similarTracks)
                        {
                            if (songDisplay.ToLower().Contains(song.Artist.ToLower()) &&
                                similar.ToLower().Contains(song.Artist.ToLower()))
                            {
                                matchesInPlaylist.Add(song);
                                break;
                            }
                        }
                    }

                    // Build recommendation message
                    string result = $"🎵 Based on '{currentSongName}' by {currentArtist}\n\n";

                    if (matchesInPlaylist.Count > 0)
                    {
                        result += "✅ From YOUR playlist:\n";
                        foreach (var song in matchesInPlaylist.Take(3))
                        {
                            result += $"  • {song.DisplayName}\n";
                        }
                        result += "\nDouble-click to play!\n\n";
                    }

                    // Show top similar tracks from Last.fm
                    result += "🌐 Similar tracks on Last.fm:\n";
                    foreach (var track in similarTracks.Take(5))
                    {
                        result += $"  • {track}\n";
                    }

                    return result;
                }

                return "🎵 No recommendations found. Try a different song!";
            }
            catch (Exception ex)
            {
                return $"❌ Error: {ex.Message}\n\nMake sure your API key is correct.";
            }
        }

        private async Task<string> GetAIRecommendation()
        {
            // This method is kept for backward compatibility but now uses Last.fm
            return await GetLastFMRecommendation();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer?.Stop();
                player?.controls.stop();
                player?.close();
            }
            base.Dispose(disposing);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timer.Stop();
            player.controls.stop();
            player.close();
            base.OnFormClosing(e);
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
