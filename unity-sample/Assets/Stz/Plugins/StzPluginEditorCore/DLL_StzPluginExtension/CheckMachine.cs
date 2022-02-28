/*
 * Copyright (C) SundayToz Corp. All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
*/

﻿namespace StzPluginEditorCore
{
    internal class CheckMachine
    {
        public bool CheckValidDLL<T>(string dllPath)
        {
            return true;
        }

        public bool IsSupportedPlatform<T>(string dllPath, string platformName)
        {
            return true;
        }

        public bool IsImplementedDLL<T>(string dllPath)
        {
            return true;
        }

        public bool IsValidUnityVersion<T>(string dllPath, string version)
        {
            return true;
        }
    }
}
