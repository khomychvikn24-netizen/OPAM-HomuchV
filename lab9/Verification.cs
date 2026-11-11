static class Verification
{
    public static bool IsSorted<T>(T[] a,Func<T,T,int> cmp){for(int i=1;i<a.Length;i++)if(cmp(a[i-1],a[i])>0)return false;return true;}
    public static bool IsPermutationOf<T>(T[] a,T[] original)
    {
        if(a.Length!=original.Length)return false;Dictionary<T,int> counts=new();foreach(T x in original){counts.TryGetValue(x,out int n);counts[x]=n+1;}
        foreach(T x in a){if(!counts.TryGetValue(x,out int n)||n==0)return false;counts[x]=n-1;}return true;
    }
    public static void Check<T>(T[] a,T[] original,Func<T,T,int> cmp){if(!IsSorted(a,cmp)||!IsPermutationOf(a,original))throw new Exception("Неправильне сортування");}
}
