using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BitcoinMaths;

namespace Bitcoin
{
    public class StackMachine
    {
        private static readonly Dictionary<byte, Func<Stack<byte[]>, bool>> Operations = new Dictionary<byte, Func<Stack<byte[]>, bool>>
        {
            { 0, OP_0 },
            { 76, OP_PUSHDATA1 },
            { 77, OP_PUSHDATA2 },
            { 78, OP_PUSHDATA4 },
            { 79, OP_1NEGATE },
            { 81, OP_1 },
            { 82, OP_2 },
            { 83, OP_3 },
            { 84, OP_4 },
            { 85, OP_5 },
            { 86, OP_6 },
            { 87, OP_7 },
            { 88, OP_8 },
            { 89, OP_9 },
            { 90, OP_10 },
            { 91, OP_11 },
            { 92, OP_12 },
            { 93, OP_13 },
            { 94, OP_14 },
            { 95, OP_15 },
            { 96, OP_16 },
            { 97, OP_NOP },
            { 99, OP_IF },
            { 100, OP_NOTIF },
            { 103, OP_ELSE },
            { 104, OP_ENDIF },
            { 105, OP_VERIFY },
            { 106, OP_RETURN },
            { 107, OP_TOALTSTACK },
            { 108, OP_FROMALTSTACK },
            { 109, OP_2DROP },
            { 110, OP_2DUP },
            { 111, OP_3DUP },
            { 112, OP_2OVER },
            { 113, OP_2ROT },
            { 114, OP_2SWAP },
            { 115, OP_IFDUP },
            { 116, OP_DEPTH },
            { 117, OP_DROP },
            { 118, OP_DUP },
            { 119, OP_NIP },
            { 120, OP_OVER },
            { 121, OP_PICK },
            { 122, OP_ROLL },
            { 123, OP_ROT },
            { 124, OP_SWAP },
            { 125, OP_TUCK },
            { 130, OP_SIZE },
            { 135, OP_EQUAL },
            { 136, OP_EQUALVERIFY },
            { 139, OP_1ADD },
            { 140, OP_1SUB },
            { 143, OP_NEGATE },
            { 144, OP_ABS },
            { 145, OP_NOT },
            { 146, OP_0NOTEQUAL },
            { 147, OP_ADD },
            { 148, OP_SUB },
            { 149, OP_MUL },
            { 154, OP_BOOLAND },
            { 155, OP_BOOLOR },
            { 156, OP_NUMEQUAL },
            { 157, OP_NUMEQUALVERIFY },
            { 158, OP_NUMNOTEQUAL },
            { 159, OP_LESSTHAN },
            { 160, OP_GREATERTHAN },
            { 161, OP_LESSTHANOREQUAL },
            { 162, OP_GREATERTHANOREQUAL },
            { 163, OP_MIN },
            { 164, OP_MAX },
            { 165, OP_WITHIN },
            { 166, OP_RIPEMD160 },
            { 167, OP_SHA1 },
            { 168, OP_SHA256 },
            { 169, OP_HASH160 },
            { 170, OP_HASH256 },
            { 171, OP_CODESEPARATOR },
            { 172, OP_CHECKSIG },
            { 173, OP_CHECKSIGVERIFY },
            { 174, OP_CHECKMULTISIG },
            { 175, OP_CHECKMULTISIGVERIFY },
            { 176, OP_NOP1 },
            { 177, OP_CHECKLOCKTIMEVERIFY },
            { 178, OP_CHECKSEQUENCEVERIFY },
            { 179, OP_NOP4 },
            { 180, OP_NOP5 },
            { 181, OP_NOP6 },
            { 182, OP_NOP7 },
            { 183, OP_NOP8 },
            { 184, OP_NOP9 },
            { 185, OP_NOP10 }
        };

        public bool Execute(Script script, BigInteger docHash)
        {
            var stack = new Stack<byte[]>();
            var altStack = new Stack<byte[]>();

            foreach (var command in script.Commands)
            {
                if (command.Length > 1 || Operations[command[0]] == null)
                {
                    stack.Push(command);
                    continue;
                }
                var operation = Operations[command[0]];
                if (operation == OP_IF || operation == OP_NOTIF)
                {

                }
                else if (operation == OP_TOALTSTACK || operation == OP_FROMALTSTACK)
                {

                }
                else if (operation == OP_CHECKSIG || operation == OP_CHECKSIGVERIFY ||
                    operation == OP_CHECKMULTISIG || operation == OP_CHECKMULTISIGVERIFY)
                {

                }
                else
                {
                    var result = operation(stack);
                }
            }
            return (stack.Count > 0 && DecodeNumber(stack.Peek()) > 0);
        }

        public static byte[] EncodeNumber(BigInteger number)
        {
            if (number == 0) return new byte[] { };

            var result = new List<byte>();
            var isNegative = number < 0;
            var absValue = isNegative ? -number : number;
            while(absValue > 0)
            {
                result.Add((byte)(absValue & 255));
                absValue >>= 8;
            }
            if ((result[result.Count() - 1] & 128) > 0)
            {
                result.Add((byte)(isNegative ? 128 : 0));
            }
            else if (isNegative)
            {
                result[result.Count() - 1] |= 128;
            }
            return result.ToArray();
        }

        public static BigInteger DecodeNumber(byte[] element)
        {
            if (element.Length == 0) return 0;

            var result = 0;
            var isNegative = (element[element.Length - 1] & 128) > 0;
            if (isNegative)
            {
                element[element.Length - 1] &= 127;
            }
            foreach (var b in element.Reverse())
            {
                result <<= 8;
                result += b;
            }
            return isNegative ? -result : result;
        }

        #region Operations
        public static bool OP_0(Stack<byte[]> stack) { return false; }
        public static bool OP_PUSHDATA1(Stack<byte[]> stack) { return false; }
        public static bool OP_PUSHDATA2(Stack<byte[]> stack) { return false; }
        public static bool OP_PUSHDATA4(Stack<byte[]> stack) { return false; }
        public static bool OP_1NEGATE(Stack<byte[]> stack) { return false; }
        public static bool OP_1(Stack<byte[]> stack) { return false; }
        public static bool OP_2(Stack<byte[]> stack) { return false; }
        public static bool OP_3(Stack<byte[]> stack) { return false; }
        public static bool OP_4(Stack<byte[]> stack) { return false; }
        public static bool OP_5(Stack<byte[]> stack) { return false; }
        public static bool OP_6(Stack<byte[]> stack) { return false; }
        public static bool OP_7(Stack<byte[]> stack) { return false; }
        public static bool OP_8(Stack<byte[]> stack) { return false; }
        public static bool OP_9(Stack<byte[]> stack) { return false; }
        public static bool OP_10(Stack<byte[]> stack) { return false; }
        public static bool OP_11(Stack<byte[]> stack) { return false; }
        public static bool OP_12(Stack<byte[]> stack) { return false; }
        public static bool OP_13(Stack<byte[]> stack) { return false; }
        public static bool OP_14(Stack<byte[]> stack) { return false; }
        public static bool OP_15(Stack<byte[]> stack) { return false; }
        public static bool OP_16(Stack<byte[]> stack) { return false; }

        public static bool OP_NOP(Stack<byte[]> stack) { return true; }

        public static bool OP_IF(Stack<byte[]> stack) { return false; }
        public static bool OP_IF(Stack<byte[]> stack, List<byte[]> commands) { return false; }
        public static bool OP_NOTIF(Stack<byte[]> stack) { return false; }
        public static bool OP_NOTIF(Stack<byte[]> stack, List<byte[]> commands) { return false; }
        public static bool OP_ELSE(Stack<byte[]> stack) { return false; }
        public static bool OP_ENDIF(Stack<byte[]> stack) { return false; }

        public static bool OP_VERIFY(Stack<byte[]> stack)
        {
            if (stack.Count < 1) return false;

            return !(DecodeNumber(stack.Pop()) == 0);
        }

        public static bool OP_RETURN(Stack<byte[]> stack) { return false; }
        public static bool OP_TOALTSTACK(Stack<byte[]> stack) { return false; }
        public static bool OP_FROMALTSTACK(Stack<byte[]> stack) { return false; }
        public static bool OP_2DROP(Stack<byte[]> stack) { return false; }
        public static bool OP_2DUP(Stack<byte[]> stack) { return false; }
        public static bool OP_3DUP(Stack<byte[]> stack) { return false; }
        public static bool OP_2OVER(Stack<byte[]> stack) { return false; }
        public static bool OP_2ROT(Stack<byte[]> stack) { return false; }
        public static bool OP_2SWAP(Stack<byte[]> stack) { return false; }
        public static bool OP_IFDUP(Stack<byte[]> stack) { return false; }
        public static bool OP_DEPTH(Stack<byte[]> stack) { return false; }
        public static bool OP_DROP(Stack<byte[]> stack) { return false; }

        public static bool OP_DUP(Stack<byte[]> stack)
        {
            if (stack.Count < 1) return false;

            stack.Push(stack.Peek());
            return true;
        }

        public static bool OP_NIP(Stack<byte[]> stack) { return false; }
        public static bool OP_OVER(Stack<byte[]> stack) { return false; }
        public static bool OP_PICK(Stack<byte[]> stack) { return false; }
        public static bool OP_ROLL(Stack<byte[]> stack) { return false; }
        public static bool OP_ROT(Stack<byte[]> stack) { return false; }
        public static bool OP_SWAP(Stack<byte[]> stack) { return false; }
        public static bool OP_TUCK(Stack<byte[]> stack) { return false; }
        public static bool OP_SIZE(Stack<byte[]> stack) { return false; }

        public static bool OP_EQUAL(Stack<byte[]> stack)
        {
            if (stack.Count < 2) return false;

            var element1 = DecodeNumber(stack.Pop());
            var element2 = DecodeNumber(stack.Pop());
            stack.Push(EncodeNumber(element1 == element2 ? 1 : 0));
            return true;
        }

        public static bool OP_EQUALVERIFY(Stack<byte[]> stack)
        {
            return OP_EQUAL(stack) && OP_VERIFY(stack);
        }

        public static bool OP_1ADD(Stack<byte[]> stack) { return false; }
        public static bool OP_1SUB(Stack<byte[]> stack) { return false; }
        public static bool OP_NEGATE(Stack<byte[]> stack) { return false; }
        public static bool OP_ABS(Stack<byte[]> stack) { return false; }
        public static bool OP_NOT(Stack<byte[]> stack) { return false; }
        public static bool OP_0NOTEQUAL(Stack<byte[]> stack) { return false; }
        public static bool OP_ADD(Stack<byte[]> stack) { return false; }
        public static bool OP_SUB(Stack<byte[]> stack) { return false; }
        public static bool OP_MUL(Stack<byte[]> stack) { return false; }
        public static bool OP_BOOLAND(Stack<byte[]> stack) { return false; }
        public static bool OP_BOOLOR(Stack<byte[]> stack) { return false; }
        public static bool OP_NUMEQUAL(Stack<byte[]> stack) { return false; }
        public static bool OP_NUMEQUALVERIFY(Stack<byte[]> stack) { return false; }
        public static bool OP_NUMNOTEQUAL(Stack<byte[]> stack) { return false; }
        public static bool OP_LESSTHAN(Stack<byte[]> stack) { return false; }
        public static bool OP_GREATERTHAN(Stack<byte[]> stack) { return false; }
        public static bool OP_LESSTHANOREQUAL(Stack<byte[]> stack) { return false; }
        public static bool OP_GREATERTHANOREQUAL(Stack<byte[]> stack) { return false; }
        public static bool OP_MIN(Stack<byte[]> stack) { return false; }
        public static bool OP_MAX(Stack<byte[]> stack) { return false; }
        public static bool OP_WITHIN(Stack<byte[]> stack) { return false; }
        public static bool OP_RIPEMD160(Stack<byte[]> stack) { return false; }
        public static bool OP_SHA1(Stack<byte[]> stack) { return false; }
        public static bool OP_SHA256(Stack<byte[]> stack) { return false; }

        public static bool OP_HASH160(Stack<byte[]> stack)
        {
            if (stack.Count < 1) return false;

            stack.Push(stack.Pop().Hash160());
            return true;
        }

        public static bool OP_HASH256(Stack<byte[]> stack)
        {
            if (stack.Count < 1) return false;

            stack.Push(stack.Pop().DoubleSha256());
            return true;
        }

        public static bool OP_CODESEPARATOR(Stack<byte[]> stack) { return false; }
        public static bool OP_CHECKSIG(Stack<byte[]> stack) { return false; }

        public static bool OP_CHECKSIG(Stack<byte[]> stack, BigInteger z)
        {
            if (stack.Count < 2) return false;

            var pkBytes = stack.Pop();
            var pk = PublicKey.Parse(pkBytes);
            var sigBytes = stack.Pop();
            var sig = Signature.Parse(sigBytes);
            stack.Push(EncodeNumber(pk.Verify(sig, z) ? 1 : 0));
            return true;
        }

        public static bool OP_CHECKSIGVERIFY(Stack<byte[]> stack) { return false; }
        public static bool OP_CHECKSIGVERIFY(Stack<byte[]> stack, BigInteger z) { return false; }
        public static bool OP_CHECKMULTISIG(Stack<byte[]> stack) { return false; }
        public static bool OP_CHECKMULTISIG(Stack<byte[]> stack, BigInteger z) { return false; }
        public static bool OP_CHECKMULTISIGVERIFY(Stack<byte[]> stack) { return false; }
        public static bool OP_CHECKMULTISIGVERIFY(Stack<byte[]> stack, BigInteger z) { return false; }
        public static bool OP_NOP1(Stack<byte[]> stack) { return false; }
        public static bool OP_CHECKLOCKTIMEVERIFY(Stack<byte[]> stack) { return false; }
        public static bool OP_CHECKSEQUENCEVERIFY(Stack<byte[]> stack) { return false; }
        public static bool OP_NOP4(Stack<byte[]> stack) { return false; }
        public static bool OP_NOP5(Stack<byte[]> stack) { return false; }
        public static bool OP_NOP6(Stack<byte[]> stack) { return false; }
        public static bool OP_NOP7(Stack<byte[]> stack) { return false; }
        public static bool OP_NOP8(Stack<byte[]> stack) { return false; }
        public static bool OP_NOP9(Stack<byte[]> stack) { return false; }
        public static bool OP_NOP10(Stack<byte[]> stack) { return false; }
        #endregion
    }
}
