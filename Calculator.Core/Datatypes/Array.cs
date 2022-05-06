
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Core.Datatypes {
    public class Array : Datatype, IEnumerable<Datatype> {

        private List<Datatype> items = new List<Datatype>();

        public Array(double[] vals) : base(Type.ARRAY) {
            items.AddRange(vals.Select(item => (Number)item).ToArray());
        }
        public Array(Datatype[] vals) : base(Type.ARRAY) {
            items.AddRange(vals);
        }

        public static implicit operator List<Datatype>(Array array) {
            return array.items;
        }

        public static implicit operator Array(Datatype[] vals) {
            return new Array(vals);
        }

        public override string ToString() {
            return $"[{string.Join(", ", items)}]";
        }

        public IEnumerator<Datatype> GetEnumerator() {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return items.GetEnumerator();
        }
    }
}
