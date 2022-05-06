using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Datatypes {
    public class Text : Datatype {

        private string value;

        public Text(string val) : base(Type.TEXT) {
            this.value = val;
        }

        public static implicit operator string(Text txt) {
            return txt.value;
        }

        public static implicit operator Text(string txt) {
            return new Text(txt);
        }

        public static Logic operator ==(Text a, Text b) {
            return AreEqual(a, b);
        }
        public static Logic operator !=(Text a, Text b) {
            return !AreEqual(a, b);
        }

        public static Text operator +(Text a, Text b) {
            return new Text(a.value + b.value);
        }
        public static Text operator +(Text a, Number b) {
            return new Text(a.value + b.ToString());
        }
        public static Text operator +(Text a, Logic b) {
            return new Text(a.value + b.ToString());
        }
        public static Text operator +(Number a, Text b) {
            return new Text(a.ToString() + b.value);
        }
        public static Text operator +(Logic a, Text b) {
            return new Text(a.ToString() + b.value);
        }


        public static Number operator -(Text a, Number b) {
            return new Number(Number.Parse(a) - b);
        }
        public static Number operator -(Number a, Text b) {
            return new Number(a - Number.Parse(b));
        }
        public static Number operator *(Text a, Number b) {
            return new Number(Number.Parse(a) * b);
        }
        public static Number operator *(Number a, Text b) {
            return new Number(a * Number.Parse(b));
        }
        public static Number operator /(Text a, Number b) {
            return new Number(Number.Parse(a) / b);
        }
        public static Number operator /(Number a, Text b) {
            return new Number(a / Number.Parse(b));
        }
        public static Number operator %(Text a, Number b) {
            return new Number(Number.Parse(a) % b);
        }
        public static Number operator %(Number a, Text b) {
            return new Number(a % Number.Parse(b));
        }

        public int Length { get => this.value.Length; }

        public override bool Equals(object obj) {
            if (obj == null || !(obj is Text))
                return false;
            else
                return AreEqual(this, (Text)obj);
        }

        private static Logic AreEqual(Text a, Text b) {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return new Logic(false);
            else
                return new Logic(a.value == b.value);
        }

        public override string ToString() {
            return value.ToString();
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
