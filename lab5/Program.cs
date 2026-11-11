using System.Globalization;
using System.Text;
class IntStack
{
    private int[] items = new int[5];
    private int count;
    public int Count => count;
    public int Capacity => items.Length;
    public void Push(int x) { if(count==items.Length){int[] next=new int[items.Length*2];for(int i=0;i<count;i++)next[i]=items[i];items=next;}items[count++]=x; }
    public int Pop(){if(count==0)throw new InvalidOperationException("Порожній стек");return items[--count];}
    public int Peek(){if(count==0)throw new InvalidOperationException("Порожній стек");return items[count-1];}
}
class Program
{
    static void Check(bool ok,string name){if(!ok)throw new Exception(name);Console.WriteLine("OK: "+name);}
    static bool Brackets(string text,bool own,out int error)
    {
        Stack<int> standard=new Stack<int>();IntStack custom=new IntStack();
        bool quoted=false;error=-1;
        for(int i=0;i<text.Length;i++)
        {
            char ch=text[i];
            if(ch=='"'){int slash=0;for(int j=i-1;j>=0&&text[j]=='\\';j--)slash++;if(slash%2==0)quoted=!quoted;continue;}
            if(quoted)continue;
            if(ch=='('||ch=='['||ch=='{'){if(own)custom.Push(i);else standard.Push(i);}
            if(ch==')'||ch==']'||ch=='}')
            {
                int count=own?custom.Count:standard.Count;
                if(count==0){error=i;return false;}
                int pos=own?custom.Pop():standard.Pop();char open=text[pos];
                if(!((open=='('&&ch==')')||(open=='['&&ch==']')||(open=='{'&&ch=='}'))){error=i;return false;}
            }
        }
        // Перша незакрита відкривальна дужка, а не позиція кінця рядка.
        while((own?custom.Count:standard.Count)>0)error=own?custom.Pop():standard.Pop();
        return error==-1;
    }
    static bool Evaluate(string text,out double result,out string error)
    {
        result=0;error="";Stack<double> stack=new Stack<double>();
        foreach(string token in text.Split((char[])null,StringSplitOptions.RemoveEmptyEntries))
        {
            if(double.TryParse(token,NumberStyles.Float,CultureInfo.InvariantCulture,out double x)&&double.IsFinite(x)){stack.Push(x);continue;}
            if(token!="+"&&token!="-"&&token!="*"&&token!="/"&&token!="%") {error="Невідомий токен: "+token;return false;}
            if(stack.Count<2){error="Недостатньо чисел";return false;}
            double right=stack.Pop(); // Count >= 2 перед першим Pop.
            if(stack.Count==0){error="Недостатньо чисел";return false;}
            double left=stack.Pop();
            if((token=="/"||token=="%")&&right==0){error="Ділення або остача на нуль";return false;}
            double value=token switch{"+"=>left+right,"-"=>left-right,"*"=>left*right,"/"=>left/right,_=>left%right};
            if(!double.IsFinite(value)){error="Числове переповнення";return false;}stack.Push(value);
        }
        if(stack.Count!=1){error=stack.Count==0?"Порожній вираз":"Забагато чисел";return false;}
        result=stack.Pop();return true;
    }
    static int Simulate(int count,bool print)
    {
        Random random=new Random(29);Queue<int> queue=new Queue<int>();int[] weights=new int[count],remaining=new int[count],start=new int[count],finish=new int[count];
        for(int i=0;i<count;i++){weights[i]=remaining[i]=random.Next(1,10);start[i]=-1;queue.Enqueue(i);}
        int maximum=queue.Count,time=0;
        while(queue.Count>0)
        {
            int id=queue.Dequeue();if(start[id]<0)start[id]=time;
            int work=remaining[id]<2?remaining[id]:2;time+=work;remaining[id]-=work;
            if(remaining[id]>0)queue.Enqueue(id);else finish[id]=time;
            if(queue.Count>maximum)maximum=queue.Count;
        }
        int total=0;
        for(int i=0;i<count;i++){total+=weights[i];if(print)Console.WriteLine($"{i+1}: вага={weights[i]}, початок={start[i]}, завершення={finish[i]}");}
        Check(total==time,"час дорівнює сумі ваг");
        if(print)Console.WriteLine($"Максимальна черга={maximum}; час={time}");return maximum;
    }
    static void Main()
    {
        Console.OutputEncoding=Encoding.UTF8;
        string[] cases={"(a[b]{c})","([)]",")(","((","","((())","abc","\"([)]\"","(\"[\")"};
        bool[] expected={true,false,false,false,true,false,true,true,true};int[] errors={-1,2,0,0,-1,0,-1,-1,-1};
        for(int i=0;i<cases.Length;i++){bool ok=Brackets(cases[i],false,out int e);bool own=Brackets(cases[i],true,out int e2);Check(ok==expected[i]&&e==errors[i]&&ok==own&&e==e2,"дужки "+i);Console.WriteLine($"{cases[i]} -> {ok}, індекс={e}");}
        string[] expressions={"3 4 + 2 *","5 3 -","8 2 /","2 3 4 * +","5 0 /","10 4 % 2 * 1 -","","42","1 ?","1 2","1 +","2 0 %"};
        double[] values={14,2,4,14,0,3,0,42,0,0,0,0};bool[] valid={true,true,true,true,false,true,false,true,false,false,false,false};
        for(int i=0;i<expressions.Length;i++){bool ok=Evaluate(expressions[i],out double r,out string error);Check(ok==valid[i]&&(!ok||r==values[i]),"ЗПЗ "+i);Console.WriteLine(expressions[i]+" -> "+(ok?r.ToString(CultureInfo.InvariantCulture):error));}
        Simulate(11,true);Check(Simulate(0,false)==0,"жодної заявки");
        IntStack s=new IntStack();for(int i=1;i<=6;i++){s.Push(i);Console.WriteLine($"Push({i}): Count={s.Count}, Capacity={s.Capacity}");}
        Check(s.Capacity==10&&s.Peek()==6&&s.Count==6,"подвоєння і Peek");
        for(int i=6;i>=1;i--){Check(s.Count>0&&s.Pop()==i,"LIFO "+i);}
        bool caught=false;try{s.Pop();}catch(InvalidOperationException){caught=true;}Check(caught,"очікуваний виняток Pop на порожньому");
        caught=false;try{s.Peek();}catch(InvalidOperationException){caught=true;}Check(caught,"очікуваний виняток Peek на порожньому");
    }
}
