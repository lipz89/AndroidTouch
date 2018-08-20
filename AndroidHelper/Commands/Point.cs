using System;

namespace AndroidHelper
{
    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            return $"{X} {Y}";
        }
        public static bool operator ==(Point x, Point y)
        {
            return x.X == y.X && x.Y == y.Y;
        }
        public static bool operator !=(Point x, Point y)
        {
            return !(x == y);
        }
        //public bool Equals(Point other)
        //{
        //    return X == other.X && Y == other.Y;
        //}
        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(null, obj)) return false;
        //    return obj is Point && Equals((Point)obj);
        //}
        //public override int GetHashCode()
        //{
        //    return ToString().GetHashCode();
        //}
        public static bool TryParse(string input, out Point point)
        {
            point = new Point();
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var ps = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ps.Length != 2)
            {
                return false;
            }

            if (int.TryParse(ps[0], out var x) && int.TryParse(ps[1], out var y))
            {
                point.X = x;
                point.Y = y;
                return true;
            }

            return false;
        }
    }
}