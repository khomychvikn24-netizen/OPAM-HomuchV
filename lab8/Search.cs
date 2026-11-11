static class Search
{
    public static int LinearSearch<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons){comparisons=0;for(int i=0;i<a.Length;i++){comparisons++;if(cmp(a[i],target)==0)return i;}return -1;}
    public static int BinarySearch<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons)=>Window(a,target,cmp,0,a.Length-1,out comparisons);
    public static int Window<T>(T[] a,T target,Func<T,T,int> cmp,int lo,int hi,out int comparisons){comparisons=0;while(lo<=hi){int mid=lo+(hi-lo)/2;comparisons++;int c=cmp(a[mid],target);if(c==0)return mid;if(c<0)lo=mid+1;else hi=mid-1;}return -1;}
    // Умова повинна мати вигляд false...false,true...true.
    public static int Bound<T>(T[] a,Func<T,bool> condition,out int comparisons){comparisons=0;int lo=0,hi=a.Length;while(lo<hi){int mid=lo+(hi-lo)/2;comparisons++;if(condition(a[mid]))hi=mid;else lo=mid+1;}return lo;}
    // Для інтерполяції cmp має повертати різницю числових ключів (не лише знак).
    public static int InterpolationSearch<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons)
    {
        comparisons=0;if(a.Length==0)return -1;long low=0,high=cmp(a[a.Length-1],a[0]),wanted=cmp(target,a[0]);comparisons+=2;
        long offset=high==low?0:(wanted-low)*(a.Length-1)/(high-low);if(offset<0)offset=0;if(offset>=a.Length)offset=a.Length-1;int mid=(int)offset;
        comparisons++;int c=cmp(a[mid],target);if(c==0)return mid;
        int result=Window(a,target,cmp,c<0?mid+1:0,c<0?a.Length-1:mid-1,out int rest);comparisons+=rest;return result;
    }
}

