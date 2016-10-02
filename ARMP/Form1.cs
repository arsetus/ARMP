using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARMP
{
    public partial class MainWindow : Form
    {
       
        public MainWindow()
        {
            
            InitializeComponent();
            label1.Text = "";
            timer1.Stop();
            song_control.song_info(s_index,current_song);
        }
        struct song
        {
            public string[] s_name;
            public string song_path;
            
        }
        private int s_index = 0, ssdd = 0, current_song = 0 ;
        private bool s_i_change, is_loaded, rand_checked = false, first_song_selected = false;
        private string dostringa (string[] s)
        {
            string ss = "";
            for (int i = 0; i < s.Length; i++)
            {
                ss += s[i];
            }
            return ss;
        }

        private int first_song()
        {
            if (first_song_selected == false && s_index >= 1)
            {
                axWindowsMediaPlayer1.URL = files[0].song_path;
                listBox1.SelectedIndex = 0;
                label1.Text = dostringa(files[0].s_name);
                first_song_selected = true;
            }
            return 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            s_i_change = true;
            current_song = listBox1.SelectedIndex;
            song_control.song_info(s_index, current_song);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {    
            
            label2.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            trackBar1.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (is_loaded)
            {
               
                int song_index;
                if (rand_checked)
                {
                    song_index = song_control.nextSong(rand_checked);
                }
                else
                {
                    if (listBox1.SelectedIndex + 1 >= s_index)
                    {
                        song_index = 0;
                    }
                    else
                    {
                        song_index = listBox1.SelectedIndex + 1;
                    }
                }
                axWindowsMediaPlayer1.URL = files[song_index].song_path;
                label1.Text = dostringa(files[song_index].s_name);
                listBox1.SelectedIndex = song_index;
            }
        }
        private song[] files = new song[10];
        
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.URL = files[listBox1.SelectedIndex].song_path;
            label1.Text = dostringa(files[listBox1.SelectedIndex].s_name);
            first_song_selected = true;
        }
        string[] f, p;

        private void button2_Click(object sender, EventArgs e)
        {
            first_song();
            if (is_loaded)
            {
                if (s_i_change)
                {
                    axWindowsMediaPlayer1.URL = files[listBox1.SelectedIndex].song_path;
                    label1.Text = dostringa(files[listBox1.SelectedIndex].s_name);
                    s_i_change = false;
                }
                else
                {
                    if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                        axWindowsMediaPlayer1.Ctlcontrols.pause();
                    else
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (is_loaded)
            {
                int song_index;
                if (listBox1.SelectedIndex - 1 < 0)
                {
                    song_index = s_index - 1;
                }
                else
                {
                    song_index = listBox1.SelectedIndex - 1;
                }
                axWindowsMediaPlayer1.URL = files[song_index].song_path;
                label1.Text = dostringa(files[song_index].s_name);
                listBox1.SelectedIndex = song_index;
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            first_song_selected = true;
            is_loaded = true;
            s_i_change = true;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = hScrollBar1.Value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                timer1.Stop();
                label2.Text = "00:00";
                trackBar1.Value = 0;
            }
            else axWindowsMediaPlayer1.Ctlcontrols.play();
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if(is_loaded)
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = trackBar1.Value;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = files[ssdd].song_path; ;
            if (s_index >= 0)
            {
                
                label1.Text = dostringa(files[listBox1.SelectedIndex].s_name);
            }
            timer2.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(rand_checked)
            { rand_checked = false; }
            else
            { rand_checked = true; }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                ssdd = song_control.nextSong(rand_checked);
                label1.Text = ssdd.ToString();
                listBox1.SelectedIndex = ssdd;
                timer2.Enabled = true;
                timer2.Start();

            }
            if (is_loaded != true) is_loaded = true;
            trackBar1.Maximum = (int)axWindowsMediaPlayer1.currentMedia.duration;
            label3.Text = axWindowsMediaPlayer1.currentMedia.durationString;
            timer1.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                f = fileDialog.SafeFileNames;
                files[s_index].s_name = f;
                p = fileDialog.FileNames;
                files[s_index].song_path = p[0];

                foreach (string fileName in fileDialog.SafeFileNames)
                {
                    if (fileName == fileDialog.SafeFileNames[0])
                    {
                        listBox1.Items.Add(fileName.Replace(fileName, (s_index+1) + ": "  + fileName));
                    }
                    else
                    {
                        listBox1.Items.Add(fileName.Replace(".mp3", ""));
                    }
                }
            }
            s_index++;
            song_control.song_info(s_index,current_song);
        }
    }
}
