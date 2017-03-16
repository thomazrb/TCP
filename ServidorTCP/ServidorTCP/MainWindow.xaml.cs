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
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;

namespace ServidorTCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        IPAddress ipAd = IPAddress.Parse("127.0.0.1");
        Thread receptor;
        TcpListener myList;
        string alpha = "";
        public DispatcherTimer TimerJanela;

        List<string> linhas = new List<string>();
        int linhaAtual = 0;

        public MainWindow()
        {
            InitializeComponent();
            TimerJanela = new DispatcherTimer();
            
        }

        private void JanelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            receptor = new Thread(Captura);
            receptor.Start();
            TimerJanela.Tick += new System.EventHandler(AtualizaJanela);
            TimerJanela.Interval = new System.TimeSpan(0, 0, 0, 0,50); 
            TimerJanela.Start();

        }
        void AtualizaJanela(object sender, EventArgs e)
        {
            if (linhaAtual < linhas.Count())
            {
                for (int i = linhaAtual; i < linhas.Count(); i++)
                {
                    TBLog.AppendText(linhas[i] + '\n');
                    linhaAtual++;
                }
                TBLog.ScrollToEnd();
            }
        }


        public void Captura()
        {
            myList = new TcpListener(ipAd, 8001);
            Socket s;
            myList.Start();
            while (true)
            {

                s = myList.AcceptSocket();
                byte[] b = new byte[100];
                int k = s.Receive(b);   
                var builder = new StringBuilder();

                for (int i = 0; i < k; i++)
                     builder.Append(Convert.ToChar(b[i]));

                linhas.Add(builder.ToString());
                s.Close();
            }
            myList.Stop();
        }

        private void BTEnvia_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}