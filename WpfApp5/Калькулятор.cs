namespace WpfApp5;

public static class Калькулятор
{
    public static long Факториал(int число)
    {
        if (число == 0) return 1;
        Task.Delay(1000).Wait();
        return число * Факториал(число - 1);
    }
    public static async Task<long> ФакториалAsync(int number, IProgress<int>? progress = null)
    {
        return await Task.Run(async () =>
        {
            long factorial = 1;

            for (int i = number; i > 1; i--)
            {
                progress?.Report(number - i);
                factorial *= i;
                await Task.Delay(1000);
            }
            
            return factorial;
        });
    }

    public static async Task<long> СуммаAsync(int число, IProgress<int>? progress = null)
    {
        return await Task.Run(async () =>
        {
            long sum = 0;
            for (int i = число; i > 0; i--)
            {
                progress?.Report(число - i);
                sum += i;
                await Task.Delay(10);
            }

            return sum;
        });
    }
}