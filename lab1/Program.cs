using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        int N = 29;
        int K = 3;

        RunTask1Automatic();
        RunTask2And3Automatic(N, K);
        RunTask4Automatic(N);
    }

    static void RunTask1Automatic()
    {
        Console.WriteLine("=== Завдання 1 ===");
        int x = 12;
        int y = 5;
        int z = 19;
        Console.WriteLine($"Введені числа: x = {x}, y = {y}, z = {z}");

        int max, mid, min;

        if (x >= y && x >= z)
        {
            max = x;
            if (y >= z) { mid = y; min = z; }
            else { mid = z; min = y; }
        }
        else if (y >= x && y >= z)
        {
            max = y;
            if (x >= z) { mid = x; min = z; }
            else { mid = z; min = x; }
        }
        else
        {
            max = z;
            if (x >= y) { mid = x; min = y; }
            else { mid = y; min = x; }
        }

        int uniqueCount;
        if (x == y && y == z) uniqueCount = 1;
        else if (x == y || x == z || y == z) uniqueCount = 2;
        else uniqueCount = 3;

        Console.WriteLine($"За спаданням: {max}, {mid}, {min}");
        Console.WriteLine($"Кількість різних значень: {uniqueCount}\n");
    }

    static void RunTask2And3Automatic(int N, int K)
    {
        Console.WriteLine("=== Завдання 2 та 3 ===");
        Random rnd = new Random(N);
        int count = 15;
        int sumAll = 0;
        int zeroCount = 0;
        int maxSelected = -1;
        bool hasSelected = false;

        Console.Write("Згенерована послідовність: ");
        for (int i = 0; i < count; i++)
        {
            int val = rnd.Next(1, 100);
            Console.Write(val + (i < count - 1 ? ", " : ""));

            sumAll += val;
            if (val == 0) zeroCount++;

            if (val % 2 != 0 && val < K)
            {
                if (!hasSelected || val > maxSelected)
                {
                    maxSelected = val;
                    hasSelected = true;
                }
            }
        }
        Console.WriteLine();

        Console.WriteLine($"Сума всіх елементів: {sumAll}");
        Console.WriteLine($"Кількість нулів: {zeroCount}");
        
        if (hasSelected)
            Console.WriteLine($"Найбільший елемент (непарний і менший за {K}): {maxSelected}\n");
        else
            Console.WriteLine($"Елементів, що задовольняють критерій, не знайдено.\n");
    }

    static void RunTask4Automatic(int N)
    {
        Console.WriteLine("=== Завдання 4 ===");
        long A = 1000000 + N;
        long B = 999999 - N;

        Console.WriteLine($"A = {A}, B = {B}");

        long gcd1 = 0;
        int iterations1 = 0;
        long minAB = Math.Min(A, B);

        for (long i = minAB; i >= 1; i--)
        {
            iterations1++;
            if (A % i == 0 && B % i == 0)
            {
                gcd1 = i;
                break;
            }
        }

        long gcd2 = 0;
        int iterations2 = 0;
        long tempA = A;
        long tempB = B;

        while (tempB != 0)
        {
            iterations2++;
            long remainder = tempA % tempB;
            tempA = tempB;
            tempB = remainder;
        }
        gcd2 = tempA;

        Console.WriteLine($"Метод перебором: НСД = {gcd1}, Ітерацій = {iterations1}");
        Console.WriteLine($"Алгоритм Евкліда: НСД = {gcd2}, Ітерацій = {iterations2}");
    }
}