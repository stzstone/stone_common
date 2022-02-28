using System;
using System.Text;
using UnityEngine;

[Serializable]
public class StzNativeResult_StartUpdateFlow : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public int start_update;
    }

    [SerializeField] private Data data = null;
    
    public bool IsOkay => data?.start_update == -1;//Activity.RESULT_OK : -1, Activity.RESULT_CANCELED : 0
}