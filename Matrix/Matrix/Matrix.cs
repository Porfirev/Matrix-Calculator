using System;
using System.Collections.Generic;

namespace Matrix
{
    /// <summary>
    /// Этот класс реализует матрицы.
    /// </summary>
    public class Matrix: Program
    {
        //Имя hor - horizontal, ver - vertical.
        public int hor;
        public int ver;
        //mat - сама матрица.
        public List<List<Fraction>> mat;

        /// <summary>
        /// Этот метод клонирует матрицу
        /// </summary>
        /// <returns>копия нашей матрицы</returns>
        public Matrix Clone()
        {
            Matrix ans = new Matrix();
            ans.hor = hor;
            ans.ver = ver;
            ans.mat = new List<List<Fraction>>();
            for (int i = 0; i < hor; ++i)
            {
                List<Fraction> line = new List<Fraction>();
                for(int j = 0; j < ver; ++j)
                {
                    line.Add(mat[i][j].Clone());
                }
                ans.mat.Add(line);
            }
            return ans;
        }

        /// <summary>
        /// Этот конструктор делает матрицу из двумерного списка.
        /// </summary>
        /// <param name="newmat"></param>
        public Matrix(List<List<Fraction>> newmat = null)
        {
            List<List<Fraction>> emptymat = new List<List<Fraction>>();
            List<List<Fraction>> realnewmat = (newmat == null ? emptymat : newmat);
            mat = realnewmat;
            hor = realnewmat.Count;
            ver = (realnewmat == emptymat ? 0 : realnewmat[0].Count);
        }

        /// <summary>
        /// Этот конструктор делает случайную матрицу данного размера.
        /// </summary>
        /// <param name="newhor"></param>
        /// <param name="newver"></param>
        public Matrix(int newhor, int newver)
        {
            hor = newhor;
            ver = newver;
            mat = new List<List<Fraction>>();
            for (int i = 0; i < hor; ++i)
            {
                List<Fraction> buff = new List<Fraction>();
                mat.Add(buff);
                for (int j = 0; j < ver; ++j)
                {
                    Fraction frac = new Fraction();
                    mat[mat.Count - 1].Add(frac);
                }
            }
        }
        
        /// <summary>
        /// Этот метод считает след.
        /// </summary>
        /// <returns></returns>
        public Fraction Trace()
        {
            Fraction ans = new Fraction();
            for (int i = 0; i < hor; ++i)
            {
                ans += mat[i][i];
            }
            return ans;
        }

        /// <summary>
        /// Этот метод возвращает транспонированную матрицу, причём изначальная не меняется.
        /// </summary>
        /// <returns>транспонированная матрица</returns>
        public Matrix Transpose()
        {
            Matrix ans = new Matrix(ver, hor);
            for (int i = 0; i < hor; ++i)
            {
                for (int j = 0; j < ver; ++j)
                {
                    ans[j][i] = mat[i][j];
                }
            }
            return ans;
        }
        
        /// <summary>
        /// Это перегрузка оператора обращения по индексу, чтобы можно было обращаться сразу к матрице.
        /// </summary>
        /// <param name="i">индекс</param>
        /// <returns>значение поля mat с индексом i</returns>
        public List<Fraction> this[int i] => mat[i];


        /// <summary>
        /// Это перегрузка оператора +.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns>разность</returns>
        public static Matrix operator +(Matrix self, Matrix other)
        {
            Matrix ans = new Matrix(self.hor, self.ver);
            for (int i = 0; i < self.hor; ++i)
            {
                for (int j = 0; j < self.ver; ++j)
                {
                    ans[i][j] = self[i][j] + other[i][j];
                }
            }
            return ans;
        }

        /// <summary>
        /// Это перегрузка оператора -.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns>сумма</returns>
        public static Matrix operator -(Matrix self, Matrix other)
        {
            Matrix ans = new Matrix(self.hor, self.ver);
            for (int i = 0; i < self.hor; ++i)
            {
                for (int j = 0; j < self.ver; ++j)
                {
                    ans[i][j] = self[i][j] - other[i][j];
                }
            }
            return ans;
        }


        /// <summary>
        /// Это перегрузка оператора *.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other">число(дробь)</param>
        /// <returns>произведение</returns>
        public static Matrix operator *(Matrix self, Fraction other)
        {
            Matrix ans = new Matrix(self.hor, self.ver);
            for (int i = 0; i < self.hor; ++i)
            {
                for (int j = 0; j < self.ver; ++j)
                {
                    ans[i][j] = other * self[i][j];
                }
            }
            return ans;
        }


        /// <summary>
        /// Это перегрузка оператора * матриц.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other">число(дробь)</param>
        /// <returns>произведение</returns>
        public static Matrix operator *(Matrix self, Matrix other)
        {
            Matrix ans = new Matrix(self.hor, other.ver);
            for (int i = 0; i < self.hor; ++i)
            {
                for (int j = 0; j < other.ver; ++j)
                {
                    for (int q = 0; q < self.ver; ++q)
                    {
                        ans[i][j] += self[i][q] * other[q][j];
                    }
                }
            }
            return ans;
        }

        /// <summary>
        /// Этот метод считает определитель, с помощю метода Гаусса.
        /// </summary>
        /// <returns>определитель</returns>
        public Fraction Determinant()
        {
            Fraction ans = new Fraction(1);
            Matrix newmatrix = this.Canonic();
            for(int i = 0; i < hor; ++i)
            {
                ans *= newmatrix[i][i];
            }
            return ans;
        }

        /// <summary>
        /// Это элементарное преобразование, умножающая строку на число.
        /// </summary>
        /// <param name="i">индекс строки</param>
        /// <param name="lambda">на какое число домножаем</param>
        public void FirstElementary(int i, Fraction lambda)
        {
            for(int j = 0; j < ver; ++j)
            {
                mat[i][j] *= lambda;
            }
        }

        /// <summary>
        /// Это элементарное преобразование, переставляющая строки местами.
        /// </summary>
        /// <param name="i">индекс первой строки</param>
        /// <param name="j">индекс второй строки</param>
        public void SecondElementary(int i, int j)
        {
            List<Fraction> buff = mat[i];
            mat[i] = mat[j];
            mat[j] = mat[i];
        }

        /// <summary>
        /// Это элементарное преобразование, которая к одной строке прибавляет другую умноженную на коэффициент.
        /// </summary>
        /// <param name="i">индекс первой строки</param>
        /// <param name="j">индекс второй строки</param>
        /// <param name="lambda">коэффициент</param>
        public void ThirdElementary(int i, int j, Fraction lambda)
        {
            for (int q = 0; q < ver; ++q)
            {
                mat[i][q] += mat[j][q] * lambda;
            }
        }

        /// <summary>
        /// Этот метод делает матрицу кононического вида, с помощью элементарных преобразований.
        /// </summary>
        /// <returns>матрица канонического вида</returns>
        public Matrix Canonic()
        {
            Matrix newmatrix = new Matrix();
            newmatrix = this.Clone();
            int i = 0, j = 0;
            Fraction zero = new Fraction();
            while (true)
            {
                int starti = i;
                while (i < hor && j < ver && newmatrix[i][j] == zero)
                {
                    ++i;
                }
                if (i == hor || j == ver)
                {
                    i = starti;
                    ++j;
                    if (j >= ver || i >= hor)
                    {
                        break;
                    }
                    continue;
                }
                for (int q = 0; q < hor; ++q)
                {
                    if (q == i || newmatrix[q][j] == zero)
                    {
                        continue;
                    }
                    newmatrix.ThirdElementary(q, i, -newmatrix[q][j] / newmatrix[i][j]);
                }
                i++;
                j++;
            }
            return newmatrix;
        }

        /// <summary>
        /// Этот метод делает матрицу кононического вида, с помощью элементарных преобразований, игнорирую последний столбец(нужно для решения СЛАУ).
        /// </summary>
        public void CanonicForSolve()
        {
            int i = 0, j = 0;
            Fraction zero = new Fraction();
            while (true)
            {
                int starti = i;
                while (i < hor && j < ver - 1 && mat[i][j] == zero)
                {
                    ++i;
                }
                if (i == hor || j == ver - 1)
                {
                    i = starti;
                    ++j;
                    if (j >= ver - 1 || i >= hor)
                    {
                        return;
                    }
                    continue;
                }
                for (int q = 0; q < hor; ++q)
                {
                    if (q == i || mat[q][j] == zero)
                    {
                        continue;
                    }
                    this.ThirdElementary(q, i, -mat[q][j] / mat[i][j]);
                }
                i++;
                j++;
            }
        }

        /// <summary>
        /// Этот медот проверяет, является ли матрица квадратной.
        /// </summary>
        /// <returns></returns>
        public bool CheckSquare()
        {
            return hor == ver;
        }
    }
}
