using Calculator.Core.Datatypes;
using System;
using Type = Calculator.Core.Datatypes.Type;

namespace Calculator.Core.Operations {
    public abstract class Operator {

        public static Datatype Execute(Datatype a, Datatype b) {
            throw new NotImplementedException();
        }

    }
    public class Add : Operator {
        public static new Datatype Execute(Datatype a, Datatype b) {
            switch(a.GetType()) {
                case Type.NUMBER:

                case Type.LOGIC:
                case Type.TEXT:
                case Type.ARRAY:
                    
            }
        }
    }
}
