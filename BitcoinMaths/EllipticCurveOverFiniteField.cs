using System.Numerics;

namespace BitcoinMaths
{
    public class EllipticCurveOverFiniteField
    {
        public FiniteFieldElement A { get; }
        public FiniteFieldElement B { get; }
        public BigInteger Order { get; }

        public EllipticCurveOverFiniteField(BigInteger a, BigInteger b, BigInteger order)
        {
            Order = order;
            A = new FiniteFieldElement(a, order);
            B = new FiniteFieldElement(b, order);
        }

        public override string ToString()
        {
            var xTerm = A.Value > 0 ? $" + {A.Value}x" : "";
            var constantTerm = B.Value > 0 ? $" + {B.Value}" : "";
            return $"y^2 = x^3{xTerm}{constantTerm} over F({Order})";
        }

        public bool PointIsOnCurve(BigInteger x, BigInteger? y)
        {
            var xElement = new FiniteFieldElement(x, Order);
            FiniteFieldElement yElement = y.HasValue ? new FiniteFieldElement(y.Value, Order) : null;
            return yElement.Pow(2) == xElement.Pow(3) + A * xElement + B;
        }

        public EllipticCurveFiniteFieldPoint GetPoint(BigInteger x, BigInteger? y)
        {
            return new EllipticCurveFiniteFieldPoint(x, y, this);
        }
    }
}
