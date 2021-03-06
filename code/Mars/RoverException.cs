﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mars
{
    [Serializable]
    public class RoverException : Exception
    {
        public RoverException()
        {
        }

        public RoverException(string message) : base(message)
        {
        }

        public RoverException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RoverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}