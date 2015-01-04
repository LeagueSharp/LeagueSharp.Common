using LeagueSharp.Network.Util;

namespace LeagueSharp.Network.Cryptography
{
    internal class Ror : IOperation
    {
        private readonly byte parameter;

        public Ror(byte parameter)
        {
            this.parameter = parameter;
        }

        public byte Encrypt(byte data)
        {
            return data.RotateRight(parameter);
        }

        public byte Decrypt(byte data)
        {
            return data.RotateLeft(parameter);
        }
    }
}