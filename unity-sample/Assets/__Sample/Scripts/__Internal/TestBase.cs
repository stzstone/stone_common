using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TestBase : MonoBehaviour
{
	public class GroupInfo
	{
		protected  string _groupName;
		public List<string> inputFields 									= new List<string>();
		public Dictionary<string, UnityEngine.Events.UnityAction> buttons 	= new Dictionary<string, UnityEngine.Events.UnityAction>();

		public string groupName { get { return _groupName; } }

		public GroupInfo(string inGroupName)
		{
			_groupName = inGroupName;
		}

		public void AddInputField(string name)
		{
			inputFields.Add(name);
		}

		public void AddButton(string name, UnityEngine.Events.UnityAction call)
		{
			buttons.Add(name, call);
		}
	}

	public GameObject buttonBlockTemplate;
	public GameObject groupButtonBlockTemplate;
	public GameObject inputFieldBlockTemplate;

	public Canvas _canvas;
	public GameObject _groupList;
	public GameObject _inputFieldList;
	public GameObject _buttonList;
	public GameObject _outputTextList;

	public List<InputFieldBlock> _inputFields = new List<InputFieldBlock>();
	public List<GameObject> _buttons = new List<GameObject>();

	protected Dictionary <string, GroupInfo> _groupInfos = new Dictionary<string, GroupInfo>();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

	/// <summary>
	/// 리스트 정리
	/// </summary>
	protected void ClearList()
	{
		for (int i = 0; i < _inputFields.Count; i++)
		{
			Destroy(_inputFields[i].gameObject);
		}

		for (int i = 0; i < _buttons.Count; i++)
		{
			Destroy(_buttons[i].gameObject);
		}

		_inputFields.Clear();
		_buttons.Clear();

		_outputTextList.GetComponent<OutputText>().Clear();
	}

	/// <summary>
	/// 화면 최상단 그룹 버튼 생성.
	/// </summary>
	/// <param name="groupInfo">그룹에 대한 정보.</param>
	protected void CreateGroup(GroupInfo groupInfo)
	{
		Transform tempParent = _groupList.GetComponent<ScrollRect>().content.transform;
		GameObject g = Instantiate(groupButtonBlockTemplate) as GameObject;
		g.transform.SetParent(tempParent, false);
		g.GetComponent<RectTransform>().localPosition = new Vector3(300 * _groupInfos.Count, 0, 0);
		g.GetComponent<GroupButtonBlock>()._delegate = OnClickGroupButton;
		g.GetComponent<GroupButtonBlock>().text = groupInfo.groupName;
		_groupInfos.Add(groupInfo.groupName, groupInfo);

		RectTransform rectTransform = _groupList.GetComponent<ScrollRect>().content.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(_groupInfos.Count * 300, rectTransform.sizeDelta.y);
	}

	/// <summary>
	/// 그룹 버튼을 눌렀을 때 리스트 정보 변경.
	/// </summary>
	/// <param name="name">그룹에 대한 정보.</param>
	private void OnClickGroupButton(string name)
	{
		ClearList();

		GroupInfo info = _groupInfos[name];

		Transform tempParent = _inputFieldList.GetComponent<ScrollRect>().content.transform;
		RectTransform rectTransform = _inputFieldList.GetComponent<ScrollRect>().content.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, info.inputFields.Count * 100);
		rectTransform.localPosition = new Vector3(rectTransform.localPosition.x,0,0);
		for (int i = 0; i < info.inputFields.Count; i++)
		{
			GameObject g = Instantiate(inputFieldBlockTemplate) as GameObject;
			g.transform.SetParent(tempParent, false);
			g.GetComponent<RectTransform>().localPosition = new Vector3(0, -100 * i, 0);
            InputFieldBlock target = g.GetComponent<InputFieldBlock>();
            target._placeText.text = info.inputFields[i];
            _inputFields.Add(target);
		}

		int idx = 0;
		tempParent = _buttonList.GetComponent<ScrollRect>().content.transform;
		rectTransform = _buttonList.GetComponent<ScrollRect>().content.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, info.buttons.Count * 100);
		rectTransform.localPosition = new Vector3(rectTransform.localPosition.x,0,0);
		foreach(KeyValuePair<string, UnityEngine.Events.UnityAction> pair in info.buttons)
		{
			GameObject g = Instantiate(buttonBlockTemplate) as GameObject;
			g.transform.SetParent(tempParent, false);
			g.GetComponent<Button>().onClick.AddListener(pair.Value);
			g.GetComponent<ButtonBlock>().text = pair.Key;
			g.GetComponent<RectTransform>().localPosition = new Vector3(0, -100 * idx, 0);
			_buttons.Add(g);
			idx++;
		}
	}

    /// <summary>
    /// OUTPUT TEXT 기록.
    /// </summary>
    /// <param name="s">내용.</param>
    protected void WriteOutputText(string s)
	{
        Text target = _outputTextList.GetComponent<OutputText>().text;
        target.text = s;
	}

	/// <summary>
	/// OUTPUT TEXT 초기화.
	/// </summary>
	protected void ClearOutputText()
	{
        Text target = _outputTextList.GetComponent<OutputText>().text;
        target.text = string.Empty;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inIndex"></param>
    /// <param name="inDefault"></param>
    /// <param name="inUsePlaceText"></param>
    /// <returns></returns>
    protected int GetParamAsInt(int inIndex, int inDefault = 0, bool inUsePlaceText = true)
    {
        string source = (inIndex < _inputFields.Count ? (_inputFields[inIndex].text.Length > 0 ? _inputFields[inIndex].text : (inUsePlaceText ? _inputFields[inIndex].placeText : null)) : null);

        if(source == null || source.Length == 0)
        {
            return inDefault;
        }

        return int.Parse(source);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inIndex"></param>
    /// <param name="inDefault"></param>
    /// <param name="inUsePlaceText"></param>
    /// <returns></returns>
    protected string GetParamAsString(int inIndex, string inDefault = null, bool inUsePlaceText = true)
    {
        string source = (inIndex < _inputFields.Count ? (_inputFields[inIndex].text.Length > 0 ? _inputFields[inIndex].text : (inUsePlaceText ? _inputFields[inIndex].placeText : null)) : null);

        if(source == null || source.Length == 0)
        {
            return inDefault;
        }

        return source;
    }
}
