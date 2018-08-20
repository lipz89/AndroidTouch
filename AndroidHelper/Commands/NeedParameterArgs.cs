using System;
using System.Collections.Generic;

namespace AndroidHelper
{
    class NeedParameterArgs : EventArgs
    {
        internal List<IParameter> Parameters { get; set; }
        internal bool IsCancel { get; set; }
    }
}