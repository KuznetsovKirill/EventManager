﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message)
        {

        }
    }
}
