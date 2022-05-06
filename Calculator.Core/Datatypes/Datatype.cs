using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Datatypes {
    public abstract class Datatype {

        private Type type;

        public Type GetType() { return type; }

        public Datatype(Type type) {
            this.type = type;
        }
    }

    public enum Type {
        NUMBER,
        LOGIC,
        TEXT,
        ARRAY
    }
}
