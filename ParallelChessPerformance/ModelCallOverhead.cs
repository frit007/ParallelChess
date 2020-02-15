using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessPerformance {
    [SimpleJob]
    public class ModelCallOverhead {

        public class TestClassA {
            int work = 0;
            static int staticWork = 0;
            public void methodDoWork() {
                work++;
            }

            public void methodCallDoWork() {
                methodDoWork();
            }

            public void methodCallStaticWork() {
                staticFunctionCallStaticWork();
            }

            public static void staticFunctionCallStaticWork() {
                staticWork++;
            }
            public static void staticFunctionCallDoWork(TestClassA testClassA) {
                testClassA.methodDoWork();
            }


            public static void staticFunctionCallStaticDoWork() {
                staticFunctionCallStaticWork();
            }
        }
        
        void methodCallExternalDoWork(TestClassA testClassA) {
            testClassA.methodDoWork();
        }

        int iterations = 100000000;







        [Benchmark]
        public void methodToExternalMethod() {
            var c = new TestClassA();
            for (int i = 0; i < iterations; i++) {
                methodCallExternalDoWork(c);
            }
        }

        [Benchmark]
        public void methodToInternalMethod() {
            var c = new TestClassA();
            for (int i = 0; i < iterations; i++) {
                c.methodCallDoWork();
            }
        }

        [Benchmark]
        public void methodToStaticFunction() {
            var c = new TestClassA();
            for (int i = 0; i < iterations; i++) {
                c.methodCallStaticWork();
            }
        }

        [Benchmark]
        public void staticFunctionToStaticFunction() {
            for (int i = 0; i < iterations; i++) {
                TestClassA.staticFunctionCallStaticDoWork();
            }
        }

        [Benchmark]
        public void staticFunctionTomethod() {
            var c = new TestClassA();
            for (int i = 0; i < iterations; i++) {
                TestClassA.staticFunctionCallDoWork(c);
            }
        }

        [Benchmark]
        public void callMethodDirectly() {
            var c = new TestClassA();
            for (int i = 0; i < iterations; i++) {
                c.methodDoWork();
            }
        }

        [Benchmark]
        public void callStaticFunctionDirectly() {
            for (int i = 0; i < iterations; i++) {
                TestClassA.staticFunctionCallStaticWork();
            }
        }
    }
}
