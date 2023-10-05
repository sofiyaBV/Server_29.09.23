using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
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

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int port = 8080;
        static string ip = "127.0.0.1";
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        Socket socket;


        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(ipPoint);
                socket.Listen();    // запускаем сервер
                tb_client.Text = "Сервер запущен. Ожидание подключений...";

                Socket client = await socket.AcceptAsync();
                tb_client.Text = "Клиент подключён...";

                byte[] data = new byte[512];
                while (true)
                {
                    int bytes = await client.ReceiveAsync(data, SocketFlags.None);
                    string receivedMessage = Encoding.UTF8.GetString(data, 0, bytes);
                    tb_client.Text = $"Получено сообщение от клиента: {receivedMessage}";

                    // Отправляем приветственное сообщение клиенту
                    string responseMessage = "Привет, клиент!";
                    byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
                    await client.SendAsync(responseData, SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}
