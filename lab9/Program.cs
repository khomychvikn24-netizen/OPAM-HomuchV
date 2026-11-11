using System.Text;
// LINQ дозволено лише для явно запитаного еталонного експерименту OrderBy.
class Student {public string Name;public string Group;public int Score;public Student(string n,string g,int s){Name=n;Group=g;Score=s;}}
class Program
{
    public static int Cmp(int x,int y)=>x.CompareTo(y);
    public static int ScoreCmp(Student x,Student y)=>x.Score.CompareTo(y.Score);
    public static int GroupCmp(Student x,Student y)=>string.CompareOrdinal(x.Group,y.Group);
    public static int Combined(Student x,Student y){int c=GroupCmp(x,y);return c!=0?c:ScoreCmp(x,y);}
    public static int[] Generate(int n){Random r=new(29);int[] a=new int[n];for(int i=0;i<n;i++)a[i]=r.Next(61);return a;}
    public static Student[] MakeStudents(int[] values){Student[] s=new Student[values.Length];for(int i=0;i<s.Length;i++)s[i]=new Student("Прізвище"+i.ToString("D7"),"КН-3/"+(i%2+1),values[i]);return s;}
    static void Check(bool ok,string name){if(!ok)throw new Exception(name);Console.WriteLine("OK: "+name);}
    static void Trace()
    {
        int[] a=Generate(9);Console.WriteLine("Трасування: "+string.Join(", ",a));for(int end=a.Length-1;end>0;end--){bool changed=false;for(int i=0;i<end;i++)if(a[i]>a[i+1]){int t=a[i];a[i]=a[i+1];a[i+1]=t;changed=true;}Console.WriteLine("Прохід "+(a.Length-end)+": "+string.Join(", ",a));if(!changed)break;}
    }
    static void Counts()
    {
        Console.WriteLine("n | тип | Bubble C/M | Merge C/M | Two-pass Student C/M");
        foreach(int n in new[]{100,1000,10000})
        {
            int[] random=Generate(n),sorted=(int[])random.Clone();Sorts.Merge(sorted,Cmp,out _,out _);
            for(int kind=0;kind<4;kind++)
            {
                int[] original=(int[])(kind==0?random:sorted).Clone();
                if(kind==2)for(int i=0;i<n/2;i++){int t=original[i];original[i]=original[n-1-i];original[n-1-i]=t;}
                if(kind==3){Random r=new(29);for(int k=0;k<3;k++){int i=r.Next(n),j=r.Next(n),t=original[i];original[i]=original[j];original[j]=t;}}
                int[] a=(int[])original.Clone(),b=(int[])original.Clone();Sorts.BubbleSort(a,Cmp,out int bc,out int bm);Sorts.MergeSort(b,Cmp,out int mc,out int mm);Verification.Check(a,original,Cmp);Verification.Check(b,original,Cmp);
                // Для додаткового досліду ті самі типи входу будуються за складеним ключем.
                Student[] students=MakeStudents(random);if(kind!=0)Sorts.Merge(students,Combined,out _,out _);
                if(kind==2)for(int i=0;i<n/2;i++){var t=students[i];students[i]=students[n-1-i];students[n-1-i]=t;}
                if(kind==3){Random r=new(29);for(int k=0;k<3;k++){int i=r.Next(n),j=r.Next(n);var t=students[i];students[i]=students[j];students[j]=t;}}
                var before=(Student[])students.Clone();Sorts.TwoPass(students,GroupCmp,ScoreCmp,out long tc,out long tm);Verification.Check(students,before,Combined);
                Console.WriteLine($"{n} | {kind} | {bc}/{bm} | {mc}/{mm} | {tc}/{tm}");
            }
        }
    }
    static void Stability()
    {
        // Очікування зафіксовані до запуску: Bubble/Merge/OrderBy — стабільні; Array.Sort — гарантії немає.
        Student[] original=MakeStudents(Generate(100));Array.Sort(original,(x,y)=>string.CompareOrdinal(x.Name,y.Name));
        foreach(string name in new[]{"Bubble","Merge","Array.Sort","OrderBy"})
        {
            Student[] a=(Student[])original.Clone();
            if(name=="Bubble")Sorts.Bubble(a,ScoreCmp,out _,out _);else if(name=="Merge")Sorts.Merge(a,ScoreCmp,out _,out _);else if(name=="Array.Sort")Array.Sort(a,(x,y)=>ScoreCmp(x,y));else{var ordered=System.Linq.Enumerable.OrderBy(original,s=>s.Score);int i=0;foreach(var s in ordered)a[i++]=s;}
            Verification.Check(a,original,ScoreCmp);int bad=-1;for(int i=1;i<a.Length;i++)if(a[i].Score==a[i-1].Score&&string.CompareOrdinal(a[i-1].Name,a[i].Name)>0){bad=i;break;}
            Console.WriteLine($"Стабільність {name}: {bad==-1}; "+(bad<0?"контрприкладу немає":$"{a[bad-1].Name} перед {a[bad].Name}, бал={a[bad].Score}"));
        }
        var one=(Student[])original.Clone();var two=(Student[])original.Clone();Sorts.Merge(one,Combined,out _,out _);Sorts.TwoPass(two,GroupCmp,ScoreCmp,out _,out _);bool equal=true;for(int i=0;i<one.Length;i++)equal&=ReferenceEquals(one[i],two[i]);Check(equal,"складене порівняння і два стабільні проходи збігаються");Verification.Check(one,original,Combined);Verification.Check(two,original,Combined);
    }
    static void Edges()
    {
        int[][] cases={Array.Empty<int>(),new[]{1},new[]{1,2},new[]{2,1},new[]{3,3,3,3},new[]{2,2,2,1,1,1}};
        for(int i=0;i<cases.Length;i++){int[] a=(int[])cases[i].Clone(),b=(int[])cases[i].Clone();Sorts.Bubble(a,Cmp,out _,out _);Sorts.Merge(b,Cmp,out _,out _);Verification.Check(a,cases[i],Cmp);Verification.Check(b,cases[i],Cmp);var students=MakeStudents(cases[i]);var original=(Student[])students.Clone();Sorts.TwoPass(students,GroupCmp,ScoreCmp,out _,out _);Verification.Check(students,original,Combined);Check(true,"крайовий "+i);}
    }
    static void Main(){Console.OutputEncoding=Encoding.UTF8;Trace();Edges();Stability();Counts();Measurements.Run();}
}
