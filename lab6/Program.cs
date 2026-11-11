using System.Diagnostics;
using System.Text;
class Student { public string Name; public string Group; public int Score; public Student(string n,string g,int s){Name=n;Group=g;Score=s;} }
class Program
{
    const string Text="Алгоритм описує послідовність дій. Алгоритм можна записати словами, а можна записати кодом. Кодом алгоритм стає тоді, коли його виконує машина, а не людина. Одна задача — багато алгоритмів.";
    static void Check(bool ok,string name){if(!ok)throw new Exception(name);Console.WriteLine("OK: "+name);}
    static List<int> Lengths(string text){List<int> result=new();int length=0;foreach(char c in text){if(char.IsLetter(c))length++;else if(length>0){result.Add(length);length=0;}}if(length>0)result.Add(length);return result;}
    static Dictionary<T,int> Frequencies<T>(List<T> a){Dictionary<T,int> d=new();foreach(T x in a){d.TryGetValue(x,out int count);d[x]=count+1;}return d;}
    static void FrequenciesList<T>(List<T> a,out List<T> keys,out List<int> counts){keys=new();counts=new();foreach(T x in a){int j=0;while(j<keys.Count&&!EqualityComparer<T>.Default.Equals(keys[j],x))j++;if(j==keys.Count){keys.Add(x);counts.Add(1);}else counts[j]++;}}
    static List<Student> Students()=>new(){new("Коваленко","КН-3/1",88),new("Мельник","КН-3/2",64),new("Іщенко","КН-3/1",93),new("Бондар","КН-3/1",71),new("Савченко","КН-3/2",55),new("Ткачук","КН-3/2",90),new("Кравець","КН-3/1",76),new("Іванів","КН-3/1",48),new("Мороз","КН-3/2",82),new("Шевчук","КН-3/1",67),new("Бойко","КН-3/2",95),new("Кушнір","КН-3/2",73)};
    static Dictionary<string,List<Student>> Group(List<Student> a){Dictionary<string,List<Student>> d=new();foreach(Student s in a){if(!d.TryGetValue(s.Group,out List<Student> group)){group=new();d[s.Group]=group;}group.Add(s);}return d;}
    static void GroupList(List<Student> a,out List<string> keys,out List<List<Student>> groups){keys=new();groups=new();foreach(Student s in a){int j=0;while(j<keys.Count&&keys[j]!=s.Group)j++;if(j==keys.Count){keys.Add(s.Group);groups.Add(new());}groups[j].Add(s);}}
    static bool Has<T>(List<T> a,T x){for(int i=0;i<a.Count;i++)if(EqualityComparer<T>.Default.Equals(a[i],x))return true;return false;}
    static List<int> UniqueList(List<int> a){List<int> r=new();foreach(int x in a)if(!Has(r,x))r.Add(x);return r;}
    static List<int> UniqueOrdered(List<int> a){HashSet<int> seen=new();List<int> r=new();foreach(int x in a)if(seen.Add(x))r.Add(x);return r;}
    static HashSet<int> Union(List<int> a,List<int> b){HashSet<int> r=new(a);foreach(int x in b)r.Add(x);return r;}
    static List<int> UnionList(List<int> a,List<int> b){List<int> r=UniqueList(a);foreach(int x in b)if(!Has(r,x))r.Add(x);return r;}
    static HashSet<int> Intersection(List<int> a,List<int> b){HashSet<int> other=new(b),r=new();foreach(int x in a)if(other.Contains(x))r.Add(x);return r;}
    static List<int> IntersectionList(List<int> a,List<int> b){List<int> r=new();foreach(int x in a)if(Has(b,x)&&!Has(r,x))r.Add(x);return r;}
    static bool Same(HashSet<int> set,List<int> list){if(set.Count!=list.Count)return false;foreach(int x in list)if(!set.Contains(x))return false;return true;}
    static List<int> Generate(int n,int seed,int upper){Random random=new(seed);List<int> r=new(n);for(int i=0;i<n;i++)r.Add(random.Next(1,upper+1));return r;}
    static void Print<T>(string label,IEnumerable<T> a){Console.Write(label+": ");foreach(T x in a)Console.Write(x+" ");Console.WriteLine();}
    static double Measure(Action action){action();Stopwatch w=Stopwatch.StartNew();action();w.Stop();return w.Elapsed.TotalMilliseconds;}
    static void Pair(string name,Action list,Action hash){double l=Measure(list),h=Measure(hash);Console.WriteLine($"{name}: {l:F4} | {h:F4} | {l/h:F2}");}
    static void Benchmarks()
    {
        List<int> big=new(100000);for(int i=0;i<100000;i++)big.Add(i);HashSet<int> set=new(big);int found=0;
        Pair("10000 перевірок / 100000",()=>{for(int i=0;i<10000;i++)if(Has(big,100000+i))found++;},()=>{for(int i=0;i<10000;i++)if(set.Contains(100000+i))found++;});
        List<string> words=new(50000);for(int i=0;i<50000;i++)words.Add("слово"+(i%5000));
        Pair("50000 слів / 5000 різних",()=>FrequenciesList(words,out _,out _),()=>GC.KeepAlive(Frequencies(words)));
        List<int> dup=Generate(20000,29,20000);
        Pair("20000 чисел / дублікати",()=>GC.KeepAlive(UniqueList(dup)),()=>GC.KeepAlive(UniqueOrdered(dup)));
        List<int> a=Generate(5000,29,10000),b=Generate(5000,129,10000);
        Pair("перетин 5000 + 5000",()=>GC.KeepAlive(IntersectionList(a,b)),()=>GC.KeepAlive(Intersection(a,b)));
        GC.KeepAlive(found);
    }
    static void Main()
    {
        Console.OutputEncoding=Encoding.UTF8;
        List<int> lengths=Lengths(Text);Dictionary<int,int> f=Frequencies(lengths);FrequenciesList(lengths,out List<int> keys,out List<int> counts);
        foreach(var pair in f)if(pair.Value>=2)Console.WriteLine($"Довжина {pair.Key}: {pair.Value}");Console.WriteLine("Різних довжин: "+f.Count);
        bool equal=f.Count==keys.Count;for(int i=0;i<keys.Count;i++)equal=equal&&f.TryGetValue(keys[i],out int n)&&n==counts[i];Check(equal,"частоти двома способами");
        var students=Students();var groups=Group(students);GroupList(students,out List<string> gkeys,out List<List<Student>> glists);
        foreach(var pair in groups){int sum=0;foreach(var s in pair.Value)sum+=s.Score;Console.WriteLine($"{pair.Key}: {pair.Value.Count}, середній={(double)sum/pair.Value.Count:F4}");}
        equal=groups.Count==gkeys.Count;for(int i=0;i<gkeys.Count;i++){equal=equal&&groups.TryGetValue(gkeys[i],out var group)&&group.Count==glists[i].Count;if(groups.TryGetValue(gkeys[i],out var actual))for(int j=0;j<actual.Count;j++)equal=equal&&actual[j]==glists[i][j];}Check(equal,"групування двома способами");
        List<int> data=Generate(25,29,9);Print("Початкові",data);Print("HashSet (порядок не гарантований)",new HashSet<int>(data));Print("Збережений порядок",UniqueOrdered(data));Check(Same(new HashSet<int>(data),UniqueList(data)),"дублікати");
        List<int> a=Generate(30,29,23),b=Generate(30,129,23);Print("A",a);Print("B",b);var union=Union(a,b);Print("Об'єднання",union);Check(Same(union,UnionList(a,b)),"об'єднання двома способами");Console.WriteLine($"Сирі Count: {a.Count+b.Count}; унікальні A+B: {new HashSet<int>(a).Count+new HashSet<int>(b).Count}; об'єднання: {union.Count}");
        Check(Frequencies(Lengths("")).Count==0,"порожній текст");
        var distinctWords=Frequencies(Lengths("кіт дім сад"));Check(distinctWords.TryGetValue(3,out int freq)&&freq==3,"різні слова однієї довжини");
        var oneGroup=new List<Student>{new("А","X",60),new("Б","X",80)};Check(Group(oneGroup).Count==1,"одна група");
        Check(Same(new HashSet<int>{1,2,3},UniqueOrdered(new(){1,2,3})),"без дублікатів");
        Check(Same(Union(new(){1,2},new(){3,4}),new(){1,2,3,4}),"неперетинні набори");Check(Same(Union(new(){1,2},new(){1,2}),new(){1,2}),"однакові набори");
        Benchmarks();
    }
}
