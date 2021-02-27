using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer;
        SoundPlayer player = new SoundPlayer();
        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += TimerElapsed;

        }

        delegate void UpdateLable(Label lbl, string value);
        void UpdateDataLable(Label lbl, string value)
        {
            lbl.Content = value;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                DateTime currentTime = DateTime.Now;
                DateTime userTime = (DateTime)dateTimePicker.Value;

                if (currentTime.Hour == userTime.Hour && currentTime.Minute == userTime.Minute && currentTime.Second == userTime.Second)
                {
                    timer.Stop();
                    try
                    {
                        UpdateLable upd = UpdateDataLable;
                        
                        player.SoundLocation = @"E:\Різне\samsung_galaxy_morning_flower-namobilu.com.wav";
                        player.PlayLooping();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
            
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            lblStatus.Content = "Running...";
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            lblStatus.Content = "Stop";
            player.Stop();
        }
    }
}
