/*
 * Copyright (C) SundayToz Corp. All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
*/

﻿using System;

namespace StzPluginEditorCore
{
    internal class ExceptionHandler
    {
        //----------------------------------------------------------------
        // variables
        //----------------------------------------------------------------
        private Type               _type = null;
        private Action<Exception>  _func = null;

        //----------------------------------------------------------------
        // functions
        //----------------------------------------------------------------
        public ExceptionHandler(Type type, Action<Exception> func)
        {
            _type = type;
            _func = func;
        }

        public void Invoke(Exception x)
        {
            _func?.Invoke(x);
        }
    }
}
