using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;


namespace lab3 //ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE 
{
    public delegate void MathMulCompletedEventHandler(object sender, CompletedEventArgs e);

    public class CompletedEventArgs : AsyncCompletedEventArgs
    {
        public double[][] firstMatrix;
        public double[][] secondMatrix;
        public double[][] finalMatrix;
        public CompletedEventArgs(Exception ex, bool canceled, object userState, object[] matrixes) : base(ex, canceled, userState)
        {
            firstMatrix = (double[][])matrixes[0];
            secondMatrix = (double[][])matrixes[1];
            finalMatrix = (double[][])matrixes[2];
        }
    }

    public class MathMult
    {

        //delegate will execute main worker method asynchronously
        private delegate void WorkerEventHandler(double[][] firstM, double[][] secondM, AsyncOperation asyncOp);

        //This delegate raise the event post completing the async operation.
        private SendOrPostCallback onCompletedDelegate;

        //To allow async method to call multiple time, We need to store tasks in the list
        //so we can send back the proper value back to main thread
        private HybridDictionary tasks = new HybridDictionary();

        //Event will we captured by the main thread.
        public event MathMulCompletedEventHandler MathMulCompleted;

        public MathMult()
        {
            onCompletedDelegate = new SendOrPostCallback(CalculateCompleted);
        }

        /// <summary>
        /// This function will be called by SendOrPostCallback to raise Method1Completed Event
        /// </summary>
        /// <param name="operationState">Method1CompletedEventArgs object</param>

        private void CalculateCompleted(object operationState)
        {
            CompletedEventArgs e = operationState as CompletedEventArgs;
            if (MathMulCompleted != null)
            {
                MathMulCompleted(this, e);
            }
        }

        public void CancelAsync(object taskId)
        {
            AsyncOperation asyncOp = tasks[taskId] as AsyncOperation;
            if (asyncOp != null)
            {
                lock (tasks.SyncRoot)
                {
                    tasks.Remove(taskId);
                }
            }
        }

        private bool TaskCancelled(object taskId)
        {
            return (tasks[taskId] == null);
        }

        /// <summary>
        /// Asynchoronous version of the method
        /// </summary>
        /// <param name="message">just simple message to display</param>
        /// <param name="userState">Unique value to maintain the task</param>
        /// 
        public virtual void MathMulAsync(double[][] firstMatrix, double[][] secondMatrix, object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(userState);
            //Multiple threads will access the task dictionary, so it must be locked to serialze access
            lock (tasks.SyncRoot)
            {
                if (tasks.Contains(userState))
                {
                    throw new ArgumentException("User state parameter must be unique", "userState");
                }
                tasks[userState] = asyncOp;
            }
            WorkerEventHandler worker = new WorkerEventHandler(MathMulWorker);
            //Execute process Asynchronously
            worker.BeginInvoke(firstMatrix, secondMatrix, asyncOp, null, null);
        }

        public double[][] MathMul(double[][] firstMatrix, double[][] secondMatrix)
        {
            double[][] matrixFinal = Methods.MultiplyMatrix(firstMatrix, secondMatrix);
            return matrixFinal;
        }

        public int getVal(int[][] mat, int column, int row, int size)
        {
            return mat[column][row];
        }

        /// <summary>
        /// This method does the actual work
        /// </summary>
        /// <param name="message"></param>
        /// <param name="asyncOp"></param>
        private void MathMulWorker(double[][] firstMatrix, double[][] secondMatrix, AsyncOperation asyncOp)
        {
            double[][] matrixFinal = Methods.MultiplyMatrix(firstMatrix, secondMatrix);

            lock (tasks.SyncRoot)
            {
                tasks.Remove(asyncOp.UserSuppliedState);
            }

            object[] matrixes = new object[3];
            matrixes[0] = firstMatrix;
            matrixes[1] = secondMatrix;
            matrixes[2] = matrixFinal;

            CompletedEventArgs e = new CompletedEventArgs(null, false, asyncOp.UserSuppliedState, matrixes);
            asyncOp.PostOperationCompleted(onCompletedDelegate, e);
        }
    }

    public static class Program
    {
        public static int matrixSize = 20;
        public static object locked = new object();

        static void Main(string[] args)
        {
            int nrOfTask = 0;
            MathMult mad = new MathMult();
            mad.MathMulCompleted += onMathMulCompleted;
            for (int i = 0; i < 3; i++)
            {
                double[][] matrix1 = Methods.CreateMatrix(matrixSize);
                double[][] matrix2 = Methods.CreateMatrix(matrixSize);
                matrix1 = Methods.FillMatrix(matrix1);
                Thread.Sleep(20);
                matrix2 = Methods.FillMatrix(matrix2);
                mad.MathMulAsync(matrix1, matrix2, nrOfTask);
                nrOfTask++;
            }

            Thread.Sleep(3000);
            Console.WriteLine("");
            double[][] matrix11 = Methods.CreateMatrix(matrixSize);
            matrix11 = Methods.FillMatrix(matrix11);
            double[][] matrix22 = Methods.CreateMatrix(matrixSize);
            matrix22 = Methods.FillMatrix(matrix22);
            double[][] someMat = mad.MathMul(matrix11, matrix22);
            Methods.PrintMatrix("last matrix", someMat);
        }

        private static void onMathMulCompleted(object sender, CompletedEventArgs e)
        {
            lock (locked)
            {
                Methods.PrintMatrix("First", e.firstMatrix);
                Console.WriteLine();
                Methods.PrintMatrix("Second", e.secondMatrix);
                Console.WriteLine();
                Methods.PrintMatrix("Result", e.finalMatrix);
                Console.WriteLine();
            }
        }
    }

    public static class Methods
    {
        public static double[][] CreateMatrix(int size)
        {
            double[][] matrix = new double[size][];
            for (int i = 0; i < size; i++)
            {
                matrix[i] = new double[size];
            }

            return matrix;
        }

        public static double[][] FillMatrix(double[][] matrix)
        {
            Random r = new Random();
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    int x = r.Next(1, 20);
                    matrix[i][j] = r.NextDouble()*(double)x;
                }
            }
            return matrix;
        }

        public static double[][] MultiplyMatrix(double[][] firstMatrix, double[][] secondMatrix)
        {
            double[][] x = Methods.CreateMatrix(Program.matrixSize);

            for (int i = 0; i < firstMatrix.Length; i++)
            {
                for (int j = 0; j < firstMatrix.Length; j++)
                {
                    double result = 0;
                    for (int k = 0; k < firstMatrix.Length; k++)
                    {
                        result += firstMatrix[i][k] * secondMatrix[k][j];
                    }
                    x[i][j] = result;
                }
            }
            return x;
        }

        public static void PrintMatrix(string name, double[][] matrix)
        {
            Console.WriteLine("Matrix: " + name);
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine("");
            }
        }
    }
}
//ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE ZADANIE DODATKOWE 