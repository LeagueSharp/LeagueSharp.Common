namespace LeagueSharp.Network.Cryptography
{
    internal class Inc : IOperation
    {
        public byte Encrypt(byte data)
        {
            return (byte) (data + 1);
        }

        public byte Decrypt(byte data)
        {
            return (byte) (data - 1);
        }
    }
}