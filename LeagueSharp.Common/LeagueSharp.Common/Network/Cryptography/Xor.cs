namespace LeagueSharp.Network.Cryptography
{
    internal class Xor : IOperation
    {
        private readonly byte parameter;

        public Xor(byte parameter)
        {
            this.parameter = parameter;
        }

        public byte Encrypt(byte data)
        {
            return (byte) (data ^ parameter);
        }

        public byte Decrypt(byte data)
        {
            return (byte) (data ^ parameter);
        }
    }
}