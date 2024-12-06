using Microsoft.OData.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using XLabFileReader.Services;
using XLabGlobal;

namespace XLabFileReader.Models
{
    public class XLabFile : ViewModel
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                Set(ref _id, value);
            }
        }

        private string _fileTitle;
        public string FileTitle
        {
            get { return _fileTitle; }
            set
            {
                _fileTitle = value;
                Set(ref _fileTitle, value);
            }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                Set(ref _fileName, value);
            }
        }

        private string _fileData;
        public string FileData
        {
            get { return _fileData; }
            set
            {
                _fileData = value;
                Set(ref _fileData, value);
            }
        }

        public async Task GetXLabFileDataAsync()
        {
            try
            {
                var context = new DefaultContainer(new Uri("https://testapi.x-lab.by/webapi"));
                context.SendingRequest2 += new EventHandler<SendingRequest2EventArgs>(delegate (object sender, SendingRequest2EventArgs e)
                {
                    e.RequestMessage.SetHeader("Authorization", "Bearer " + AuthorizationService.Token.AccessToken);
                });
                FileData = await context.GetXLabFileData(Id).GetValueAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveXLabFileDataAsync()
        {
            try
            {
                var context = new DefaultContainer(new Uri("https://testapi.x-lab.by/webapi"));
                context.SendingRequest2 += new EventHandler<SendingRequest2EventArgs>(delegate (object sender, SendingRequest2EventArgs e)
                {
                    e.RequestMessage.SetHeader("Authorization", "Bearer " + AuthorizationService.Token.AccessToken);
                });
                if (!await context.SaveXLabFileData(Id, FileData).GetValueAsync())
                {
                    throw new Exception("Не удалось сохранить файл на сервере");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
