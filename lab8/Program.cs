using System.Text;
class Student {public string Name;public string Group;public int Score;public Student(int s,string n=""){Score=s;Name=n;Group="КН-3/1";}}
class Program
{
    public static int Cmp(Student a,Student b)=>a.Score-b.Score;
    public static Student[] Generate(int n){Random random=new(29);Student[] a=new Student[n];for(int i=0;i<n;i++)a[i]=new Student(random.Next(101),"Студент"+i);return a;}
    static void Check(bool ok,string name){if(!ok)throw new Exception(name);Console.WriteLine("OK: "+name);}
    static void EdgeTests()
    {
        int[][] inputs={Array.Empty<int>(),new[]{3},new[]{1,3},new[]{2,4},new[]{2,4},new[]{3,3,3}};int[] targets={3,3,3,1,5,3};
        Func<int,int,int> cmp=(x,y)=>x.CompareTo(y);
        for(int i=0;i<inputs.Length;i++){var a=inputs[i];int x=targets[i];int b=Search.BinarySearch(a,x,cmp,out _),l=Search.Bound(a,v=>v>=x,out _),u=Search.Bound(a,v=>v>x,out _);int expected=Search.LinearSearch(a,x,cmp,out _);Check((expected==-1?b==-1:b>=0&&a[b]==x),"Binary крайовий "+i);int el=0,eu=0;while(el<a.Length&&a[el]<x)el++;while(eu<a.Length&&a[eu]<=x)eu++;Check(l==el&&u==eu,"межі "+i);}
    }
    static void Main()
    {
        Console.OutputEncoding=Encoding.UTF8;Student[] raw=Generate(100000),a=(Student[])raw.Clone();Array.Sort(a,(x,y)=>Cmp(x,y));
        Console.WriteLine("Запит | Linear index/count | Binary index/count | Interpolation index/count");
        foreach(Student target in new[]{new Student(53),a[0],new Student(101)}){int l=Search.LinearSearch(a,target,Cmp,out int lc),b=Search.BinarySearch(a,target,Cmp,out int bc),p=Search.InterpolationSearch(a,target,Cmp,out int pc);Console.WriteLine($"{target.Score} | {l}/{lc} | {b}/{bc} | {p}/{pc}");Check((l<0)==(b<0)&&(b<0)==(p<0),"три пошуки");}
        int lower=Search.Bound(a,s=>s.Score>=53,out int lcount),upper=Search.Bound(a,s=>s.Score>53,out int ucount);int last=-1,count=0;for(int i=0;i<a.Length;i++)if(Cmp(a[i],new Student(53))==0){last=i;count++;}
        Console.WriteLine($"Межі [{lower};{upper}), останній={upper-1}, кількість={upper-lower}; порівняння={lcount}+{ucount}, лінійно={a.Length}; лінійний результат={last}/{count}");Check(last==upper-1&&count==upper-lower,"межі та лінійна відповідь");
        int invalid=Search.Bound(a,s=>s.Score%7==0,out int invalidCount),actual=-1;for(int i=0;i<a.Length;i++)if(a[i].Score%7==0){actual=i;break;}Console.WriteLine($"Немонотонна умова: отримано={invalid}, правильно={actual}, збіг={invalid==actual}, порівнянь={invalidCount}");
        Func<int,int,int> cmp=(x,y)=>x.CompareTo(y);Console.WriteLine("Поломка 1: [53], пошук 53 -> "+Broken.StrictLoop(new[]{53},53,cmp,out _));
        Broken.Stuck(new[]{1,2},2,cmp,out _,out string state);Console.WriteLine("Поломка 2: [1,2], пошук 2 -> "+state);
        int unsafeResult=Broken.UnsafeMid(a,new Student(53),Cmp,out int unsafeCount);Console.WriteLine($"Поломка 3: на 100000 індекс={unsafeResult}, порівнянь={unsafeCount}; unchecked(1073741824+1073741824)/2={unchecked(1073741824+1073741824)/2}");
        int missed=0;Random rnd=new(129);for(int i=0;i<100;i++)if(Broken.Unsorted(raw,raw[rnd.Next(raw.Length)],Cmp,out _)<0)missed++;Console.WriteLine("Поломка 4: не знайдено наявних "+missed+" / 100");
        EdgeTests();Measurements.Run();
    }
}

