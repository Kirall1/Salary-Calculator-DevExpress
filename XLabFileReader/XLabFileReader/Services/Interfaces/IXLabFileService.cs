using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XLabFileReader.Models;

namespace XLabFileReader.Services.Interfaces
{
    public interface IXLabFileService
    {
        public void SetAuthorization();
        public Task<ObservableCollection<XLabFile>> GetXLabFilesAsync();
        public Task SaveXLabFileAsync(XLabFile file);
        public Task DeleteXLabFileAsync(XLabFile file);
    }
}
