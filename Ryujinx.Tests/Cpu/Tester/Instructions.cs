// https://github.com/LDj3SNuD/ARM_v8-A_AArch64_Instructions_Tester/blob/master/Tester/Instructions.cs

// https://developer.arm.com/products/architecture/a-profile/exploration-tools
// ..\A64_v83A_ISA_xml_00bet6.1\ISA_v83A_A64_xml_00bet6.1_OPT\xhtml\

using System.Numerics;

namespace Ryujinx.Tests.Cpu.Tester
{
    using Types;

    using static AArch64;
    using static Shared;

    // index.html
    internal static class Base
    {
#region "Alu"
        // cls_int.html
        public static void Cls(bool sf, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits operand1 = X(datasize, n);

            BigInteger result = (BigInteger)CountLeadingSignBits(operand1);

            X(d, result.SubBigInteger(datasize - 1, 0));
        }

        // clz_int.html
        public static void Clz(bool sf, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits operand1 = X(datasize, n);

            BigInteger result = (BigInteger)CountLeadingZeroBits(operand1);

            X(d, result.SubBigInteger(datasize - 1, 0));
        }

        // rbit_int.html
        public static void Rbit(bool sf, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result = new Bits(datasize);
            Bits operand = X(datasize, n);

            for (int i = 0; i <= datasize - 1; i++)
            {
                result[datasize - 1 - i] = operand[i];
            }

            X(d, result);
        }

        // rev16_int.html
        public static void Rev16(bool sf, Bits Rn, Bits Rd)
        {
            /* Bits opc = "01"; */

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            int container_size = 16;

            /* Operation */
            Bits result = new Bits(datasize);
            Bits operand = X(datasize, n);

            int containers = datasize / container_size;
            int elements_per_container = container_size / 8;
            int index = 0;
            int rev_index;

            for (int c = 0; c <= containers - 1; c++)
            {
                rev_index = index + ((elements_per_container - 1) * 8);

                for (int e = 0; e <= elements_per_container - 1; e++)
                {
                    result[rev_index + 7, rev_index] = operand[index + 7, index];

                    index = index + 8;
                    rev_index = rev_index - 8;
                }
            }

            X(d, result);
        }

        // rev32_int.html
        // (rev.html)
        public static void Rev32(bool sf, Bits Rn, Bits Rd)
        {
            /* Bits opc = "10"; */

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            int container_size = 32;

            /* Operation */
            Bits result = new Bits(datasize);
            Bits operand = X(datasize, n);

            int containers = datasize / container_size;
            int elements_per_container = container_size / 8;
            int index = 0;
            int rev_index;

            for (int c = 0; c <= containers - 1; c++)
            {
                rev_index = index + ((elements_per_container - 1) * 8);

                for (int e = 0; e <= elements_per_container - 1; e++)
                {
                    result[rev_index + 7, rev_index] = operand[index + 7, index];

                    index = index + 8;
                    rev_index = rev_index - 8;
                }
            }

            X(d, result);
        }

        // rev64_rev.html
        // (rev.html)
        public static void Rev64(Bits Rn, Bits Rd)
        {
            /* Bits opc = "11"; */

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int container_size = 64;

            /* Operation */
            Bits result = new Bits(64);
            Bits operand = X(64, n);

            int containers = 64 / container_size;
            int elements_per_container = container_size / 8;
            int index = 0;
            int rev_index;

            for (int c = 0; c <= containers - 1; c++)
            {
                rev_index = index + ((elements_per_container - 1) * 8);

                for (int e = 0; e <= elements_per_container - 1; e++)
                {
                    result[rev_index + 7, rev_index] = operand[index + 7, index];

                    index = index + 8;
                    rev_index = rev_index - 8;
                }
            }

            X(d, result);
        }
#endregion

#region "AluImm"
        // add_addsub_imm.html
        public static void Add_Imm(bool sf, Bits shift, Bits imm12, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits imm;

            switch (shift)
            {
                default:
                case Bits bits when bits == "00":
                    imm = ZeroExtend(imm12, datasize);
                    break;
                case Bits bits when bits == "01":
                    imm = ZeroExtend(Bits.Concat(imm12, Zeros(12)), datasize);
                    break;
                /* when '1x' ReservedValue(); */
            }

            /* Operation */
            Bits result;
            Bits operand1 = (n == 31 ? SP(datasize) : X(datasize, n));

            (result, _) = AddWithCarry(datasize, operand1, imm, false);

            if (d == 31)
            {
                SP(result);
            }
            else
            {
                X(d, result);
            }
        }

        // adds_addsub_imm.html
        public static void Adds_Imm(bool sf, Bits shift, Bits imm12, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits imm;

            switch (shift)
            {
                default:
                case Bits bits when bits == "00":
                    imm = ZeroExtend(imm12, datasize);
                    break;
                case Bits bits when bits == "01":
                    imm = ZeroExtend(Bits.Concat(imm12, Zeros(12)), datasize);
                    break;
                /* when '1x' ReservedValue(); */
            }

            /* Operation */
            Bits result;
            Bits operand1 = (n == 31 ? SP(datasize) : X(datasize, n));
            Bits nzcv;

            (result, nzcv) = AddWithCarry(datasize, operand1, imm, false);

            PSTATE.NZCV(nzcv);

            X(d, result);
        }

        // and_log_imm.html
        public static void And_Imm(bool sf, bool N, Bits immr, Bits imms, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits imm;

            /* if sf == '0' && N != '0' then ReservedValue(); */

            (imm, _) = DecodeBitMasks(datasize, N, imms, immr, true);

            /* Operation */
            Bits operand1 = X(datasize, n);

            Bits result = AND(operand1, imm);

            if (d == 31)
            {
                SP(result);
            }
            else
            {
                X(d, result);
            }
        }

        // ands_log_imm.html
        public static void Ands_Imm(bool sf, bool N, Bits immr, Bits imms, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits imm;

            /* if sf == '0' && N != '0' then ReservedValue(); */

            (imm, _) = DecodeBitMasks(datasize, N, imms, immr, true);

            /* Operation */
            Bits operand1 = X(datasize, n);

            Bits result = AND(operand1, imm);

            PSTATE.NZCV(result[datasize - 1], IsZeroBit(result), false, false);

            X(d, result);
        }

        // eor_log_imm.html
        public static void Eor_Imm(bool sf, bool N, Bits immr, Bits imms, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits imm;

            /* if sf == '0' && N != '0' then ReservedValue(); */

            (imm, _) = DecodeBitMasks(datasize, N, imms, immr, true);

            /* Operation */
            Bits operand1 = X(datasize, n);

            Bits result = EOR(operand1, imm);

            if (d == 31)
            {
                SP(result);
            }
            else
            {
                X(d, result);
            }
        }

        // orr_log_imm.html
        public static void Orr_Imm(bool sf, bool N, Bits immr, Bits imms, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits imm;

            /* if sf == '0' && N != '0' then ReservedValue(); */

            (imm, _) = DecodeBitMasks(datasize, N, imms, immr, true);

            /* Operation */
            Bits operand1 = X(datasize, n);

            Bits result = OR(operand1, imm);

            if (d == 31)
            {
                SP(result);
            }
            else
            {
                X(d, result);
            }
        }

        // sub_addsub_imm.html
        public static void Sub_Imm(bool sf, Bits shift, Bits imm12, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits imm;

            switch (shift)
            {
                default:
                case Bits bits when bits == "00":
                    imm = ZeroExtend(imm12, datasize);
                    break;
                case Bits bits when bits == "01":
                    imm = ZeroExtend(Bits.Concat(imm12, Zeros(12)), datasize);
                    break;
                /* when '1x' ReservedValue(); */
            }

            /* Operation */
            Bits result;
            Bits operand1 = (n == 31 ? SP(datasize) : X(datasize, n));
            Bits operand2 = NOT(imm);

            (result, _) = AddWithCarry(datasize, operand1, operand2, true);

            if (d == 31)
            {
                SP(result);
            }
            else
            {
                X(d, result);
            }
        }

        // subs_addsub_imm.html
        public static void Subs_Imm(bool sf, Bits shift, Bits imm12, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits imm;

            switch (shift)
            {
                default:
                case Bits bits when bits == "00":
                    imm = ZeroExtend(imm12, datasize);
                    break;
                case Bits bits when bits == "01":
                    imm = ZeroExtend(Bits.Concat(imm12, Zeros(12)), datasize);
                    break;
                /* when '1x' ReservedValue(); */
            }

            /* Operation */
            Bits result;
            Bits operand1 = (n == 31 ? SP(datasize) : X(datasize, n));
            Bits operand2 = NOT(imm);
            Bits nzcv;

            (result, nzcv) = AddWithCarry(datasize, operand1, operand2, true);

            PSTATE.NZCV(nzcv);

            X(d, result);
        }
#endregion

#region "AluRs"
        // adc.html
        public static void Adc(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            (result, _) = AddWithCarry(datasize, operand1, operand2, PSTATE.C);

            X(d, result);
        }

        // adcs.html
        public static void Adcs(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);
            Bits nzcv;

            (result, nzcv) = AddWithCarry(datasize, operand1, operand2, PSTATE.C);

            PSTATE.NZCV(nzcv);

            X(d, result);
        }

        // add_addsub_shift.html
        public static void Add_Rs(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if shift == '11' then ReservedValue(); */
            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            (result, _) = AddWithCarry(datasize, operand1, operand2, false);

            X(d, result);
        }

        // adds_addsub_shift.html
        public static void Adds_Rs(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if shift == '11' then ReservedValue(); */
            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);
            Bits nzcv;

            (result, nzcv) = AddWithCarry(datasize, operand1, operand2, false);

            PSTATE.NZCV(nzcv);

            X(d, result);
        }

        // and_log_shift.html
        public static void And_Rs(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            Bits result = AND(operand1, operand2);

            X(d, result);
        }

        // ands_log_shift.html
        public static void Ands_Rs(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            Bits result = AND(operand1, operand2);

            PSTATE.NZCV(result[datasize - 1], IsZeroBit(result), false, false);

            X(d, result);
        }

        // asrv.html
        public static void Asrv(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /*readonly */Bits op2 = "10";

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            ShiftType shift_type = DecodeShift(op2);

            /* Operation */
            Bits operand2 = X(datasize, m);

            Bits result = ShiftReg(datasize, n, shift_type, (int)(UInt(operand2) % datasize)); // BigInteger.Modulus Operator (BigInteger, BigInteger)

            X(d, result);
        }

        // bic_log_shift.html
        public static void Bic(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            operand2 = NOT(operand2);

            Bits result = AND(operand1, operand2);

            X(d, result);
        }

        // bics.html
        public static void Bics(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            operand2 = NOT(operand2);

            Bits result = AND(operand1, operand2);

            PSTATE.NZCV(result[datasize - 1], IsZeroBit(result), false, false);

            X(d, result);
        }

        // crc32.html
        public static void Crc32(bool sf, Bits Rm, Bits sz, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if sf == '1' && sz != '11' then UnallocatedEncoding(); */
            /* if sf == '0' && sz == '11' then UnallocatedEncoding(); */

            int size = 8 << (int)UInt(sz);

            /* Operation */
            /* if !HaveCRCExt() then UnallocatedEncoding(); */

            Bits acc = X(32, n); // accumulator
            Bits val = X(size, m); // input value
            Bits poly = new Bits(0x04C11DB7u);

            Bits tempacc = Bits.Concat(BitReverse(acc), Zeros(size));
            Bits tempval = Bits.Concat(BitReverse(val), Zeros(32));

            // Poly32Mod2 on a bitstring does a polynomial Modulus over {0,1} operation
            X(d, BitReverse(Poly32Mod2(EOR(tempacc, tempval), poly)));
        }

        // crc32c.html
        public static void Crc32c(bool sf, Bits Rm, Bits sz, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if sf == '1' && sz != '11' then UnallocatedEncoding(); */
            /* if sf == '0' && sz == '11' then UnallocatedEncoding(); */

            int size = 8 << (int)UInt(sz);

            /* Operation */
            /* if !HaveCRCExt() then UnallocatedEncoding(); */

            Bits acc = X(32, n); // accumulator
            Bits val = X(size, m); // input value
            Bits poly = new Bits(0x1EDC6F41u);

            Bits tempacc = Bits.Concat(BitReverse(acc), Zeros(size));
            Bits tempval = Bits.Concat(BitReverse(val), Zeros(32));

            // Poly32Mod2 on a bitstring does a polynomial Modulus over {0,1} operation
            X(d, BitReverse(Poly32Mod2(EOR(tempacc, tempval), poly)));
        }

        // eon.html
        public static void Eon(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            operand2 = NOT(operand2);

            Bits result = EOR(operand1, operand2);

            X(d, result);
        }

        // eor_log_shift.html
        public static void Eor_Rs(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            Bits result = EOR(operand1, operand2);

            X(d, result);
        }

        // extr.html
        public static void Extr(bool sf, bool N, Bits Rm, Bits imms, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if N != sf then UnallocatedEncoding(); */
            /* if sf == '0' && imms<5> == '1' then ReservedValue(); */

            int lsb = (int)UInt(imms);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);
            Bits concat = Bits.Concat(operand1, operand2);

            Bits result = concat[lsb + datasize - 1, lsb];

            X(d, result);
        }

        // lslv.html
        public static void Lslv(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /*readonly */Bits op2 = "00";

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            ShiftType shift_type = DecodeShift(op2);

            /* Operation */
            Bits operand2 = X(datasize, m);

            Bits result = ShiftReg(datasize, n, shift_type, (int)(UInt(operand2) % datasize)); // BigInteger.Modulus Operator (BigInteger, BigInteger)

            X(d, result);
        }

        // lsrv.html
        public static void Lsrv(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /*readonly */Bits op2 = "01";

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            ShiftType shift_type = DecodeShift(op2);

            /* Operation */
            Bits operand2 = X(datasize, m);

            Bits result = ShiftReg(datasize, n, shift_type, (int)(UInt(operand2) % datasize)); // BigInteger.Modulus Operator (BigInteger, BigInteger)

            X(d, result);
        }

        // orn_log_shift.html
        public static void Orn(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            operand2 = NOT(operand2);

            Bits result = OR(operand1, operand2);

            X(d, result);
        }

        // orr_log_shift.html
        public static void Orr_Rs(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            Bits result = OR(operand1, operand2);

            X(d, result);
        }

        // rorv.html
        public static void Rorv(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /*readonly */Bits op2 = "11";

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            ShiftType shift_type = DecodeShift(op2);

            /* Operation */
            Bits operand2 = X(datasize, m);

            Bits result = ShiftReg(datasize, n, shift_type, (int)(UInt(operand2) % datasize)); // BigInteger.Modulus Operator (BigInteger, BigInteger)

            X(d, result);
        }

        // sbc.html
        public static void Sbc(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            operand2 = NOT(operand2);

            (result, _) = AddWithCarry(datasize, operand1, operand2, PSTATE.C);

            X(d, result);
        }

        // sbcs.html
        public static void Sbcs(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);
            Bits nzcv;

            operand2 = NOT(operand2);

            (result, nzcv) = AddWithCarry(datasize, operand1, operand2, PSTATE.C);

            PSTATE.NZCV(nzcv);

            X(d, result);
        }

        // sdiv.html
        public static void Sdiv(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            BigInteger result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            if (IsZero(operand2))
            {
                result = (BigInteger)0m;
            }
            else
            {
                result = RoundTowardsZero(Real(Int(operand1, false)) / Real(Int(operand2, false)));
            }

            X(d, result.SubBigInteger(datasize - 1, 0));
        }

        // sub_addsub_shift.html
        public static void Sub_Rs(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if shift == '11' then ReservedValue(); */
            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);

            operand2 = NOT(operand2);

            (result, _) = AddWithCarry(datasize, operand1, operand2, true);

            X(d, result);
        }

        // subs_addsub_shift.html
        public static void Subs_Rs(bool sf, Bits shift, Bits Rm, Bits imm6, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* if shift == '11' then ReservedValue(); */
            /* if sf == '0' && imm6<5> == '1' then ReservedValue(); */

            ShiftType shift_type = DecodeShift(shift);
            int shift_amount = (int)UInt(imm6);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = ShiftReg(datasize, m, shift_type, shift_amount);
            Bits nzcv;

            operand2 = NOT(operand2);

            (result, nzcv) = AddWithCarry(datasize, operand1, operand2, true);

            PSTATE.NZCV(nzcv);

            X(d, result);
        }

        // udiv.html
        public static void Udiv(bool sf, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            BigInteger result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            if (IsZero(operand2))
            {
                result = (BigInteger)0m;
            }
            else
            {
                result = RoundTowardsZero(Real(Int(operand1, true)) / Real(Int(operand2, true)));
            }

            X(d, result.SubBigInteger(datasize - 1, 0));
        }
#endregion

#region "AluRx"
        // add_addsub_ext.html
        public static void Add_Rx(bool sf, Bits Rm, Bits option, Bits imm3, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            ExtendType extend_type = DecodeRegExtend(option);
            int shift = (int)UInt(imm3);

            /* if shift > 4 then ReservedValue(); */

            /* Operation */
            Bits result;
            Bits operand1 = (n == 31 ? SP(datasize) : X(datasize, n));
            Bits operand2 = ExtendReg(datasize, m, extend_type, shift);

            (result, _) = AddWithCarry(datasize, operand1, operand2, false);

            if (d == 31)
            {
                SP(result);
            }
            else
            {
                X(d, result);
            }
        }

        // adds_addsub_ext.html
        public static void Adds_Rx(bool sf, Bits Rm, Bits option, Bits imm3, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            ExtendType extend_type = DecodeRegExtend(option);
            int shift = (int)UInt(imm3);

            /* if shift > 4 then ReservedValue(); */

            /* Operation */
            Bits result;
            Bits operand1 = (n == 31 ? SP(datasize) : X(datasize, n));
            Bits operand2 = ExtendReg(datasize, m, extend_type, shift);
            Bits nzcv;

            (result, nzcv) = AddWithCarry(datasize, operand1, operand2, false);

            PSTATE.NZCV(nzcv);

            X(d, result);
        }

        // sub_addsub_ext.html
        public static void Sub_Rx(bool sf, Bits Rm, Bits option, Bits imm3, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            ExtendType extend_type = DecodeRegExtend(option);
            int shift = (int)UInt(imm3);

            /* if shift > 4 then ReservedValue(); */

            /* Operation */
            Bits result;
            Bits operand1 = (n == 31 ? SP(datasize) : X(datasize, n));
            Bits operand2 = ExtendReg(datasize, m, extend_type, shift);

            operand2 = NOT(operand2);

            (result, _) = AddWithCarry(datasize, operand1, operand2, true);

            if (d == 31)
            {
                SP(result);
            }
            else
            {
                X(d, result);
            }
        }

        // subs_addsub_ext.html
        public static void Subs_Rx(bool sf, Bits Rm, Bits option, Bits imm3, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            ExtendType extend_type = DecodeRegExtend(option);
            int shift = (int)UInt(imm3);

            /* if shift > 4 then ReservedValue(); */

            /* Operation */
            Bits result;
            Bits operand1 = (n == 31 ? SP(datasize) : X(datasize, n));
            Bits operand2 = ExtendReg(datasize, m, extend_type, shift);
            Bits nzcv;

            operand2 = NOT(operand2);

            (result, nzcv) = AddWithCarry(datasize, operand1, operand2, true);

            PSTATE.NZCV(nzcv);

            X(d, result);
        }
#endregion

#region "Bfm"
        // bfm.html
        public static void Bfm(bool sf, bool N, Bits immr, Bits imms, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            int R;
            Bits wmask;
            Bits tmask;

            /* if sf == '1' && N != '1' then ReservedValue(); */
            /* if sf == '0' && (N != '0' || immr<5> != '0' || imms<5> != '0') then ReservedValue(); */

            R = (int)UInt(immr);
            (wmask, tmask) = DecodeBitMasks(datasize, N, imms, immr, false);

            /* Operation */
            Bits dst = X(datasize, d);
            Bits src = X(datasize, n);

            // perform bitfield move on low bits
            Bits bot = OR(AND(dst, NOT(wmask)), AND(ROR(src, R), wmask));

            // combine extension bits and result bits
            X(d, OR(AND(dst, NOT(tmask)), AND(bot, tmask)));
        }

        // sbfm.html
        public static void Sbfm(bool sf, bool N, Bits immr, Bits imms, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            int R;
            int S;
            Bits wmask;
            Bits tmask;

            /* if sf == '1' && N != '1' then ReservedValue(); */
            /* if sf == '0' && (N != '0' || immr<5> != '0' || imms<5> != '0') then ReservedValue(); */

            R = (int)UInt(immr);
            S = (int)UInt(imms);
            (wmask, tmask) = DecodeBitMasks(datasize, N, imms, immr, false);

            /* Operation */
            Bits src = X(datasize, n);

            // perform bitfield move on low bits
            Bits bot = AND(ROR(src, R), wmask);

            // determine extension bits (sign, zero or dest register)
            Bits top = Replicate(datasize, src[S]);

            // combine extension bits and result bits
            X(d, OR(AND(top, NOT(tmask)), AND(bot, tmask)));
        }

        // ubfm.html
        public static void Ubfm(bool sf, bool N, Bits immr, Bits imms, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            int R;
            Bits wmask;
            Bits tmask;

            /* if sf == '1' && N != '1' then ReservedValue(); */
            /* if sf == '0' && (N != '0' || immr<5> != '0' || imms<5> != '0') then ReservedValue(); */

            R = (int)UInt(immr);
            (wmask, tmask) = DecodeBitMasks(datasize, N, imms, immr, false);

            /* Operation */
            Bits src = X(datasize, n);

            // perform bitfield move on low bits
            Bits bot = AND(ROR(src, R), wmask);

            // combine extension bits and result bits
            X(d, AND(bot, tmask));
        }
#endregion

#region "CcmpImm"
        // ccmn_imm.html
        public static void Ccmn_Imm(bool sf, Bits imm5, Bits cond, Bits Rn, Bits nzcv)
        {
            /* Decode */
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits flags = nzcv;
            Bits imm = ZeroExtend(imm5, datasize);

            /* Operation */
            Bits operand1 = X(datasize, n);

            if (ConditionHolds(cond))
            {
                (_, flags) = AddWithCarry(datasize, operand1, imm, false);
            }

            PSTATE.NZCV(flags);
        }

        // ccmp_imm.html
        public static void Ccmp_Imm(bool sf, Bits imm5, Bits cond, Bits Rn, Bits nzcv)
        {
            /* Decode */
            int n = (int)UInt(Rn);

            int datasize = (sf ? 64 : 32);

            Bits flags = nzcv;
            Bits imm = ZeroExtend(imm5, datasize);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2;

            if (ConditionHolds(cond))
            {
                operand2 = NOT(imm);
                (_, flags) = AddWithCarry(datasize, operand1, operand2, true);
            }

            PSTATE.NZCV(flags);
        }
#endregion

#region "CcmpReg"
        // ccmn_reg.html
        public static void Ccmn_Reg(bool sf, Bits Rm, Bits cond, Bits Rn, Bits nzcv)
        {
            /* Decode */
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            Bits flags = nzcv;

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            if (ConditionHolds(cond))
            {
                (_, flags) = AddWithCarry(datasize, operand1, operand2, false);
            }

            PSTATE.NZCV(flags);
        }

        // ccmp_reg.html
        public static void Ccmp_Reg(bool sf, Bits Rm, Bits cond, Bits Rn, Bits nzcv)
        {
            /* Decode */
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            Bits flags = nzcv;

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            if (ConditionHolds(cond))
            {
                operand2 = NOT(operand2);
                (_, flags) = AddWithCarry(datasize, operand1, operand2, true);
            }

            PSTATE.NZCV(flags);
        }
#endregion

#region "Csel"
        // csel.html
        public static void Csel(bool sf, Bits Rm, Bits cond, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            if (ConditionHolds(cond))
            {
                result = operand1;
            }
            else
            {
                result = operand2;
            }

            X(d, result);
        }

        // csinc.html
        public static void Csinc(bool sf, Bits Rm, Bits cond, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            if (ConditionHolds(cond))
            {
                result = operand1;
            }
            else
            {
                result = operand2 + 1;
            }

            X(d, result);
        }

        // csinv.html
        public static void Csinv(bool sf, Bits Rm, Bits cond, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            if (ConditionHolds(cond))
            {
                result = operand1;
            }
            else
            {
                result = NOT(operand2);
            }

            X(d, result);
        }

        // csneg.html
        public static void Csneg(bool sf, Bits Rm, Bits cond, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits result;
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);

            if (ConditionHolds(cond))
            {
                result = operand1;
            }
            else
            {
                result = NOT(operand2);
                result = result + 1;
            }

            X(d, result);
        }
#endregion

#region "Mov"
        // movk.html
        public static void Movk(bool sf, Bits hw, Bits imm16, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && hw<1> == '1' then UnallocatedEncoding(); */

            int pos = (int)UInt(Bits.Concat(hw, "0000"));

            /* Operation */
            Bits result = X(datasize, d);

            result[pos + 15, pos] = imm16;

            X(d, result);
        }

        // movn.html
        public static void Movn(bool sf, Bits hw, Bits imm16, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && hw<1> == '1' then UnallocatedEncoding(); */

            int pos = (int)UInt(Bits.Concat(hw, "0000"));

            /* Operation */
            Bits result = Zeros(datasize);

            result[pos + 15, pos] = imm16;
            result = NOT(result);

            X(d, result);
        }

        // movz.html
        public static void Movz(bool sf, Bits hw, Bits imm16, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);

            int datasize = (sf ? 64 : 32);

            /* if sf == '0' && hw<1> == '1' then UnallocatedEncoding(); */

            int pos = (int)UInt(Bits.Concat(hw, "0000"));

            /* Operation */
            Bits result = Zeros(datasize);

            result[pos + 15, pos] = imm16;

            X(d, result);
        }
#endregion

#region "Mul"
        // madd.html
        public static void Madd(bool sf, Bits Rm, Bits Ra, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);
            int a = (int)UInt(Ra);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);
            Bits operand3 = X(datasize, a);

            BigInteger result = UInt(operand3) + (UInt(operand1) * UInt(operand2));

            X(d, result.SubBigInteger(datasize - 1, 0));
        }

        // msub.html
        public static void Msub(bool sf, Bits Rm, Bits Ra, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);
            int a = (int)UInt(Ra);

            int datasize = (sf ? 64 : 32);

            /* Operation */
            Bits operand1 = X(datasize, n);
            Bits operand2 = X(datasize, m);
            Bits operand3 = X(datasize, a);

            BigInteger result = UInt(operand3) - (UInt(operand1) * UInt(operand2));

            X(d, result.SubBigInteger(datasize - 1, 0));
        }

        // smaddl.html
        public static void Smaddl(Bits Rm, Bits Ra, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);
            int a = (int)UInt(Ra);

            /* Operation */
            Bits operand1 = X(32, n);
            Bits operand2 = X(32, m);
            Bits operand3 = X(64, a);

            BigInteger result = Int(operand3, false) + (Int(operand1, false) * Int(operand2, false));

            X(d, result.SubBigInteger(63, 0));
        }

        // umaddl.html
        public static void Umaddl(Bits Rm, Bits Ra, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);
            int a = (int)UInt(Ra);

            /* Operation */
            Bits operand1 = X(32, n);
            Bits operand2 = X(32, m);
            Bits operand3 = X(64, a);

            BigInteger result = Int(operand3, true) + (Int(operand1, true) * Int(operand2, true));

            X(d, result.SubBigInteger(63, 0));
        }

        // smsubl.html
        public static void Smsubl(Bits Rm, Bits Ra, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);
            int a = (int)UInt(Ra);

            /* Operation */
            Bits operand1 = X(32, n);
            Bits operand2 = X(32, m);
            Bits operand3 = X(64, a);

            BigInteger result = Int(operand3, false) - (Int(operand1, false) * Int(operand2, false));

            X(d, result.SubBigInteger(63, 0));
        }

        // umsubl.html
        public static void Umsubl(Bits Rm, Bits Ra, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);
            int a = (int)UInt(Ra);

            /* Operation */
            Bits operand1 = X(32, n);
            Bits operand2 = X(32, m);
            Bits operand3 = X(64, a);

            BigInteger result = Int(operand3, true) - (Int(operand1, true) * Int(operand2, true));

            X(d, result.SubBigInteger(63, 0));
        }

        // smulh.html
        public static void Smulh(Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* Operation */
            Bits operand1 = X(64, n);
            Bits operand2 = X(64, m);

            BigInteger result = Int(operand1, false) * Int(operand2, false);

            X(d, result.SubBigInteger(127, 64));
        }

        // umulh.html
        public static void Umulh(Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* Operation */
            Bits operand1 = X(64, n);
            Bits operand2 = X(64, m);

            BigInteger result = Int(operand1, true) * Int(operand2, true);

            X(d, result.SubBigInteger(127, 64));
        }
#endregion
    }

    // fpsimdindex.html
    internal static class SimdFp
    {
#region "Simd"
        // abs_advsimd.html#ABS_asisdmisc_R
        public static void Abs_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool neg = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            BigInteger element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                if (neg)
                {
                    element = -element;
                }
                else
                {
                    element = Abs(element);
                }

                Elem(result, e, esize, element.SubBigInteger(esize - 1, 0));
            }

            V(d, result);
        }

        // abs_advsimd.html#ABS_asimdmisc_R
        public static void Abs_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool neg = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            BigInteger element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                if (neg)
                {
                    element = -element;
                }
                else
                {
                    element = Abs(element);
                }

                Elem(result, e, esize, element.SubBigInteger(esize - 1, 0));
            }

            V(d, result);
        }

        // addp_advsimd_pair.html
        public static void Addp_S(Bits size, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize * 2;
            // int elements = 2;

            ReduceOp op = ReduceOp.ReduceOp_ADD;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand = V(datasize, n);

            V(d, Reduce(op, operand, esize));
        }

        // addv_advsimd.html
        public static void Addv_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '100' then ReservedValue(); */
            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            // int elements = datasize / esize;

            ReduceOp op = ReduceOp.ReduceOp_ADD;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand = V(datasize, n);

            V(d, Reduce(op, operand, esize));
        }

        // cls_advsimd.html
        public static void Cls_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            CountOp countop = (U ? CountOp.CountOp_CLZ : CountOp.CountOp_CLS);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            BigInteger count;

            for (int e = 0; e <= elements - 1; e++)
            {
                if (countop == CountOp.CountOp_CLS)
                {
                    count = (BigInteger)CountLeadingSignBits(Elem(operand, e, esize));
                }
                else
                {
                    count = (BigInteger)CountLeadingZeroBits(Elem(operand, e, esize));
                }

                Elem(result, e, esize, count.SubBigInteger(esize - 1, 0));
            }

            V(d, result);
        }

        // clz_advsimd.html
        public static void Clz_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            CountOp countop = (U ? CountOp.CountOp_CLZ : CountOp.CountOp_CLS);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            BigInteger count;

            for (int e = 0; e <= elements - 1; e++)
            {
                if (countop == CountOp.CountOp_CLS)
                {
                    count = (BigInteger)CountLeadingSignBits(Elem(operand, e, esize));
                }
                else
                {
                    count = (BigInteger)CountLeadingZeroBits(Elem(operand, e, esize));
                }

                Elem(result, e, esize, count.SubBigInteger(esize - 1, 0));
            }

            V(d, result);
        }

        // cmeq_advsimd_zero.html#CMEQ_asisdmisc_Z
        public static void Cmeq_Zero_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool op = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            CompareOp comparison;

            switch (Bits.Concat(op, U))
            {
                case Bits bits when bits == "00":
                    comparison = CompareOp.CompareOp_GT;
                    break;
                case Bits bits when bits == "01":
                    comparison = CompareOp.CompareOp_GE;
                    break;
                default:
                case Bits bits when bits == "10":
                    comparison = CompareOp.CompareOp_EQ;
                    break;
                case Bits bits when bits == "11":
                    comparison = CompareOp.CompareOp_LE;
                    break;
            }

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    default:
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmeq_advsimd_zero.html#CMEQ_asimdmisc_Z
        public static void Cmeq_Zero_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool op = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            CompareOp comparison;

            switch (Bits.Concat(op, U))
            {
                case Bits bits when bits == "00":
                    comparison = CompareOp.CompareOp_GT;
                    break;
                case Bits bits when bits == "01":
                    comparison = CompareOp.CompareOp_GE;
                    break;
                default:
                case Bits bits when bits == "10":
                    comparison = CompareOp.CompareOp_EQ;
                    break;
                case Bits bits when bits == "11":
                    comparison = CompareOp.CompareOp_LE;
                    break;
            }

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    default:
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmge_advsimd_zero.html#CMGE_asisdmisc_Z
        public static void Cmge_Zero_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool op = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            CompareOp comparison;

            switch (Bits.Concat(op, U))
            {
                case Bits bits when bits == "00":
                    comparison = CompareOp.CompareOp_GT;
                    break;
                default:
                case Bits bits when bits == "01":
                    comparison = CompareOp.CompareOp_GE;
                    break;
                case Bits bits when bits == "10":
                    comparison = CompareOp.CompareOp_EQ;
                    break;
                case Bits bits when bits == "11":
                    comparison = CompareOp.CompareOp_LE;
                    break;
            }

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    default:
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmge_advsimd_zero.html#CMGE_asimdmisc_Z
        public static void Cmge_Zero_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool op = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            CompareOp comparison;

            switch (Bits.Concat(op, U))
            {
                case Bits bits when bits == "00":
                    comparison = CompareOp.CompareOp_GT;
                    break;
                default:
                case Bits bits when bits == "01":
                    comparison = CompareOp.CompareOp_GE;
                    break;
                case Bits bits when bits == "10":
                    comparison = CompareOp.CompareOp_EQ;
                    break;
                case Bits bits when bits == "11":
                    comparison = CompareOp.CompareOp_LE;
                    break;
            }

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    default:
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmgt_advsimd_zero.html#CMGT_asisdmisc_Z
        public static void Cmgt_Zero_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool op = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            CompareOp comparison;

            switch (Bits.Concat(op, U))
            {
                default:
                case Bits bits when bits == "00":
                    comparison = CompareOp.CompareOp_GT;
                    break;
                case Bits bits when bits == "01":
                    comparison = CompareOp.CompareOp_GE;
                    break;
                case Bits bits when bits == "10":
                    comparison = CompareOp.CompareOp_EQ;
                    break;
                case Bits bits when bits == "11":
                    comparison = CompareOp.CompareOp_LE;
                    break;
            }

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    default:
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmgt_advsimd_zero.html#CMGT_asimdmisc_Z
        public static void Cmgt_Zero_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool op = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            CompareOp comparison;

            switch (Bits.Concat(op, U))
            {
                default:
                case Bits bits when bits == "00":
                    comparison = CompareOp.CompareOp_GT;
                    break;
                case Bits bits when bits == "01":
                    comparison = CompareOp.CompareOp_GE;
                    break;
                case Bits bits when bits == "10":
                    comparison = CompareOp.CompareOp_EQ;
                    break;
                case Bits bits when bits == "11":
                    comparison = CompareOp.CompareOp_LE;
                    break;
            }

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    default:
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmle_advsimd.html#CMLE_asisdmisc_Z
        public static void Cmle_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool op = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            CompareOp comparison;

            switch (Bits.Concat(op, U))
            {
                case Bits bits when bits == "00":
                    comparison = CompareOp.CompareOp_GT;
                    break;
                case Bits bits when bits == "01":
                    comparison = CompareOp.CompareOp_GE;
                    break;
                case Bits bits when bits == "10":
                    comparison = CompareOp.CompareOp_EQ;
                    break;
                default:
                case Bits bits when bits == "11":
                    comparison = CompareOp.CompareOp_LE;
                    break;
            }

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    default:
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmle_advsimd.html#CMLE_asimdmisc_Z
        public static void Cmle_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool op = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            CompareOp comparison;

            switch (Bits.Concat(op, U))
            {
                case Bits bits when bits == "00":
                    comparison = CompareOp.CompareOp_GT;
                    break;
                case Bits bits when bits == "01":
                    comparison = CompareOp.CompareOp_GE;
                    break;
                case Bits bits when bits == "10":
                    comparison = CompareOp.CompareOp_EQ;
                    break;
                default:
                case Bits bits when bits == "11":
                    comparison = CompareOp.CompareOp_LE;
                    break;
            }

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    default:
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmlt_advsimd.html#CMLT_asisdmisc_Z
        public static void Cmlt_S(Bits size, Bits Rn, Bits Rd)
        {
            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            CompareOp comparison = CompareOp.CompareOp_LT;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    default:
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmlt_advsimd.html#CMLT_asimdmisc_Z
        public static void Cmlt_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            CompareOp comparison = CompareOp.CompareOp_LT;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                switch (comparison)
                {
                    case CompareOp.CompareOp_GT:
                        test_passed = (element > (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_GE:
                        test_passed = (element >= (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_EQ:
                        test_passed = (element == (BigInteger)0);
                        break;
                    case CompareOp.CompareOp_LE:
                        test_passed = (element <= (BigInteger)0);
                        break;
                    default:
                    case CompareOp.CompareOp_LT:
                        test_passed = (element < (BigInteger)0);
                        break;
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cnt_advsimd.html
        public static void Cnt_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '00' then ReservedValue(); */

            int esize = 8;
            int datasize = (Q ? 128 : 64);
            int elements = datasize / 8;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            BigInteger count;

            for (int e = 0; e <= elements - 1; e++)
            {
                count = (BigInteger)BitCount(Elem(operand, e, esize));

                Elem(result, e, esize, count.SubBigInteger(esize - 1, 0));
            }

            V(d, result);
        }

        // fcvtns_advsimd.html#FCVTNS_asisdmisc_R
        public static void Fcvtns_S(Bits sz, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o2 = false;
            const bool o1 = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int esize = 32 << (int)UInt(sz);
            int datasize = esize;
            int elements = 1;

            FPRounding rounding = FPDecodeRounding(Bits.Concat(o1, o2));

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, esize);

                Elem(result, e, esize, FPToFixed(esize, element, 0, unsigned, FPCR, rounding));
            }

            V(d, result);
        }

        // fcvtns_advsimd.html#FCVTNS_asimdmisc_R
        public static void Fcvtns_V(bool Q, Bits sz, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o2 = false;
            const bool o1 = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if sz:Q == '10' then ReservedValue(); */

            int esize = 32 << (int)UInt(sz);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            FPRounding rounding = FPDecodeRounding(Bits.Concat(o1, o2));

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, esize);

                Elem(result, e, esize, FPToFixed(esize, element, 0, unsigned, FPCR, rounding));
            }

            V(d, result);
        }

        // fcvtnu_advsimd.html#FCVTNU_asisdmisc_R
        public static void Fcvtnu_S(Bits sz, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o2 = false;
            const bool o1 = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int esize = 32 << (int)UInt(sz);
            int datasize = esize;
            int elements = 1;

            FPRounding rounding = FPDecodeRounding(Bits.Concat(o1, o2));

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, esize);

                Elem(result, e, esize, FPToFixed(esize, element, 0, unsigned, FPCR, rounding));
            }

            V(d, result);
        }

        // fcvtnu_advsimd.html#FCVTNU_asimdmisc_R
        public static void Fcvtnu_V(bool Q, Bits sz, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o2 = false;
            const bool o1 = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if sz:Q == '10' then ReservedValue(); */

            int esize = 32 << (int)UInt(sz);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            FPRounding rounding = FPDecodeRounding(Bits.Concat(o1, o2));

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, esize);

                Elem(result, e, esize, FPToFixed(esize, element, 0, unsigned, FPCR, rounding));
            }

            V(d, result);
        }

        // neg_advsimd.html#NEG_asisdmisc_R
        public static void Neg_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool neg = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            BigInteger element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                if (neg)
                {
                    element = -element;
                }
                else
                {
                    element = Abs(element);
                }

                Elem(result, e, esize, element.SubBigInteger(esize - 1, 0));
            }

            V(d, result);
        }

        // neg_advsimd.html#NEG_asimdmisc_R
        public static void Neg_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool neg = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            BigInteger element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                if (neg)
                {
                    element = -element;
                }
                else
                {
                    element = Abs(element);
                }

                Elem(result, e, esize, element.SubBigInteger(esize - 1, 0));
            }

            V(d, result);
        }

        // not_advsimd.html
        public static void Not_V(bool Q, Bits Rn, Bits Rd)
        {
            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int esize = 8;
            int datasize = (Q ? 128 : 64);
            int elements = datasize / 8;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, esize);

                Elem(result, e, esize, NOT(element));
            }

            V(d, result);
        }

        // rbit_advsimd.html
        public static void Rbit_V(bool Q, Bits Rn, Bits Rd)
        {
            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int esize = 8;
            int datasize = (Q ? 128 : 64);
            int elements = datasize / 8;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits element;
            Bits rev = new Bits(esize);

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, esize);

                for (int i = 0; i <= esize - 1; i++)
                {
                    rev[esize - 1 - i] = element[i];
                }

                Elem(result, e, esize, rev);
            }

            V(d, result);
        }

        // rev16_advsimd.html
        public static void Rev16_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o0 = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            // size=esize:   B(0),  H(1),  S(1), D(S)
            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);

            // op=REVx: 64(0), 32(1), 16(2)
            Bits op = Bits.Concat(o0, U);

            // => op+size: 
            //    64+B = 0, 64+H = 1, 64+S = 2, 64+D = X
            //    32+B = 1, 32+H = 2, 32+S = X, 32+D = X
            //    16+B = 2, 16+H = X, 16+S = X, 16+D = X
            //     8+B = X,  8+H = X,  8+S = X,  8+D = X
            // => 3-(op+size) (index bits in group)
            //    64/B = 3, 64+H = 2, 64+S = 1, 64+D = X
            //    32+B = 2, 32+H = 1, 32+S = X, 32+D = X
            //    16+B = 1, 16+H = X, 16+S = X, 16+D = X
            //     8+B = X,  8+H = X,  8+S = X,  8+D = X

            // index bits within group: 1, 2, 3
            /* if UInt(op) + UInt(size) >= 3 then UnallocatedEncoding(); */

            int container_size;

            switch (op)
            {
                default:
                case Bits bits when bits == "10":
                    container_size = 16;
                    break;
                case Bits bits when bits == "01":
                    container_size = 32;
                    break;
                case Bits bits when bits == "00":
                    container_size = 64;
                    break;
            }

            int containers = datasize / container_size;
            int elements_per_container = container_size / esize;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            int element = 0;
            int rev_element;

            for (int c = 0; c <= containers - 1; c++)
            {
                rev_element = element + elements_per_container - 1;

                for (int e = 0; e <= elements_per_container - 1; e++)
                {
                    Elem(result, rev_element, esize, Elem(operand, element, esize));

                    element = element + 1;
                    rev_element = rev_element - 1;
                }
            }

            V(d, result);
        }

        // rev32_advsimd.html
        public static void Rev32_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o0 = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            // size=esize:   B(0),  H(1),  S(1), D(S)
            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);

            // op=REVx: 64(0), 32(1), 16(2)
            Bits op = Bits.Concat(o0, U);

            // => op+size: 
            //    64+B = 0, 64+H = 1, 64+S = 2, 64+D = X
            //    32+B = 1, 32+H = 2, 32+S = X, 32+D = X
            //    16+B = 2, 16+H = X, 16+S = X, 16+D = X
            //     8+B = X,  8+H = X,  8+S = X,  8+D = X
            // => 3-(op+size) (index bits in group)
            //    64/B = 3, 64+H = 2, 64+S = 1, 64+D = X
            //    32+B = 2, 32+H = 1, 32+S = X, 32+D = X
            //    16+B = 1, 16+H = X, 16+S = X, 16+D = X
            //     8+B = X,  8+H = X,  8+S = X,  8+D = X

            // index bits within group: 1, 2, 3
            /* if UInt(op) + UInt(size) >= 3 then UnallocatedEncoding(); */

            int container_size;

            switch (op)
            {
                case Bits bits when bits == "10":
                    container_size = 16;
                    break;
                default:
                case Bits bits when bits == "01":
                    container_size = 32;
                    break;
                case Bits bits when bits == "00":
                    container_size = 64;
                    break;
            }

            int containers = datasize / container_size;
            int elements_per_container = container_size / esize;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            int element = 0;
            int rev_element;

            for (int c = 0; c <= containers - 1; c++)
            {
                rev_element = element + elements_per_container - 1;

                for (int e = 0; e <= elements_per_container - 1; e++)
                {
                    Elem(result, rev_element, esize, Elem(operand, element, esize));

                    element = element + 1;
                    rev_element = rev_element - 1;
                }
            }

            V(d, result);
        }

        // rev64_advsimd.html
        public static void Rev64_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o0 = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            // size=esize:   B(0),  H(1),  S(1), D(S)
            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);

            // op=REVx: 64(0), 32(1), 16(2)
            Bits op = Bits.Concat(o0, U);

            // => op+size: 
            //    64+B = 0, 64+H = 1, 64+S = 2, 64+D = X
            //    32+B = 1, 32+H = 2, 32+S = X, 32+D = X
            //    16+B = 2, 16+H = X, 16+S = X, 16+D = X
            //     8+B = X,  8+H = X,  8+S = X,  8+D = X
            // => 3-(op+size) (index bits in group)
            //    64/B = 3, 64+H = 2, 64+S = 1, 64+D = X
            //    32+B = 2, 32+H = 1, 32+S = X, 32+D = X
            //    16+B = 1, 16+H = X, 16+S = X, 16+D = X
            //     8+B = X,  8+H = X,  8+S = X,  8+D = X

            // index bits within group: 1, 2, 3
            /* if UInt(op) + UInt(size) >= 3 then UnallocatedEncoding(); */

            int container_size;

            switch (op)
            {
                case Bits bits when bits == "10":
                    container_size = 16;
                    break;
                case Bits bits when bits == "01":
                    container_size = 32;
                    break;
                default:
                case Bits bits when bits == "00":
                    container_size = 64;
                    break;
            }

            int containers = datasize / container_size;
            int elements_per_container = container_size / esize;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);

            int element = 0;
            int rev_element;

            for (int c = 0; c <= containers - 1; c++)
            {
                rev_element = element + elements_per_container - 1;

                for (int e = 0; e <= elements_per_container - 1; e++)
                {
                    Elem(result, rev_element, esize, Elem(operand, element, esize));

                    element = element + 1;
                    rev_element = rev_element - 1;
                }
            }

            V(d, result);
        }

        // sadalp_advsimd.html
        public static void Sadalp_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool op = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / (2 * esize);

            bool acc = (op == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand = V(datasize, n);
            Bits sum;
            BigInteger op1;
            BigInteger op2;

            Bits result = (acc ? V(datasize, d) : Zeros(datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                op1 = Int(Elem(operand, 2 * e + 0, esize), unsigned);
                op2 = Int(Elem(operand, 2 * e + 1, esize), unsigned);

                sum = (op1 + op2).SubBigInteger(2 * esize - 1, 0);

                Elem(result, e, 2 * esize, Elem(result, e, 2 * esize) + sum);
            }

            V(d, result);
        }

        // saddlp_advsimd.html
        public static void Saddlp_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool op = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / (2 * esize);

            bool acc = (op == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand = V(datasize, n);
            Bits sum;
            BigInteger op1;
            BigInteger op2;

            Bits result = (acc ? V(datasize, d) : Zeros(datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                op1 = Int(Elem(operand, 2 * e + 0, esize), unsigned);
                op2 = Int(Elem(operand, 2 * e + 1, esize), unsigned);

                sum = (op1 + op2).SubBigInteger(2 * esize - 1, 0);

                Elem(result, e, 2 * esize, Elem(result, e, 2 * esize) + sum);
            }

            V(d, result);
        }

        // sha256su0_advsimd.html
        public static void Sha256su0_V(Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if !HaveCryptoExt() then UnallocatedEncoding(); */

            /* Operation */
            /* CheckCryptoEnabled64(); */

            Bits result = new Bits(128);
            Bits operand1 = V(128, d);
            Bits operand2 = V(128, n);
            Bits T = Bits.Concat(operand2[31, 0], operand1[127, 32]); // bits(128)
            Bits elt; // bits(32)

            for (int e = 0; e <= 3; e++)
            {
                elt = Elem(T, e, 32);
                elt = EOR(EOR(ROR(elt, 7), ROR(elt, 18)), LSR(elt, 3));
                Elem(result, e, 32, elt + Elem(operand1, e, 32));
            }

            V(d, result);
        }

        // sqabs_advsimd.html#SQABS_asisdmisc_R
        public static void Sqabs_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool neg = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                if (neg)
                {
                    element = -element;
                }
                else
                {
                    element = Abs(element);
                }

                (Bits _result, bool _sat) = SignedSatQ(element, esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqabs_advsimd.html#SQABS_asimdmisc_R
        public static void Sqabs_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool neg = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                if (neg)
                {
                    element = -element;
                }
                else
                {
                    element = Abs(element);
                }

                (Bits _result, bool _sat) = SignedSatQ(element, esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqneg_advsimd.html#SQNEG_asisdmisc_R
        public static void Sqneg_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool neg = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                if (neg)
                {
                    element = -element;
                }
                else
                {
                    element = Abs(element);
                }

                (Bits _result, bool _sat) = SignedSatQ(element, esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqneg_advsimd.html#SQNEG_asimdmisc_R
        public static void Sqneg_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool neg = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            BigInteger element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = SInt(Elem(operand, e, esize));

                if (neg)
                {
                    element = -element;
                }
                else
                {
                    element = Abs(element);
                }

                (Bits _result, bool _sat) = SignedSatQ(element, esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqxtn_advsimd.html#SQXTN_asisdmisc_N
        public static void Sqxtn_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int part = 0;
            int elements = 1;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(2 * datasize, n);
            Bits element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, 2 * esize);

                (Bits _result, bool _sat) = SatQ(Int(element, unsigned), esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            Vpart(d, part, result);
        }

        // sqxtn_advsimd.html#SQXTN_asimdmisc_N
        public static void Sqxtn_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(2 * datasize, n);
            Bits element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, 2 * esize);

                (Bits _result, bool _sat) = SatQ(Int(element, unsigned), esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            Vpart(d, part, result);
        }

        // sqxtun_advsimd.html#SQXTUN_asisdmisc_N
        public static void Sqxtun_S(Bits size, Bits Rn, Bits Rd)
        {
            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int part = 0;
            int elements = 1;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(2 * datasize, n);
            Bits element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, 2 * esize);

                (Bits _result, bool _sat) = UnsignedSatQ(SInt(element), esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            Vpart(d, part, result);
        }

        // sqxtun_advsimd.html#SQXTUN_asimdmisc_N
        public static void Sqxtun_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(2 * datasize, n);
            Bits element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, 2 * esize);

                (Bits _result, bool _sat) = UnsignedSatQ(SInt(element), esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            Vpart(d, part, result);
        }

        // suqadd_advsimd.html#SUQADD_asisdmisc_R
        public static void Suqadd_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits operand2 = V(datasize, d);
            BigInteger op1;
            BigInteger op2;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                op1 = Int(Elem(operand, e, esize), !unsigned);
                op2 = Int(Elem(operand2, e, esize), unsigned);

                (Bits _result, bool _sat) = SatQ(op1 + op2, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // suqadd_advsimd.html#SUQADD_asimdmisc_R
        public static void Suqadd_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits operand2 = V(datasize, d);
            BigInteger op1;
            BigInteger op2;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                op1 = Int(Elem(operand, e, esize), !unsigned);
                op2 = Int(Elem(operand2, e, esize), unsigned);

                (Bits _result, bool _sat) = SatQ(op1 + op2, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // uadalp_advsimd.html
        public static void Uadalp_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool op = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / (2 * esize);

            bool acc = (op == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand = V(datasize, n);
            Bits sum;
            BigInteger op1;
            BigInteger op2;

            Bits result = (acc ? V(datasize, d) : Zeros(datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                op1 = Int(Elem(operand, 2 * e + 0, esize), unsigned);
                op2 = Int(Elem(operand, 2 * e + 1, esize), unsigned);

                sum = (op1 + op2).SubBigInteger(2 * esize - 1, 0);

                Elem(result, e, 2 * esize, Elem(result, e, 2 * esize) + sum);
            }

            V(d, result);
        }

        // uaddlp_advsimd.html
        public static void Uaddlp_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool op = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / (2 * esize);

            bool acc = (op == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand = V(datasize, n);
            Bits sum;
            BigInteger op1;
            BigInteger op2;

            Bits result = (acc ? V(datasize, d) : Zeros(datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                op1 = Int(Elem(operand, 2 * e + 0, esize), unsigned);
                op2 = Int(Elem(operand, 2 * e + 1, esize), unsigned);

                sum = (op1 + op2).SubBigInteger(2 * esize - 1, 0);

                Elem(result, e, 2 * esize, Elem(result, e, 2 * esize) + sum);
            }

            V(d, result);
        }

        // uqxtn_advsimd.html#UQXTN_asisdmisc_N
        public static void Uqxtn_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int part = 0;
            int elements = 1;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(2 * datasize, n);
            Bits element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, 2 * esize);

                (Bits _result, bool _sat) = SatQ(Int(element, unsigned), esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            Vpart(d, part, result);
        }

        // uqxtn_advsimd.html#UQXTN_asimdmisc_N
        public static void Uqxtn_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(2 * datasize, n);
            Bits element;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, 2 * esize);

                (Bits _result, bool _sat) = SatQ(Int(element, unsigned), esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            Vpart(d, part, result);
        }

        // usqadd_advsimd.html#USQADD_asisdmisc_R
        public static void Usqadd_S(Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits operand2 = V(datasize, d);
            BigInteger op1;
            BigInteger op2;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                op1 = Int(Elem(operand, e, esize), !unsigned);
                op2 = Int(Elem(operand2, e, esize), unsigned);

                (Bits _result, bool _sat) = SatQ(op1 + op2, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // usqadd_advsimd.html#USQADD_asimdmisc_R
        public static void Usqadd_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(datasize, n);
            Bits operand2 = V(datasize, d);
            BigInteger op1;
            BigInteger op2;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                op1 = Int(Elem(operand, e, esize), !unsigned);
                op2 = Int(Elem(operand2, e, esize), unsigned);

                (Bits _result, bool _sat) = SatQ(op1 + op2, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // xtn_advsimd.html
        public static void Xtn_V(bool Q, Bits size, Bits Rn, Bits Rd)
        {
            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand = V(2 * datasize, n);
            Bits element;

            for (int e = 0; e <= elements - 1; e++)
            {
                element = Elem(operand, e, 2 * esize);

                Elem(result, e, esize, element[esize - 1, 0]);
            }

            Vpart(d, part, result);
        }
#endregion

#region "SimdReg"
        // add_advsimd.html#ADD_asisdsame_only
        public static void Add_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool sub_op = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, esize);
                element2 = Elem(operand2, e, esize);

                if (sub_op)
                {
                    Elem(result, e, esize, element1 - element2);
                }
                else
                {
                    Elem(result, e, esize, element1 + element2);
                }
            }

            V(d, result);
        }

        // add_advsimd.html#ADD_asimdsame_only
        public static void Add_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool sub_op = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, esize);
                element2 = Elem(operand2, e, esize);

                if (sub_op)
                {
                    Elem(result, e, esize, element1 - element2);
                }
                else
                {
                    Elem(result, e, esize, element1 + element2);
                }
            }

            V(d, result);
        }

        // addhn_advsimd.html
        public static void Addhn_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o1 = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool round = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(2 * datasize, n);
            Bits operand2 = V(2 * datasize, m);
            BigInteger round_const = (round ? (BigInteger)1 << (esize - 1) : 0);
            Bits sum;
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, 2 * esize);
                element2 = Elem(operand2, e, 2 * esize);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                sum = sum + round_const;

                Elem(result, e, esize, sum[2 * esize - 1, esize]);
            }

            Vpart(d, part, result);
        }

        // addp_advsimd_vec.html
        public static void Addp_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits concat =  Bits.Concat(operand2, operand1);
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(concat, 2 * e, esize);
                element2 = Elem(concat, (2 * e) + 1, esize);

                Elem(result, e, esize, element1 + element2);
            }

            V(d, result);
        }

        // and_advsimd.html
        public static void And_V(bool Q, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (Q ? 128 : 64);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);

            Bits result = AND(operand1, operand2);

            V(d, result);
        }

        // bic_advsimd_reg.html
        public static void Bic_V(bool Q, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (Q ? 128 : 64);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);

            operand2 = NOT(operand2);

            Bits result = AND(operand1, operand2);

            V(d, result);
        }

        // bif_advsimd.html
        public static void Bif_V(bool Q, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (Q ? 128 : 64);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1;
            Bits operand3;
            Bits operand4 = V(datasize, n);

            operand1 = V(datasize, d);
            operand3 = NOT(V(datasize, m));

            V(d, EOR(operand1, AND(EOR(operand1, operand4), operand3)));
        }

        // bit_advsimd.html
        public static void Bit_V(bool Q, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (Q ? 128 : 64);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1;
            Bits operand3;
            Bits operand4 = V(datasize, n);

            operand1 = V(datasize, d);
            operand3 = V(datasize, m);

            V(d, EOR(operand1, AND(EOR(operand1, operand4), operand3)));
        }

        // bsl_advsimd.html
        public static void Bsl_V(bool Q, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (Q ? 128 : 64);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1;
            Bits operand3;
            Bits operand4 = V(datasize, n);

            operand1 = V(datasize, m);
            operand3 = V(datasize, d);

            V(d, EOR(operand1, AND(EOR(operand1, operand4), operand3)));
        }

        // cmeq_advsimd_reg.html#CMEQ_asisdsame_only
        public static void Cmeq_Reg_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool and_test = (U == false);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits element1;
            Bits element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, esize);
                element2 = Elem(operand2, e, esize);

                if (and_test)
                {
                    test_passed = !IsZero(AND(element1, element2));
                }
                else
                {
                    test_passed = (element1 == element2);
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmeq_advsimd_reg.html#CMEQ_asimdsame_only
        public static void Cmeq_Reg_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool and_test = (U == false);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits element1;
            Bits element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, esize);
                element2 = Elem(operand2, e, esize);

                if (and_test)
                {
                    test_passed = !IsZero(AND(element1, element2));
                }
                else
                {
                    test_passed = (element1 == element2);
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmge_advsimd_reg.html#CMGE_asisdsame_only
        public static void Cmge_Reg_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool eq = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);
            bool cmp_eq = (eq == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                test_passed = (cmp_eq ? element1 >= element2 : element1 > element2);

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmge_advsimd_reg.html#CMGE_asimdsame_only
        public static void Cmge_Reg_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool eq = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);
            bool cmp_eq = (eq == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                test_passed = (cmp_eq ? element1 >= element2 : element1 > element2);

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmgt_advsimd_reg.html#CMGT_asisdsame_only
        public static void Cmgt_Reg_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool eq = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);
            bool cmp_eq = (eq == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                test_passed = (cmp_eq ? element1 >= element2 : element1 > element2);

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmgt_advsimd_reg.html#CMGT_asimdsame_only
        public static void Cmgt_Reg_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool eq = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);
            bool cmp_eq = (eq == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                test_passed = (cmp_eq ? element1 >= element2 : element1 > element2);

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmhi_advsimd.html#CMHI_asisdsame_only
        public static void Cmhi_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool eq = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);
            bool cmp_eq = (eq == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                test_passed = (cmp_eq ? element1 >= element2 : element1 > element2);

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmhi_advsimd.html#CMHI_asimdsame_only
        public static void Cmhi_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool eq = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);
            bool cmp_eq = (eq == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                test_passed = (cmp_eq ? element1 >= element2 : element1 > element2);

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmhs_advsimd.html#CMHS_asisdsame_only
        public static void Cmhs_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool eq = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);
            bool cmp_eq = (eq == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                test_passed = (cmp_eq ? element1 >= element2 : element1 > element2);

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmhs_advsimd.html#CMHS_asimdsame_only
        public static void Cmhs_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool eq = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);
            bool cmp_eq = (eq == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                test_passed = (cmp_eq ? element1 >= element2 : element1 > element2);

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmtst_advsimd.html#CMTST_asisdsame_only
        public static void Cmtst_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool and_test = (U == false);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits element1;
            Bits element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, esize);
                element2 = Elem(operand2, e, esize);

                if (and_test)
                {
                    test_passed = !IsZero(AND(element1, element2));
                }
                else
                {
                    test_passed = (element1 == element2);
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // cmtst_advsimd.html#CMTST_asimdsame_only
        public static void Cmtst_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool and_test = (U == false);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits element1;
            Bits element2;

            bool test_passed;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, esize);
                element2 = Elem(operand2, e, esize);

                if (and_test)
                {
                    test_passed = !IsZero(AND(element1, element2));
                }
                else
                {
                    test_passed = (element1 == element2);
                }

                Elem(result, e, esize, test_passed ? Ones(esize) : Zeros(esize));
            }

            V(d, result);
        }

        // eor_advsimd.html
        public static void Eor_V(bool Q, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (Q ? 128 : 64);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, m);
            Bits operand2 = Zeros(datasize);
            Bits operand3 = Ones(datasize);
            Bits operand4 = V(datasize, n);

            Bits result = EOR(operand1, AND(EOR(operand2, operand4), operand3));

            V(d, result);
        }

        // orn_advsimd.html
        public static void Orn_V(bool Q, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (Q ? 128 : 64);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);

            operand2 = NOT(operand2);

            Bits result = OR(operand1, operand2);

            V(d, result);
        }

        // orr_advsimd_reg.html
        public static void Orr_V(bool Q, Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int datasize = (Q ? 128 : 64);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);

            Bits result = OR(operand1, operand2);

            V(d, result);
        }

        // raddhn_advsimd.html
        public static void Raddhn_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o1 = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool round = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(2 * datasize, n);
            Bits operand2 = V(2 * datasize, m);
            BigInteger round_const = (round ? (BigInteger)1 << (esize - 1) : 0);
            Bits sum;
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, 2 * esize);
                element2 = Elem(operand2, e, 2 * esize);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                sum = sum + round_const;

                Elem(result, e, esize, sum[2 * esize - 1, esize]);
            }

            Vpart(d, part, result);
        }

        // rsubhn_advsimd.html
        public static void Rsubhn_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o1 = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool round = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(2 * datasize, n);
            Bits operand2 = V(2 * datasize, m);
            BigInteger round_const = (round ? (BigInteger)1 << (esize - 1) : 0);
            Bits sum;
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, 2 * esize);
                element2 = Elem(operand2, e, 2 * esize);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                sum = sum + round_const;

                Elem(result, e, esize, sum[2 * esize - 1, esize]);
            }

            Vpart(d, part, result);
        }

        // saba_advsimd.html
        public static void Saba_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool ac = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);
            bool accumulate = (ac == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            Bits absdiff;

            Bits result = (accumulate ? V(datasize, d) : Zeros(datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                absdiff = Abs(element1 - element2).SubBigInteger(esize - 1, 0);

                Elem(result, e, esize, Elem(result, e, esize) + absdiff);
            }

            V(d, result);
        }

        // sabal_advsimd.html
        public static void Sabal_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool op = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool accumulate = (op == false);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            Bits absdiff;

            Bits result = (accumulate ? V(2 * datasize, d) : Zeros(2 * datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                absdiff = Abs(element1 - element2).SubBigInteger(2 * esize - 1, 0);

                Elem(result, e, 2 * esize, Elem(result, e, 2 * esize) + absdiff);
            }

            V(d, result);
        }

        // sabd_advsimd.html
        public static void Sabd_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool ac = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);
            bool accumulate = (ac == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            Bits absdiff;

            Bits result = (accumulate ? V(datasize, d) : Zeros(datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                absdiff = Abs(element1 - element2).SubBigInteger(esize - 1, 0);

                Elem(result, e, esize, Elem(result, e, esize) + absdiff);
            }

            V(d, result);
        }

        // sabdl_advsimd.html
        public static void Sabdl_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool op = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool accumulate = (op == false);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            Bits absdiff;

            Bits result = (accumulate ? V(2 * datasize, d) : Zeros(2 * datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                absdiff = Abs(element1 - element2).SubBigInteger(2 * esize - 1, 0);

                Elem(result, e, 2 * esize, Elem(result, e, 2 * esize) + absdiff);
            }

            V(d, result);
        }

        // saddl_advsimd.html
        public static void Saddl_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o1 = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                Elem(result, e, 2 * esize, sum.SubBigInteger(2 * esize - 1, 0));
            }

            V(d, result);
        }

        // saddw_advsimd.html
        public static void Saddw_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o1 = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = V(2 * datasize, n);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, 2 * esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                Elem(result, e, 2 * esize, sum.SubBigInteger(2 * esize - 1, 0));
            }

            V(d, result);
        }

        // sha256h_advsimd.html
        public static void Sha256h_V(Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if !HaveCryptoExt() then UnallocatedEncoding(); */

            /* Operation */
            /* CheckCryptoEnabled64(); */

            Bits result = SHA256hash(V(128, d), V(128, n), V(128, m), true);

            V(d, result);
        }

        // sha256h2_advsimd.html
        public static void Sha256h2_V(Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if !HaveCryptoExt() then UnallocatedEncoding(); */

            /* Operation */
            /* CheckCryptoEnabled64(); */

            Bits result = SHA256hash(V(128, n), V(128, d), V(128, m), false);

            V(d, result);
        }

        // sha256su1_advsimd.html
        public static void Sha256su1_V(Bits Rm, Bits Rn, Bits Rd)
        {
            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if !HaveCryptoExt() then UnallocatedEncoding(); */

            /* Operation */
            /* CheckCryptoEnabled64(); */

            Bits result = new Bits(128);
            Bits operand1 = V(128, d);
            Bits operand2 = V(128, n);
            Bits operand3 = V(128, m);
            Bits T0 = Bits.Concat(operand3[31, 0], operand2[127, 32]); // bits(128)
            Bits T1; // bits(64)
            Bits elt; // bits(32)

            T1 = operand3[127, 64];
            for (int e = 0; e <= 1; e++)
            {
                elt = Elem(T1, e, 32);
                elt = EOR(EOR(ROR(elt, 17), ROR(elt, 19)), LSR(elt, 10));
                elt = elt + Elem(operand1, e, 32) + Elem(T0, e, 32);
                Elem(result, e, 32, elt);
            }

            T1 = result[63, 0];
            for (int e = 2; e <= 3; e++)
            {
                elt = Elem(T1, e - 2, 32);
                elt = EOR(EOR(ROR(elt, 17), ROR(elt, 19)), LSR(elt, 10));
                elt = elt + Elem(operand1, e, 32) + Elem(T0, e, 32);
                Elem(result, e, 32, elt);
            }

            V(d, result);
        }

        // shadd_advsimd.html
        public static void Shadd_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                sum = element1 + element2;

                Elem(result, e, esize, sum.SubBigInteger(esize, 1));
            }

            V(d, result);
        }

        // shsub_advsimd.html
        public static void Shsub_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger diff;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                diff = element1 - element2;

                Elem(result, e, esize, diff.SubBigInteger(esize, 1));
            }

            V(d, result);
        }

        // smlal_advsimd_vec.html
        public static void Smlal_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o1 = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            Bits operand3 = V(2 * datasize, d);
            BigInteger element1;
            BigInteger element2;
            Bits product;
            Bits accum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                product = (element1 * element2).SubBigInteger(2 * esize - 1, 0);

                if (sub_op)
                {
                    accum = Elem(operand3, e, 2 * esize) - product;
                }
                else
                {
                    accum = Elem(operand3, e, 2 * esize) + product;
                }

                Elem(result, e, 2 * esize, accum);
            }

            V(d, result);
        }

        // smlsl_advsimd_vec.html
        public static void Smlsl_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o1 = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            Bits operand3 = V(2 * datasize, d);
            BigInteger element1;
            BigInteger element2;
            Bits product;
            Bits accum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                product = (element1 * element2).SubBigInteger(2 * esize - 1, 0);

                if (sub_op)
                {
                    accum = Elem(operand3, e, 2 * esize) - product;
                }
                else
                {
                    accum = Elem(operand3, e, 2 * esize) + product;
                }

                Elem(result, e, 2 * esize, accum);
            }

            V(d, result);
        }

        // sqadd_advsimd.html#SQADD_asisdsame_only
        public static void Sqadd_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                sum = element1 + element2;

                (Bits _result, bool _sat) = SatQ(sum, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqadd_advsimd.html#SQADD_asimdsame_only
        public static void Sqadd_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                sum = element1 + element2;

                (Bits _result, bool _sat) = SatQ(sum, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqdmulh_advsimd_vec.html#SQDMULH_asisdsame_only
        public static void Sqdmulh_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' || size == '00' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool rounding = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger round_const = (rounding ? (BigInteger)1 << (esize - 1) : 0);
            BigInteger element1;
            BigInteger element2;
            BigInteger product;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = SInt(Elem(operand1, e, esize));
                element2 = SInt(Elem(operand2, e, esize));

                product = (2 * element1 * element2) + round_const;

                (Bits _result, bool _sat) = SignedSatQ(product >> esize, esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqdmulh_advsimd_vec.html#SQDMULH_asimdsame_only
        public static void Sqdmulh_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' || size == '00' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool rounding = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger round_const = (rounding ? (BigInteger)1 << (esize - 1) : 0);
            BigInteger element1;
            BigInteger element2;
            BigInteger product;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = SInt(Elem(operand1, e, esize));
                element2 = SInt(Elem(operand2, e, esize));

                product = (2 * element1 * element2) + round_const;

                (Bits _result, bool _sat) = SignedSatQ(product >> esize, esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqrdmulh_advsimd_vec.html#SQRDMULH_asisdsame_only
        public static void Sqrdmulh_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' || size == '00' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool rounding = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger round_const = (rounding ? (BigInteger)1 << (esize - 1) : 0);
            BigInteger element1;
            BigInteger element2;
            BigInteger product;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = SInt(Elem(operand1, e, esize));
                element2 = SInt(Elem(operand2, e, esize));

                product = (2 * element1 * element2) + round_const;

                (Bits _result, bool _sat) = SignedSatQ(product >> esize, esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqrdmulh_advsimd_vec.html#SQRDMULH_asimdsame_only
        public static void Sqrdmulh_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' || size == '00' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool rounding = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger round_const = (rounding ? (BigInteger)1 << (esize - 1) : 0);
            BigInteger element1;
            BigInteger element2;
            BigInteger product;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = SInt(Elem(operand1, e, esize));
                element2 = SInt(Elem(operand2, e, esize));

                product = (2 * element1 * element2) + round_const;

                (Bits _result, bool _sat) = SignedSatQ(product >> esize, esize);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqsub_advsimd.html#SQSUB_asisdsame_only
        public static void Sqsub_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger diff;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                diff = element1 - element2;

                (Bits _result, bool _sat) = SatQ(diff, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // sqsub_advsimd.html#SQSUB_asimdsame_only
        public static void Sqsub_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger diff;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                diff = element1 - element2;

                (Bits _result, bool _sat) = SatQ(diff, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // srhadd_advsimd.html
        public static void Srhadd_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                Elem(result, e, esize, (element1 + element2 + 1).SubBigInteger(esize, 1));
            }

            V(d, result);
        }

        // ssubl_advsimd.html
        public static void Ssubl_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o1 = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                Elem(result, e, 2 * esize, sum.SubBigInteger(2 * esize - 1, 0));
            }

            V(d, result);
        }

        // ssubw_advsimd.html
        public static void Ssubw_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o1 = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = V(2 * datasize, n);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, 2 * esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                Elem(result, e, 2 * esize, sum.SubBigInteger(2 * esize - 1, 0));
            }

            V(d, result);
        }

        // sub_advsimd.html#SUB_asisdsame_only
        public static void Sub_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size != '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool sub_op = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, esize);
                element2 = Elem(operand2, e, esize);

                if (sub_op)
                {
                    Elem(result, e, esize, element1 - element2);
                }
                else
                {
                    Elem(result, e, esize, element1 + element2);
                }
            }

            V(d, result);
        }

        // sub_advsimd.html#SUB_asimdsame_only
        public static void Sub_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool sub_op = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, esize);
                element2 = Elem(operand2, e, esize);

                if (sub_op)
                {
                    Elem(result, e, esize, element1 - element2);
                }
                else
                {
                    Elem(result, e, esize, element1 + element2);
                }
            }

            V(d, result);
        }

        // subhn_advsimd.html
        public static void Subhn_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = false;
            const bool o1 = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool round = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(2 * datasize, n);
            Bits operand2 = V(2 * datasize, m);
            BigInteger round_const = (round ? (BigInteger)1 << (esize - 1) : 0);
            Bits sum;
            Bits element1;
            Bits element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Elem(operand1, e, 2 * esize);
                element2 = Elem(operand2, e, 2 * esize);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                sum = sum + round_const;

                Elem(result, e, esize, sum[2 * esize - 1, esize]);
            }

            Vpart(d, part, result);
        }

        // trn1_advsimd.html
        public static void Trn1_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool op = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;
            int part = (int)UInt(op);
            int pairs = elements / 2;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);

            for (int p = 0; p <= pairs - 1; p++)
            {
                Elem(result, 2 * p + 0, esize, Elem(operand1, 2 * p + part, esize));
                Elem(result, 2 * p + 1, esize, Elem(operand2, 2 * p + part, esize));
            }

            V(d, result);
        }

        // trn2_advsimd.html
        public static void Trn2_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool op = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;
            int part = (int)UInt(op);
            int pairs = elements / 2;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);

            for (int p = 0; p <= pairs - 1; p++)
            {
                Elem(result, 2 * p + 0, esize, Elem(operand1, 2 * p + part, esize));
                Elem(result, 2 * p + 1, esize, Elem(operand2, 2 * p + part, esize));
            }

            V(d, result);
        }

        // uaba_advsimd.html
        public static void Uaba_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool ac = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);
            bool accumulate = (ac == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            Bits absdiff;

            Bits result = (accumulate ? V(datasize, d) : Zeros(datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                absdiff = Abs(element1 - element2).SubBigInteger(esize - 1, 0);

                Elem(result, e, esize, Elem(result, e, esize) + absdiff);
            }

            V(d, result);
        }

        // uabal_advsimd.html
        public static void Uabal_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool op = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool accumulate = (op == false);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            Bits absdiff;

            Bits result = (accumulate ? V(2 * datasize, d) : Zeros(2 * datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                absdiff = Abs(element1 - element2).SubBigInteger(2 * esize - 1, 0);

                Elem(result, e, 2 * esize, Elem(result, e, 2 * esize) + absdiff);
            }

            V(d, result);
        }

        // uabd_advsimd.html
        public static void Uabd_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool ac = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);
            bool accumulate = (ac == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            Bits absdiff;

            Bits result = (accumulate ? V(datasize, d) : Zeros(datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                absdiff = Abs(element1 - element2).SubBigInteger(esize - 1, 0);

                Elem(result, e, esize, Elem(result, e, esize) + absdiff);
            }

            V(d, result);
        }

        // uabdl_advsimd.html
        public static void Uabdl_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool op = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool accumulate = (op == false);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            Bits absdiff;

            Bits result = (accumulate ? V(2 * datasize, d) : Zeros(2 * datasize));

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                absdiff = Abs(element1 - element2).SubBigInteger(2 * esize - 1, 0);

                Elem(result, e, 2 * esize, Elem(result, e, 2 * esize) + absdiff);
            }

            V(d, result);
        }

        // uaddl_advsimd.html
        public static void Uaddl_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o1 = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                Elem(result, e, 2 * esize, sum.SubBigInteger(2 * esize - 1, 0));
            }

            V(d, result);
        }

        // uaddw_advsimd.html
        public static void Uaddw_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o1 = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = V(2 * datasize, n);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, 2 * esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                Elem(result, e, 2 * esize, sum.SubBigInteger(2 * esize - 1, 0));
            }

            V(d, result);
        }

        // uhadd_advsimd.html
        public static void Uhadd_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                sum = element1 + element2;

                Elem(result, e, esize, sum.SubBigInteger(esize, 1));
            }

            V(d, result);
        }

        // uhsub_advsimd.html
        public static void Uhsub_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger diff;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                diff = element1 - element2;

                Elem(result, e, esize, diff.SubBigInteger(esize, 1));
            }

            V(d, result);
        }

        // umlal_advsimd_vec.html
        public static void Umlal_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o1 = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            Bits operand3 = V(2 * datasize, d);
            BigInteger element1;
            BigInteger element2;
            Bits product;
            Bits accum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                product = (element1 * element2).SubBigInteger(2 * esize - 1, 0);

                if (sub_op)
                {
                    accum = Elem(operand3, e, 2 * esize) - product;
                }
                else
                {
                    accum = Elem(operand3, e, 2 * esize) + product;
                }

                Elem(result, e, 2 * esize, accum);
            }

            V(d, result);
        }

        // umlsl_advsimd_vec.html
        public static void Umlsl_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o1 = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            Bits operand3 = V(2 * datasize, d);
            BigInteger element1;
            BigInteger element2;
            Bits product;
            Bits accum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                product = (element1 * element2).SubBigInteger(2 * esize - 1, 0);

                if (sub_op)
                {
                    accum = Elem(operand3, e, 2 * esize) - product;
                }
                else
                {
                    accum = Elem(operand3, e, 2 * esize) + product;
                }

                Elem(result, e, 2 * esize, accum);
            }

            V(d, result);
        }

        // uqadd_advsimd.html#UQADD_asisdsame_only
        public static void Uqadd_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                sum = element1 + element2;

                (Bits _result, bool _sat) = SatQ(sum, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // uqadd_advsimd.html#UQADD_asimdsame_only
        public static void Uqadd_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                sum = element1 + element2;

                (Bits _result, bool _sat) = SatQ(sum, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // uqsub_advsimd.html#UQSUB_asisdsame_only
        public static void Uqsub_S(Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Scalar */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            int esize = 8 << (int)UInt(size);
            int datasize = esize;
            int elements = 1;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger diff;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                diff = element1 - element2;

                (Bits _result, bool _sat) = SatQ(diff, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // uqsub_advsimd.html#UQSUB_asimdsame_only
        public static void Uqsub_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode Vector */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;
            BigInteger diff;
            bool sat;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                diff = element1 - element2;

                (Bits _result, bool _sat) = SatQ(diff, esize, unsigned);
                Elem(result, e, esize, _result);
                sat = _sat;

                if (sat)
                {
                    /* FPSR.QC = '1'; */
                    FPSR[27] = true; // TODO: Add named fields.
                }
            }

            V(d, result);
        }

        // urhadd_advsimd.html
        public static void Urhadd_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;

            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);
            BigInteger element1;
            BigInteger element2;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                Elem(result, e, esize, (element1 + element2 + 1).SubBigInteger(esize, 1));
            }

            V(d, result);
        }

        // usubl_advsimd.html
        public static void Usubl_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o1 = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = Vpart(datasize, n, part);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                Elem(result, e, 2 * esize, sum.SubBigInteger(2 * esize - 1, 0));
            }

            V(d, result);
        }

        // usubw_advsimd.html
        public static void Usubw_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool U = true;
            const bool o1 = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size == '11' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = 64;
            int part = (int)UInt(Q);
            int elements = datasize / esize;

            bool sub_op = (o1 == true);
            bool unsigned = (U == true);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(2 * datasize);
            Bits operand1 = V(2 * datasize, n);
            Bits operand2 = Vpart(datasize, m, part);
            BigInteger element1;
            BigInteger element2;
            BigInteger sum;

            for (int e = 0; e <= elements - 1; e++)
            {
                element1 = Int(Elem(operand1, e, 2 * esize), unsigned);
                element2 = Int(Elem(operand2, e, esize), unsigned);

                if (sub_op)
                {
                    sum = element1 - element2;
                }
                else
                {
                    sum = element1 + element2;
                }

                Elem(result, e, 2 * esize, sum.SubBigInteger(2 * esize - 1, 0));
            }

            V(d, result);
        }

        // uzp1_advsimd.html
        public static void Uzp1_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool op = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;
            int part = (int)UInt(op);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operandl = V(datasize, n);
            Bits operandh = V(datasize, m);

            Bits zipped = Bits.Concat(operandh, operandl);

            for (int e = 0; e <= elements - 1; e++)
            {
                Elem(result, e, esize, Elem(zipped, 2 * e + part, esize));
            }

            V(d, result);
        }

        // uzp2_advsimd.html
        public static void Uzp2_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool op = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;
            int part = (int)UInt(op);

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operandl = V(datasize, n);
            Bits operandh = V(datasize, m);

            Bits zipped = Bits.Concat(operandh, operandl);

            for (int e = 0; e <= elements - 1; e++)
            {
                Elem(result, e, esize, Elem(zipped, 2 * e + part, esize));
            }

            V(d, result);
        }

        // zip1_advsimd.html
        public static void Zip1_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool op = false;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;
            int part = (int)UInt(op);
            int pairs = elements / 2;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);

            int @base = part * pairs;

            for (int p = 0; p <= pairs - 1; p++)
            {
                Elem(result, 2 * p + 0, esize, Elem(operand1, @base + p, esize));
                Elem(result, 2 * p + 1, esize, Elem(operand2, @base + p, esize));
            }

            V(d, result);
        }

        // zip2_advsimd.html
        public static void Zip2_V(bool Q, Bits size, Bits Rm, Bits Rn, Bits Rd)
        {
            const bool op = true;

            /* Decode */
            int d = (int)UInt(Rd);
            int n = (int)UInt(Rn);
            int m = (int)UInt(Rm);

            /* if size:Q == '110' then ReservedValue(); */

            int esize = 8 << (int)UInt(size);
            int datasize = (Q ? 128 : 64);
            int elements = datasize / esize;
            int part = (int)UInt(op);
            int pairs = elements / 2;

            /* Operation */
            /* CheckFPAdvSIMDEnabled64(); */

            Bits result = new Bits(datasize);
            Bits operand1 = V(datasize, n);
            Bits operand2 = V(datasize, m);

            int @base = part * pairs;

            for (int p = 0; p <= pairs - 1; p++)
            {
                Elem(result, 2 * p + 0, esize, Elem(operand1, @base + p, esize));
                Elem(result, 2 * p + 1, esize, Elem(operand2, @base + p, esize));
            }

            V(d, result);
        }
#endregion
    }
}
