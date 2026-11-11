static class Algorithms
{
    public static List<T> Filter<T>(List<T> src,Func<T,bool> keep){List<T> r=new();foreach(T x in src)if(keep(x))r.Add(x);return r;}
    public static List<TOut> Map<TIn,TOut>(List<TIn> src,Func<TIn,TOut> convert){List<TOut> r=new(src.Count);foreach(TIn x in src)r.Add(convert(x));return r;}
    public static TAcc Aggregate<T,TAcc>(List<T> src,TAcc start,Func<TAcc,T,TAcc> step){TAcc state=start;foreach(T x in src)state=step(state,x);return state;}
    public static Dictionary<TKey,List<T>> GroupBy<T,TKey>(List<T> src,Func<T,TKey> keyOf){Dictionary<TKey,List<T>> r=new();foreach(T x in src){TKey key=keyOf(x);if(!r.TryGetValue(key,out List<T> group)){group=new();r[key]=group;}group.Add(x);}return r;}
}
