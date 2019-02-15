using System;
using System.Globalization;
using System.Numerics;

namespace BitcoinMaths
{
    public class EllipticCurveFiniteFieldPoint
    {
        public FiniteFieldElement X { get; }
        public FiniteFieldElement Y { get; }
        private EllipticCurveOverFiniteField Curve { get; }

        public FiniteFieldElement A
        {
            get { return Curve.A; }
        }
        public FiniteFieldElement B
        {
            get { return Curve.B; }
        }

        public EllipticCurveFiniteFieldPoint(BigInteger x, BigInteger? y, EllipticCurveOverFiniteField curve)
            : this(
                  new FiniteFieldElement(x, curve.Order),
                  y.HasValue ? new FiniteFieldElement(y.Value, curve.Order) : null,
                  curve
              )
        {
        }

        public EllipticCurveFiniteFieldPoint(FiniteFieldElement x, FiniteFieldElement y, EllipticCurveOverFiniteField curve)
        {
            Curve = curve;
            if (y != null && !curve.PointIsOnCurve(x.Value, y.Value))
            {
                throw new ArgumentException($"Point ({x.Value}, {y.Value}) is not on the curve {curve}.");
            }
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            var yValue = Y == null ? "infinity" : Y.Value.ToString();
            return $"({X.Value}, {yValue}) on {Curve}";
        }

        public bool AtInfinity
        {
            get { return Y == null; }
        }

        public bool TangentIsVertical
        {
            get { return Y != null && Y.Value == 0; }
        }

        public FiniteFieldElement SlopeOfTangent
        {
            get
            {
                if (AtInfinity || TangentIsVertical) throw new InvalidOperationException("Slope of tangent is undefined.");

                var x2Coefft = new FiniteFieldElement(3, Curve.Order);
                var yCoefft = new FiniteFieldElement(2, Curve.Order);
                return (x2Coefft * X * X + A) / (yCoefft * Y);
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
                EllipticCurveFiniteFieldPoint p = (EllipticCurveFiniteFieldPoint)obj;
                return (X == p.X) && (Y == p.Y) && (A == p.A) && (B == p.B);
            }
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static bool operator ==(EllipticCurveFiniteFieldPoint p1, EllipticCurveFiniteFieldPoint p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(EllipticCurveFiniteFieldPoint p1, EllipticCurveFiniteFieldPoint p2)
        {
            return !p1.Equals(p2);
        }

        public static EllipticCurveFiniteFieldPoint operator +(EllipticCurveFiniteFieldPoint p1, EllipticCurveFiniteFieldPoint p2)
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
                return new EllipticCurveFiniteFieldPoint(p1.X, null, p1.Curve);
            }

            //Where p1 coincides with p2, find the slope of the tangent.
            //Otherwise find the slope of the line between the points
            var slope = (p1 == p2)
                ? p1.SlopeOfTangent
                : (p2.Y - p1.Y) / (p2.X - p1.X);
            var x3 = slope * slope - p1.X - p2.X;
            var y3 = slope * (p1.X - x3) - p1.Y;

            return new EllipticCurveFiniteFieldPoint(x3, y3, p1.Curve);
        }

        public static EllipticCurveFiniteFieldPoint operator *(BigInteger s, EllipticCurveFiniteFieldPoint p)
        {
            if (s <= 0)
            {
                throw new ArgumentOutOfRangeException("Multiplication operator is not defined for non-positive scalar values.");
            }
            var coefficient = s % p.Curve.Order;
            var currentMultiple = p;
            var result = new EllipticCurveFiniteFieldPoint(0, null, p.Curve);

            //Binary expansion to multiply in log time.
            while (coefficient > 0)
            {
                if((coefficient & 1) > 0)
                {
                    result += currentMultiple;
                }
                currentMultiple += currentMultiple;
                coefficient >>= 1;
            }
            return result;
        }

        public static EllipticCurveFiniteFieldPoint operator *(EllipticCurveFiniteFieldPoint p, BigInteger s)
        {
            return s * p;
        }
    }
}
