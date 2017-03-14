using System;
using System.Collections.Generic;
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
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace ClienteTCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        int porta = 33333;
        IPEndPoint cliente;
        Socket socketComunicacao;
        Thread receptor;
        public static ManualResetEvent allDone;

        public MainWindow()
        {
            InitializeComponent();
            cliente = new IPEndPoint(ip, porta);
            socketComunicacao = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketComunicacao.Bind(cliente);
            socketComunicacao.Listen(100);
        }

        private void JanelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            receptor = new Thread(Captura);
            receptor.Start();
        }

        public void Captura()
        {
            while (true)
            {
                

            }
        }
    }
}
