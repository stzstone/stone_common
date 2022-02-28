/*
 * Copyright (C) SundayToz Corp. All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
*/

﻿namespace StzPluginEditorCore
{
    public class StzLogConnector
    {
        //----------------------------------------------------------------
        // singleton
        //----------------------------------------------------------------
        private StzLogConnector() { }
        private static StzLogConnector _instance = null;
        public  static StzLogConnector Instance  { get { return _instance ?? (_instance = new StzLogConnector()); } }

        //----------------------------------------------------------------
        // variables
        //----------------------------------------------------------------
        private LogPrinter  _printer    = null;
        private Connector   _connector  = null;

        //----------------------------------------------------------------
        // get, set
        //----------------------------------------------------------------
        public bool IsInited            { get; private set; } = false;

        public bool UseFileLog          { get; set; } = false;
        public bool UseFakeNetworkLog   { get; set; } = false;
        public bool UseUnityEditorLog   { get; set; } = false;

        //----------------------------------------------------------------
        // functions
        //----------------------------------------------------------------
        public bool Init()
        {
            if (IsInited)
                return true;

            _printer?.Release();
            _connector?.Release();

            _printer = new LogPrinter();
            if (!_printer.Init())
                return false;

            _connector = new Connector();
            if (!_connector.Init())
                return false;

            if (!StzPluginExtension.Instance.IsValidDLL<StzLogConnector>("StzLogConnector")
                || !StzPluginExtension.Instance.IsImplementedDLL<StzLogConnector>("StzLogConnector"))
                return false;

            IsInited = true;
            return true;
        }

        public void TryConnectByHelper<T>(T logger)
        {
            if (!IsInited)
                return;

            _connector.Connect(logger);
        }

        public void Verbose(string log)
        {
            if (!IsInited)
                return;

            _printer.Verbose(log);
        }

        public void Warning(string log)
        {
            if (!IsInited)
                return;

            _printer.Warning(log);
        }

        public void Error(string log)
        {
            if (!IsInited)
                return;

            _printer.Error(log);
        }

        public void Exception(string log)
        {
            if (!IsInited)
                return;

            _printer.Exception(log);
        }
    }
}
