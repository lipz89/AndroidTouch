using System;

namespace AndroidHelper
{
    class CommondRunArgs : EventArgs
    {
        internal ICommandInfo Commond { get; set; }
    }
}