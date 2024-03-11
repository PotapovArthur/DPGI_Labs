using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TicTacToeViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new TicTacToeViewModel();
            DataContext = viewModel;
        }
    }

    public class TicTacToeViewModel : ViewModelBase
    {

        private List<List<Mark>> board;
        private Mark currentPlayer;
        private bool isGameEnded;

        public ObservableCollection<ObservableCollection<Mark>> Board { get; set; }
        private string gameStatus;
        public string GameStatus
        {
            get { return gameStatus; }
            set
            {
                if (value != gameStatus)
                {
                    gameStatus = value;
                    OnPropertyChanged(nameof(GameStatus));
                }
            }
        }

        public ICommand CellClickCommand { get; set; }
        public ICommand NewGameCommand { get; set; }

        public TicTacToeViewModel()
        {
            InitializeBoard();
            CellClickCommand = new RelayCommand<string>(CellClick, CanCellClick);
            NewGameCommand = new RelayCommand<object>(NewGame);
        }

        private void InitializeBoard()
        {
            board = new List<List<Mark>>
            {
                new List<Mark> { Mark.Empty, Mark.Empty, Mark.Empty },
                new List<Mark> { Mark.Empty, Mark.Empty, Mark.Empty },
                new List<Mark> { Mark.Empty, Mark.Empty, Mark.Empty }
            };

            Board = new ObservableCollection<ObservableCollection<Mark>>();

            foreach (var row in board)
            {
                var observableRow = new ObservableCollection<Mark>(row);
                Board.Add(observableRow);
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i][j] == Mark.Empty)
                    {
                        Board[i][j] = Mark.Empty;
                    }
                }
            }

            currentPlayer = Mark.X;
            isGameEnded = false;
            GameStatus = "Game in progress...";
        }

        private void NewGame(object obj)
        {
            ClearBoard();
            currentPlayer = Mark.X;
            isGameEnded = false;
            GameStatus = "New game started!";
        }

        private void ClearBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i][j] = Mark.Empty;
                    Board[i][j] = Mark.Empty;
                }
            }
        }

        private bool CanCellClick(string parameter)
        {
            if (isGameEnded)
                return false;

            var indices = parameter.Split(',');
            var row = int.Parse(indices[0]);
            var col = int.Parse(indices[1]);

            return board[row][col] == Mark.Empty;
        }

        private void CellClick(string parameter)
        {
            var indices = parameter.Split(',');
            var row = int.Parse(indices[0]);
            var col = int.Parse(indices[1]);

            if (board[row][col] == Mark.Empty)
            {
                board[row][col] = currentPlayer;
                Board[row][col] = currentPlayer == Mark.X ? Mark.X : Mark.O;

                if (CheckForWin())
                {
                    GameStatus = $"{currentPlayer} wins!";
                    isGameEnded = true;
                }
                else if (IsBoardFull())
                {
                    GameStatus = "It's a draw!";
                    isGameEnded = true;
                }
                else
                {
                    currentPlayer = currentPlayer == Mark.X ? Mark.O : Mark.X;
                    GameStatus = $"Player {currentPlayer}'s turn";
                }
            }
        }

        private bool CheckForWin()
        {

            for (int i = 0; i < 3; i++)
            {
                if (board[i][0] != Mark.Empty && board[i][0] == board[i][1] && board[i][0] == board[i][2])
                    return true;
            }

            for (int i = 0; i < 3; i++)
            {
                if (board[0][i] != Mark.Empty && board[0][i] == board[1][i] && board[0][i] == board[2][i])
                    return true;
            }


            if (board[0][0] != Mark.Empty && board[0][0] == board[1][1] && board[0][0] == board[2][2])
                return true;

            if (board[0][2] != Mark.Empty && board[0][2] == board[1][1] && board[0][2] == board[2][0])
                return true;

            return false;
        }

        private bool IsBoardFull()
        {
            foreach (var row in board)
            {
                foreach (var cell in row)
                {
                    if (cell == Mark.Empty)
                        return false;
                }
            }
            return true;
        }
    }

    public enum Mark
    {
        Empty,
        X,
        O
    }

    public class MarkToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Mark mark && mark != Mark.Empty)
            {
                return mark.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
