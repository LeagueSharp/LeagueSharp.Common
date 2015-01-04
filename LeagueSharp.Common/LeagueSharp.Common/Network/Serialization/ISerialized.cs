namespace LeagueSharp.Network.Serialization
{
    public interface ISerialized
    {
        bool Decode(byte[] data);
        byte[] Encode();
    }
}