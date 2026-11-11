using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Лабораторна робота 3. Варвара. N=29, K=3, a=2, b=1, c=4");
        if (args.Length > 0 && args[0] == "--test") { RunTests(); return; }
        Console.WriteLine("Завдання 1. Введіть шлях до папки:");
        string path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            Console.WriteLine("Папка не існує або недоступна.");
        else
        {
            int depth = FolderDepth(path);
            if (depth >= 0) Console.WriteLine("Максимальна глибина доступних папок: " + depth);
            else Console.WriteLine("Глибину визначити не вдалося: папку пропущено.");
        }
        int[] a = new int[16];
        Random random = new Random(29);
        for (int i = 0; i < a.Length; i++) a[i] = random.Next(-20, 21);
        Console.WriteLine("Завдання 2. Масив:"); PrintArray(a);
        bool recursive = Solve(a, 0, a.Length - 1, 0, out int maxDepth, false);
        Console.WriteLine("Усі елементи однакові: " + recursive);
        Console.WriteLine($"Максимальна глибина: {maxDepth} переходи, {maxDepth + 1} рівнів; log₂16 = {Math.Log2(a.Length)}.");
        Console.WriteLine("Завдання 3. Розмін 11 грн (кількість монет кожного номіналу):");
        int count = Change(11, new int[] { 1, 2, 5 }, 0, new int[3], true);
        Console.WriteLine("Кількість способів: " + count);
        Console.WriteLine("Завдання 4. Трасування [7, 7, 7, 4]:");
        Solve(new int[] { 7, 7, 7, 4 }, 0, 3, 0, out _, true);
        bool iterative = SolveIterative(a);
        Console.WriteLine($"Завдання 5. Рекурсія: {recursive}; цикл: {iterative}; збіг: {recursive == iterative}");
    }

    // -1 означає пропущену папку. Параметр enumerate дозволяє перевірити відмову доступу.
    static int FolderDepth(string path, Func<string, string[]> enumerate = null)
    {
        try
        {
            string[] children = enumerate == null ? Directory.GetDirectories(path) : enumerate(path);
            int best = 0;
            for (int i = 0; i < children.Length; i++)
            {
                try
                {
                    // Не переходимо за посиланнями: вони можуть створювати цикли.
                    if ((File.GetAttributes(children[i]) & FileAttributes.ReparsePoint) != 0)
                    { Console.WriteLine("Пропущено посилання: " + children[i]); continue; }
                    int childDepth = FolderDepth(children[i], enumerate);
                    if (childDepth >= 0 && childDepth + 1 > best) best = childDepth + 1;
                }
                catch (UnauthorizedAccessException) { Console.WriteLine("Немає доступу, пропущено: " + children[i]); }
                catch (IOException) { Console.WriteLine("Помилка читання, пропущено: " + children[i]); }
            }
            return best;
        }
        catch (UnauthorizedAccessException) { Console.WriteLine("Немає доступу, пропущено: " + path); return -1; }
        catch (IOException) { Console.WriteLine("Помилка читання, пропущено: " + path); return -1; }
    }

    // Межі включні. Для порожнього діапазону рівність усіх елементів істинна.
    static bool Solve(int[] a, int lo, int hi)
    {
        return Solve(a, lo, hi, 0, out _, false);
    }

    static bool Solve(int[] a, int lo, int hi, int depth, out int maxDepth, bool trace)
    {
        maxDepth = depth;
        string indent = trace ? new string(' ', depth * 2) : "";
        if (trace) Console.WriteLine($"{indent}enter Solve({lo}..{hi})");
        bool result;
        if (lo >= hi) result = true;
        else
        {
            int mid = lo + (hi - lo) / 2;
            // Обидві половини обчислюємо окремо, без скороченого обходу через &&.
            bool left = Solve(a, lo, mid, depth + 1, out int leftDepth, trace);
            bool right = Solve(a, mid + 1, hi, depth + 1, out int rightDepth, trace);
            maxDepth = leftDepth > rightDepth ? leftDepth : rightDepth;
            result = left && right && a[lo] == a[mid + 1];
        }
        if (trace) Console.WriteLine($"{indent}exit  Solve({lo}..{hi}) -> {result}");
        return result;
    }

    static bool SolveIterative(int[] a)
    {
        for (int i = 1; i < a.Length; i++) if (a[i] != a[0]) return false;
        return true;
    }

    // Вибір — кількість монет поточного номіналу; порядок монет не враховується.
    static int Change(int remaining, int[] coins, int index, int[] counts, bool print)
    {
        if (remaining < 0) return 0;
        if (index == coins.Length)
        {
            if (remaining != 0) return 0;
            if (print)
            {
                for (int i = 0; i < coins.Length; i++)
                {
                    if (i > 0) Console.Write("; ");
                    Console.Write($"{coins[i]} грн: {counts[i]}");
                }
                Console.WriteLine();
            }
            return 1;
        }
        int total = 0;
        for (int amount = 0; amount <= remaining / coins[index]; amount++)
        {
            counts[index] = amount; // Зробити вибір.
            total += Change(remaining - amount * coins[index], coins, index + 1, counts, print);
            counts[index] = 0; // Скасувати вибір після повернення.
        }
        return total;
    }

    static void PrintArray(int[] a)
    {
        Console.Write("[");
        for (int i = 0; i < a.Length; i++)
        { if (i > 0) Console.Write(", "); Console.Write(a[i]); }
        Console.WriteLine("]");
    }

    static void Check(bool condition, string name)
    {
        if (!condition) throw new Exception("Не пройдено: " + name);
        Console.WriteLine("OK: " + name);
    }

    static void RunTests()
    {
        string root = Path.Combine(Path.GetTempPath(), "opam-lab03-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            Check(FolderDepth(root) == 0, "порожня папка: 0");
            Directory.CreateDirectory(Path.Combine(root, "a", "b"));
            Directory.CreateDirectory(Path.Combine(root, "c"));
            Check(FolderDepth(root) == 2, "дерево папок: 2");
            Check(FolderDepth(root, DenyAccess) == -1, "відмова доступу: папку пропущено (імітація винятку)");
            Check(FolderDepth(Path.Combine(root, "missing")) == -1, "відсутня папка: пропущено");
        }
        finally
        {
            // Видаляємо тільки власне тимчасове дерево з унікальним ім'ям.
            Directory.Delete(root, true);
        }
        Check(Solve(new int[] { 5 }, 0, 0), "один елемент: True");
        Check(Solve(new int[] { 3, 3, 3, 3 }, 0, 3), "однакові: True");
        Check(!Solve(new int[] { 3, 3, 4, 4 }, 0, 3), "різні однорідні половини: False");
        Check(Solve(new int[0], 0, -1), "порожній масив: True");
        int[] a = new int[16]; Random random = new Random(29);
        for (int i = 0; i < a.Length; i++) a[i] = random.Next(-20, 21);
        bool result = Solve(a, 0, a.Length - 1, 0, out int depth, false);
        Check(!result && result == SolveIterative(a) && depth == 4, "варіант 29: False, збіг, глибина 4");
        Check(SolveIterative(new int[0]) && SolveIterative(new int[] { 5 }) && SolveIterative(new int[] { 3, 3, 3, 3 }), "цикл: крайові випадки");
        int[] coins = { 1, 2, 5 }; int[] counts = new int[3];
        Check(Change(11, coins, 0, counts, false) == 11, "розмін 11 грн: 11 способів");
        Check(counts[0] == 0 && counts[1] == 0 && counts[2] == 0, "повернення скасовує вибір");
        Check(Change(-1, coins, 0, counts, false) == 0, "від'ємна сума: 0 способів");
        Check(Change(3, new int[] { 2, 5 }, 0, new int[2], false) == 0, "неможливий розмін: 0 способів");
        Check(Change(0, coins, 0, counts, false) == 1, "нуль: один спосіб без монет");
    }

    static string[] DenyAccess(string path)
    {
        throw new UnauthorizedAccessException("Імітація відмови доступу для перевірки обробника.");
    }
}
