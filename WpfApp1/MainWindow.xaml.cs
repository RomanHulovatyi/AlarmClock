using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
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
using System.Xml.Serialization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer;
        SoundPlayer player = new SoundPlayer();
        public ObservableCollection<DateTime> alarmClock = new ObservableCollection<DateTime>();
        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += TimerElapsed;
            
        }

        delegate void UpdateLable(Label statusLbl, string value);
        void UpdateDataLable(Label statusLbl, string value)
        {
            statusLbl.Content = value;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            foreach(var c in alarmClock)
            {
                this.Dispatcher.Invoke(() =>
                {
                    DateTime currentTime = DateTime.Now;
                    DateTime userTime = c;
                    //alarmClock.Add(userTime);
                    //AlarmsList.ItemsSource = alarmClock;

                    if (currentTime.Hour == userTime.Hour && currentTime.Minute == userTime.Minute && currentTime.Second == userTime.Second)
                    {
                        timer.Stop();
                        try
                        {
                            UpdateLable upd = UpdateDataLable;

                            player.SoundLocation = ConfigurationManager.AppSettings["Path"];
                            player.PlayLooping();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }            
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            DateTime userTime = (DateTime)dateTimePicker.Value;
            timer.Start();
            lblStatus.Content = "Running...";
            alarmClock.Add(userTime);

            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DateTime>));

            TextWriter txtWriter = new StreamWriter(ConfigurationManager.AppSettings["XmlPath"]);

            xs.Serialize(txtWriter, alarmClock);

            txtWriter.Close();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            lblStatus.Content = "Stop";
            player.Stop();
            timer.Start();
        }

        private void btnPostponed_Click(object sender, RoutedEventArgs e)
        {
            DateTime userTime = DateTime.Now.AddMinutes(1);
            lblStatus.Content = "Stop";
            player.Stop();
            timer.Start();
            lblStatus.Content = "Running...";
            alarmClock.Add(userTime);
        }

        private void AlarmClock_Loaded(object sender, RoutedEventArgs e)
        {
            string xmlString = ConfigurationManager.AppSettings["XmlPath"];
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<DateTime>));
            StreamReader reader = new StreamReader(xmlString);
            alarmClock = (ObservableCollection<DateTime>)serializer.Deserialize(reader);
            reader.Close();
            List.ItemsSource = alarmClock;
        }
    }
}
