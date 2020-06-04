using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip8_sharp.Machine
{
    public class Machine
    {
        private Memory.RAM _ram;
        private Memory.Registers _regs;
        private Random _rand;

        public Machine()
        {
            _ram = new Memory.RAM();
            _regs = new Memory.Registers();
            _rand = new Random();
        }

        public void clockTrigger()
        {
        }

        public void RAMBurn(int address, byte[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                _ram.Write(address + i, values[i]);
            }
        }

        private void instructionControl(byte instruction)
        {
            string ins = Convert.ToString(instruction, 16);
            switch (ins[0])
            {
                case '0':
                    instruction0Series(ins);
                    break;

                case '1':
                    // 1NNN Jump to address NNN.
                    _regs.setPC(ins.Substring(1, 3));
                    break;

                case '2':
                    // 2NNN Execute subroutine starting at address NNN.
                    _regs.goSubroutine(ins.Substring(1, 3));
                    break;

                case '3':
                    // 3XNN Skip the following instruction if the value of register VX equals NN.
                    byte val3 = convertStringToIndex(ins.Substring(2, 2));
                    if (_regs.getGPR(ins.Substring(1, 1)) == val3)
                    {
                        _regs.skipNextInstruction();
                    }
                    break;

                case '4':
                    // 4XNN Skip the following instruction if the value of register VX is not equal to NN.
                    byte val4 = convertStringToIndex(ins.Substring(2, 2));
                    if (_regs.getGPR(ins.Substring(1, 1)) != val4)
                    {
                        _regs.skipNextInstruction();
                    }
                    break;

                case '5':
                    // 5XY0 Skip the following instruction if the value of register VX is equal to the value of register VY.
                    if (_regs.getGPR(ins.Substring(1, 1)) == _regs.getGPR(ins.Substring(2, 1)))
                    {
                        _regs.skipNextInstruction();
                    }
                    break;

                case '6':
                    // 6XNN Store number NN in register VX.
                    _regs.setGPR(ins.Substring(1, 1), ins.Substring(2, 2));
                    break;

                case '7':
                    // 7XNN Add the value NN to register VX.
                    _regs.setGPR(ins.Substring(1, 1), (byte)(_regs.getGPR(ins.Substring(1, 1)) + convertStringToIndex(ins.Substring(2, 2))));
                    break;

                case '8':
                    instruction8series(ins);
                    break;

                case 'A':
                    // ANNN Store memory address NNN in register I.
                    _regs.setI(Convert.ToInt32(ins.Substring(1, 3), 16));
                    break;

                case 'B':
                    // BNNN Jump to address NNN + V0.
                    int addr = _regs.getGPR("0");
                    addr += Convert.ToInt32(ins.Substring(1, 3), 16);
                    _regs.setPC(addr - 1);
                    break;

                case 'C':
                    // CXNN Set VX to a random number with a mask of NN.
                    _regs.setGPR(ins.Substring(1, 1), (byte)((byte)_rand.Next(0, 255) & convertStringToIndex(ins.Substring(2, 2))));
                    break;

                case 'D':
                    // DXYN Draw a sprite at position VX, VY with N bytes of sprite data starting at the address stored in I.
                    // TODO: Some other time.
                    break;

                case 'E':
                    instructionESeries(ins);
                    break;

                case 'F':
                    instructionFSeries(ins);
                    break;

                default:
                    throw new Exception(String.Format("Invalid instruction: {0}", ins));
            }
        }

        private void instruction0Series(string ins)
        {
            if (ins[1] == '0')
            {
                if (ins == "00E0")
                {
                    // 00E0 Clear the screen.
                    // TODO: Clear the screen.
                }
                else if (ins == "00EE")
                {
                    // 00EE Return from a subroutine.
                    _regs.subroutineReturn();
                }
                else
                {
                    throw new Exception(String.Format("Invalid instruction: {0}", ins));
                }
            }
        }

        private void instruction8series(string ins)
        {
            switch (ins[3])
            {
                case '0':
                    // 8XY0 Store the value of register VY in register VX.
                    _regs.setGPR(ins.Substring(2, 1), _regs.getGPR(ins.Substring(1, 1)));
                    break;

                case '1':
                    // 8XY1 Set VX to VX OR VY.
                    _regs.setGPR(ins.Substring(1, 1), (byte)(_regs.getGPR(ins.Substring(1, 1)) | _regs.getGPR(ins.Substring(2, 1))));
                    break;

                case '2':
                    // 8XY2 Set VX to VX AND VY.
                    _regs.setGPR(ins.Substring(1, 1), (byte)(_regs.getGPR(ins.Substring(1, 1)) & _regs.getGPR(ins.Substring(2, 1))));
                    break;

                case '3':
                    // 8XY3 Set VX to VX XOR VY.
                    _regs.setGPR(ins.Substring(1, 1), (byte)(_regs.getGPR(ins.Substring(1, 1)) ^ _regs.getGPR(ins.Substring(2, 1))));
                    break;

                case '4':
                    // 8XY4 Add the value of register VY. Set VF to 1 if borrow occurs.
                    // TODO: Add with carry flag.
                    break;

                case '5':
                    // 8XY5 Subtract the value of register VY from VX. Set VF to 0 if borrow occurs.
                    break;

                case '6':
                    // 8XY6 Store the value of register VY shifted right one bit in register VX.
                    // Set register VF to the least significant bit prior to the shift.
                    byte regY6 = _regs.getGPR(ins.Substring(2, 1));
                    byte lsb6 = (byte)(regY6 & (byte)1); // lsb AND 00000001
                    _regs.setGPR(ins.Substring(1, 1), (byte)(regY6 >> 1));
                    _regs.setGPR("F", lsb6);
                    break;

                case '7':
                    // 8XY7 Set register VX to the value of VY minux VX. Set VF to 0 if a borrow occurs.
                    // TODO: VY - VX
                    break;

                case 'E':
                    // 8XYE Store the value of register VY shifted left one bit in register VX. Set register VF to the most significant bit prior to the shift.
                    byte regYE = _regs.getGPR(ins.Substring(2, 1));
                    byte msbE = (byte)((byte)(regYE & (byte)128) >> 7); // msb AND 10000000 >> 0000000X
                    _regs.setGPR(ins.Substring(1, 1), (byte)(regYE << 1));
                    _regs.setGPR("F", msbE);
                    break;

                default:
                    break;
            }
        }

        private void instructionESeries(string ins)
        {
        }

        private void instructionFSeries(string ins)
        {
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