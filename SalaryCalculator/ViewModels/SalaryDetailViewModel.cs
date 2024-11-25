using DevExpress.Mvvm.CodeGenerators;
using SalaryCalculator.Models;
using SalaryCalculator.Data.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System.Collections.Generic;
using DevExpress.XtraPrinting.Native.Extensions;

namespace SalaryCalculator.ViewModels
{
    public class SalaryDetailViewModel : ViewModelBase
    {
        private readonly ISalaryDetailRepository _salaryDetailRepository;
        private readonly IRankCoefficientRepository _rankCoefficientRepository;

        public ICollection<SalaryDetail> SalaryDetails 
        { 
            get => GetValue<ICollection<SalaryDetail>>(); 
            set => SetValue(value); 
        }

        public ICollection<RankCoefficient> RankCoefficients
        {
            get => GetValue<ICollection<RankCoefficient>>();
            set => SetValue(value);
        }

        public SalaryDetail SelectedSalaryDetail 
        {
            get => GetValue<SalaryDetail>(); 
            set 
            {
                SetValue(value);
                SelectedSalaryDetail.RecalculateAll();
            }
        }

        public AsyncCommand LoadDataCommand { get; }
        public DelegateCommand AddCommand { get; }
        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        public SalaryDetailViewModel(ISalaryDetailRepository salaryDetailRepository, IRankCoefficientRepository rankCoefficientRepository)
        {
            _salaryDetailRepository = salaryDetailRepository;
            _rankCoefficientRepository = rankCoefficientRepository;

            SalaryDetails = new ObservableCollection<SalaryDetail>();
            LoadDataCommand = new AsyncCommand(LoadDataAsync);
            AddCommand = new DelegateCommand(AddSalaryDetail);
            UpdateCommand = new DelegateCommand(UpdateSalaryDetail, CanExecuteCommands);
            DeleteCommand = new DelegateCommand(DeleteSalaryDetail, CanExecuteCommands);

            RankCoefficients = new ObservableCollection<RankCoefficient>(rankCoefficientRepository.GetAllAsync().Await());
            LoadDataAsync().Await();
        }

        private async Task LoadDataAsync()
        {
            SalaryDetails = new ObservableCollection<SalaryDetail>(await _salaryDetailRepository.GetAllAsync());
        }

        private void AddSalaryDetail()
        {
            var newDetail = new SalaryDetail();
            newDetail.RecalculateAll();
            SalaryDetails.Add(newDetail);
        }

        private void UpdateSalaryDetail()
        {
            if (SelectedSalaryDetail != null)
            {
                _salaryDetailRepository.Update(SelectedSalaryDetail);
                SelectedSalaryDetail.RecalculateAll();
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

        private bool CanExecuteCommands() => SelectedSalaryDetail != null;
    }
}
