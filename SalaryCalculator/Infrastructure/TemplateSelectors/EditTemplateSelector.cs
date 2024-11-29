using System.Windows.Controls;
using System.Windows;
using SalaryCalculator.ViewModels;

namespace SalaryCalculator.Infrastructure.TemplateSelectors
{
    public class EditTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (item is SalaryDetailViewModel)
                return element.FindResource("SalaryEditTemplate") as DataTemplate;
            if (item is AdditionToSalaryViewModel)
                return element.FindResource("AdditionEditTemplate") as DataTemplate;
            return base.SelectTemplate(item, container);
        }
    }

}
