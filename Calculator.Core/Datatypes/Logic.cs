
namespace Calculator.Core.Datatypes {
    public class Logic : Datatype {

        private bool value;

        public Logic(bool val) : base(Type.LOGIC) {
            this.value = val;
        }

        public override string ToString() {
            return this.value.ToString();
        }

        public static implicit operator bool(Logic num) {
            return num.value;
        }

        public static implicit operator Logic(bool val) {
            return new Logic(val);
        }


        public static Logic Parse(string value) {
            return new Logic(bool.Parse(value));
        }

        public static Logic operator ==(Logic a, Logic b) {
            return AreEqual(a, b);
        }
        public static Logic operator !=(Logic a, Logic b) {
            return !AreEqual(a, b);
        }

        public static Logic operator &(Logic a, Logic b) {
            return new Logic(a.value & b.value);
        }
        public static Logic operator |(Logic a, Logic b) {
            return new Logic(a.value | b.value);
        }
        public static Logic operator !(Logic a) {
            return new Logic(!a.value);
        }

        public static bool operator true(Logic a) {
            return a.value;
        }
        public static bool operator false(Logic a) {
            return !a.value;
        }


        public override bool Equals(object obj) {
            if (obj == null || !(obj is Logic))
                return false;
            else
                return AreEqual(this, (Logic)obj);
        }

        private static Logic AreEqual(Logic a, Logic b) {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return new Logic(false);
            else
                return new Logic(a.value == b.value);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
