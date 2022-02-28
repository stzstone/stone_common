using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonBlock : MonoBehaviour 
{
	public Text textUI;
	public string text { get { return textUI.text; } set { textUI.text = value; } }

	public void AddListener(UnityEngine.Events.UnityAction call)
	{
		GetComponent<Button>().onClick.AddListener(call);
	}
}
