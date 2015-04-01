using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Program {
    public class A {
        int sequence, concurrence;
        public Stopwatch sw1;
        public Stopwatch sw2;
        public A() {
            sequence = 0;
            concurrence = 0;
            sw1 = new Stopwatch();
            sw2 = new Stopwatch();
        }
        public int countEdgesTask(int n, byte[,] tab) {
            sw2.Start();
            var tasks = new List<Task>();
            for(int i = 0; i < n; i++) {
                int temp = i;
                tasks.Add(Task.Run(() => {
                    for(int j = 0; j < temp; j++) {
                        int temp2 = j;
                        if(tab[temp, temp2] == 1)
                            lock(this)
                                concurrence++;
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            sw2.Stop();
            return concurrence;
        }
        public int countEdges(int n, byte[,] tab) {
            sw1.Start();
            for(var i = 0; i < n - 1; ++i) {
                for(int j = i + 1; j < n; j++) {
                    if(tab[i, j] == 1)
                        sequence++;
                }
            }
            sw2.Stop();
            return sequence;
        }
    }

    public class main {
        public static int Main() {
            const int n = 10000;
            Console.WriteLine("Creating matrix[{0},{0}]", n);
            Random random = new Random();
            var matrix = new byte[n, n];
            for(int i = 0; i < n; i++) {
                for(int j = 0; j < i; j++) {
                    matrix[i, j] = (byte)random.Next(2);
                    matrix[j, i] = matrix[i, j];
                }
            }
            Console.WriteLine("Done.\nCount edges:");
            var edges = new A();
            Console.WriteLine("\nSequence function: {0}", edges.countEdges(n, matrix));
            Console.WriteLine("Concurrence function: {0}\n", edges.countEdgesTask(n, matrix)); ;
            Console.WriteLine("Times[ms]:\nSequence: {0}\nConcurrence: {1}", edges.sw1.Elapsed.TotalMilliseconds, edges.sw2.Elapsed.TotalMilliseconds);
            Console.ReadKey();
            return 0;
        }

    }
}
