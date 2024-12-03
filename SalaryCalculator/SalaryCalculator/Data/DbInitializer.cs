using SalaryCalculator.Models;
using System.Collections.Generic;
using System.Linq;

namespace SalaryCalculator.Data
{
    public static class DbInitializer
    {
        //Initialize Db if both tables are empty
        public static void Initialize(DatabaseContext context)
        {
            try
            {
                context.Database.EnsureCreated();
                if (!context.RankCoefficients.Any() && !context.SalaryDetails.Any() && !context.AdditionToSalaries.Any())
                {
                    var rankCoefficients = new List<RankCoefficient>()
                    {
                        new RankCoefficient { Rank = 1, Coefficient = 1.00M },
                        new RankCoefficient { Rank = 2, Coefficient = 1.07M },
                        new RankCoefficient { Rank = 3, Coefficient = 1.14M },
                        new RankCoefficient { Rank = 4, Coefficient = 1.21M },
                        new RankCoefficient { Rank = 5, Coefficient = 1.29M },
                        new RankCoefficient { Rank = 6, Coefficient = 1.38M },
                        new RankCoefficient { Rank = 7, Coefficient = 1.47M },
                        new RankCoefficient { Rank = 8, Coefficient = 1.57M },
                        new RankCoefficient { Rank = 9, Coefficient = 1.68M },
                        new RankCoefficient { Rank = 10, Coefficient = 1.79M },
                        new RankCoefficient { Rank = 11, Coefficient = 1.91M },
                        new RankCoefficient { Rank = 12, Coefficient = 2.03M },
                        new RankCoefficient { Rank = 13, Coefficient = 2.17M },
                        new RankCoefficient { Rank = 14, Coefficient = 2.31M },
                        new RankCoefficient { Rank = 15, Coefficient = 2.47M },
                        new RankCoefficient { Rank = 16, Coefficient = 2.63M },
                        new RankCoefficient { Rank = 17, Coefficient = 2.81M },
                        new RankCoefficient { Rank = 18, Coefficient = 3.00M },
                    };

                    var salaryDetails = new List<SalaryDetail>()
                    {
                        new SalaryDetail
                        {
                            Performer = "Программист",
                            RankCoefficient = rankCoefficients.FirstOrDefault(rc => rc.Rank == 10),
                            MonthlyBaseRate = 452.87M,
                            HourBaseRate = 2.69M,
                            HoursOfWorkPerDay = 8,
                            EffectiveWorkingTimeFund = 27,
                            Salary = 697.24M
                        },
                        new SalaryDetail
                        {
                            Performer = "Тестировщик",
                            RankCoefficient = rankCoefficients.FirstOrDefault(rc => rc.Rank == 11),
                            MonthlyBaseRate = 488.23M,
                            HourBaseRate = 2.87M,
                            HoursOfWorkPerDay = 8,
                            EffectiveWorkingTimeFund = 13,
                            Salary = 358.17M
                        },
                        new SalaryDetail
                        {
                            Performer = "Дизайнер",
                            RankCoefficient = rankCoefficients.FirstOrDefault(rc => rc.Rank == 9),
                            MonthlyBaseRate = 425.87M,
                            HourBaseRate = 2.53M,
                            HoursOfWorkPerDay = 8,
                            EffectiveWorkingTimeFund = 5,
                            Salary = 121.44M
                        },
                    };

                    var additionToSalaries = new List<AdditionToSalary>()
                    {
                        new AdditionToSalary
                        {
                            SalaryDetail = salaryDetails.FirstOrDefault(sd => sd.Performer == "Программист"),
                            Standard = 20
                        },
                        new AdditionToSalary
                        {
                            SalaryDetail = salaryDetails.FirstOrDefault(sd => sd.Performer == "Программист"),
                            Standard = 10
                        },
                        new AdditionToSalary
                        {
                            SalaryDetail = salaryDetails.FirstOrDefault(sd => sd.Performer == "Тестировщик"),
                            Standard = 30
                        }
                    };
                    foreach (var item in additionToSalaries)
                    {
                        item.CalculateAddition();
                    }

                    context.RankCoefficients.AddRange(rankCoefficients);

                    context.SalaryDetails.AddRange(salaryDetails);

                    context.AdditionToSalaries.AddRange(additionToSalaries);

                }
                context.SaveChanges();
            }
            catch { }
            //TODO: Add error message displaying
        }
    }
}