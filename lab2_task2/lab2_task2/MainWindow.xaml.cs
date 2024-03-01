using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace lab2_task2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommandBinding saveCommand = new CommandBinding(ApplicationCommands.Save, Execute_Save, CanExecute_Save);
            CommandBindings.Add(saveCommand);

            CommandBinding openCommand = new CommandBinding(ApplicationCommands.Open, Execute_Open, CanExecute_Open);
            CommandBindings.Add(openCommand);

            CommandBinding clearCommand = new CommandBinding(ApplicationCommands.Delete, Execute_Clear, CanExecute_Clear);
            CommandBindings.Add(clearCommand);
        }

        private void CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            if (textBox.Text.Trim().Length > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void Execute_Save(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                System.IO.File.WriteAllText("d:\\myFile.txt", textBox.Text);
                MessageBox.Show("The file was saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void CanExecute_Open(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Execute_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                if (System.IO.Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            string fileContent = reader.ReadToEnd();
                            textBox.Text = fileContent;
                        }

                        MessageBox.Show("File opened successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid text file (.txt).");
                }
            }
        }

        private void CanExecute_Clear(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(textBox.Text);
        }

        private void Execute_Clear(object sender, ExecutedRoutedEventArgs e)
        {
            textBox.Clear();
        }
    }
}