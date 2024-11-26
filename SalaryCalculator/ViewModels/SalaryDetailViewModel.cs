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

namespace SalaryCalculator.ViewModels
{
    public class SalaryDetailViewModel : ViewModel
    {
        private readonly ISalaryDetailRepository _salaryDetailRepository;
        private readonly IRankCoefficientRepository _rankCoefficientRepository;


        private ObservableCollection<SalaryDetail> _salaryDetails;
        public ObservableCollection<SalaryDetail> SalaryDetails 
        { 
            get => _salaryDetails; 
            set => Set(ref _salaryDetails, value); 
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
                InitializeEditableSalaryDetail();
            }
        }

        private SalaryDetail _editableSalaryDetail;
        public SalaryDetail EditableSalaryDetail
        {
            get => _editableSalaryDetail;
            set => Set(ref _editableSalaryDetail, value);
        }


        public ICommand LoadDataCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand<RowValidationArgs> UpdateRowCommand { get; }
        public ICommand<InvalidRowExceptionArgs> InvalidRowCommand { get; }

        #region constructor
        public SalaryDetailViewModel(ISalaryDetailRepository salaryDetailRepository, IRankCoefficientRepository rankCoefficientRepository)
        {
            _salaryDetailRepository = salaryDetailRepository;
            _rankCoefficientRepository = rankCoefficientRepository;

            SalaryDetails = new ObservableCollection<SalaryDetail>();
            LoadDataCommand = new AsyncCommand(LoadDataAsync);
            AddCommand = new DelegateCommand(AddSalaryDetail);
            UpdateCommand = new DelegateCommand(UpdateSalaryDetail);
            DeleteCommand = new DelegateCommand(DeleteSalaryDetail);
            UpdateRowCommand = new DelegateCommand<RowValidationArgs>(args => UpdateSalaryDetailByRow(args));

            _rankCoefficients = new ObservableCollection<RankCoefficient>(_rankCoefficientRepository.GetAllAsync().Await());
            LoadDataAsync().Await();
        }
        #endregion
        //Loading data from DB
        private async Task LoadDataAsync()
        {
            SalaryDetails = new ObservableCollection<SalaryDetail>(await _salaryDetailRepository.GetAllAsync());
        }

        //Adding SalaryDetail through input fields
        private void AddSalaryDetail()
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
            SelectedSalaryDetail = newDetail;
            _salaryDetailRepository.AddAsync(newDetail);
            LoadDataAsync().Await();
        }

        //Editng SalaryDetail through input fields
        private void UpdateSalaryDetail()
        {
            if (SelectedSalaryDetail != null && EditableSalaryDetail != null)
            {
                SelectedSalaryDetail.Performer = EditableSalaryDetail.Performer;
                SelectedSalaryDetail.RankCoefficient = EditableSalaryDetail.RankCoefficient;
                SelectedSalaryDetail.EffectiveWorkingTimeFund = EditableSalaryDetail.EffectiveWorkingTimeFund;
                SelectedSalaryDetail.HoursOfWorkPerDay = EditableSalaryDetail.HoursOfWorkPerDay;

                SelectedSalaryDetail.RecalculateAll();
                _salaryDetailRepository.Update(SelectedSalaryDetail);
                LoadDataAsync().Await();
            }
        }

        private void InitializeEditableSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                EditableSalaryDetail = new SalaryDetail
                {
                    Performer = SelectedSalaryDetail.Performer,
                    RankCoefficient = SelectedSalaryDetail.RankCoefficient,
                    MonthlyBaseRate = SelectedSalaryDetail.MonthlyBaseRate,
                    HourBaseRate = SelectedSalaryDetail.HourBaseRate,
                    HoursOfWorkPerDay = SelectedSalaryDetail.HoursOfWorkPerDay,
                    EffectiveWorkingTimeFund = SelectedSalaryDetail.EffectiveWorkingTimeFund,
                    PremiumCoefficient = SelectedSalaryDetail.PremiumCoefficient,
                    Salary = SelectedSalaryDetail.Salary
                };
            }
        }

        //Editng and adding SalaryDetail through grid
        private void UpdateSalaryDetailByRow(RowValidationArgs args)
        {
            if (args.Item == null) return;
            var item = args.Item as SalaryDetail;
            item.RankCoefficient = _rankCoefficientRepository.GetByIdAsync(item.RankCoefficient.Id).Await();
            item.RecalculateAll();
            SelectedSalaryDetail = item;
            if(args.IsNewItem)
            {
                _salaryDetailRepository.AddAsync(item);
            }
            else
            {
                _salaryDetailRepository.Update(item);
            }
            LoadDataAsync().Await();
        }

        //Delete the selected line.
        private void DeleteSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                var confirmationResult = MessageBox.Show(
                    "Вы действительно хотите удалить эту запись?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (confirmationResult == DialogResult.No)
                    return;
                _salaryDetailRepository.Delete(SelectedSalaryDetail);
                SalaryDetails.Remove(SelectedSalaryDetail);
                LoadDataAsync().Await();
            }
        }
    }
}
