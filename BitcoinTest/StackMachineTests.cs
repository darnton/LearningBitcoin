using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitcoin;
using BitcoinMaths;

namespace BitcoinTest
{
    [TestClass]
    public class StackMachineTests
    {
        #region EncodeNumber
        [TestMethod]
        public void EncodeNumber_zero()
        {
            var expectedBytes = new byte[] { };
            var actualBytes = StackMachine.EncodeNumber(0);

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public void EncodeNumber_negative()
        {
            var expectedBytes = new byte[] { 129 };
            var actualBytes = StackMachine.EncodeNumber(-1);

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public void EncodeNumber_highBitSet()
        {
            var expectedBytes = new byte[] { 129, 0 };
            var actualBytes = StackMachine.EncodeNumber(129);

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public void EncodeNumber_negativeAndHighBitSet()
        {
            var expectedBytes = new byte[] { 129, 128 };
            var actualBytes = StackMachine.EncodeNumber(-129);

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public void EncodeNumber_multiByte()
        {
            var expectedBytes = new byte[] { 7, 129, 0 };
            var actualBytes = StackMachine.EncodeNumber(33031);

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }
        #endregion


        #region DecodeNumber
        [TestMethod]
        public void DecodeNumber_zero()
        {
            var expectedNumber = 0;
            var actualNumber = StackMachine.DecodeNumber(new byte[] { });

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void DecodeNumber_negative()
        {
            var expectedNumber = -1;
            var actualNumber = StackMachine.DecodeNumber(new byte[] { 129 });

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void DecodeNumber_highBitSet()
        {
            var expectedNumber = 129;
            var actualNumber = StackMachine.DecodeNumber(new byte[] { 129, 0 });

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void DecodeNumber_negativeAndHighBitSet()
        {
            var expectedNumber = -129;
            var actualNumber = StackMachine.DecodeNumber(new byte[] { 129, 128 });

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void DecodeNumber_multiByte()
        {
            var expectedNumber = 33031;
            var actualNumber = StackMachine.DecodeNumber(new byte[] { 7, 129, 0 });

            Assert.AreEqual(expectedNumber, actualNumber);
        }
        #endregion

        private void PerformEmptyCheck(Func<Stack<byte[]>, bool> operation)
        {
            var stack = new Stack<byte[]>();

            var result = operation(stack);

            Assert.IsFalse(result);
            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        public void OP_VERIFY_one()
        {
            var stack = new Stack<byte[]>();
            stack.Push(StackMachine.EncodeNumber(1));

            var result = StackMachine.OP_VERIFY(stack);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OP_VERIFY_zero()
        {
            var stack = new Stack<byte[]>();
            stack.Push(StackMachine.EncodeNumber(0));

            var result = StackMachine.OP_VERIFY(stack);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OP_VERIFY_empty()
        {
            PerformEmptyCheck(StackMachine.OP_VERIFY);
        }

        [TestMethod]
        public void OP_DUP_true()
        {
            var stack = new Stack<byte[]>();
            stack.Push(BitConverter.GetBytes(12345));

            var result = StackMachine.OP_DUP(stack);
            var stackElements = stack.ToArray();

            Assert.IsTrue(result);
            Assert.AreEqual(2, stack.Count);
            CollectionAssert.AreEqual(stackElements[0], stackElements[1]);
        }

        [TestMethod]
        public void OP_DUP_empty()
        {
            PerformEmptyCheck(StackMachine.OP_DUP);
        }

        [TestMethod]
        public void OP_EQUAL_areEqual()
        {
            var stack = new Stack<byte[]>();
            stack.Push(BitConverter.GetBytes(12345));
            stack.Push(BitConverter.GetBytes(12345));

            var result = StackMachine.OP_EQUAL(stack);

            Assert.IsTrue(result);
            Assert.AreEqual(1, StackMachine.DecodeNumber(stack.Peek()));
        }

        [TestMethod]
        public void OP_EQUAL_notEqual()
        {
            var stack = new Stack<byte[]>();
            stack.Push(StackMachine.EncodeNumber(12345));
            stack.Push(StackMachine.EncodeNumber(54321));

            var result = StackMachine.OP_EQUAL(stack);

            Assert.IsTrue(result);
            Assert.AreEqual(0, StackMachine.DecodeNumber(stack.Peek()));
        }

        [TestMethod]
        public void OP_EQUAL_oneElement()
        {
            var stack = new Stack<byte[]>();
            stack.Push(BitConverter.GetBytes(12345));

            var result = StackMachine.OP_EQUAL(stack);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OP_EQUALVERIFY_areEqual()
        {
            var stack = new Stack<byte[]>();
            stack.Push(BitConverter.GetBytes(12345));
            stack.Push(BitConverter.GetBytes(12345));

            var result = StackMachine.OP_EQUALVERIFY(stack);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OP_EQUALVERIFY_notEqual()
        {
            var stack = new Stack<byte[]>();
            stack.Push(StackMachine.EncodeNumber(12345));
            stack.Push(StackMachine.EncodeNumber(54321));

            var result = StackMachine.OP_EQUALVERIFY(stack);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OP_HASH256_true()
        {
            //Values from https://en.bitcoin.it/wiki/Block_hashing_algorithm
            var stack = new Stack<byte[]>();
            var operand = ("01000000" +
                 "81cd02ab7e569e8bcd9317e2fe99f2de44d49ab2b8851ba4a308000000000000" +
                 "e320b6c2fffc8d750423db8b1eb942ae710e951ed797f7affc8892b0f1fc122b" +
                 "c7f5d74d" +
                 "f2b9441a" +
                 "42a14695").GetBytesFromHex();
            stack.Push(operand);

            var result = StackMachine.OP_HASH256(stack);
            var expectedHash = "1dbd981fe6985776b644b173a4d0385ddc1aa2a829688d1e0000000000000000".GetBytesFromHex();
            var actualHash = stack.Peek();

            Assert.IsTrue(result);
            Assert.AreEqual(1, stack.Count);
            CollectionAssert.AreEqual(expectedHash, actualHash);
        }

        [TestMethod]
        public void OP_HASH256_empty()
        {
            PerformEmptyCheck(StackMachine.OP_HASH256);
        }

        [TestMethod]
        public void OP_HASH160_true()
        {
            //Values from https://en.bitcoin.it/wiki/Block_hashing_algorithm
            var stack = new Stack<byte[]>();
            var operand = "02b4632d08485ff1df2db55b9dafd23347d1c47a457072a1e87be26896549a8737".GetBytesFromHex();
            stack.Push(operand);

            var result = StackMachine.OP_HASH160(stack);
            var expectedHash = "93ce48570b55c42c2af816aeaba06cfee1224fae".GetBytesFromHex();
            var actualHash = stack.Peek();

            Assert.IsTrue(result);
            Assert.AreEqual(1, stack.Count);
            CollectionAssert.AreEqual(expectedHash, actualHash);
        }

        [TestMethod]
        public void OP_HASH160_empty()
        {
            PerformEmptyCheck(StackMachine.OP_HASH160);
        }

        [TestMethod]
        public void OP_CHECKSIG_valid()
        {
            var stack = new Stack<byte[]>();
            var z = BigInteger.Parse("7c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d", NumberStyles.AllowHexSpecifier);
            var pubKey = "04887387e452b8eacc4acfde10d9aaf7f6d9a0f975aabb10d006e4da568744d06c61de6d95231cd89026e286df3b6ae4a894a3378e393e93a0f45b666329a0ae34".GetBytesFromHex();
            var sig = "3045022000eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c022100c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6".GetBytesFromHex();
            stack.Push(sig);
            stack.Push(pubKey);

            var result = StackMachine.OP_CHECKSIG(stack, z);

            Assert.IsTrue(result);
            Assert.IsTrue(StackMachine.DecodeNumber(stack.Peek()) > 0);
        }

        [TestMethod]
        public void OP_CHECKSIG_oneElement()
        {
            var stack = new Stack<byte[]>();
            var z = BigInteger.Zero;
            stack.Push(BitConverter.GetBytes(12345));

            var result = StackMachine.OP_CHECKSIG(stack, z);

            Assert.IsFalse(result);
        }
    }
}
