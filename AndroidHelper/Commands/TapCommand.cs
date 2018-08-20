namespace AndroidHelper
{
    class TapCommand : BaseCommond, ICommand
    {
        public IValue<Point> Point { get; set; }

        protected override void RunCore(IScriptContext token)
        {
            if (IsFast)
            {
                Global.Runner.FastTap(Point.Value);
            }
            else
            {
                Global.Runner.Tap(Point.Value);
            }
        }

        public override string ToString()
        {
            return new string(' ', Depth * 4) + $"点击：({Point})";
        }
    }
}