public class Tuple<T1,T2>
{
        public T1 item1;
        public T2 item2;
        public Tuple(T1 item1, T2 item2)
        {
                this.item1 = item1;
                this.item2 = item2;
        }
        public override bool Equals(object obj)
        {
                Tuple<T1, T2> tuple = (Tuple<T1, T2>) obj;
                if (tuple == null)
                        return false;
                return tuple.item1.Equals(item1)  && tuple.item2.Equals(item2);
        }
}

public class Tuple<T1,T2,T3>
{
        public T1 item1;
        public T2 item2;
        public T3 item3;
        public Tuple(T1 item1, T2 item2,T3 item3)
        {
                this.item1 = item1;
                this.item2 = item2;
                this.item3 = item3;
        }
        public override bool Equals(object obj)
        {
                Tuple<T1, T2> tuple = (Tuple<T1, T2>) obj;
                if (tuple == null)
                        return false;
                return tuple.item1.Equals(item1)  && tuple.item2.Equals(item2);
        }
}