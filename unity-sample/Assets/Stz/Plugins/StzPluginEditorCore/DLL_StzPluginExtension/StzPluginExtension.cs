/*
 * Copyright (C) SundayToz Corp. All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
*/

﻿namespace StzPluginEditorCore
{
    public sealed class StzPluginExtension
    {
        //----------------------------------------------------------------
        // singleton
        //----------------------------------------------------------------
        private StzPluginExtension() { }
        private static StzPluginExtension _instance = null;
        public  static StzPluginExtension Instance  { get { return _instance ?? (_instance = new StzPluginExtension()); } }

        //----------------------------------------------------------------
        // variables
        //----------------------------------------------------------------
        private readonly CheckMachine     _checkMachine     = new CheckMachine();
        private readonly CacheController  _cacheController  = new CacheController(); 
        private readonly InnerExtension   _innerExtension   = new InnerExtension();

        //----------------------------------------------------------------
        // functions
        //----------------------------------------------------------------
        public bool IsValidDLL<T>(string dllName)
        {
            return _checkMachine?.CheckValidDLL<T>(dllName) ?? false;
        }

        public bool IsSupportedPlatform<T>(string dllName, string platformType)
        {
            return _checkMachine?.IsSupportedPlatform<T>(dllName, platformType) ?? false;
        }

        public bool IsImplementedDLL<T>(string dllName)
        {
            return _checkMachine?.IsImplementedDLL<T>(dllName) ?? false;
        }

        public bool IsValidUnityVersion<T>(string dllName, string version)
        {
            return _checkMachine?.IsValidUnityVersion<T>(dllName, version) ?? false;
        }

        public void CacheDLLStackTrace<T>(T dllClass)
        {
            _cacheController?.CacheDLLStackTrace(dllClass);
        }

        public void RemoveDLLStackTrace<T>(T dllClass)
        {
            _cacheController?.RemoveDLLStackTrace(dllClass);
        }

        public void RunLineBreaker<T>(string dllName)
        {
            _innerExtension?.LineBreaker<T>(dllName);   
        }
    }
}
