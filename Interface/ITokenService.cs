using NewsAggregator.Models;

namespace NewsAggregator.Interface
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}
