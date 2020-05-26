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

        public Machine()
        {
            _ram = new Memory.RAM();
            _regs = new Memory.Registers();
        }

        public void RAMBurn(int address, byte[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                _ram.Write(address + i, values[i]);
            }
        }
    }
}