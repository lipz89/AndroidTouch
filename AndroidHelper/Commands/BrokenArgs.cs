using System;

namespace AndroidHelper
{
    class BrokenArgs : EventArgs
    {
        internal string Reason { get; set; }
    }
}