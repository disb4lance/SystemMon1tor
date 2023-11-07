using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SystemMonitor
{
    /// <summary>
    /// Логика взаимодействия для monitor.xaml
    /// </summary>
    public partial class monitor : Window
    {
        public monitor()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;
            this.MouseMove += Window_MouseMove;
            functions Functions = new functions();
        }
        private bool isDragging = false;
        private Point lastPosition;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            lastPosition = e.GetPosition(this);
            this.CaptureMouse();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPosition = e.GetPosition(this);
                double deltaX = currentPosition.X - lastPosition.X;
                double deltaY = currentPosition.Y - lastPosition.Y;
                this.Left += deltaX;
                this.Top += deltaY;
                lastPosition = currentPosition;
            }
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            isDragging = false;
            this.ReleaseMouseCapture();
        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
                float CPU = functions.GetCpuUsage();
                List<string> diskinfo = functions.DiscInfo();
                List<string> operatingSystem = functions.Operating_System();
                List<string> network = functions.DataTransmissionAndReception();
                //ProcessDisplay();
                cpu.Text = "Загрузка ЦПУ: " + CPU + "%";
                diskname.Text = "Имя диска: " + diskinfo[0];
                all.Text = "Всего: " + diskinfo[1] + " Гб";
                zanyato.Text = "Свободно: " + diskinfo[3] + " Гб";
                free.Text = "Занято: " + diskinfo[2] + " Гб";
                system.Text = "Операционная система: " + operatingSystem[0];
                name_pc.Text = "Имя компьютера: " + operatingSystem[1];
                count.Text = "Число процессоров: " + operatingSystem[2];
                ethernet.Text = network[0];
                speedon.Text = "Скорость передачи: " + network[1] + " Мб";
                speedout.Text = "Скорость приема: " + network[2] + " Мб";
                wifi.Text = network[3];
                speedon2.Text = "Скорость передачи: " + network[4] + " Мб";
                speedout2.Text = "Скорость приема: " + network[5] + " Мб";
        }
    }
}
