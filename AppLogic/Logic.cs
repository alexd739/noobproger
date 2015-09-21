using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCMalgo;
using System.Runtime.InteropServices;
using System.Globalization;


namespace AppLogic
{
    public static class FileManager
    {
        public static double[,] read()
        {
            string[] input = File.ReadAllLines("D:/data.txt");
            int lengthfinder = input[0].Split(',').Length;
            double[,] data=new double[input.Length,lengthfinder];
            for (int i = 0; i < input.Length; i++)
            {
                string[] temp = input[i].Split(',');
                for (int j = 0; j < temp.Length; j++)
                {
                    string cv = temp[j];
                    if (temp[j].Contains('.'))
                    {
                        string[] db = temp[j].Split('.');
                        cv = db[0] + ',' + db[1];
                    }
                    data[i, j]= Double.Parse(cv, NumberStyles.AllowTrailingSign |
            NumberStyles.Float);
                }
            }
            
            return data;
        }
    }

    public static class ClusterAnalysis
    {
        //Х-входные данные, NumberОfClusters-число кластеров,Parameter-экспоненциальный вес,eps-точность
        public static double[,] CalculateFCM(double[,] X, int NumberOfClusters, double Parameter,double eps)
        {
            var LengthOfVectors = X.GetLength(1);
            var NumberOfVectors = X.GetLength(0);
            int iteration = 0;
            List<double> NormList = new List<double>();
            //Начальная инициализация нечеткой матрици центров  
            //размерность матрици ЧислоКластеров*ДлинаВекторов 
            double[,] V = new double[NumberOfClusters, LengthOfVectors];

            //Начальная инициализация нечеткой матрици принадлежностей
            //размерность матрици ЧислоВекторов*ЧислоКластеров 
            double[,] U = new double[NumberOfVectors, NumberOfClusters];

            //Заполнение матрици принадлежности случайным образом
            //значение каждого элемента лежит в интервале [0,1];
            Random rand = new Random();

            for (int i = 0; i < NumberOfVectors; i++)
            {
                for (int j = 0; j < NumberOfClusters; j++)
                {
                    U[i, j] = rand.NextDouble();
                }
            }

            //Начальная инициализация матрици расстояний
            //размерность матрици ЧислоВекторов*ЧислоКластеров 
            double[,] D = new double[NumberOfVectors, NumberOfClusters];

            while (true)
            {
                //Рассчет центров кластеров
                //по формуле Vij=SUM(Uki^m*Xkj)/SUM(Uki^m)
                //где i-число кластеров,j-длина векторов,k-число векторов
                for (int i = 0; i < NumberOfClusters; i++)
                {
                    for (int j = 0; j < LengthOfVectors; j++)
                    {
                        double t1 = 0;
                        double t2 = 0;
                        for (int k = 0; k < NumberOfVectors; k++)
                        {
                            t1 += (Math.Pow(U[k, i], Parameter) * X[k, i]);
                            t2 += (Math.Pow(U[k, i], Parameter));
                        }
                        V[i, j] = t1 / t2;
                    }
                }

                //Рассчет матрици расстояний
                //i-число векторов j-число кластеров
                for (int i = 0; i < NumberOfVectors; i++)
                {
                    for (int k = 0; k < NumberOfClusters; k++)
                    {
                        for (int j = 0; j < LengthOfVectors; j++)
                        {
                            D[i, k] += CalculateDistance(X[i, j], V[k, j]);
                        }
                    }
                }

                //Восстановление матрици принадлежностей
                //формула Uij=1/SUM((Dij/Dik)^2/(m-1))
                //i-Число Векторов,j-Число кластеров, k-тоже число кластеров, повтор
                //сделан для обеспечения правильности границ циклов
                for (int i = 0; i < NumberOfVectors; i++)
                {
                    for (int j = 0; j < NumberOfClusters; j++)
                    {
                        if (D[i, j] < double.Epsilon)//если расстояние до центра равно 0, принадлежность максимальна
                            U[i, j] = 1;
                        else
                        {
                            double t = 0;
                            for (int k = 0; k < NumberOfClusters; k++)
                            {
                                t += Math.Pow(D[i, j], 2) / Math.Pow(D[i, k], 2);
                            }
                            U[i, j] = 1 / t;
                        }
                    }
                }

                NormList.Add(FindMatrixNorm(U));

                if(iteration>0)
                {
                    if (NormList.ElementAt<double>(iteration) - NormList.ElementAt<double>(iteration - 1) < eps)
                        break;
                }
                iteration++;
            }
            
            var maxU = U.
            return U;
        }
    


        static double CalculateDistance(double w1, double w2)
        {
            return Math.Sqrt(Math.Pow((w1 - w2), 2));
        }

        static double FindMatrixNorm(double[,] X)
        {
            double[] sums = new double[X.GetLength(1)];
            for(int i =0;i<X.GetLength(1);i++)
            {
                for(int j =0;j<X.GetLength(0);j++)
                {
                    sums[i] += X[j, i];
                }
            }

            return sums.Max();
        }
    }

    public class Logic
    {
        public event EventHandler<ChartData> AnalysisMade;
        public void Analysis(int NumberOfClusters)
        {
            var data = FileManager.read();
            var output = ClusterAnalysis.CalculateFCM(data, NumberOfClusters, 2,0.01);
            ChartData send = new ChartData(data, output);
            AnalysisMade(this, send);
        }
    }

    public class ChartData
    {
        public ChartData(double[,] Xo,double[,] Yo)
        {
            X = Xo;
            Y = Yo;
        }


        public double[,] X
        { get; set; }
        public double[,] Y
        { get; set; }
    }
}
