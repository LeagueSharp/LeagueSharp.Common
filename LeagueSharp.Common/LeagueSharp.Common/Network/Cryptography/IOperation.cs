namespace LeagueSharp.Network.Cryptography
{
    public interface IOperation
    {
        byte Encrypt(byte data);
        byte Decrypt(byte data);
    }
}