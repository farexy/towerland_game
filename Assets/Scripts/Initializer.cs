using System.IO;
using Assets.Scripts.Network;
using UnityEngine;

public class Initializer : MonoBehaviour {
	private void Awake()
	{
		using (var reader = new StreamReader(Application.dataPath + "/StreamingAssets/server.txt"))
		{
			ConfigurationManager.Server = reader.ReadLine();
		}
	}

}
