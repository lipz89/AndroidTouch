using System;

namespace AndroidHelper
{
    interface IScript : IDisposable
    {
        void Start();
        void Stop();
        void Pause();
        void Continue();
        bool SetParameters();
        IScriptContext Context { get; }
        string Name { get; }
        string Desc { get; }
        bool HasParameters { get; }

        event EventHandler Stopped;
        event EventHandler<CommondRunArgs> CommandRunning;
        event EventHandler<CommondRunArgs> CommandRunned;
        event EventHandler<NeedParameterArgs> NeedParameters;
    }
}