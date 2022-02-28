using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OutputText : MonoBehaviour 
{
	public Text text;
	public void Clear()
	{
		text.text = string.Empty;
	}
}
