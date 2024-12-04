using DevExpress.Xpf.Reports.UserDesigner;
using DevExpress.XtraReports.UI;
using SalaryCalculator.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace ReportRedactor.ViewModels
{
    internal class ReportStorageViewModel : IReportStorage
    {
        private readonly ReportDesigner _reportDesigner;

        public ReportStorageViewModel(IReportDesignerUI reportDesigner) 
        {
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(ObservableCollection<SalaryDetail>));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(SalaryDetail));
            _reportDesigner = (ReportDesigner)reportDesigner;
            CreateNew();
        }

        public bool CanCreateNew()
        {
            return true;
        }

        public XtraReport CreateNew()
        {
            var report = _reportDesigner.ReportStorage.CreateNew();
            report.DataSource = new ObservableCollection<SalaryDetail>();
            _reportDesigner.OpenDocument(report);
            return report;
        }

        public XtraReport CreateNewSubreport()
        {
            return new XtraReport();
        }

        public bool CanOpen()
        {
            return true;
        }

        public string Open(IReportDesignerUI designer)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Файлы макета отчетов (*.xml)|*.xml|Все файлы (*.*)|*.*"
            };
            openFileDialog.ShowDialog();
            string filePath = openFileDialog.FileName;
            return filePath;
        }

        public XtraReport Load(string reportID, IReportSerializer designerReportSerializer)
        {
            var xtraReport = new XtraReport();
            try
            {
                using (FileStream stream = new FileStream(reportID, FileMode.Open, FileAccess.Read))
                {
                    xtraReport = designerReportSerializer.Load(stream);
                    xtraReport.DataSource = new ObservableCollection<SalaryDetail>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return xtraReport;
        }

        public string Save(string reportID, IReportProvider reportProvider, bool saveAs, string reportTitle, IReportDesignerUI designer)
        {
            XtraReport report = reportProvider.GetReport();
            try
            {
                if (saveAs)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "XtraReport Files (*.xml)|*.xml|All Files (*.*)|*.*",
                        Title = "Сохранить отчет",
                        FileName = "Report"
                    };
                    saveFileDialog.ShowDialog();
                    report.SaveLayoutToXml(saveFileDialog.FileName);
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        report.SaveLayoutToXml(ms);
                    }
                }
                return reportID;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetErrorMessage(Exception exception)
        {
            return exception.Message;
        }
    }
}
