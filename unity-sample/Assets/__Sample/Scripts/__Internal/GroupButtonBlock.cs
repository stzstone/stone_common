using UnityEngine;
using System.Collections;

public class GroupButtonBlock : ButtonBlock 
{
	public delegate void OnClickButtonDelegate(string s);
	public OnClickButtonDelegate _delegate;

	public void OnClick()
	{
		_delegate(text);
	}
}
