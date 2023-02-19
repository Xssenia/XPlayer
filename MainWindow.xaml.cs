using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace XMusic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PauseButton.Visibility = Visibility.Hidden;
            Repeat1Button.Visibility = Visibility.Hidden;
            ShuffleOffButton.Visibility = Visibility.Hidden;
            TreckButton.Visibility = Visibility.Hidden;
        }

        private string[] files;
        List<string> songsList = new();

        private void СhoiceDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChoiceDir();
        }

        private void Lsongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChoiceSong();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            Forward();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Back();
        }

        private void RepeatAllButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatAllButton.Visibility = Visibility.Hidden;
            Repeat1Button.Visibility = Visibility.Visible;
        }

        private void Repeat1Button_Click(object sender, RoutedEventArgs e)
        {
            RepeatAllButton.Visibility = Visibility.Visible;
            Repeat1Button.Visibility = Visibility.Hidden;

        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            ShuffleButton.Visibility = Visibility.Hidden;
            ShuffleOffButton.Visibility = Visibility.Visible;
        }

        private void ShuffleButtonOff_Click(object sender, RoutedEventArgs e)
        {
            ShuffleButton.Visibility = Visibility.Visible;
            ShuffleOffButton.Visibility = Visibility.Hidden;
        }

        private void СhoiceTreckButton_Click(object sender, RoutedEventArgs e)
        {
            СhoiceTreckButton.Visibility = Visibility.Hidden;
            TreckButton.Visibility = Visibility.Visible;
        }

        private void TreckButton_Click(object sender, RoutedEventArgs e)
        {
            СhoiceTreckButton.Visibility = Visibility.Visible;
            TreckButton.Visibility = Visibility.Hidden;
        }







        private void ChoiceDir()
        {
            Lsongs.Items.Clear();

            CommonOpenFileDialog dialog = new() { IsFolderPicker = true };
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                files = Directory.GetFiles(dialog.FileName);
                foreach (var file in files)
                {
                    if (System.IO.Path.GetExtension(file) == ".mp3")
                    {
                        songsList.Add(file);
                        Lsongs.Items.Add(System.IO.Path.GetFileName(file));
                    }
                }
            }

            Lsongs.SelectedIndex = 0;
            media.Source = new Uri(songsList[0]);
            Play();
        }

        private void ChoiceSong()
        {
            media.Source = new Uri(songsList[Lsongs.SelectedIndex]);
            Play();
        }

        private void Play()
        {
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Hidden;
            media.Play();
        }

        private void Pause()
        {
            PauseButton.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Visible;
            media.Pause();
        }

        private void Forward()
        {
            if (Lsongs.SelectedIndex < Lsongs.Items.Count - 1)
            {
                media.Stop();
                Lsongs.SelectedIndex += 1;
                ChoiceSong();
            }
            else
            {
                media.Stop();
                Lsongs.SelectedIndex = 0;
                ChoiceSong();
            }
        }

        private void Back()
        {
            if (Lsongs.SelectedIndex == 0)
            {
                media.Stop();
                Lsongs.SelectedIndex = Lsongs.Items.Count - 1;
                ChoiceSong();
            }
            else 
            {
                media.Stop();
                Lsongs.SelectedIndex -= 1;
                ChoiceSong();
            }
        }
    }
}
