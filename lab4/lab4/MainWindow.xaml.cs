using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdoAssistant myTable = new AdoAssistant();
            list.SelectedIndex = 0;
            list.Focus();
            list.DataContext = myTable.TableLoad();
        }
        private void Create(object sender, RoutedEventArgs e)
        {
            string recordBookNumber = txtRecordBookNumber.Text;
            string fullName = txtFullName.Text;
            string group = txtGroup.Text;
            string address = txtAddress.Text;

            AdoAssistant myTable = new AdoAssistant();
            bool success = myTable.AddRecord(recordBookNumber, fullName, group, address);
            if (success)
            {
                MessageBox.Show("Запис успішно додано!");
                list.DataContext = myTable.TableLoad();
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            string recordBookNumber = txtRecordBookNumber.Text;
            string fullName = txtFullName.Text;
            string group = txtGroup.Text;
            string address = txtAddress.Text;

            AdoAssistant myTable = new AdoAssistant();
            bool success = myTable.UpdateRecord(recordBookNumber, fullName, group, address);
            if (success)
            {
                MessageBox.Show("Запис успішно оновлено!");
                list.DataContext = myTable.TableLoad();
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            string recordBookNumber = txtRecordBookNumber.Text;

            AdoAssistant myTable = new AdoAssistant();
            bool success = myTable.DeleteRecord(recordBookNumber);
            if (success)
            {
                MessageBox.Show("Запис успішно видалено!");
                list.DataContext = myTable.TableLoad();
            }
        }
    }
}
