﻿using System;
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


namespace ClienteTCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }

        private void JanelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BTEnvia_Click(object sender, RoutedEventArgs e)
        {
            TcpClient tcpclnt = new TcpClient();
            tcpclnt.Connect("127.0.0.1", 8001);
            
            String str = TBEnvia.Text;
            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
         
            stm.Write(ba, 0, ba.Length);

            tcpclnt.Close();
           
        }
 
    }

}
    