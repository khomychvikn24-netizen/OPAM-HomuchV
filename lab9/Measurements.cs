using System.Diagnostics;
static class Measurements
{
    public static double Time<T>(T[] source,Action<T[]> sort,Func<T,T,int> cmp)
    {
        var warm=(T[])source.Clone();sort(warm);Verification.Check(warm,source,cmp);
        double best=double.PositiveInfinity;
        for(int r=0;r<3;r++){var a=(T[])source.Clone();Stopwatch w=Stopwatch.StartNew();sort(a);w.Stop();Verification.Check(a,source,cmp);if(w.Elapsed.TotalMilliseconds<best)best=w.Elapsed.TotalMilliseconds;}
        return best;
    }
    public static void Run()
    {
        Console.WriteLine("n | Bubble ms | Merge ms | Two-pass Student ms | Array.Sort ms");
        foreach(int n in new[]{1000,10000,100000,1000000})
        {
            var source=Program.Generate(n);var students=Program.MakeStudents(source);
            double bubble=n==1000000?double.NaN:Time(source,a=>Sorts.Bubble(a,Program.Cmp,out _,out _),Program.Cmp);
            double merge=Time(source,a=>Sorts.Merge(a,Program.Cmp,out _,out _),Program.Cmp);
            double two=Time(students,a=>Sorts.TwoPass(a,Program.GroupCmp,Program.ScoreCmp,out _,out _),Program.Combined);
            double standard=Time(source,a=>Array.Sort(a,(x,y)=>Program.Cmp(x,y)),Program.Cmp);
            Console.WriteLine($"{n} | {bubble:F4} | {merge:F4} | {two:F4} | {standard:F4}");
        }
        Console.WriteLine("Межа Bubble (сітка розмірів, мінімум трьох замірів):");
        foreach(int n in new[]{1000,2000,5000,10000,20000,30000,50000,100000})
        {
            double time=Time(Program.Generate(n),a=>Sorts.Bubble(a,Program.Cmp,out _,out _),Program.Cmp);Console.WriteLine($"{n}: {time:F4} мс");if(time>1000)break;
        }
    }
}
