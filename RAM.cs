using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip8_sharp.Machine.Memory
{
    public class RAM
    {
        private byte[] _memory;

        public RAM()
        {
            // Initialized to support memory addresses up to FFFF.
            _memory = new byte[65535];
        }

        public byte Read(int address)
        {
            return _memory[address];
        }

        public void Write(int address, byte value)
        {
            _memory[address] = value;
        }
    }
}