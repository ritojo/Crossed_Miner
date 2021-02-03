using System.Windows.Controls;

namespace Crossed_Miner
{
    /// <summary>
    /// Interaction logic for MinerView.xaml
    /// </summary>
    public partial class MinerView : UserControl
    {
        public MinerView()
        {
            DataContext = new MinerViewModel();
            InitializeComponent();
        }
    }
}
