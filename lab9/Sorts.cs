static class Sorts
{
    // Довгі лічильники потрібні для замірів на 100000+; навчальні int-обгортки нижче.
    public static void Bubble<T>(T[] a,Func<T,T,int> cmp,out long comparisons,out long moves)
    {comparisons=moves=0;for(int end=a.Length-1;end>0;end--){bool changed=false;for(int i=0;i<end;i++){comparisons++;if(cmp(a[i],a[i+1])>0){T tmp=a[i];a[i]=a[i+1];a[i+1]=tmp;moves+=2;changed=true;}}if(!changed)break;}}
    public static void Merge<T>(T[] a,Func<T,T,int> cmp,out long comparisons,out long moves)
    {
        comparisons=moves=0;T[] buffer=new T[a.Length];
        for(int width=1;width<a.Length;width=width>a.Length/2?a.Length:width*2)
        {
            for(int lo=0;lo<a.Length;lo+=2*width)
            {
                int mid=Math.Min(lo+width,a.Length),hi=Math.Min(lo+2*width,a.Length),i=lo,j=mid,k=lo;
                while(i<mid&&j<hi){comparisons++;buffer[k++]=cmp(a[i],a[j])<=0?a[i++]:a[j++];moves++;}
                while(i<mid){buffer[k++]=a[i++];moves++;}while(j<hi){buffer[k++]=a[j++];moves++;}
                for(k=lo;k<hi;k++){a[k]=buffer[k];moves++;}
            }
        }
    }
    public static void BubbleSort<T>(T[] a,Func<T,T,int> cmp,out int comparisons,out int moves){Bubble(a,cmp,out long c,out long m);comparisons=checked((int)c);moves=checked((int)m);}
    public static void MergeSort<T>(T[] a,Func<T,T,int> cmp,out int comparisons,out int moves){Merge(a,cmp,out long c,out long m);comparisons=checked((int)c);moves=checked((int)m);}
    public static void TwoPass<T>(T[] a,Func<T,T,int> primary,Func<T,T,int> secondary,out long comparisons,out long moves){Merge(a,secondary,out long c1,out long m1);Merge(a,primary,out long c2,out long m2);comparisons=c1+c2;moves=m1+m2;}
}
