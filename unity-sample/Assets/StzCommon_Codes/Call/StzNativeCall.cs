namespace STZ_Common
{
	//----------------------------------------------------------------
	// IStzNativeCall
	//----------------------------------------------------------------
	public interface IStzNativeCall
	{
		string GetAction();
		string GetParamString();
	}

	//----------------------------------------------------------------
	// StzNativeCall<T>
	//----------------------------------------------------------------
	public class StzNativeCall<T> : IStzNativeCall where T : StzNativeCallData_Base
	{
		protected T _data = null;

		public StzNativeCall(T data)
		{
			if (null == data)
			{
				return;
			}

			_data = data;
		}

		public string GetAction()
		{
			return _data?.action ?? string.Empty;
		}

		public string GetParamString()
		{
			return UnityEngine.JsonUtility.ToJson(_data);
		}
	}

	//----------------------------------------------------------------
	// StzNativeCallSimple
	//----------------------------------------------------------------
	public class StzNativeCallSimple : IStzNativeCall
	{
		private StzNativeCallData_Base _data = null;

		public StzNativeCallSimple(EStzNativeAction action)
		{
			_data = new StzNativeCallData_Base() { action = action.ToString() };
		}

		public string GetAction()
		{
			return _data?.action ?? string.Empty;
		}

		public string GetParamString()
		{
			return UnityEngine.JsonUtility.ToJson(_data);
		}
	}
}