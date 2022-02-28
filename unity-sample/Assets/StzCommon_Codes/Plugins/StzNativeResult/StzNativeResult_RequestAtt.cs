using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_RequestAtt : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public int att_status = 0;
    }

    [SerializeField] private Data data = null;

    public EATTrackingManagerAuthorizationStatus Status { get { return (EATTrackingManagerAuthorizationStatus)data.att_status; } }
}