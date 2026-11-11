using System.Text;
class Program
{
    static void Check(bool ok,string name){if(!ok)throw new Exception(name);Console.WriteLine("OK: "+name);}
    static void Print<T>(string name,List<T> a){Console.Write(name+" ("+a.Count+"): ");foreach(var x in a)Console.Write(x+"; ");Console.WriteLine();}
    static void PrintGroups<T,TKey>(Dictionary<TKey,List<T>> groups){foreach(var p in groups)Console.WriteLine($"{p.Key}: {p.Value.Count}");}
    static string Range(int score)=>score<60?"0–59":score<75?"60–74":score<90?"75–89":"90–100";
    static int? Maximum(List<int> src)=>Algorithms.Aggregate<int,int?>(src,null,(s,x)=>!s.HasValue||x>s.Value?x:s);
    static double? Average(List<int> src){var pair=Algorithms.Aggregate(src,(sum:0L,count:0),(s,x)=>(s.sum+x,s.count+1));return pair.count==0?null:(double)pair.sum/pair.count;}
    static void Captures()
    {
        int threshold=3;Func<int,bool> keep=x=>x>threshold;threshold=1000;Console.WriteLine("Поріг змінено: keep(4)="+keep(4));
        List<Func<int>> callbacks=new();for(int i=0;i<3;i++)callbacks.Add(()=>i);Console.Write("for: ");foreach(var f in callbacks)Console.Write(f()+" ");Console.WriteLine();
        callbacks=new();foreach(int i in new[]{0,1,2})callbacks.Add(()=>i);Console.Write("foreach: ");foreach(var f in callbacks)Console.Write(f()+" ");Console.WriteLine();
        callbacks=new();for(int i=0;i<3;i++){int copy=i;callbacks.Add(()=>copy);}Console.Write("Виправлений for: ");foreach(var f in callbacks)Console.Write(f()+" ");Console.WriteLine();
    }
    static void Main()
    {
        Console.OutputEncoding=Encoding.UTF8;Random random=new(29);List<int> numbers=new(20);for(int i=0;i<20;i++)numbers.Add(random.Next(-3,4));
        List<string> words=new(){"код","масив","алгоритм","стек","черга","словник","ввід-вивід","блок-схема","тест","дані","цикл","C#-код"};var students=Data.Students();
        Print("Числа",numbers);Print("Слова",words);
        var selected=Algorithms.Filter(students,s=>s.Score>=60&&s.Score<=89);
        Print("Filter числа",Algorithms.Filter(numbers,x=>x<=0));Print("Filter слова",Algorithms.Filter(words,s=>s.Contains('-')));Print("Filter студенти",Algorithms.Map(selected,s=>s.Name));
        Print("Map остача %5",Algorithms.Map(numbers,x=>x%5));Print("Map верхній регістр",Algorithms.Map(words,s=>s.ToUpperInvariant()));Print("Map студенти",Algorithms.Map(students,s=>$"{s.Name} ({s.Score})"));
        Console.WriteLine("Сума: "+Algorithms.Aggregate(numbers,0L,(s,x)=>s+x));Console.WriteLine("Добуток: "+Algorithms.Aggregate(numbers,1L,(s,x)=>s*x));Console.WriteLine("Максимум: "+Maximum(numbers));Console.WriteLine("Середнє: "+Average(numbers));
        Console.WriteLine("За групою");PrintGroups(Algorithms.GroupBy(students,s=>s.Group));Console.WriteLine("За першою літерою");PrintGroups(Algorithms.GroupBy(students,s=>s.Name[0]));Console.WriteLine("За балами");PrintGroups(Algorithms.GroupBy(students,s=>Range(s.Score)));Console.WriteLine("За довжиною слів");PrintGroups(Algorithms.GroupBy(words,s=>s.Length));
        Print("Композиція Filter → Map",Algorithms.Map(Algorithms.Filter(students,s=>s.Score>=60&&s.Score<=89),s=>$"{s.Name} ({s.Score})"));
        Captures();
        List<int> empty=new();Check(Algorithms.Filter(empty,x=>true).Count==0&&Algorithms.Map(empty,x=>x.ToString()).Count==0&&Algorithms.GroupBy(empty,x=>x).Count==0,"порожні Filter, Map, GroupBy");
        Check(Maximum(empty)==null&&Average(empty)==null,"порожній максимум і середнє не визначені");Check(Algorithms.Filter(new List<int>{1,2},x=>x<0).Count==0,"жоден не підходить");
        Check(Algorithms.GroupBy(new List<int>{2,4},x=>x%2).Count==1,"одна група");List<int> one=new(){-2};Check(Algorithms.Aggregate(one,0,(s,x)=>s+x)==-2&&Algorithms.Aggregate(one,1,(s,x)=>s*x)==-2&&Maximum(one)==-2&&Average(one)==-2,"один елемент: чотири агрегації");
        Check(Algorithms.Filter(numbers,x=>x<=0).Count==Algorithms.Aggregate(numbers,0,(s,x)=>s+(x<=0?1:0)),"відбір і підрахунок");
    }
}
