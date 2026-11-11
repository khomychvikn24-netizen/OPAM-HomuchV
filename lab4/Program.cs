using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Лабораторна робота 4. Варвара. N=29, K=3, a=2, b=1, c=4");
        List<int> source = Generate(20, 29);
        Print("Основний список", source);
        List<int> selected = PositiveOdd(source);
        Print("1. Додатні непарні", selected);
        Console.WriteLine("Count = " + selected.Count);
        double average = Average(source);
        Console.WriteLine("2. Початкове середнє = " + average);
        List<int> wrong = Copy(source), backward = Copy(source), fresh = KeepAtLeastAverage(source);
        RemoveBelowAverageWrongInPlace(wrong);
        RemoveBelowAverageInPlace(backward);
        Print("Хибний цикл", wrong); Print("Цикл з кінця", backward); Print("Новий список", fresh);
        Console.WriteLine("Збіг правильних версій: " + Equal(backward, fresh));
        Console.Write("Пропущені хибним циклом (початковий індекс: значення): ");
        PrintSkipped(source, average);
        List<int> first = GenerateSorted(13, 29), second = GenerateSorted(9, 129);
        Print("3. Перший", first); Print("Другий", second);
        List<int> merged = Merge(first, second);
        Print("Злиття", merged);
        Console.WriteLine("Упорядкований: " + IsSorted(merged));
        SplitParity(source, out List<int> even, out List<int> odd);
        Print("4. Парні", even); Print("Непарні", odd);
        Console.WriteLine("Сума Count правильна: " + (even.Count + odd.Count == source.Count));
        RunTests();
        Console.WriteLine("5. Заміри Release, мс (прогрів кожної операції перед заміром):");
        Benchmark();
    }

    static List<int> Generate(int count, int seed)
    {
        List<int> result = new List<int>(count); Random random = new Random(seed);
        for (int i = 0; i < count; i++) result.Add(random.Next(-9, 10));
        return result;
    }
    static List<int> Copy(List<int> a)
    {
        List<int> result = new List<int>(a.Count);
        for (int i = 0; i < a.Count; i++) result.Add(a[i]);
        return result;
    }
    static List<int> PositiveOdd(List<int> a)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < a.Count; i++) if (a[i] > 0 && a[i] % 2 != 0) result.Add(a[i]);
        return result;
    }
    // Для порожнього списку середнє не визначене; вилучення його не обчислює.
    static double Average(List<int> a)
    {
        if (a.Count == 0) return double.NaN;
        long sum = 0;
        for (int i = 0; i < a.Count; i++) sum += a[i];
        return (double)sum / a.Count;
    }
    // Навмисна помилка для демонстрації: після вилучення i зростає.
    static void RemoveBelowAverageWrongInPlace(List<int> a)
    {
        if (a.Count == 0) return;
        double threshold = Average(a);
        for (int i = 0; i < a.Count; i++) if (a[i] < threshold) a.RemoveAt(i);
    }
    static void RemoveBelowAverageInPlace(List<int> a)
    {
        if (a.Count == 0) return;
        double threshold = Average(a);
        for (int i = a.Count - 1; i >= 0; i--) if (a[i] < threshold) a.RemoveAt(i);
    }
    static List<int> KeepAtLeastAverage(List<int> a)
    {
        List<int> result = new List<int>();
        if (a.Count == 0) return result;
        double threshold = Average(a);
        for (int i = 0; i < a.Count; i++) if (a[i] >= threshold) result.Add(a[i]);
        return result;
    }
    static void PrintSkipped(List<int> a, double threshold)
    {
        // Повторюємо хибний обхід над індексами, щоб точно визначити походження залишків.
        List<int> indices = new List<int>(a.Count);
        for (int i = 0; i < a.Count; i++) indices.Add(i);
        for (int i = 0; i < indices.Count; i++) if (a[indices[i]] < threshold) indices.RemoveAt(i);
        for (int i = 0; i < indices.Count; i++)
            if (a[indices[i]] < threshold) Console.Write($"{indices[i]}: {a[indices[i]]}; ");
        Console.WriteLine();
    }
    static List<int> GenerateSorted(int count, int seed)
    {
        List<int> result = new List<int>(count); Random random = new Random(seed);
        int value = 0;
        for (int i = 0; i < count; i++) { value += random.Next(1, 10); result.Add(value); }
        return result;
    }
    static List<int> Merge(List<int> a, List<int> b)
    {
        List<int> result = new List<int>(a.Count + b.Count);
        int i = 0, j = 0;
        while (i < a.Count && j < b.Count)
        {
            if (a[i] <= b[j]) result.Add(a[i++]); else result.Add(b[j++]);
        }
        while (i < a.Count) result.Add(a[i++]);
        while (j < b.Count) result.Add(b[j++]);
        return result;
    }
    static bool IsSorted(List<int> a)
    {
        for (int i = 1; i < a.Count; i++) if (a[i] < a[i - 1]) return false;
        return true;
    }
    static void SplitParity(List<int> a, out List<int> even, out List<int> odd)
    {
        even = new List<int>(); odd = new List<int>();
        for (int i = 0; i < a.Count; i++)
            if (a[i] % 2 == 0) even.Add(a[i]); else odd.Add(a[i]);
    }
    static bool Equal(List<int> a, List<int> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++) if (a[i] != b[i]) return false;
        return true;
    }
    static void Print(string label, List<int> a)
    {
        Console.Write(label + ": [");
        for (int i = 0; i < a.Count; i++) { if (i > 0) Console.Write(", "); Console.Write(a[i]); }
        Console.WriteLine("]");
    }
    static void Check(bool ok, string name)
    {
        if (!ok) throw new Exception("Помилка: " + name);
        Console.WriteLine("OK: " + name);
    }
    static void RunTests()
    {
        List<int> empty = new List<int>();
        RemoveBelowAverageInPlace(empty);
        SplitParity(empty, out List<int> even, out List<int> odd);
        Check(PositiveOdd(empty).Count == 0 && KeepAtLeastAverage(empty).Count == 0 && empty.Count == 0
            && Merge(empty, empty).Count == 0 && even.Count + odd.Count == 0, "порожні списки: завдання 1–4");
        List<int> one = new List<int> { 3 };
        SplitParity(one, out even, out odd);
        RemoveBelowAverageInPlace(one);
        Check(Equal(PositiveOdd(one), one) && Equal(KeepAtLeastAverage(one), one) && one.Count == 1
            && even.Count == 0 && Equal(odd, one), "[3]: завдання 1,2,4");
        Check(PositiveOdd(new List<int> { -3, 0, 2 }).Count == 0, "відбір: жоден не підходить");
        List<int> all = new List<int> { 1, 3, 5 };
        Check(Equal(PositiveOdd(all), all), "відбір: усі підходять");
        List<int> equal = new List<int> { 2, 2, 2 };
        RemoveBelowAverageInPlace(equal);
        Check(Equal(equal, new List<int> { 2, 2, 2 }) && Equal(KeepAtLeastAverage(equal), equal), "вилучення: жоден не нижче середнього");
        Check(Equal(Merge(empty, all), all) && Equal(Merge(all, empty), all), "злиття з порожнім з обох боків");
        List<int> a = new List<int> { -5, -4, 9 }, snapshot = Copy(a), changed = Copy(a);
        RemoveBelowAverageInPlace(changed);
        Check(Equal(changed, new List<int> { 9 }) && Equal(changed, KeepAtLeastAverage(a)), "послідовні вилучення");
        PositiveOdd(a); SplitParity(a, out even, out odd); Merge(a, empty);
        Check(Equal(a, snapshot), "методи читання не змінюють оригінал");
        Check(Equal(Merge(new List<int> { 1, 3, 3 }, new List<int> { 2, 3, 4, 8 }),
            new List<int> { 1, 2, 3, 3, 3, 4, 8 }), "злиття: повтори та хвіст");
        Check(Equal(even, new List<int> { -4 }) && Equal(odd, new List<int> { -5, 9 }), "парність від'ємних");
    }
    static List<int> AddOperation(int n, bool capacity)
    {
        List<int> a = capacity ? new List<int>(n) : new List<int>();
        for (int i = 0; i < n; i++) a.Add(i);
        return a;
    }
    static List<int> InsertOperation(int n)
    {
        List<int> a = new List<int>();
        for (int i = 0; i < n; i++) a.Insert(0, i);
        return a;
    }
    static int ContainsOperation(List<int> a, int repeats)
    {
        int found = 0;
        for (int i = 0; i < repeats; i++) if (a.Contains(-1)) found++;
        return found;
    }
    static void Benchmark()
    {
        const int n = 100000;
        AddOperation(1000, false);
        Stopwatch watch = Stopwatch.StartNew();
        List<int> a = AddOperation(n, false); watch.Stop(); double add = watch.Elapsed.TotalMilliseconds;
        GC.KeepAlive(a);
        AddOperation(1000, true); watch.Restart();
        a = AddOperation(n, true); watch.Stop(); double reserved = watch.Elapsed.TotalMilliseconds;
        GC.KeepAlive(a);
        InsertOperation(1000); watch.Restart();
        a = InsertOperation(n); watch.Stop(); double insert = watch.Elapsed.TotalMilliseconds;
        GC.KeepAlive(a);
        ContainsOperation(AddOperation(1000, true), 1000);
        a = AddOperation(n, true); watch.Restart();
        int found = ContainsOperation(a, 1000); watch.Stop(); double contains = watch.Elapsed.TotalMilliseconds;
        Console.WriteLine($"Add без місткості: {add:F4}");
        Console.WriteLine($"Add із місткістю: {reserved:F4}");
        Console.WriteLine($"Insert на початок: {insert:F4}");
        Console.WriteLine($"Contains: {contains:F4}; знайдено: {found}");
        Console.WriteLine($"Insert / Add без місткості: {insert / add:F2}");
    }
}
