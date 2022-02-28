/*
 * Copyright (C) SundayToz Corp. All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
*/

﻿using System;
using System.Collections.Generic;

namespace StzPluginEditorCore
{
    public class StzServiceExtension
    {
        //----------------------------------------------------------------
        // singleton
        //----------------------------------------------------------------
        private StzServiceExtension() { }
        private static StzServiceExtension _instance = null;
        public  static StzServiceExtension Instance  { get { return _instance ?? (_instance = new StzServiceExtension()); } }

        //----------------------------------------------------------------
        // variables
        //----------------------------------------------------------------
        private Dictionary<Type, ExceptionHandler> _handlers = new Dictionary<Type, ExceptionHandler>();

        //----------------------------------------------------------------
        // functions
        //----------------------------------------------------------------
        public bool CheckServerModelFunctions<T>()
        {
            if (!StzPluginExtension.Instance.IsValidDLL<T>(nameof(T)))
                return false;

            return ServerModelChecker.Check<T>();
        }

        public bool CheckStaticsFunctions<T>()
        {
            if (!StzPluginExtension.Instance.IsValidDLL<T>(nameof(T)))
                return false;

            return StaticsChecker.Check<T>();
        }

        public bool CheckServerFunctions<T>()
        {
            if (!StzPluginExtension.Instance.IsValidDLL<T>(nameof(T)))
                return false;

            return ServerChecker.Check<T>();
        }

        public void ExceptionHandling<T>(Exception x)
        {
            if (_handlers.TryGetValue(typeof(T), out var handler))
                handler.Invoke(x);
        }

        public void AddExceptionListener<T>(Action<Exception> func)
        {
            if (!StzPluginExtension.Instance.IsValidDLL<T>(nameof(T)))
                return;

            var type = typeof(T);
            if (_handlers.ContainsKey(type))
                _handlers.Remove(type);

            _handlers.Add(type, new ExceptionHandler(type, func));
        }

        public void RemoveExteptionListener<T>(Action<Exception> func)
        {
            if (!StzPluginExtension.Instance.IsValidDLL<T>(nameof(T)))
                return;

            var type = typeof(T);
            if (_handlers.ContainsKey(type))
                _handlers.Remove(type);
        }
    }
}
