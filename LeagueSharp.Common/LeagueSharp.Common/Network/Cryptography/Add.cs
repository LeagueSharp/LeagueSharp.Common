namespace LeagueSharp.Network.Cryptography
{
    internal class Add : IOperation
    {
        private readonly byte parameter;

        public Add(byte parameter)
        {
            this.parameter = parameter;
        }

        public byte Encrypt(byte data)
        {
            return (byte) (data + parameter);
        }

        public byte Decrypt(byte data)
        {
            return (byte) (data - parameter);
        }
    }
}