static class Broken
{
    public static int StrictLoop<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons){comparisons=0;int lo=0,hi=a.Length-1;while(lo<hi){int mid=lo+(hi-lo)/2;comparisons++;int c=cmp(a[mid],target);if(c==0)return mid;if(c<0)lo=mid+1;else hi=mid-1;}return -1;}
    public static int Stuck<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons,out string state){comparisons=0;int lo=0,hi=a.Length-1;while(lo<=hi&&comparisons<100){int mid=lo+(hi-lo)/2;comparisons++;int c=cmp(a[mid],target);if(c==0){state="знайдено";return mid;}if(c<0)lo=mid;else hi=mid-1;}state=$"lo={lo}, hi={hi}, кроків={comparisons}";return -1;}
    public static int UnsafeMid<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons){comparisons=0;int lo=0,hi=a.Length-1;while(lo<=hi){int mid=unchecked(lo+hi)/2;comparisons++;int c=cmp(a[mid],target);if(c==0)return mid;if(c<0)lo=mid+1;else hi=mid-1;}return -1;}
    public static int Unsorted<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons)=>Search.BinarySearch(a,target,cmp,out comparisons);
}
