using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NextUI
{
    public class JsonAnalasysException : Exception
    {
        public JsonAnalasysException() : base() { }
        public JsonAnalasysException(string message) : base(message) { }
    }
}
