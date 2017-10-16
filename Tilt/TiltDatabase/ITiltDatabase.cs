using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tilt
{
    /// <summary>
    /// Interface to connect to a Tilt database
    /// </summary>
    public interface ITiltDatabase
    {
        Task<List<string>> GetItems();
        Task AddItem(string item);
        Task ClearItems();
    }
}
