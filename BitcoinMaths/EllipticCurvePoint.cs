using System;

namespace BitcoinMaths
{
    public class EllipticCurvePoint
    {
        public int X { get; }
        public int? Y { get; }
        public int A { get; }
        public int B { get; }

        public EllipticCurvePoint(int x, int? y, int a, int b)
        {
            if (y.HasValue && !PointIsOnCurve(x, y.Value, a, b))
            {
                throw new ArgumentException($"Point ({x}, {y}) is not on the curve y^2 = x^3 + {a}x + {b}.");
            }
            X = x;
            Y = y;
            A = a;
            B = b;
        }

        public bool AtInfinity
        {
            get { return !Y.HasValue; }
        }

        public bool TangentIsVertical
        {
            get { return Y.HasValue && Y.Value == 0; }
        }

        public decimal SlopeOfTangent
        {
            get
            {
                if (AtInfinity || TangentIsVertical) throw new InvalidOperationException("Slope of tangent is undefined.");

                return (decimal)(3 * X * X + A) / 2 * Y.Value;
            }
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                EllipticCurvePoint p = (EllipticCurvePoint)obj;
                return (X == p.X) && (Y == p.Y) && (A == p.A) && (B == p.B);
            }
        }

        public override int GetHashCode()
        {
            return (A << 6) ^ (B << 4) ^ (X << 2) ^ (!AtInfinity ? Y.Value : 0);
        }

        public static bool operator ==(EllipticCurvePoint p1, EllipticCurvePoint p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(EllipticCurvePoint p1, EllipticCurvePoint p2)
        {
            return !p1.Equals(p2);
        }

        public static EllipticCurvePoint operator +(EllipticCurvePoint p1, EllipticCurvePoint p2)
        {
            if (p1.A != p2.A || p1.B != p2.B)
            {
                throw new ArgumentException($"Addition operator is not defined for points not on the same curve.");
            }

            //At least one of the points is at infinity
            //This is the identity operation for addition so return the other point.
            if (p1.AtInfinity) return p2;
            if (p2.AtInfinity) return p1;

            //Same x, different y (or y is zero), meaning that the line is vertical.
            //Return the point at infinity.
            if ((p1.X == p2.X && p1.Y != p2.Y) || (p1 == p2 && p1.TangentIsVertical))
            {
                return new EllipticCurvePoint(p1.X, null, p1.A, p1.B);
            }

            //Where p1 coincides with p2, find the slope of the tangent.
            //Otherwise find the slope of the line between the points
            var slope = (p1 == p2)
                ? p1.SlopeOfTangent
                : (decimal)(p2.Y.Value - p1.Y.Value) / (p2.X - p1.X);
            var x3 = slope * slope - p1.X - p2.X;
            var y3 = slope * (p1.X - x3) - p1.Y.Value;
            
            return new EllipticCurvePoint((int)x3, (int)y3, p1.A, p1.B);
        }

        public static bool PointIsOnCurve(int x, int y, int a, int b)
        {
            return (int)Math.Pow(y, 2) == (int)Math.Pow(x, 3) + a * x + b;
        }
    }
}
