using SalaryCalculator.Models;
using SalaryCalculator.Data.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.XtraPrinting.Native.Extensions;
using SalaryCalculator.ViewModels.Base;
using System.Windows.Input;
using DevExpress.Mvvm.Xpf;

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
                if(_selectedSalaryDetail == null) _selectedSalaryDetail = new SalaryDetail();
                Set(ref _selectedSalaryDetail, value);
            }
        }



        public ICommand LoadDataCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand<RowValidationArgs> UpdateRowCommand { get; }

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

        private async Task LoadDataAsync()
        {
            SalaryDetails = new ObservableCollection<SalaryDetail>(await _salaryDetailRepository.GetAllAsync());
        }

        private void AddSalaryDetail()
        {
            if (SelectedSalaryDetail == null) return;
            var newDetail = new SalaryDetail()
            {
                Performer = SelectedSalaryDetail.Performer,
                RankCoefficient = SelectedSalaryDetail.RankCoefficient,
                HoursOfWorkPerDay = SelectedSalaryDetail.HoursOfWorkPerDay,
                EffectiveWorkingTimeFund = SelectedSalaryDetail.EffectiveWorkingTimeFund
            };
            newDetail.RecalculateAll();
            SelectedSalaryDetail = newDetail;
            SalaryDetails.Add(newDetail);
            _salaryDetailRepository.AddAsync(newDetail);
        }

        private void UpdateSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                SelectedSalaryDetail.RankCoefficient = _rankCoefficientRepository.GetByIdAsync(SelectedSalaryDetail.RankCoefficient.Id).Await();
                SelectedSalaryDetail.RecalculateAll();
                _salaryDetailRepository.Update(SelectedSalaryDetail);
            }
        }

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
        }


        private void DeleteSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                _salaryDetailRepository.Delete(SelectedSalaryDetail);
                SalaryDetails.Remove(SelectedSalaryDetail);
            }
        }
    }
}
