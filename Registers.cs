using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip8_sharp.Machine.Memory
{
    public class Registers
    {
        private List<byte> gpr; // General purpose registers.
        private int pc; // Program counter.
        private Stack<byte> stk; // Stack.
        private int I; // I Register.

        public Registers()
        {
            // General purpose registers to hold registers 00 through FF.
            gpr = new List<byte>(16);
            // Program counter to hold address of current instruction.
            pc = 200; // TODO: Need to verify start address.
            // Stack to hold return addresses while subroutines run.
            stk = new Stack<byte>();
            // Special register for holding longer values such as memory addresses.
            I = 0;
        }

        public void setGPR(string address, byte value)
        {
            byte addr = convertStringToIndex(address);
            gpr[addr] = value;
        }

        public void setGPR(string address, string value)
        {
            setGPR(address, convertStringToIndex(value));
        }

        public byte getGPR(string address)
        {
            byte addr = convertStringToIndex(address);
            return gpr[addr];
        }

        public void setPC(string address)
        {
            pc = convertStringToIndex(address);
        }

        public void setPC(int address)
        {
            pc = address;
        }

        public void goSubroutine(string address)
        {
            stk.Push((byte)pc);
            pc = convertStringToIndex(address);
        }

        public void subroutineReturn()
        {
            pc = stk.Pop();
        }

        public void skipNextInstruction()
        {
            pc++;
        }

        public int getI()
        {
            return I;
        }

        public void setI(int value)
        {
            I = value;
        }

        private byte convertStringToIndex(string address)
        {
            byte addr = Convert.ToByte(address, 16);
            //if (addr >= 0 && addr < 16)
            //{
            return addr;
            //}
            //else
            //{
            //    throw new Exception(String.Format("Address {0} out of range", addr));
            //}
        }
    }
}