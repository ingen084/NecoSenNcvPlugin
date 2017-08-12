using System;
using Plugin;
using System.Windows.Forms;
using System.Linq;

namespace NecoSenNcvPlugin
{
	public class NcvPlugin : IPlugin
	{
		public IPluginHost Host { get; set; }
		public bool IsAutoRun => true;

		public string Name => "&NECOSen";
		public string Description => "NCVからNECOSenに接続しやすくするプラグインです。";
		public string Version => "0";

		public void AutoRun()
		{
			var pluginsMenu = (ToolStripMenuItem)Host.MainForm.MainMenuStrip.Items.Find("プラグイン(&P)", false).First();
			var pluginMenu = (ToolStripMenuItem)pluginsMenu.DropDownItems.Find(Name, false).First();
			pluginMenu.DropDownItems.Add("東方ch");
		}

		public void Run()
		{
		}
	}
}
