
namespace Calculator.Core.Datatypes {
    public class Number : Datatype {

        private double value;

        public Number(double val) : base(Type.NUMBER) {
            this.value = val;
        }

        public override string ToString() {
            return this.value.ToString();
        }

        public string ToString(string format) {
            return this.value.ToString(format);
        }

        public static implicit operator double(Number num) {
            return num.value;
        }
        public static implicit operator int(Number num) {
            return (int)num.value;
        }
        public static implicit operator decimal(Number num) {
            return (decimal)num.value;
        }

        public static implicit operator Number(double num) {
            return new Number(num);
        }
        public static implicit operator Number(int num) {
            return new Number(num);
        }
        public static implicit operator Number(decimal num) {
            return new Number((double)num);
        }

        public static Number Parse(string value) {
            return new Number(double.Parse(value));
        }

        public static Logic operator ==(Number a, Number b) {
            return AreEqual(a, b);
        }
        public static Logic operator !=(Number a, Number b) {
            return !AreEqual(a, b);
        }
        public static Logic operator <(Number a, Number b) {
            return new Logic(a.value < b.value);
        }
        public static Logic operator <=(Number a, Number b) {
            return new Logic(a.value <= b.value);
        }
        public static Logic operator >(Number a, Number b) {
            return new Logic(a.value > b.value);
        }
        public static Logic operator >=(Number a, Number b) {
            return new Logic(a.value >= b.value);
        }

        public static Number operator +(Number a, Number b) {
            return new Number(a.value + b.value);
        }
        public static Number operator -(Number a, Number b) {
            return new Number(a.value - b.value);
        }
        public static Number operator *(Number a, Number b) {
            return new Number(a.value * b.value);
        }
        public static Number operator /(Number a, Number b) {
            return new Number(a.value + b.value);
        }
        public static Number operator %(Number a, Number b) {
            return new Number(a.value % b.value);
        }

        public override bool Equals(object obj) {
            if (obj == null || !(obj is Number))
                return false;
            else
                return AreEqual(this, (Number)obj);
        }

        private static Logic AreEqual(Number a, Number b) {
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
