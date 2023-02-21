using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Threading;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace XMusic;

public partial class MainWindow : Window
{
    bool isRepeatButtonOn = false;
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
        isRepeatButtonOn = true;
    }

    private void Repeat1Button_Click(object sender, RoutedEventArgs e)
    {
        RepeatAllButton.Visibility = Visibility.Visible;
        Repeat1Button.Visibility = Visibility.Hidden;
        isRepeatButtonOn = false;
    }

    private void ShuffleButton_Click(object sender, RoutedEventArgs e)
    {
        ShuffleButton.Visibility = Visibility.Hidden;
        ShuffleOffButton.Visibility = Visibility.Visible;
        // ShuffleOn();
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
        Lsongs.Visibility = Visibility.Hidden;
        ImageBox.Visibility = Visibility.Visible;
        
    }

    private void TreckButton_Click(object sender, RoutedEventArgs e)
    {
        СhoiceTreckButton.Visibility = Visibility.Visible;
        TreckButton.Visibility = Visibility.Hidden;
        ImageBox.Visibility = Visibility.Hidden;
        Lsongs.Visibility = Visibility.Visible;
    }

    private void Media_MediaOpened(object sender, RoutedEventArgs e)
    {
        Label2.Content = media.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
    }

    private void Media_MediaEnded(object sender, RoutedEventArgs e)
    {
        if (isRepeatButtonOn)
        {
            ChoiceSong();
        }
        else
        {
            Forward();
        }
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

        ShowImg();
        ShowTime();
    }

    private void ShowImg()
    {
        try
        {
            TagLib.File song = new TagLib.Mpeg.AudioFile(songsList[Lsongs.SelectedIndex]);
            TagLib.IPicture img = song.Tag.Pictures[0];
            MemoryStream memStream = new(img.Data.Data);
            
            BitmapImage bi = new();
            bi.BeginInit();
            bi.StreamSource = memStream;
            bi.EndInit();

            ImageBox.Source = bi;
        }
        catch
        {
            // ну и не очень то и хотелось
        }
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

    private void ShowTime()
    {
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += timerTick;
        timer.Start();
    }

    private void timerTick(object sender, EventArgs e)
    {
        Label1.Content = media.Position.ToString(@"mm\:ss");
    }

    /*private void ShuffleOn()
    {
        Random rand = new Random();
        for (int i = songsList.Count - 1; i > 0; i--)
        {
            int a = rand.Next(i);
            string tr = songsList[i];
            songsList[i] = songsList[a];
            songsList[a] = tr;
        }

        media.Stop();
        Lsongs.Items.Clear();
        foreach (var song in songsList)
        {
            Lsongs.Items.Add(System.IO.Path.GetFileName(song));
        }
        Lsongs.SelectedIndex = 0;
        media.Source = new Uri(songsList[0]);
        Play();
    }*/
}