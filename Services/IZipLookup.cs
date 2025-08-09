namespace OurMarketBackend.Services
{
    public interface IZipLookup
    {
        bool TryGet(string zip, out (string City, string State) info);
    }
}
