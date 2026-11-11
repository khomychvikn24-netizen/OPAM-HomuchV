using System;
using System.Text;

class Program
{
    const int N = 29;
    const int K = 3;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Random random = new Random(N);
        int[] values = new int[16];
        for (int i = 0; i < values.Length; i++) values[i] = random.Next(0, 6);
        Console.WriteLine("Лабораторна робота 2. Варвара. N=29, K=3, a=2, b=1, c=4");
        Console.WriteLine("Початковий масив:");
        PrintArray(values);
        Console.WriteLine("Завдання 1:");
        PrintStatistics(values);
        Console.WriteLine("Завдання 2. Елементи, менші за 3:");
        PrintArray(Filter(values));
        Console.WriteLine("Завдання 3:");
        PrintSeries(values);
        Console.WriteLine("Завдання 4. До обміну половин:");
        PrintArray(values);
        SwapHalves(values);
        Console.WriteLine("Після:");
        PrintArray(values);
        int[,] matrix = new int[5, 4];
        // Продовжуємо ту саму послідовність генератора після 16 чисел масиву.
        for (int row = 0; row < matrix.GetLength(0); row++)
            for (int col = 0; col < matrix.GetLength(1); col++)
                matrix[row, col] = random.Next(0, 6);
        Console.WriteLine("Завдання 5:");
        PrintMatrix(matrix);
        AnalyzeMatrix(matrix, out int[] sums, out int[] maxima, out int bestRow);
        Console.WriteLine("Суми рядків:"); PrintArray(sums);
        Console.WriteLine("Максимуми стовпців:"); PrintArray(maxima);
        Console.WriteLine("Номер рядка з найбільшою сумою: " + (bestRow + 1));
        Console.WriteLine("Завдання 6. Автоматичні перевірки:");
        RunChecks();
    }

    // false означає, що статистика порожнього масиву не визначена.
    static bool Analyze(int[] a, out long sum, out double average,
        out int min, out int minIndex, out int max, out int maxIndex, out int zeros)
    {
        sum = 0; average = double.NaN; min = max = 0;
        minIndex = maxIndex = -1; zeros = 0;
        if (a.Length == 0) return false;
        min = max = a[0]; minIndex = maxIndex = 0;
        for (int i = 0; i < a.Length; i++)
        {
            sum += a[i];
            if (a[i] < min) { min = a[i]; minIndex = i; }
            if (a[i] > max) { max = a[i]; maxIndex = i; }
            if (a[i] == 0) zeros++;
        }
        average = (double)sum / a.Length;
        return true;
    }

    static int[] Filter(int[] a)
    {
        int count = 0;
        for (int i = 0; i < a.Length; i++) if (a[i] < K % 5) count++;
        int[] result = new int[count];
        int next = 0;
        for (int i = 0; i < a.Length; i++)
            if (a[i] < K % 5) result[next++] = a[i];
        return result;
    }

    static bool LongestSeries(int[] a, out int value, out int length, out int start)
    {
        value = 0; length = 0; start = -1;
        if (a.Length == 0) return false;
        value = a[0]; length = 1; start = 0;
        int currentLength = 1;
        for (int i = 1; i < a.Length; i++)
        {
            if (a[i] == a[i - 1]) currentLength++;
            else currentLength = 1;
            // Строге порівняння зберігає першу серію при однаковій довжині.
            if (currentLength > length)
            {
                length = currentLength; start = i - currentLength + 1; value = a[i];
            }
        }
        return true;
    }

    static int[] SwapHalves(int[] a)
    {
        int half = a.Length / 2;
        int secondStart = half + a.Length % 2;
        // Для непарної довжини середній елемент залишається на місці.
        for (int i = 0; i < half; i++)
        {
            int temp = a[i]; a[i] = a[secondStart + i]; a[secondStart + i] = temp;
        }
        return a;
    }

    static bool AnalyzeMatrix(int[,] matrix, out int[] sums, out int[] maxima, out int bestRow)
    {
        int rows = matrix.GetLength(0), cols = matrix.GetLength(1);
        sums = new int[rows]; maxima = new int[cols]; bestRow = -1;
        if (rows == 0 || cols == 0) return false;
        for (int col = 0; col < cols; col++) maxima[col] = matrix[0, col];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                sums[row] += matrix[row, col];
                if (matrix[row, col] > maxima[col]) maxima[col] = matrix[row, col];
            }
            if (bestRow == -1 || sums[row] > sums[bestRow]) bestRow = row;
        }
        return true;
    }

    static void PrintArray(int[] a)
    {
        Console.Write("[");
        for (int i = 0; i < a.Length; i++)
        {
            if (i > 0) Console.Write(", ");
            Console.Write(a[i]);
        }
        Console.WriteLine("]");
    }

    static void PrintStatistics(int[] a)
    {
        if (!Analyze(a, out long sum, out double average, out int min, out int minIndex,
            out int max, out int maxIndex, out int zeros))
        { Console.WriteLine("Порожній масив: статистика не визначена."); return; }
        Console.WriteLine($"Сума={sum}; середнє={average}; мінімум={min}, індекс={minIndex}; максимум={max}, індекс={maxIndex}; нулів={zeros}");
    }

    static void PrintSeries(int[] a)
    {
        if (!LongestSeries(a, out int value, out int length, out int start))
        { Console.WriteLine("Порожній масив: найдовша серія не визначена."); return; }
        Console.WriteLine($"Значення={value}; довжина={length}; індекс початку={start}");
    }

    static void PrintMatrix(int[,] matrix)
    {
        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            for (int col = 0; col < matrix.GetLength(1); col++) Console.Write($"{matrix[row, col],4}");
            Console.WriteLine();
        }
    }

    static void Check(bool condition, string name)
    {
        if (!condition) throw new Exception("Не пройдено перевірку: " + name);
        Console.WriteLine("OK: " + name);
    }

    static void RunChecks()
    {
        int[] empty = new int[0];
        Check(!Analyze(empty, out _, out _, out _, out _, out _, out _, out _), "порожній: статистика не визначена");
        Check(Filter(empty).Length == 0, "порожній: фільтр []");
        Check(!LongestSeries(empty, out _, out _, out _), "порожній: серія не визначена");
        Check(SwapHalves(empty) == empty && empty.Length == 0, "порожній: обмін []");
        int[] single = { 2 };
        Check(Analyze(single, out long sum, out double avg, out int min, out int mi, out int max, out int ma, out int z)
            && sum == 2 && avg == 2 && min == 2 && max == 2 && mi == 0 && ma == 0 && z == 0, "[2]: статистика");
        int[] filtered = Filter(single);
        Check(filtered.Length == 1 && filtered[0] == 2, "[2]: фільтр [2]");
        Check(LongestSeries(single, out int v, out int len, out int start) && v == 2 && len == 1 && start == 0, "[2]: серія (2,1,0)");
        Check(SwapHalves(single) == single && single[0] == 2, "[2]: обмін [2]");
        int[] equal = { 4, 4, 4, 4 };
        Check(Analyze(equal, out sum, out avg, out min, out mi, out max, out ma, out z)
            && sum == 16 && avg == 4 && min == 4 && max == 4 && mi == 0 && ma == 0 && z == 0, "однакові: статистика");
        Check(LongestSeries(equal, out v, out len, out start) && v == 4 && len == 4 && start == 0, "однакові: серія (4,4,0)");
        Check(LongestSeries(new int[] { 1, 2, 2, 2 }, out v, out len, out start) && v == 2 && len == 3 && start == 1, "серія в кінці");
        Check(LongestSeries(new int[] { 1, 1, 2, 2 }, out v, out len, out start) && v == 1 && len == 2 && start == 0, "перша з рівних серій");
        int[] odd = { 1, 2, 3, 4, 5 };
        SwapHalves(odd);
        Check(odd[0] == 4 && odd[1] == 5 && odd[2] == 3 && odd[3] == 1 && odd[4] == 2, "непарний: обмін половин");
        int[] even = { 1, 2, 3, 4 };
        SwapHalves(even);
        Check(even[0] == 3 && even[1] == 4 && even[2] == 1 && even[3] == 2, "парний: обмін половин");
        AnalyzeMatrix(new int[,] { { -3, -1 }, { -2, -2 } }, out int[] sums, out int[] maxima, out int best);
        Check(sums[0] == -4 && sums[1] == -4 && maxima[0] == -2 && maxima[1] == -1 && best == 0, "матриця: від'ємні значення і рівні суми");
    }
}
