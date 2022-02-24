using System;
using System.Collections.Generic;

namespace Matrix
{
    /// <summary>
    /// Этот класс реализует дроби.
    /// </summary>
    public class Fraction
    {
        // Числитель и знаменатель.
        public long num;
        public long denum;

        /// <summary>
        /// Этот метод копирует дробь.
        /// </summary>
        /// <returns>копия дроби</returns>
        public Fraction Clone()
        {
            Fraction ans = new Fraction();
            ans.num = num;
            ans.denum = denum;
            return ans;
        }

        /// <summary>
        /// Этот конструктор создаёт дробь по числителю и знаменатилю.
        /// </summary>
        /// <param name="newnum"></param>
        /// <param name="newdenum"></param>
        public Fraction(long newnum = 0, long newdenum = 1)
        {
            // Поддерживаем инвариант, что знаменатель > 0.
            num = newnum * Math.Sign(newdenum);
            denum = Math.Abs(newdenum);
            this.Gcd();
        }

        /// <summary>
        /// Этот метод сокращает дробь.
        /// </summary>
        public void Gcd()
        {
            long fir = Math.Abs(num);
            long sec = Math.Abs(denum);
            while (fir != 0)
            {
                sec = sec % fir;
                long c = sec;
                sec = fir;
                fir = c;
            }
            num = num / sec;
            denum = denum / sec;
        }

        /// <summary>
        /// Этот метод получаят обратную дробь. 
        /// </summary>
        /// <returns>обратная дробь</returns>
        public Fraction Reciprocal()
        {
            Fraction ans = new Fraction();
            ans.num = denum * Math.Sign(num);
            ans.denum = Math.Abs(num);
            return ans;
        }

        /// <summary>
        /// Здесь реализованна сумма дробей.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns>сумма</returns>
        public static Fraction operator +(Fraction self, Fraction other)
        {
            Fraction ans = new Fraction();
            ans.num = self.num * other.denum + other.num * self.denum;
            ans.denum = self.denum * other.denum;
            ans.Gcd();
            return ans;
        }

        /// <summary>
        /// Здесь реализованно получения противоположной дроби(умножение на (-1)).
        /// </summary>
        /// <param name="self"></param>
        /// <returns>противоположная дробь</returns>
        public static Fraction operator -(Fraction self)
        {
            Fraction ans = new Fraction();
            ans.num = -self.num;
            ans.denum = self.denum;
            return ans;
        }

        /// <summary>
        /// Здесь реализованна разность дробей.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns>разность</returns>
        public static Fraction operator -(Fraction self, Fraction other)
        {
            Fraction ans = self + (-other);
            return ans;
        }

        /// <summary>
        /// Здесь реализовано произведение дробей.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns>произведение</returns>
        public static Fraction operator *(Fraction self, Fraction other)
        {
            Fraction ans = new Fraction();
            ans.num = self.num * other.num;
            ans.denum = self.denum * other.denum;
            ans.Gcd();
            return ans;
        }

        /// <summary>
        /// Здесь реализовано деление дробей.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns>отношение</returns>
        public static Fraction operator /(Fraction self, Fraction other)
        {
            Fraction ans = self * (other.Reciprocal());
            return ans;
        }

        /// <summary>
        /// Здесь реализованн оператор неравенства.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator !=(Fraction self, Fraction other)
        {
            return self.num * other.denum != other.num * self.denum;
        }

        /// <summary>
        /// Здесь реализованн оператор неравенства.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator ==(Fraction self, Fraction other)
        {
            return self.num * other.denum == other.num * self.denum;
        }
    }
}
