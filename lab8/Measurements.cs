using System.Diagnostics;
static class Measurements
{
    static double Minimum(Action action){action();double best=double.PositiveInfinity;for(int r=0;r<3;r++){Stopwatch w=Stopwatch.StartNew();action();w.Stop();if(w.Elapsed.TotalMilliseconds<best)best=w.Elapsed.TotalMilliseconds;}return best;}
    public static void Run()
    {
        Console.WriteLine("n | Linear | Binary | ratio | n/2 | log2(n)");
        foreach(int n in new[]{1000,10000,100000,1000000})
        {
            Student[] a=Program.Generate(n);Array.Sort(a,(x,y)=>Program.Cmp(x,y));Random rnd=new(129);long linear=0,binary=0;
            for(int j=0;j<100;j++){Student target=a[rnd.Next(n)];Search.LinearSearch(a,target,Program.Cmp,out int l);Search.BinarySearch(a,target,Program.Cmp,out int b);linear+=l;binary+=b;}
            Console.WriteLine($"{n} | {linear/100.0:F2} | {binary/100.0:F2} | {(double)linear/binary:F2} | {n/2} | {Math.Log2(n):F2}");
        }
        Student[] raw=Program.Generate(100000);Random random=new(129);Student[] queries=new Student[1000000];for(int i=0;i<queries.Length;i++)queries[i]=raw[random.Next(raw.Length)];
        Console.WriteLine("q | Linear ms | Sort+Binary ms");int checksum=0;
        foreach(int q in new[]{1,5,10,20,30,50,100,1000,10000,20000,30000,40000,50000,60000,80000,100000,200000,500000,1000000})
        {
            double linear=Minimum(()=>{int value=0;for(int i=0;i<q;i++)value+=Search.LinearSearch(raw,queries[i],Program.Cmp,out _);checksum=value;});
            // Клонування однаково виключено з часу сортування: вихідний масив зберігається.
            double best=double.PositiveInfinity;
            for(int rep=-1;rep<3;rep++){Student[] a=(Student[])raw.Clone();Stopwatch w=Stopwatch.StartNew();Array.Sort(a,(x,y)=>Program.Cmp(x,y));int value=0;for(int i=0;i<q;i++)value+=Search.BinarySearch(a,queries[i],Program.Cmp,out _);w.Stop();checksum=value;if(rep>=0&&w.Elapsed.TotalMilliseconds<best)best=w.Elapsed.TotalMilliseconds;}
            Console.WriteLine($"{q} | {linear:F4} | {best:F4} | {(linear<best?"Linear":"Sort+Binary")}");
        }
        GC.KeepAlive(checksum);
    }
}


