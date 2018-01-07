using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Models;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{

	private bool _showIncorrectMsg;
	
	private InputField _login;
	private InputField _password;
	public GameObject StartMenu;
	public GameObject Go;
	private void Start()
	{
		if (ConfigurationManager.Debug)
		{
			LocalStorage.PlayerId = new Guid("71dc126b-f804-4cd5-93ec-dfa5087ba2da");
			LocalStorage.HelpPlayerId = new Guid("c4920571-89a1-43cc-9888-b267b415bf40");
		}
		if (LocalStorage.PlayerId != Guid.Empty)
		{
			StartMenu.SetActive(true);
			gameObject.SetActive(false);
			Go.GetComponent<StartMenuController>().LoadExp();
		}
		if (!ConfigurationManager.Debug)
		{
			_login = GameObject.Find("LoginField").GetComponent<InputField>();
			_password = GameObject.Find("PasswordField").GetComponent<InputField>();
		}

	}

	private void OnGUI()
	{
		if (_showIncorrectMsg)
		{
			GUI.Box(new Rect(Screen.width / 2 - 200 / 2, Screen.height / 2 - 200 / 2, 200, 200), "Incorrect email or password");
		}	
	}

	public void Login()
	{
		StartCoroutine(LoginPost(_login.text, _password.text));
	}

	private IEnumerator LoginPost(string login, string password)
	{
		var requestModel = new SignInRequestModel {Email = login, Password = password};
		var postData = JsonConvert.SerializeObject(requestModel);
		Dictionary<string,string> headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json");
 
		byte[] pData = Encoding.ASCII.GetBytes(postData.ToCharArray());
		
		WWW www = new WWW(ConfigurationManager.LoginUserUrl, pData, headers);
		yield return www;
		Guid id = new Guid(www.text.Replace("\"", String.Empty));
		if (id != Guid.Empty)
		{
			LocalStorage.PlayerId = id;
			StartMenu.SetActive(true);
			gameObject.SetActive(false);
			Go.GetComponent<StartMenuController>().LoadExp();
		}
		else
		{
			_showIncorrectMsg = true;
			yield return new WaitForSeconds(1);
			_showIncorrectMsg = false;
		}
	}
}
