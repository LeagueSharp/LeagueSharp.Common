using LeagueSharp.Network.Util;

namespace LeagueSharp.Network.Cryptography
{
    internal class Rol : IOperation
    {
        private readonly byte parameter;

        public Rol(byte parameter)
        {
            this.parameter = parameter;
        }

        public byte Encrypt(byte data)
        {
            return data.RotateLeft(parameter);
        }

        public byte Decrypt(byte data)
        {
            return data.RotateRight(parameter);
        }
    }
}