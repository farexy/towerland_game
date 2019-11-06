using System.Collections;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Assets.Scripts.Network;
using UnityEngine;

public class Initializer : MonoBehaviour {
	private void Awake()
	{
		SetServerUrlAsync().Wait();
	}

	private async Task SetServerUrlAsync()
	{
		if (ConfigurationManager.Debug)
		{
			using (var reader = new StreamReader(Application.dataPath + "/StreamingAssets/server.txt"))
			{
				ConfigurationManager.Server = await reader.ReadLineAsync().ConfigureAwait(false);
			}
		}
		else
		{
			var client = new HttpClient();
			var response = await client.GetAsync(ConfigurationManager.ServerUrlStore).ConfigureAwait(false);
			ConfigurationManager.Server = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
		}
	}

}
