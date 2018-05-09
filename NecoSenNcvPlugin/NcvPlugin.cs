using System;
using Plugin;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Serialization;
using System.Net;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Xml;

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
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			var pluginsMenu = Host.MainForm.MainMenuStrip.Items.OfType<ToolStripMenuItem>().First(mi => mi.Text == "プラグイン(&P)");
			var pluginMenu = pluginsMenu.DropDownItems.OfType<ToolStripMenuItem>().First(mi => mi.Text == Name);
			using (var reader = new XmlTextReader("https://ingen084.github.io/versioncheck/NECOSenNcvPlugin.xml"))
			{
				var serializer = new XmlSerializer(typeof(NecoSenPluginLiveInfos));
				var info = (NecoSenPluginLiveInfos)serializer.Deserialize(reader);
				if (info.Version > int.Parse(Version))
					if (MessageBox.Show("プラグインの更新があります。更新ページを開きますか？", "NECOSen NCVプラグイン", MessageBoxButtons.YesNo) == DialogResult.Yes)
						Process.Start("http" + info.UpdateUrl);
				foreach (var liveInfo in info.Infos)
					pluginMenu.DropDownItems.Add(new ToolStripMenuItem(liveInfo.Name, null, (s, e) => Host.ConnectLive(liveInfo.ChannelId)));
			}

		}

		public void Run()
		{
		}
	}

	public class NecoSenPluginLiveInfos
	{
		public int Version { get; set; }
		public string UpdateUrl { get; set; }
		[XmlArray("Lives")]
		[XmlArrayItem("Live")]
		public NecoSenPluginLiveInfo[] Infos { get; set; }
	}
	public class NecoSenPluginLiveInfo
	{
		[XmlAttribute]
		public string Name { get; set; }
		[XmlAttribute]
		public string ChannelId { get; set; }
	}
}
