using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfApp5;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    private long? _sum;
    public long? Sum
    {
        get => _sum;
        set => SetField(ref _sum, value);
    }

    private long? _factorial;
    public long? Factorial
    {
        get => _factorial;
        set => SetField(ref _factorial, value);
    }
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void ButtonCalculate_OnClick(object sender, RoutedEventArgs e)
    {
        ButtonCalculate.IsEnabled = false;
        
        var number = Convert.ToInt32(ВводЧисла.Text);

        var progressSum = new Progress<int>();
        progressSum.ProgressChanged += (s, i) =>
        {
            ProgressSum.Value = i;
        };
        
        var progressFactorial = new Progress<int>();
        progressFactorial.ProgressChanged += (s, i) =>
        {
            ProgressFactorial.Value = i;
        };

        _cancellationTokenSource.CancelAfter(2000);
        try
        {
            await Task.Run(async () =>
            {
                Sum = await Калькулятор.СуммаAsync(number, _cancellationTokenSource.Token, progressSum);
            }, _cancellationTokenSource.Token);
        
            await Task.Run(async () =>
            {
                Factorial = await Калькулятор.ФакториалAsync(number, _cancellationTokenSource.Token, progressFactorial);
            }, _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    
    private async void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
    {
        await _cancellationTokenSource.CancelAsync();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}