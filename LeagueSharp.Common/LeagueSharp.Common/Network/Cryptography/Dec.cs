namespace LeagueSharp.Network.Cryptography
{
    public class Dec : IOperation
    {
        public byte Encrypt(byte data)
        {
            return (byte) (data - 1);
        }

        public byte Decrypt(byte data)
        {
            return (byte) (data + 1);
        }
    }
}