using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Calculator.Core.Interpreting {
    public class StandardLibrary : IContext {

        #region Reflection Helpers / Interface Methods

        public double CallFunction(string funcName, double[] args) {
            // Test for params keyword method
            var paramTypes = new Type[1] { typeof(double[]) };
            var func = this.GetType().GetMethod(funcName,paramTypes);
            if (func != null && func.GetParameters().Last().GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0) {
                var argObjs = new object[] { args };
                return (double)func.Invoke(this, argObjs);

            // Other methods
            } else {
                funcName = funcName.ToLower();
                var types = new Type[args.Length];
                for (int i = 0; i < args.Length; ++i) {
                    types[i] = args[i].GetType();
                }
                func = this.GetType().GetMethod(funcName, types) ;
                if (func == null)
                    throw new InvalidDataException($"Unknown function: {funcName}");

                var argObjs = args.Select(x => (object)x).ToArray();
                return (double)func.Invoke(this, argObjs);
            }

        }

        public double GetVariable(string varName) {
            varName = varName.ToLower();

            var prop = this.GetType().GetProperty(varName);
            if (prop != null) {
                return (double)prop.GetValue(this);
            }

            if (varDict.ContainsKey(varName)) {
                return varDict[varName];
            }

            throw new InvalidDataException($"Unknown variable: '{varName}'");
        }

        public double SetVariable(string varName, double val) {
            varName = varName.ToLower();

            var prop = this.GetType().GetProperty(varName);
            if (prop != null) {
                throw new InvalidDataException($"Variable '{varName}' is a constant and can't be changed!");
            }

            if (varDict.ContainsKey(varName)) {
                varDict[varName] = val;
            } else {
                varDict.Add(varName, val);
            }
            return varDict[varName];
        }

        #endregion

        private Dictionary<string, double> varDict;

        public StandardLibrary() {
            varDict = new Dictionary<string, double>();
        }

        public double pi { get; } = Math.PI;
        public double e { get; } = Math.E;

        public double abs(double a) => Math.Abs(a);
        public double acos(double a) => Math.Acos(a);
        public double acosh(double a) => Math.Acosh(a);
        public double asin(double a) => Math.Asin(a);
        public double asinh(double a) => Math.Asinh(a);
        public double atan(double a) => Math.Atan(a);
        public double atan2(double a, double b) => Math.Atan2(a, b);
        public double atanh(double a) => Math.Atanh(a);
        public double decrement(double a) => --a;
        public double increment(double a) => ++a;
        public double ceiling(double a) => Math.Ceiling(a);
        public double clamp(double val, double min, double max) => Math.Clamp(val, min, max);
        public double cos(double a) => Math.Cos(a);
        public double cosh(double a) => Math.Cosh(a);
        public double exp(double a) => Math.Exp(a);
        public double floor(double a) => Math.Floor(a);
        public double log2(double a) => Math.Log2(a);
        public double log(double a) => Math.Log(a);
        public double log10(double a) => Math.Log10(a);
        public double log(double a, double dBase) => Math.Log(a, dBase);
        public double max(double a, double b) => Math.Max(a,b);
        public double min(double a, double b) => Math.Min(a, b);
        public double pow(double a, double b) => Math.Pow(a, b);
        public double round(double a) => Math.Round(a);
        public double round(double a, double b) => Math.Round(a, (int)b);
        public double sin(double a) => Math.Sin(a);
        public double sinh(double a) => Math.Sinh(a);
        public double sqrt(double a) => Math.Sqrt(a);
        public double tan(double a) => Math.Tan(a);
        public double tanh(double a) => Math.Tanh(a);
        public double root(double a, double n) => Math.Pow(a, 1.0 / n);


        public double avg(params double[] nums) => nums.Average();
        public double sum(params double[] nums) => nums.Sum();
        public double var(params double[] nums) {
            double average = avg(nums);
            return nums.Select(val => (val - average) * (val - average)).Sum();
        }

        public double sd(params double[] nums) => Math.Sqrt(var(nums));

        public double fib(double n) {
            if (n < 0) throw new InvalidDataException("fib(n) can't have negative values for n");
            if (n == 0) return 0;
            if (n == 1) return 1;
            long n0 = 0;
            long n1 = 1;
            long n2 = 1;
            for(int i = 1; i < n; ++i) {
                n2 = n1 + n0;
                n0 = n1;
                n1 = n2;
            }
            return n2;
        }

        public double fac(double n) {
            if (n < 0) throw new InvalidDataException("fac(n) can't have negative values for n");
            if (n == 0) return 1;
            long p = 1;
            for(int i = 1; i <= n; ++i) {
                p *= i;
            }
            return p;
        }

        public double choose(double n, double k) {
            if (n < k) throw new InvalidDataException("In method 'n CHOOSE k': n must be greater or equal to k");
            if (k == 0)
                return 1;
            return (n * choose(n - 1, k - 1)) / k;
        }

    }
}
