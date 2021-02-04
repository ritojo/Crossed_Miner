using System.Windows.Controls;

namespace Crossed_Miner
{
    /// <summary>
    /// Interaction logic for MonitorView.xaml
    /// </summary>
    public partial class MonitorView : UserControl
    {
        public MonitorView()
        {
            DataContext = new MonitorViewModel();
            InitializeComponent();
        }
    }
}
