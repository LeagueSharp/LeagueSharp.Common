namespace LeagueSharp.Network.Cryptography
{
    internal class Not : IOperation
    {
        public byte Encrypt(byte data)
        {
            return (byte) ~data;
        }

        public byte Decrypt(byte data)
        {
            return (byte) ~data;
        }
    }
}