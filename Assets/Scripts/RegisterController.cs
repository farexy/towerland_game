using System.Collections;
using System.Linq;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Models;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class RegisterController : MonoBehaviour
{
	public GameObject MainMenu;
	public GameObject Go;

	private string _pwdText;
	private string _repeatPwdText;

	private InputField _passwordField;
	private InputField _repeatPasswordField;
	private InputField _nameField;
	private InputField _emailField;
	private InputField _nicknameField;

	private Text _pwdErrText;
	private Text _repeatPwrErrText;
	private Text _nameErr;
	private Text _emailErr;
	private Text _nickErrText;

	public void Init()
	{
		_pwdText = "";
		_repeatPwdText = "";

		_passwordField = GameObject.Find("PasswordField").GetComponent<InputField>();
		_repeatPasswordField = GameObject.Find("RepeatPasswordField").GetComponent<InputField>();
		_nameField = GameObject.Find("NameField").GetComponent<InputField>();
		_emailField = GameObject.Find("EmailField").GetComponent<InputField>();
		_nicknameField = GameObject.Find("NicknameField").GetComponent<InputField>();

		_pwdErrText = GameObject.Find("PwdErrorText").GetComponent<Text>();
		_repeatPwrErrText = GameObject.Find("RepeatPwdErrorText").GetComponent<Text>();
		_nameErr = GameObject.Find("NameErrorText").GetComponent<Text>();
		_emailErr = GameObject.Find("EmailErrorText").GetComponent<Text>();
		_nickErrText = GameObject.Find("NicknameErrorText").GetComponent<Text>();
		
		_passwordField.onValueChanged.AddListener(value =>
		{
			if (value.Length > _pwdText.Length)
			{
				_pwdText += value.Last();
			}
			if (value.Length < _pwdText.Length)
			{
				_pwdText = _pwdText.Substring(0, value.Length);
			}
			_passwordField.text = new string('*', value.Length); 
		});
		_repeatPasswordField.onValueChanged.AddListener(value =>
		{
			if (value.Length > _repeatPwdText.Length)
			{
				_repeatPwdText += value.Last();
			}
			if (value.Length < _repeatPwdText.Length)
			{
				_repeatPwdText = _repeatPwdText.Substring(0, value.Length);
			}
			_repeatPasswordField.text = new string('*', value.Length); 
		});
	}

	public void Send()
	{
		_pwdErrText.text = "";
		_repeatPwrErrText.text = "";
		_nameErr.text = "";
		_emailErr.text = "";
		_nickErrText.text = "";
		bool valid = true;

		if (string.IsNullOrEmpty(_emailField.text))
		{
			_emailErr.text = "Email has invalid format";
			valid = false;
		}
		if (string.IsNullOrEmpty(_nameField.text))
		{
			_nameErr.text = "Full name is invalid";
			valid = false;
		}
		if (string.IsNullOrEmpty(_nicknameField.text))
		{
			_nickErrText.text = "Nickname has invalid format";
			valid = false;
		}
		if (string.IsNullOrEmpty(_passwordField.text))
		{
			_pwdErrText.text = "Password is incorrect";
			valid = false;
		}
		if (_pwdText != _repeatPwdText)
		{
			_repeatPwrErrText.text = "Passwords must be equal";
			valid = false;
		}

		if (valid)
		{
			StartCoroutine(PostSignUp());
		}
	}

	private IEnumerator PostSignUp()
	{
		var postData = new SignUpRequestModel
		{
			FullName = _nicknameField.text,
			Email = _emailField.text,
			Nickname = _nicknameField.text,
			Password = _pwdText
		};
		var www = new WwwWrapper(ConfigurationManager.SignUpUserUrl, JsonConvert.SerializeObject(postData), null);
		yield return www.WWW;
		string session = www.WWW.text.Replace("\"", string.Empty);
		if (session != string.Empty)
		{
			LocalStorage.Session = session;
			MainMenu.SetActive(true);
			gameObject.SetActive(false);
			Go.GetComponent<StartMenuController>().LoadExp();
		}
	}
	
}
