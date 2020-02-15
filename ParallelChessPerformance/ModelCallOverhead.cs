using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class ModelCallOverhead {

        public struct TestClassA {
            public int work;
            public void methodDoWork() {
                for(int i = 0; i < 10; i++) {
                    work++;
                }
            }
            public void methodCallDoWork2() {
                methodCallDoWork();
            }
            public void methodCallDoWork() {
                methodDoWork();
            }

            public static TestClassA staticFunctionCallDoWork(TestClassA testClassA) {
                testClassA.methodDoWork();
                return testClassA;
            }
        }
        
        void methodCallExternalDoWork(TestClassA testClassA) {
            testClassA.methodDoWork();
        }

        int iterations = 100000000;







        [Benchmark]
        public void methodToExternalMethod() {
            var c = new TestClassA() { work = 0 };
            for (int i = 0; i < iterations; i++) {
                methodCallExternalDoWork(c);
            }
        }

        [Benchmark]
        public void methodToInternalMethod() {
            var c = new TestClassA() { work = 0 };
            for (int i = 0; i < iterations; i++) {
                c.methodCallDoWork();
            }
        }
        [Benchmark]
        public void methodToInternal2Method() {
            var c = new TestClassA() { work = 0 };
            for (int i = 0; i < iterations; i++) {
                c.methodCallDoWork();
            }
        }

        [Benchmark]
        public void staticFunctionTomethod() {
            var c = new TestClassA() { work = 0 };
            for (int i = 0; i < iterations; i++) {
                TestClassA.staticFunctionCallDoWork(c);
            }
        }

        [Benchmark]
        public void callMethodDirectly() {
            var c = new TestClassA() { work = 0 };
            for (int i = 0; i < iterations; i++) {
                c.methodDoWork();
            }
        }
    }
}
