namespace AndroidHelper
{
    class SwipeCommand : BaseCommond, ICommand
    {
        public IValue<Point> From { get; set; }
        public IValue<Point> To { get; set; }
        public override bool IsValid => From.Value != To.Value;
        protected override void RunCore(IScriptContext token)
        {
            if (IsFast)
            {
                Global.Runner.FastSwipe(From.Value, To.Value);
            }
            else
            {
                Global.Runner.Swipe(From.Value, To.Value);
            }
        }
        public override string ToString()
        {
            return new string(' ', Depth * 4) + $"滑动： 从({From})到({To})";
        }
    }
}