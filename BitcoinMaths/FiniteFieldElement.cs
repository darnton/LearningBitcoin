using System;
using System.Numerics;

namespace BitcoinMaths
{
    public class FiniteFieldElement
    {
        public BigInteger Value { get; }
        public BigInteger Order { get; }

        public FiniteFieldElement(BigInteger value, BigInteger order)
        {
            if (value >= order || value < 0)
            {
                throw new ArgumentOutOfRangeException($"Value not in field range 0 to {order}.");
            }
            Value = value;
            Order = order;
        }

        public FiniteFieldElement Pow(BigInteger exp)
        {
            return new FiniteFieldElement(BigInteger.ModPow(Value, exp, Order), Order);
        }

        public override string ToString()
        {
            return $"{Value} (in F({Order})";
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                FiniteFieldElement fe = (FiniteFieldElement)obj;
                return (Value == fe.Value) && (Order == fe.Order);
            }
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Order.GetHashCode();
        }

        public static bool operator ==(FiniteFieldElement a, FiniteFieldElement b)
        {
            return ((object)a == null && (object)b == null) || (object)a != null && a.Equals(b);
        }

        public static bool operator !=(FiniteFieldElement a, FiniteFieldElement b)
        {
            //Cast to object to avoid using this overridden operator.
            //If we don't do this we recursicely call != and get a stack overflow exception.
            return !((object)a == null && (object)b == null) && (object)a != null && !a.Equals(b);
        }

        public static FiniteFieldElement operator +(FiniteFieldElement a, FiniteFieldElement b)
        {
            if(a.Order != b.Order)
            {
                throw new ArgumentException($"Addition operator is not defined for elements of different order ({a.Order} and {b.Order}.");
            }
            return new FiniteFieldElement((a.Value + b.Value).Mod(a.Order), a.Order);
        }

        public static FiniteFieldElement operator -(FiniteFieldElement a, FiniteFieldElement b)
        {
            if (a.Order != b.Order)
            {
                throw new ArgumentException($"Subtraction operator is not defined for elements of different order ({a.Order} and {b.Order}.");
            }
            return new FiniteFieldElement((a.Value - b.Value).Mod(a.Order), a.Order);
        }

        public static FiniteFieldElement operator *(FiniteFieldElement a, FiniteFieldElement b)
        {
            if (a.Order != b.Order)
            {
                throw new ArgumentException($"Multiplication operator is not defined for elements of different order ({a.Order} and {b.Order}.");
            }
            return new FiniteFieldElement((a.Value * b.Value).Mod(a.Order), a.Order);
        }

        public static FiniteFieldElement operator /(FiniteFieldElement a, FiniteFieldElement b)
        {
            if (a.Order != b.Order)
            {
                throw new ArgumentException($"Division operator is not defined for elements of different order ({a.Order} and {b.Order}.");
            }
            return a * b.Pow(b.Order - 2);
        }
    }
}
