using System.Windows.Controls;
using System.Windows;
using SalaryCalculator.Models;

namespace SalaryCalculator.Infrastructure.TemplateSelectors
{
    public class EditTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (item is SalaryDetail)
                return element.FindResource("SalaryEditTemplate") as DataTemplate;
            if (item is AdditionToSalary)
                return element.FindResource("AdditionEditTemplate") as DataTemplate;
            return base.SelectTemplate(item, container);
        }
    }

}
