using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	private string _pwdText = "";
		
	private InputField _login;
	private InputField _password;
	
	public GameObject StartMenu;
	public GameObject Go;
	public GameObject RegisterPage;
	
	private void Start()
	{
		if (ConfigurationManager.Debug)
		{
			LocalStorage.Session = "PcK+EMtBhIF3jM3JkRS8nzst3jK1hQkPPmIGQVfNg1Nl9EyWS4Ubr6skhWrt6dmj";
			LocalStorage.HelpSession = "0e+Yo3bu3WTGEUj+XhbzpfL7WEuSMeu9PGiu2bzNEiIeQbPnc7FZWSwsDa45Xb52";
		}
		if (LocalStorage.Session != null)
		{
			StartMenu.SetActive(true);
			gameObject.SetActive(false);
			Go.GetComponent<StartMenuController>().Initialize();
		}
		if (!ConfigurationManager.Debug)
		{
			_login = GameObject.Find("LoginField").GetComponent<InputField>();
			_password = GameObject.Find("PasswordField").GetComponent<InputField>();
			_password.onValueChanged.AddListener(value =>
			{
				if (value.Length > _pwdText.Length)
				{
					_pwdText += value.Last();
				}
				if (value.Length < _pwdText.Length)
				{
					_pwdText = _pwdText.Substring(0, value.Length);
				}
				_password.text = new string('*', value.Length);
			});
		}
	}

	private void OnGUI()
	{
		if (_showIncorrectMsg)
		{
			GUI.Box(new Rect(Screen.width / 2 - 200 / 2, Screen.height / 2 - 200 / 2, 200, 200), "Incorrect email or password");
		}	
	}

	public void OpenSignUp()
	{
		RegisterPage.SetActive(true);
		gameObject.SetActive(false);
		RegisterPage.GetComponent<RegisterController>().Init();
	}

	public void Login()
	{
		StartCoroutine(LoginPost(_login.text, _pwdText));
	}

	private IEnumerator LoginPost(string login, string password)
	{
		var requestModel = new SignInRequestModel {Email = login, Password = password};
		var postData = JsonConvert.SerializeObject(requestModel);
		
		var www = new HttpRequest(ConfigurationManager.LoginUserUrl, postData, null).Request;
		yield return www.Send();
		string session = www.downloadHandler.text.Replace("\"", string.Empty);
		if (session != string.Empty)
		{
			LocalStorage.Session = session;
			StartMenu.SetActive(true);
			gameObject.SetActive(false);
			Go.GetComponent<StartMenuController>().Initialize();
		}
		else
		{
			_showIncorrectMsg = true;
			yield return new WaitForSeconds(1);
			_showIncorrectMsg = false;
		}
	}
}
