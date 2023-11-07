using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
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

namespace SystemMonitor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void registration_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                // Получите выбранный путь к файлу
                string filePath = saveFileDialog.FileName;
                SecureString securePassword = password.SecurePassword;

                // Преобразуйте SecureString в обычную строку
                IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(securePassword);
                string password1 = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);

                string login1 = login.Text;

                try
                {
                    // Используйте StreamWriter для записи данных в выбранный файл
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        if (login1.Length <= 8 && password1.Length == 8)
                        {
                            writer.WriteLine(login1);
                            writer.Write(password1);
                        }
                        else {
                            MessageBox.Show($"проверьте корректность данных");
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных в файл: {ex.Message}");
                }
            }
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Установите фильтр для выбора только текстовых файлов
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            // Откройте диалоговое окно открытия файла и получите результат
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // Получите путь к выбранному файлу
                string filePath = openFileDialog.FileName;

                try
                {
                    // Используйте StreamReader для чтения данных из выбранного файла
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line1 = reader.ReadLine();
                        string line2 = reader.ReadLine();
                        if ((!string.IsNullOrEmpty(line1)) && (!string.IsNullOrEmpty(line2)))
                        {
                            if (line1.Length <= 8 && line2.Length == 8)
                            {
                                monitor NewWindow = new monitor();
                                NewWindow.Show();
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных из файла: {ex.Message}");
                }
            }
        }
    }
}
