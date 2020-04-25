namespace Markit.Api.Interfaces.Utils
{
    public interface IPasswordUtil
    {
        string Hash(string password);
        bool Verify(string hash, string password);
    }
}