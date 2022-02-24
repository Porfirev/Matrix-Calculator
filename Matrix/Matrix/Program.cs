using System;
using System.Collections.Generic;
using System.IO;

namespace Matrix
{
    // В файле с программой лежит README, прочитайте его прежде чем преступать. Поэтому все подробные подсказки там, чтобы не загромождать код.
    public class Program
    {

        /// <summary>
        /// Этот метод решает систему уравнений.
        /// </summary>
        /// <param name="ratios">матрица кэффициентов.</param>
        /// <param name="ans">ответ(будующий)</param>
        /// <returns>3 варианта(одно решение, решений нет или их бесконечно много)</returns>
        public static int Solve(Matrix ratios, List<Fraction> ans)
        {
            ratios.CanonicForSolve();
            Fraction zero = new Fraction();
            int rank = 0;
            for (int i = 0; i < ratios.hor; ++i)
            {
                int countzero = 0;
                int nottravratio = 0;
                for (int j = 0; j < ratios.ver - 1; ++j)
                {
                    if (ratios[i][j] == zero)
                    {
                        countzero++;
                    }
                    else
                    {
                        nottravratio = j;
                    }
                }
                if (countzero < ratios.ver - 2)
                {
                    return -1;
                }
                else if (countzero == ratios.ver - 1 && ratios[i][ratios.ver - 1] != zero)
                {
                    return 0;
                }
                else if (countzero != ratios.ver - 1)
                {
                    ans[nottravratio] = ratios[i][ratios.ver - 1] / ratios[i][nottravratio];
                    rank++;
                }
            }
            if (rank > ans.Count)
                return 0;
            else if(rank == ans.Count)
                return 1;
            return -1;
        }

        /// <summary>
        /// Этот метод печатает дробь на экран.
        /// </summary>
        /// <param name="frac">дробь</param>
        static void Print(Fraction frac)
        {
            if (frac.denum != 1)
                Console.Write($"{frac.num} / {frac.denum} ");
            else
                Console.Write($"{frac.num} ");
        }

        /// <summary>
        /// Этот метод печатает матрицу на экран.
        /// </summary>
        /// <param name="matrix">матрица</param>
        static void Print(Matrix matrix)
        {
            Console.WriteLine();
            for (int i = 0; i < matrix.hor; ++i)
            {
                for (int j = 0; j < matrix.ver; ++j)
                {
                    Print(matrix[i][j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Этот метод печатает решение СЛАУ на экран.
        /// </summary>
        /// <param name="ans">Ответ на СЛАУ</param>
        static void PrintSolve(List<Fraction> ans)
        {
            for (int i = 0; i < ans.Count; ++i)
            {
                Console.Write($"X{i} = ");
                Print(ans[i]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Этот метод проверяет правельность ввода операции.
        /// </summary>
        /// <param name="command">операция</param>
        /// <param name="allcommands">всевозможные операции</param>
        /// <returns>Удачная-ли проверка</returns>
        static bool CheckOperation(string command, string[] allcommands)
        {
            for (int i = 0; i < allcommands.Length; ++i)
            {
                if (command == allcommands[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Этот метод считывает матрицу из консоли.
        /// </summary>
        /// <param name="horizontal">кол-во горизонталей</param>
        /// <param name="vertical">кол-во вертикалей</param>
        /// <returns>итоговую матрицу</returns>
        static Matrix MakeMatrixByMyself(uint horizontal, uint vertical)
        {
            Matrix ans = new Matrix((int)horizontal, (int)vertical);
            Console.WriteLine("Введите саму матрицу");
            for (int i = 0; i < horizontal; ++i)
            {
                char[] delimiters = { ' ' };
                string[] line = Console.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length != vertical)
                {
                    Console.WriteLine("Вы ввели строку неверно");
                    return MakeMatrixByMyself(horizontal, vertical);
                }
                for (int j = 0; j < vertical; ++j)
                {
                    int elem = 0;
                    if (!int.TryParse(line[j], out elem))
                    {
                        Console.WriteLine("Вы ввели не число или число большее 30(по модулю), придётся ввести матрицу снова");
                        return MakeMatrixByMyself(horizontal, vertical);
                    }
                    ans[i][j] = new Fraction(elem);
                }
            }
            return ans;
        }

        /// <summary>
        /// Проверяет верность команды restart.
        /// </summary>
        static void CheckRestart(StreamReader file)
        {
            Console.WriteLine("В файле некорректные данные, измените значения и введите restart");
            file.Close();
            string command = Console.ReadLine();
            while (command != "restart")
            {
                Console.WriteLine("Вы ввели не restart");
                command = Console.ReadLine();
            }
        }

        /// <summary>
        /// Она закрывает файл.
        /// </summary>
        /// <param name="file">файл</param>
        static void CloseFile(ref StreamReader file)
        {
            try
            {
                file.Close();
            }
            catch
            {
                // Мне не нужно ничего делать.
                int mus = 0;
            }
        }
        /// <summary>
        /// Этот метод считывает матрицу из файла.
        /// </summary>
        /// <param name="horizontal">кол-во горизонталей</param>
        /// <param name="vertical">кол-во вертикалей</param>
        /// <param name="file">сам файл</param>
        /// <returns>итоговую матрицу</returns>
        static Matrix MakeMatrixFromFile(uint horizontal, uint vertical, string path)
        {
            Matrix ans = new Matrix((int)horizontal, (int)vertical);
            StreamReader file = new StreamReader(@path);
            string firstline = file.ReadLine();
            int countlines = 0;
            while (file.ReadLine() != null)
            {
                countlines++;
            }
            if (countlines != horizontal)
            {
                CheckRestart(file);
            }
            file.Close();
            file = new StreamReader(@path);
            firstline = file.ReadLine();
            for (int i = 0; i < horizontal; ++i)
            {
                char[] delimiters = { ' ' };
                string[] line = file.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (line.Length != vertical)
                {
                    CheckRestart(file);
                    return MakeMatrixFromFile(horizontal, vertical, path);
                }
                for (int j = 0; j < vertical; ++j)
                {
                    int elem = 0;
                    if (!int.TryParse(line[j], out elem) || Math.Abs(elem) > 30)
                    {
                        CheckRestart(file);
                        return MakeMatrixFromFile(horizontal, vertical, path);
                    }
                    ans[i][j] = new Fraction(elem);
                }
            }
            CloseFile(ref file);
            return ans;
        }

        /// <summary>
        /// Этот метод создаёт матрицу со случайными элементами.
        /// </summary>
        /// <param name="horizontal">кол-во горизонталей</param>
        /// <param name="vertical">кол-во вертикалей</param>
        /// <param name="param">чекер на диапозон рандома</param>
        /// <returns>итоговую матрицу</returns>
        static Matrix MakeRandomMatrix(uint horizontal, uint vertical, int param)
        {
            Matrix ans = new Matrix((int)horizontal, (int)vertical);
            for (int i = 0; i < ans.hor; ++i)
            {
                for (int j = 0; j < ans.ver; ++j)
                {
                    Random rnd = new Random();
                    Fraction frac = new Fraction();
                    if (param == 0)
                        frac = new Fraction(rnd.Next(-30, 31));
                    else
                        frac = new Fraction(rnd.Next(-10, 11));
                    ans[i][j] = frac;
                }
            }
            return ans;
        }

        /// <summary>
        /// Считывает размеры матрицы из консоли.
        /// </summary>
        /// <param name="horizontal">кол-во горизонталей</param>
        /// <param name="vertical">кол-во вертикалей</param>
        static void MakeParemeters(ref uint horizontal, ref uint vertical)
        {
            Console.WriteLine("Введите размеры матрицы в одну строку: высота потом ширина");
            char[] delimiters = { ' ' };
            string[] size = Console.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            while (size.Length != 2 || !uint.TryParse(size[0], out horizontal) || !uint.TryParse(size[1], out vertical) || horizontal > 10 || vertical > 10 || horizontal <= 0 || vertical <= 0)
            {
                Console.WriteLine("Вы ввели неверную команду или размеры матрицы слишком большие(или наобарот есть 0), попробуйте снова");
                size = Console.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// Считывает размеры матрицы из файла.
        /// </summary>
        /// <param name="horizontal">кол-во горизонталей</param>
        /// <param name="vertical">кол-во вертикалей</param>
        /// <param name="file">сам файл</param>
        static void MakeParemetersFile(ref uint horizontal, ref uint vertical, string path)
        {
            string[] size;
            StreamReader file = new StreamReader(@path);
            char[] delimiters = {' '};
            size = file.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (size.Length != 2 || !uint.TryParse(size[0], out horizontal) || !uint.TryParse(size[1], out vertical) || horizontal > 10 || vertical > 10 || horizontal <= 0 || vertical <= 0)
            {
                Console.WriteLine("В файле некорректные данные, измените значения и введите restart");
                file.Close();
                string command = Console.ReadLine();
                while (command != "restart")
                {
                    Console.WriteLine("Вы ввели не restart");
                    command = Console.ReadLine();
                }
                MakeParemetersFile(ref horizontal, ref vertical, path);
            }
            CloseFile(ref file);
        }

        /// <summary>
        /// Этот метод обрабатывает ввод файла.
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns>матрицу считанную из файла</returns>
        static Matrix TreatmentFile(uint horizontal, uint vertical)
        {
            Console.WriteLine("Введите путь к вашему файлу");
            string path = Console.ReadLine().Trim(new char[] { ' ', '"'});
            MakeParemetersFile(ref horizontal, ref vertical, path);
            Matrix ans = MakeMatrixFromFile(horizontal, vertical, path);
            Print(ans);
            Console.WriteLine("Это ваша матрица");
            return ans;
        }

        /// <summary>
        /// Этот метод отвечает за создание матрицы.
        /// </summary>
        /// <param name="param">чекер на диапозон рандома</param>
        /// <returns>Созданную матрицу</returns>
        static Matrix MakeMatrix(int param = 0)
        {
            Console.WriteLine("Введите, как бы вы хотели ввести матрицу(random, myself или file)");
            string how = Console.ReadLine();
            uint horizontal = 0;
            uint vertical = 0;
            while (how != "random" && how != "myself" && how != "file")
            {
                Console.WriteLine("Вы ввели неверную команду, попробуйте снова");
                how = Console.ReadLine();
            }
            if (how == "myself")
            {
                MakeParemeters(ref horizontal, ref vertical);
                return MakeMatrixByMyself(horizontal, vertical);
            }
            else if (how == "random")
            {
                MakeParemeters(ref horizontal, ref vertical);
                Matrix ans = MakeRandomMatrix(horizontal, vertical, param);
                Print(ans);
                Console.WriteLine("Это ваша матрица");
                return ans;
            }
            else
            {
                while (true)
                {
                    try
                    {
                        return TreatmentFile(horizontal, vertical);
                    }
                    catch
                    {
                        Console.WriteLine("Вы ввели путь в неправельном формате, или по этому пути нет файла\n");
                    }
                }
            }
        }

        /// <summary>
        /// Этот метод считывает дробь из консоли.
        /// </summary>
        /// <returns>дробь</returns>
        static Fraction MakeFraction()
        {
            int num = 0;
            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Вы ввели не число");
            }
            Fraction ans = new Fraction(num);
            return ans;
        }

        /// <summary>
        /// Этот метод считывает матрица пока они не будут одного размера.
        /// </summary>
        /// <param name="first">первая матрица</param>
        /// <param name="second">вторая матрица</param>
        static void CheckSize(ref Matrix first, ref Matrix second)
        {
            while (first.hor != second.hor || first.ver != second.ver)
            {
                Console.WriteLine("Вы ввели матрицы разных размеров");
                first = MakeMatrix();
                second = MakeMatrix();
            }
        }
        /// <summary>
        /// Обновляет значения, если они не подходит для умножения
        /// </summary>
        /// <param name="left">левый операнд</param>
        /// <param name="right">правый операнд</param>
        static void New(ref Matrix left, ref Matrix right)
        {
            Console.WriteLine("Такие матрицы перемножить невозможно");
            left = MakeMatrix();
            right = MakeMatrix();
        }

        /// <summary>
        /// В этом методе реализованны базовые операцие(+, -, *(оба)).
        /// </summary>
        /// <param name="operation">операция</param>
        static void BaseCalculate(string operation)
        {
            if (operation == "+")
            {
                Matrix left = MakeMatrix();
                Matrix right = MakeMatrix();
                CheckSize(ref left, ref right);
                Print(left + right);
            }
            if (operation == "-")
            {
                Matrix left = MakeMatrix();
                Matrix right = MakeMatrix();
                CheckSize(ref left, ref right);
                Print(left - right);
            }
            if (operation == "*")
            {
                Matrix left = MakeMatrix();
                Console.WriteLine("Введите number или matrix, в зависимости от того, на что вы хотите умножить, на матрицу или число");
                string which = Console.ReadLine();
                while (which != "number" && which != "matrix")
                {
                    Console.WriteLine("Вы ввели неверную команду, попробуйте снова");
                    which = Console.ReadLine();
                }
                if (which == "number")
                {
                    Fraction right = MakeFraction();
                    Print(left * right);
                }
                else
                {
                    Matrix right = MakeMatrix();
                    while (left.ver != right.hor)
                    {
                        New(ref left, ref right);
                    }
                    Print(left * right);
                }
            }
        }

        /// <summary>
        /// Этот метод считывает матрицу пока она не будет квадратной.
        /// </summary>
        /// <param name="matrix">матрица</param>
        static void Square(ref Matrix matrix)
        {
            while (!matrix.CheckSquare())
            {
                Console.WriteLine("Вы ввели не квадратную матрицу, попробуйте снова");
                matrix = MakeMatrix();
            }
        }

        static void CheckSmallSize(ref Matrix matrix)
        {
            if (matrix.hor > 7 || matrix.ver > 7)
            {
                Console.WriteLine("Эта матрца слишком большая, для такой операции");
                matrix = MakeMatrix();
                CheckSmallSize(ref matrix);
            }
            else
            {
                for(int i = 0; i < matrix.hor; ++i)
                {
                    for(int j = 0; j < matrix.ver; ++j)
                    {
                        if (Math.Abs(matrix[i][j].num) > 15)
                        {
                            Console.WriteLine("В этой матрице слищшком большие элементы");
                            matrix = MakeMatrix();
                            CheckSmallSize(ref matrix);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Этот метод реализует сам калькулятор.
        /// </summary>
        /// <param name="operation">операция</param>
        static void Calculate(string operation)
        {
            // Базовый операции я вынес в отдельный метод.
            BaseCalculate(operation);
            if (operation == "Trace")
            {
                Matrix matrix = MakeMatrix();
                Square(ref matrix);
                Print(matrix.Trace());
            }
            if (operation == "Determinant")
            {
                Matrix matrix = MakeMatrix(1);
                CheckSmallSize(ref matrix);
                Square(ref matrix);
                Print(matrix.Determinant());
            }
            if (operation == "Transpose")
            {
                Matrix matrix = MakeMatrix();
                Print(matrix.Transpose());
            }
            if (operation == "Solve")
            {
                Matrix matrix = MakeMatrix(1);
                CheckSmallSize(ref matrix);
                List<Fraction> ans = new List<Fraction>();
                Fraction zero = new Fraction();
                for (int i = 0; i < matrix.ver - 1; ++i)
                {
                    ans.Add(zero);
                }
                int flag = Solve(matrix, ans);
                if (flag == -1)
                    Console.Write("Решений бесконечно много");
                else if (flag == 0)
                    Console.Write("У этой системы нет решений");
                else
                    PrintSolve(ans);
            }
            Console.WriteLine();
        }
        
        /// <summary>
        /// Этот метод выводит README.
        /// </summary>
        static void Readme()
        {
            StreamReader file = new StreamReader("README.txt");
            string str = file.ReadLine();
            while(str != null)
            {
                Console.WriteLine(str);
                str = file.ReadLine();
            }
            file.Close();
        }
        /// <summary>
        /// Сама программа.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите start, чтобы запустить калькулятор и finish, чтобы закончить программу или readme, чтобы вывеcти Readme");
                    Console.WriteLine("Файл Readme, также лежит отдельно, можете с ним ознакомиться");
                    string firstcommand = Console.ReadLine();
                    while (firstcommand != "start" && firstcommand != "finish" && firstcommand != "readme")
                    {
                        Console.WriteLine("Вы ввели неверную команду, попробуйте снова");
                        firstcommand = Console.ReadLine();
                    }
                    if (firstcommand == "finish")
                    {
                        return;
                    }
                    if (firstcommand == "readme")
                    {
                        Readme();
                        continue;
                    }
                    Console.WriteLine("Введите операцию, которую хотите выполнить(список с подробным описанием лежит в README)");
                    string operation = Console.ReadLine();
                    string[] alloperations = { "+", "-", "*", "Trace", "Determinant", "Solve", "Transpose" };
                    while (!CheckOperation(operation, alloperations))
                    {
                        Console.WriteLine("Вы ввели неверную команду, попробуйте снова");
                        operation = Console.ReadLine();
                    }

                    Calculate(operation);
                }
                catch
                {
                    Console.WriteLine("Вы придумали какой-то гениальный тест, который все ломает, буду надееться, что на этом тесте программа и не должна работать. Так что давайте попробуем всё сначала");
                }
            }
        }
    }
}
