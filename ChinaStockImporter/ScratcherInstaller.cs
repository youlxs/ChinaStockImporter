using System.ComponentModel;
using System.Configuration.Install;

namespace Yootech.ChinaStockImporter
{
    [RunInstaller(true)]
    public partial class ScratcherInstaller : Installer
    {
        public ScratcherInstaller()
        {
            this.InitializeComponent();
        }
    }
}