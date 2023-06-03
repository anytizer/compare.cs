using System.Security.Cryptography;

namespace compare
{
    // code using chatgpt and modified
    public class CRC32 : HashAlgorithm
    {
        private const uint Polynomial = 0xEDB88320;
        private readonly uint[] _table = new uint[256];
        private uint _crc;

        public CRC32()
        {
            InitializeTable();
            _crc = uint.MaxValue;
        }

        public override void Initialize()
        {
            _crc = uint.MaxValue;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (int i = ibStart; i < ibStart + cbSize; i++)
            {
                _crc = (_crc >> 8) ^ _table[array[i] ^ _crc & 0xFF];
            }
        }

        protected override byte[] HashFinal()
        {
            byte[] hash = BitConverter.GetBytes(~_crc);
            Array.Reverse(hash);
            return hash;
        }

        private void InitializeTable()
        {
            for (uint i = 0; i < 256; i++)
            {
                uint crc = i;
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) == 1 ? Polynomial ^ (crc >> 1) : crc >> 1;
                }
                _table[i] = crc;
            }
        }
    }
}
