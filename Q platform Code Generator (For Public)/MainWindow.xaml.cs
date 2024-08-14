using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using QP_Code_Generator.Model;
using QP_Code_Generator.ViewModel;

namespace QP_Code_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            Messenger.Default.Register<string>(this, Msg.VM, ShowMsg);

            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }


        private void ShowMsg(string strMsg)
        {
            MessageBoxImage mboxImg;

            if (strMsg.ToUpper().Contains(StaticVar.ERROR))
                mboxImg = MessageBoxImage.Error;
            else if (strMsg.ToUpper().Contains(StaticVar.WARNING))
                mboxImg = MessageBoxImage.Warning;
            else
                mboxImg = MessageBoxImage.Information;

            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(this, strMsg, StaticVar.TITLE, MessageBoxButton.OK, mboxImg);
            });
        }
    }
}