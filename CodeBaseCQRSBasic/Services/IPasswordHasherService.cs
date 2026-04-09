namespace CodeBaseCQRSBasic.Services;

public interface IPasswordHasherService
{
    string Hash(string value);
    bool Verify(string value, string hash);
}
