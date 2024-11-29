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
                if (value is SalaryDetailViewModel salaryDetail)
                {
                    AddCommand = new AsyncCommand(AddSalaryDetail);
                    UpdateCommand = new DelegateCommand(UpdateSalaryDetail);
                    DeleteCommand = new DelegateCommand(DeleteSalaryDetail);
                }
                else if (value is AdditionToSalaryViewModel additionToSalary)
                {
                    AddCommand = new AsyncCommand(AddAdditionToSalary);
                    UpdateCommand = new DelegateCommand(UpdateAdditionToSalary);
                    DeleteCommand = new DelegateCommand(DeleteAdditionToSalary);
                }
                else return;
                OnPropertyChanged(nameof(AddCommand));
                OnPropertyChanged(nameof(UpdateCommand));
                OnPropertyChanged(nameof(DeleteCommand));
                Set(ref _selectedItem, value);
            }
        }

        private ObservableCollection<SalaryDetailViewModel> _salaryDetails;
        public ObservableCollection<SalaryDetailViewModel> SalaryDetails 
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

        private SalaryDetailViewModel _selectedSalaryDetail;
        public SalaryDetailViewModel SelectedSalaryDetail 
        {
            get => _selectedSalaryDetail; 
            set 
            {
                Set(ref _selectedSalaryDetail, value);
                SelectedItem = value;
                InitializeEditableSalaryDetail();
            }
        }

        private SalaryDetailViewModel _editableSalaryDetail;
        public SalaryDetailViewModel EditableSalaryDetail
        {
            get => _editableSalaryDetail;
            set => Set(ref _editableSalaryDetail, value);
        }

        private AdditionToSalaryViewModel _seletedAdditionToSalary;
        public AdditionToSalaryViewModel SelectedAdditionToSalary
        {
            get => _seletedAdditionToSalary;
            set
            {
                Set(ref _seletedAdditionToSalary, value);
                SelectedItem = value;
                InitializeEditableAdditionToSalary();
            }
        }

        private AdditionToSalaryViewModel _editableAdditionToSalary;
        public AdditionToSalaryViewModel EditableAdditionToSalary
        {
            get => _editableAdditionToSalary;
            set
            {
                Set(ref _editableAdditionToSalary, value);
            }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand<RowValidationArgs> UpdateSalaryDetailRowCommand { get; }
        public ICommand<RowValidationArgs> UpdateAdditionToSalaryRowCommand { get; }

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
            //SelectedAdditionToSalary = new AdditionToSalaryViewModel();
        }
        #endregion

        private void InitializeEditableSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                EditableSalaryDetail = new SalaryDetailViewModel(SelectedSalaryDetail);
            }
        }

        private void InitializeEditableAdditionToSalary()
        {
            if (SelectedAdditionToSalary != null)
            {
                EditableAdditionToSalary = new AdditionToSalaryViewModel(SelectedAdditionToSalary);
            }
        }


        private async Task LoadDataAsync()
        {
            SalaryDetails = new ObservableCollection<SalaryDetailViewModel>();
            foreach (var salaryDetail in await _salaryDetailRepository.GetAllAsync())
            {
                SalaryDetails.Add(new SalaryDetailViewModel(salaryDetail));
            }
            _rankCoefficients = new ObservableCollection<RankCoefficient>(_rankCoefficientRepository.GetAllAsync().Await());
        }

        //Adding SalaryDetail through input fields
        private async Task AddSalaryDetail()
        {
            if (EditableSalaryDetail == null) return;
            var newDetailViewModel = new SalaryDetailViewModel()
            {
                Performer = EditableSalaryDetail.Performer,
                RankCoefficient = EditableSalaryDetail.RankCoefficient,
                HoursOfWorkPerDay = EditableSalaryDetail.HoursOfWorkPerDay,
                EffectiveWorkingTimeFund = EditableSalaryDetail.EffectiveWorkingTimeFund
            };
            var newDetail = newDetailViewModel.ToSalaryDetail();
            newDetail.RecalculateAll();
            SelectedSalaryDetail = new SalaryDetailViewModel(await _salaryDetailRepository.AddAsync(newDetail));
            SalaryDetails.Add(SelectedSalaryDetail);
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
                SelectedSalaryDetail = new SalaryDetailViewModel(salaryDetail);
            }
        }


        //Editng and adding SalaryDetail through grid
        private async Task UpdateSalaryDetailByRow(RowValidationArgs args)
        {
            if (args.Item == null) return;
            var item = args.Item as SalaryDetailViewModel;
            item.RankCoefficient = _rankCoefficientRepository.GetByIdAsync(item.RankCoefficient.Id).Await();
            if(args.IsNewItem)
            {
                var newDetail = item.ToSalaryDetail();
                newDetail.RecalculateAll();
                SelectedSalaryDetail = new SalaryDetailViewModel(await _salaryDetailRepository.AddAsync(newDetail));
                SalaryDetails.Add(SelectedSalaryDetail);
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
                SelectedSalaryDetail = new SalaryDetailViewModel(salaryDetail);
                item = SelectedSalaryDetail;
            }
        }

        //Delete the selected line.
        private void DeleteSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                var confirmationResult = MessageBox.Show(
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

        private void UpdateAdditionToSalaryByRow(RowValidationArgs args)
        {
            if (args.Item == null) return;
            var item = args.Item as AdditionToSalaryViewModel;
            var additionToSalary = _additionToSalaryRepository.GetByIdAsync(item.Id).Await();
            additionToSalary.Standard = item.Standard;
            additionToSalary.CalculateAddition();
            _additionToSalaryRepository.Update(additionToSalary);
            SelectedAdditionToSalary = new AdditionToSalaryViewModel(additionToSalary);
        }

        private async Task AddAdditionToSalary()
        {
            if (SelectedAdditionToSalary == null) return;

            var newAdditionViewModel = new AdditionToSalaryViewModel()
            {
                Standard = EditableAdditionToSalary.Standard,
                SalaryDetailId = EditableAdditionToSalary.SalaryDetailId
            };
            var newAddition = newAdditionViewModel.ToAdditionToSalary();
            newAddition.CalculateAddition();
            SelectedAdditionToSalary = new AdditionToSalaryViewModel(await _additionToSalaryRepository.AddAsync(newAddition));
            
        }

        private void UpdateAdditionToSalary()
        {
            if (EditableAdditionToSalary != null && SelectedAdditionToSalary != null)
            {
                SelectedAdditionToSalary.Standard = EditableAdditionToSalary.Standard;

                var additionToSalary = _additionToSalaryRepository.GetByIdAsync(SelectedAdditionToSalary.Id).Await();
                additionToSalary.Standard = SelectedAdditionToSalary.Standard;
                additionToSalary.CalculateAddition();
                _additionToSalaryRepository.Update(additionToSalary);
                SelectedAdditionToSalary = new AdditionToSalaryViewModel(additionToSalary);
            }
            
        }

        private void DeleteAdditionToSalary()
        {
            //if (SelectedAdditionToSalary != null)
            //{
            //    _additionToSalaryRepository.Delete(SelectedAdditionToSalary);
            //    SelectedSalaryDetail = _salaryDetailRepository.GetByIdAsync(SelectedSalaryDetail.Id).Await();
            //}
        }
    }
}
