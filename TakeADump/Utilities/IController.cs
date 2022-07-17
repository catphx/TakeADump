namespace TakeADump.Utilities
{
    internal interface IController
    {
        Task<byte[]> SendAsync(HttpMethod httpMethod);
    }
}