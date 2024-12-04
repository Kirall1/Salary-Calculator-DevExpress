using SalaryCalculator.Models;
using SalaryCalculator.Data.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.XtraPrinting.Native.Extensions;
using SalaryCalculator.ViewModels.Base;
using System.Windows.Input;
using DevExpress.Mvvm.Xpf;
using System.Windows.Forms;
using SalaryCalculator.Properties;
using DevExpress.XtraReports.UI;
using DevExpress.Xpf.Printing;
using System.Windows;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using System.IO;
using System;
using DevExpress.XtraReports;

namespace SalaryCalculator.ViewModels
{
    public class DataGridViewModel : ViewModel
    {
        private readonly ISalaryDetailRepository _salaryDetailRepository;
        private readonly IRankCoefficientRepository _rankCoefficientRepository;
        private readonly IAdditionToSalaryRepository _additionToSalaryRepository;

        private object _selectedItem;

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value is SalaryDetail salaryDetail)
                {
                    AddCommand = new AsyncCommand(AddSalaryDetail);
                    UpdateCommand = new DelegateCommand(UpdateSalaryDetail);
                    DeleteCommand = new DelegateCommand(DeleteSalaryDetail);
                    Set(ref _selectedItem, value);
                }
                else if (value is AdditionToSalary additionToSalary)
                {
                    AddCommand = new AsyncCommand(AddAdditionToSalary);
                    UpdateCommand = new DelegateCommand(UpdateAdditionToSalary);
                    DeleteCommand = new DelegateCommand(DeleteAdditionToSalary);
                    Set(ref _selectedItem, value);
                }
                else if (SelectedAdditionSalaryDetail != null)
                {
                    AddCommand = new AsyncCommand(AddAdditionToSalary);
                    UpdateCommand = new DelegateCommand(UpdateAdditionToSalary);
                    DeleteCommand = new DelegateCommand(DeleteAdditionToSalary);
                    Set(ref _selectedItem, new AdditionToSalary());
                }
                else
                {
                    AddCommand = new AsyncCommand(AddSalaryDetail);
                    UpdateCommand = new DelegateCommand(UpdateSalaryDetail);
                    DeleteCommand = new DelegateCommand(DeleteSalaryDetail);
                    Set(ref _selectedItem, new SalaryDetail());
                }
                OnPropertyChanged(nameof(AddCommand));
                OnPropertyChanged(nameof(UpdateCommand));
                OnPropertyChanged(nameof(DeleteCommand));
            }
        }

        private ObservableCollection<SalaryDetail> _salaryDetails;
        public ObservableCollection<SalaryDetail> SalaryDetails 
        { 
            get => _salaryDetails;
            set
            {
                Set(ref _salaryDetails, value);
            }
        }

        private ObservableCollection<RankCoefficient> _rankCoefficients;
        public ObservableCollection<RankCoefficient> RankCoefficients
        {
            get => _rankCoefficients;
        }

        private SalaryDetail _selectedSalaryDetail;
        public SalaryDetail SelectedSalaryDetail 
        {
            get => _selectedSalaryDetail; 
            set 
            {
                Set(ref _selectedSalaryDetail, value);
                SelectedItem = value;
                InitializeEditableSalaryDetail();
            }
        }

        private SalaryDetail _editableSalaryDetail;
        public SalaryDetail EditableSalaryDetail
        {
            get => _editableSalaryDetail;
            set => Set(ref _editableSalaryDetail, value);
        }

        private AdditionToSalary _seletedAdditionToSalary;
        public AdditionToSalary SelectedAdditionToSalary
        {
            get => _seletedAdditionToSalary;
            set
            {
                Set(ref _seletedAdditionToSalary, value);
                SelectedItem = value;
                InitializeEditableAdditionToSalary();
            }
        }

        private AdditionToSalary _editableAdditionToSalary;
        public AdditionToSalary EditableAdditionToSalary
        {
            get => _editableAdditionToSalary;
            set
            {
                Set(ref _editableAdditionToSalary, value);
            }
        }

        private SalaryDetail _selectedAditionSalaryDetail;
        public SalaryDetail SelectedAdditionSalaryDetail
        {
            get => _selectedAditionSalaryDetail;
            set
            {
                Set(ref _selectedAditionSalaryDetail, value);
            }
        }

        private string _isGridEditing;
        public string IsFormActive
        {
            get => _isGridEditing;
            set
            {
                Set(ref _isGridEditing, value);
            }
        }

        public ICommand<RowEditStartedArgs> RowEditStartedCommand { get; }
        public ICommand<RowEditFinishedArgs> RowEditFinishedCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand<RowValidationArgs> UpdateSalaryDetailRowCommand { get; }
        public ICommand<RowValidationArgs> UpdateAdditionToSalaryRowCommand { get; }
        public ICommand ShowPrintPreviewCommand { get; }
        public ICommand OpenReportFromFileCommand { get; }

        #region constructor
        public DataGridViewModel(ISalaryDetailRepository salaryDetailRepository, IRankCoefficientRepository rankCoefficientRepository, IAdditionToSalaryRepository additionToSalaryRepository)
        {
            _salaryDetailRepository = salaryDetailRepository;
            _rankCoefficientRepository = rankCoefficientRepository;
            _additionToSalaryRepository = additionToSalaryRepository;

            LoadDataAsync().Await();

            LoadDataCommand = new AsyncCommand(LoadDataAsync);
            AddCommand = new AsyncCommand(AddSalaryDetail);
            UpdateCommand = new DelegateCommand(UpdateSalaryDetail);
            DeleteCommand = new DelegateCommand(DeleteSalaryDetail);
            UpdateSalaryDetailRowCommand = new AsyncCommand<RowValidationArgs>(args => UpdateSalaryDetailByRow(args));
            UpdateAdditionToSalaryRowCommand = new DelegateCommand<RowValidationArgs>(args => UpdateAdditionToSalaryByRow(args));
            ShowPrintPreviewCommand = new DelegateCommand(ShowPrintPreview);
            OpenReportFromFileCommand = new DelegateCommand(OpenReportFromFile);
            RowEditStartedCommand = new DelegateCommand<RowEditStartedArgs>(args => RowEditStarted(args));
            RowEditFinishedCommand = new DelegateCommand<RowEditFinishedArgs>(args => RowEditFinished(args));
            EditableSalaryDetail = new SalaryDetail();
            EditableAdditionToSalary = new AdditionToSalary();
            IsFormActive = "True";
        }
        #endregion

        private void InitializeEditableSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                EditableSalaryDetail.Performer = SelectedSalaryDetail.Performer;
                EditableSalaryDetail.RankCoefficient = SelectedSalaryDetail.RankCoefficient;
                EditableSalaryDetail.EffectiveWorkingTimeFund = SelectedSalaryDetail.EffectiveWorkingTimeFund;
                EditableSalaryDetail.HoursOfWorkPerDay = SelectedSalaryDetail.HoursOfWorkPerDay;
            }
        }

        private void InitializeEditableAdditionToSalary()
        {
            if (SelectedAdditionToSalary != null)
            {
                EditableAdditionToSalary.Standard = SelectedAdditionToSalary.Standard;
            }
        }


        private async Task LoadDataAsync()
        {
            SalaryDetails = new ObservableCollection<SalaryDetail>(await _salaryDetailRepository.GetAllAsync());

            _rankCoefficients = new ObservableCollection<RankCoefficient>(await _rankCoefficientRepository.GetAllAsync());
        }

        //Adding SalaryDetail through input fields
        private async Task AddSalaryDetail()
        {
            if (EditableSalaryDetail == null) return;
            var newDetail = new SalaryDetail()
            {
                Performer = EditableSalaryDetail.Performer,
                RankCoefficient = EditableSalaryDetail.RankCoefficient,
                HoursOfWorkPerDay = EditableSalaryDetail.HoursOfWorkPerDay,
                EffectiveWorkingTimeFund = EditableSalaryDetail.EffectiveWorkingTimeFund
            };
            newDetail.RecalculateAll();
            SalaryDetails.Add(await _salaryDetailRepository.AddAsync(newDetail));
        }

        //Editng SalaryDetail through input fields
        private void UpdateSalaryDetail()
        {
            if (SelectedSalaryDetail != null && EditableSalaryDetail != null)
            {
                var salaryDetail = _salaryDetailRepository.GetByIdAsync(SelectedSalaryDetail.Id).Await();
                salaryDetail.Performer = EditableSalaryDetail.Performer;
                salaryDetail.RankCoefficient = EditableSalaryDetail.RankCoefficient;
                salaryDetail.EffectiveWorkingTimeFund = EditableSalaryDetail.EffectiveWorkingTimeFund;
                salaryDetail.HoursOfWorkPerDay = EditableSalaryDetail.HoursOfWorkPerDay;
                salaryDetail.RecalculateAll();
                _salaryDetailRepository.Update(salaryDetail);
                SelectedSalaryDetail = salaryDetail;
            }
        }


        //Editng and adding SalaryDetail through grid
        private async Task UpdateSalaryDetailByRow(RowValidationArgs args)
        {
            if (args.Item == null) return;
            var item = args.Item as SalaryDetail;
            if(args.IsNewItem)
            {
                item.RecalculateAll();
                await _salaryDetailRepository.AddAsync(item);
                //SalaryDetails.Add(item);
            }
            else
            {
                var salaryDetail = _salaryDetailRepository.GetByIdAsync(item.Id).Await();
                salaryDetail.Performer = item.Performer;
                salaryDetail.RankCoefficient = item.RankCoefficient;
                salaryDetail.EffectiveWorkingTimeFund = item.EffectiveWorkingTimeFund;
                salaryDetail.HoursOfWorkPerDay = item.HoursOfWorkPerDay;
                salaryDetail.RecalculateAll();
                _salaryDetailRepository.Update(salaryDetail);
                SelectedSalaryDetail = salaryDetail;
            }
        }

        //Delete the selected salary detail
        private void DeleteSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                var confirmationResult = System.Windows.Forms.MessageBox.Show(
                    Resources.DeleteMessage,
                    Resources.ConfirmDeletion,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (confirmationResult == DialogResult.No)
                    return;
                var item = _salaryDetailRepository.GetByIdAsync(SelectedSalaryDetail.Id).Await();
                _salaryDetailRepository.Delete(item);
                SalaryDetails.Remove(SelectedSalaryDetail);
            }
        }

        //Edit and add AditionToSalary through grid
        private void UpdateAdditionToSalaryByRow(RowValidationArgs args)
        {
            if (args.Item == null) return;
            var item = args.Item as AdditionToSalary;
            if (args.IsNewItem)
            {
                item.SalaryDetail = SelectedAdditionSalaryDetail;
                item.CalculateAddition();
                SelectedAdditionToSalary = _additionToSalaryRepository.AddAsync(item).Await();
            }
            else
            {
                var additionToSalary = _additionToSalaryRepository.GetByIdAsync(item.Id).Await();
                additionToSalary.Standard = item.Standard;
                additionToSalary.CalculateAddition();
                _additionToSalaryRepository.Update(additionToSalary);
                SelectedAdditionToSalary = additionToSalary;
            }
        }
        //Add AdditionToSalary through input fields
        private async Task AddAdditionToSalary()
        {
            if (SelectedAdditionSalaryDetail == null) return;

            var newAddition= new AdditionToSalary()
            {
                Standard = EditableAdditionToSalary.Standard,
                SalaryDetail = SelectedAdditionSalaryDetail
            };
            newAddition.CalculateAddition();
            SelectedAdditionSalaryDetail.Additions.Add(newAddition);
            SelectedAdditionToSalary = await _additionToSalaryRepository.AddAsync(newAddition);
            
        }
        //Edit AdditionToSalary through input fields
        private void UpdateAdditionToSalary()
        {
            if (EditableAdditionToSalary != null && SelectedAdditionToSalary != null)
            {
                SelectedAdditionToSalary.Standard = EditableAdditionToSalary.Standard;
                SelectedAdditionToSalary.CalculateAddition();
                _additionToSalaryRepository.Update(SelectedAdditionToSalary);
            }
            
        }
        //Delete selected AdditionToSalary selected salary detail
        private void DeleteAdditionToSalary()
        {
            if (SelectedAdditionToSalary != null)
            {
                var confirmationResult = System.Windows.Forms.MessageBox.Show(
                    Resources.DeleteMessage,
                    Resources.ConfirmDeletion,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (confirmationResult == DialogResult.No)
                    return;
                _additionToSalaryRepository.Delete(SelectedAdditionToSalary);
                SelectedSalaryDetail = _salaryDetailRepository.GetByIdAsync(SelectedAdditionToSalary.Id).Await();
            }
        }

        //Show Dialog with Salary Report
        public void ShowPrintPreview()
        {
            XtraReport report = new SalaryReport();
            report.DataSource = SalaryDetails;

            var previewControl = new DocumentPreviewControl
            {
                DocumentSource = report
            };

            report.CreateDocument();
            var previewWindow = new Window
            {
                Title = Resources.Report,
                Content = previewControl,
                Width = 1200,
                Height = 800
            };
            previewWindow.Owner = System.Windows.Application.Current.MainWindow;
            previewWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            previewWindow.ShowDialog();
        }

        public void OpenReportFromFile()
        {
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(ObservableCollection<SalaryDetail>));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(SalaryDetail));

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Файлы макета отчетов (*.xml)|*.xml|Все файлы (*.*)|*.*",
            };
            openFileDialog.ShowDialog();
            string filePath = openFileDialog.FileName;

            var report = new XtraReport();
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    report.LoadLayout(stream);
                    report.DataSource = SalaryDetails;
                }

                var previewControl = new DocumentPreviewControl
                {
                    DocumentSource = report
                };

                report.CreateDocument();
                var previewWindow = new Window
                {
                    Title = Resources.Report,
                    Content = previewControl,
                    Width = 1200,
                    Height = 800
                };
                previewWindow.Owner = System.Windows.Application.Current.MainWindow;
                previewWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                previewWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public void RowEditFinished(RowEditFinishedArgs args)
        {
            IsFormActive = "True";
        }

        public void RowEditStarted(RowEditStartedArgs args)
        {
            IsFormActive = "False";
        }
    }
}
