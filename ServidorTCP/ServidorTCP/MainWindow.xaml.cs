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

        IPAddress IPServidor = IPAddress.Parse("127.0.0.1");
        Thread receptor;
        TcpListener rx;
        public DispatcherTimer TimerJanela;
        Socket s = null;


        List<string> linhas = new List<string>();
        int linhaAtual = 0;
        

        public MainWindow()
        {
            InitializeComponent();
            TimerJanela = new DispatcherTimer();
        }

        private void JanelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
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
            try
            {
                s = rx.AcceptSocket();

                if (s.Connected)
                {
                    linhas.Add("Cliente Conectado ao Servidor!");
                }

                while (true)
                {
                    if (!(s.Poll(1, SelectMode.SelectRead) && s.Available == 0))
                    {
                        byte[] b = new byte[100];
                        int k = s.Receive(b);
                        var builder = new StringBuilder();

                        for (int i = 0; i < k; i++)
                            builder.Append(Convert.ToChar(b[i]));

                        if(builder.ToString() == "")
                        {
                            linhas.Add("Cliente desconectado do Servidor!");
                            rx.Stop();
                            break;
                        }
                        else
                            linhas.Add(builder.ToString());
                    }
                    else
                    {
                        rx.Stop();
                        break;
                    }
                    Thread.Sleep(1000);
                }


            }
            catch(Exception exeptionCaptura)
            {
               //linhas.Add("Falha na captura: " + exeptionCaptura.StackTrace);
            }        
        }

        private void BTEnvia_Click(object sender, RoutedEventArgs e)
        {
            ASCIIEncoding codificacao = new ASCIIEncoding();
            String linha = TBEnvia.Text;
            s.Send(codificacao.GetBytes(linha));
            TBEnvia.Text = "";
        }

        private void JanelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void BTIniciaServidor_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                BTIniciaServidor.IsEnabled = false;
                BTEncerraServidor.IsEnabled = true;
                TBEnvia.IsEnabled = true;
                BTEnvia.IsEnabled = true;
                rx = null;
                rx = new TcpListener(IPServidor, 8001);
                rx.Start();
                receptor = new Thread(Captura);
                receptor.Start();
                linhas.Add("Servidor Iniciado!");
            }
            catch (Exception exeptionServidor)
            {
                linhas.Add("Falha na criação do Servidor: " + exeptionServidor.StackTrace);

            }

        }

        private void BTEncerraServidor_Click(object sender, RoutedEventArgs e)
        {

            try
            {
               
                if (s.Connected)
                {
                    ASCIIEncoding codificacao = new ASCIIEncoding();
                    String linha = "a0";
                    s.Send(codificacao.GetBytes(linha));
                }
                rx.Stop();
                receptor.Abort();
                BTIniciaServidor.IsEnabled = true;
                BTEncerraServidor.IsEnabled = false;
                TBEnvia.IsEnabled = false;
                BTEnvia.IsEnabled = false;
                linhas.Add("Servidor Encerrado!");
            }
            catch(Exception exceptionEncerraServidor)
            {
                //linhas.Add("Falha ao desligar o servidor: " + exceptionEncerraServidor.StackTrace);
            }
        }

        private void TBEnvia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                BTEnvia_Click(sender, e);
        }
    }
}