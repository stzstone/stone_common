using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputFieldBlock : MonoBehaviour 
{
	public Text _text;
	public Text _placeText;

	public string text { set { _text.text = value; } get { return _text.text; } }
	public string placeText { get { return _placeText.text; } }
}
