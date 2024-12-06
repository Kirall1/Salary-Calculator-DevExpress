using XLabGlobal;
using System;
using System.Threading.Tasks;
using XLabFileReader.Models;
using System.Collections.ObjectModel;
using XLabFileReader.Services.Interfaces;
using Microsoft.OData.Client;

namespace XLabFileReader.Services
{
    public class XLabFileService : IXLabFileService
    {
        private DefaultContainer _context;

        public XLabFileService()
        {
            _context = new DefaultContainer(new Uri("https://testapi.x-lab.by/webapi"));
        }

        public void SetAuthorization()
        {
            _context.SendingRequest2 += new EventHandler<SendingRequest2EventArgs>(delegate (object sender, SendingRequest2EventArgs e)
            {
                e.RequestMessage.SetHeader("Authorization", "Bearer " + AuthorizationService.Token.AccessToken);
            });
        }

        public async Task<ObservableCollection<XLabFile>> GetXLabFilesAsync()
        {
            try
            {

                var items = await _context.XLabFile.ExecuteAsync();
                var result = new ObservableCollection<XLabFile>();
                foreach (var item in items)
                {
                    result.Add(new XLabFile
                    {
                        Id = item.Id,
                        FileTitle = item.FileTitle,
                        FileName = item.FileName
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveXLabFileAsync(XLabFile file)
        {
            try
            {
                var item = await _context.XLabFile.ByKey(file.Id).GetValueAsync();
                item.FileTitle = file.FileTitle;
                item.FileName = file.FileName;
                _context.UpdateObject(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteXLabFileAsync(XLabFile file)
        {
            try
            {
                var item = await _context.XLabFile.ByKey(file.Id).GetValueAsync();
                _context.DeleteObject(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 
    }
}
