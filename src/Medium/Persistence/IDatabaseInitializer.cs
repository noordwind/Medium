using System.Threading.Tasks;

namespace Medium.Persistence
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}