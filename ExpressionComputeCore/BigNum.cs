using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionComputeCore
{
    public class BigNum
    {
        string num;
        public override string ToString()
        {
            return num;
        }
        public BigNum(string num)
        {
            this.num = num;
        }
        public BigNum(char[] num)
        {
            this.num = new string(num);
        }
        public static BigNum operator +(BigNum a, BigNum b) => new BigNum(BigNumCalculator.Plus(a.ToString(), b.ToString()));
        public static BigNum operator -(BigNum a, BigNum b) => new BigNum(BigNumCalculator.Subtraction(a.ToString(), b.ToString()));
        public static BigNum operator *(BigNum a, BigNum b) => new BigNum(BigNumCalculator.Quadrature(a.ToString(), b.ToString()));
    }

    internal class BigNumCalculator
    {
        public static char[] Plus(string a, string b)
        {
            return Plus(a.ToCharArray(), b.ToCharArray());
        }

        public static char[] Plus(char[] a, char[] b)
        {
            char[] c = a;//补位后的a
            char[] d = b;//补位后的b
            int alength = a.Count();//a长度
            int blength = b.Count();//b长度
            int adot = alength;//a小数点位置
            int bdot = blength;//b小数点位置
            #region 小数点补位
            for (int i = 0; i < alength; i++)
            {
                if (a[i] == '.')
                {
                    adot = i;
                    break;
                }
            }
            for (int i = 0; i < blength; i++)
            {
                if (b[i] == '.')
                {
                    bdot = i;
                    break;
                }
            }
            if (adot != alength || bdot != blength)
            {
                if (adot == alength)
                {
                    c = new char[alength + 1 + blength - bdot - 1];
                    for (int i = 0; i < c.Length; i++)
                    {
                        if (i < alength)
                        {
                            c[i] = a[i];
                        }
                        else if (i == alength)
                        {
                            c[i] = '.';
                        }
                        else
                        {
                            c[i] = '0';
                        }
                    }

                }
                else if (bdot == blength)
                {
                    d = new char[blength + 1 + alength - adot - 1];
                    for (int i = 0; i < d.Length; i++)
                    {
                        if (i < blength)
                        {
                            d[i] = b[i];
                        }
                        else if (i == blength)
                        {
                            d[i] = '.';
                        }
                        else
                        {
                            d[i] = '0';
                        }
                    }
                }
                else
                {
                    if (alength - adot > blength - bdot)
                    {
                        d = new char[blength + ((alength - adot) - (blength - bdot))];
                        for (int i = 0; i < d.Length; i++)
                        {
                            if (i < blength)
                            {
                                d[i] = b[i];
                            }
                            else
                            {
                                d[i] = '0';
                            }
                        }
                    }
                    else
                    {
                        c = new char[alength + ((blength - bdot) - (alength - adot))];
                        for (int i = 0; i < c.Length; i++)
                        {
                            if (i < alength)
                            {
                                c[i] = a[i];
                            }
                            else
                            {
                                c[i] = '0';
                            }
                        }
                    }
                }
            }

            #endregion

            List<char> item = new List<char>();
            int cl = c.Length;
            int dl = d.Length;
            int r = 0;
            int jw = 0;//进位
            do
            {
                if (cl > 0 && dl > 0)
                {
                    if (c[cl - 1] == '.')
                    {
                        r = '.';
                    }
                    else
                    {
                        r = (int)c[cl - 1] + (int)d[dl - 1] - 96 + jw;
                        jw = 0;
                        if (r >= 10)
                        {
                            jw++;
                            r = r - 10;
                        }
                    }
                }
                else if (cl <= 0 && dl > 0)
                {
                    r = d[dl - 1] - 48 + jw;
                    jw = 0; if (r >= 10) { jw++; r = r - 10; }
                }
                else if (cl > 0 && dl <= 0)
                {
                    r = c[cl - 1] - 48 + jw; jw = 0; if (r >= 10) { jw++; r = r - 10; }
                }

                if (r == 46)
                {
                    item.Add(Convert.ToChar(r));
                }
                else
                {
                    item.Add(Convert.ToChar(r.ToString()));
                }
                cl--; dl--;
            } while (cl > 0 || dl > 0);
            if (jw == 1)
            {
                item.Add('1');
            }
            return item.ToArray();
        }

        public static char[] Subtraction(string a, string b)
        {
            BuweiF(ref a, ref b);
            List<char> item = new List<char>();
            int al = a.Length;
            int bl = b.Length;
            int r = 0;
            int jw = 0;//退位
            bool fushu = false;
            if (al > bl)
            {
                do
                {
                    if (al > 0 && bl > 0)
                    {
                        if (a[al - 1] == '.')
                        {
                            r = '.';
                        }
                        else
                        {
                            r = (int)a[al - 1] - (int)b[bl - 1] - jw;
                            jw = 0;
                            if (r < 0)
                            {
                                r = 10 + r;
                                jw++;
                            }
                        }
                    }
                    else if (al > 0 && bl <= 0)
                    {
                        r = a[al - 1] - 48 - jw;
                        jw = 0;
                        if (r < 0)
                        {
                            r = 10 + r;
                            jw++;
                        }
                    }

                    if (r == 46)
                    {
                        item.Add(Convert.ToChar(r));
                    }
                    else
                    {
                        item.Add(Convert.ToChar(r.ToString()));
                    }
                    al--; bl--;
                } while (al > 0 || bl > 0);
            }
            else if (al < bl)
            {
                fushu = true;
                do
                {
                    if (al > 0 && bl > 0)
                    {
                        if (b[bl - 1] == '.')
                        {
                            r = '.';
                        }
                        else
                        {
                            r = (int)b[bl - 1] - (int)a[al - 1] - jw;
                            jw = 0;
                            if (r < 0)
                            {
                                r = 10 + r;
                                jw++;
                            }
                        }
                    }
                    else if (bl > 0 && al <= 0)
                    {
                        r = b[bl - 1] - 48 - jw;
                        jw = 0;
                        if (r < 0)
                        {
                            r = 10 + r;
                            jw++;
                        }
                    }

                    if (r == 46)
                    {
                        item.Add(Convert.ToChar(r));
                    }
                    else
                    {
                        item.Add(Convert.ToChar(r.ToString()));
                    }
                    al--; bl--;
                } while (al > 0 || bl > 0);
            }
            else
            {
                if (a[0] > b[0])
                {
                    do
                    {
                        if (al > 0 && bl > 0)
                        {
                            if (a[al - 1] == '.')
                            {
                                r = '.';
                            }
                            else
                            {
                                r = (int)a[al - 1] - (int)b[bl - 1] - jw;
                                jw = 0;
                                if (r < 0)
                                {
                                    r = 10 + r;
                                    jw++;
                                }
                            }
                        }
                        else if (al > 0 && bl <= 0)
                        {
                            r = a[al - 1] - 48 - jw;
                            jw = 0;
                            if (r < 0)
                            {
                                r = 10 + r;
                                jw++;
                            }
                        }

                        if (r == 46)
                        {
                            item.Add(Convert.ToChar(r));
                        }
                        else
                        {
                            item.Add(Convert.ToChar(r.ToString()));
                        }
                        al--; bl--;
                    } while (al > 0 || bl > 0);
                }
                else
                {
                    fushu = true;
                    do
                    {
                        if (al > 0 && bl > 0)
                        {
                            if (b[bl - 1] == '.')
                            {
                                r = '.';
                            }
                            else
                            {
                                r = (int)b[bl - 1] - (int)a[al - 1] - jw;
                                jw = 0;
                                if (r < 0)
                                {
                                    r = 10 + r;
                                    jw++;
                                }
                            }
                        }
                        else if (bl > 0 && al <= 0)
                        {
                            r = b[bl - 1] - 48 - jw;
                            jw = 0;
                            if (r < 0)
                            {
                                r = 10 + r;
                                jw++;
                            }
                        }

                        if (r == 46)
                        {
                            item.Add(Convert.ToChar(r));
                        }
                        else
                        {
                            item.Add(Convert.ToChar(r.ToString()));
                        }
                        al--; bl--;
                    } while (al > 0 || bl > 0);
                }
            }
            if (item[item.Count() - 1] == '0' && item[item.Count() - 2] != '.')
            {
                item.RemoveAt(item.Count() - 1);
            }
            if (fushu)
            {
                item.Add('-');
            }
            return item.ToArray();
        }
        private static void BuweiF(ref string a, ref string b)
        {
            int al = a.Length;
            int bl = b.Length;
            string[] c = a.Split('.');
            string[] d = b.Split('.');
            int i = al - c[0].Length;
            int j = bl - d[0].Length;
            if (i != 0 && j != 0)
            {
                if (i > j)
                {
                    while (i != j)
                    {
                        b += "0";
                        j++;
                    }
                }
                else
                {
                    while (i != j)
                    {
                        a += "0";
                        i++;
                    }
                }
            }
            else if (i != 0 && j == 0)
            {
                b += ".";
                j++;
                while (i != j)
                {
                    b += "0";
                    j++;
                }
            }
            else if (i == 0 && j != 0)
            {
                a += ".";
                i++;
                while (i != j)
                {
                    a += "0";
                    i++;
                }
            }
            //Console.WriteLine(a);
            // Console.WriteLine(b);
        }

        public static char[] Quadrature(string a, string b)
        {
            char[] r = new char[1] { '0' };
            List<int> bu = new List<int>();
            List<int> au = new List<int>();
            int adot = a.Length;
            int bdot = b.Length;
            for (int i = b.Length - 1; i >= 0; i--)
            {
                if (b[i] == '.')
                {
                    bdot = i;
                }
                else
                {
                    bu.Add(b[i] - 48);
                }
            }
            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] == '.')
                {
                    adot = i;
                }
                else
                {
                    au.Add(a[i] - 48);
                }
            }
            a = "";
            for (int i = au.Count() - 1; i >= 0; i--)
            {
                a += au[i];
            }

            for (int i = 0; i < bu.Count(); i++)
            {
                char[] qq = QuadUnit(a, bu[i]);
                string qqq = "";
                for (int j = 1; j < i + 1; j++)
                {
                    qqq = "";
                    foreach (var item in qq)
                    {
                        qqq += item;
                    }
                    qq = QuadUnit(qqq, 10);
                }
                qqq = "";
                foreach (var item in qq)
                {
                    qqq += item;
                }
                string rr = "";
                foreach (var item in r)
                {
                    rr += item;
                }
                r = Plus(rr, qqq);

            }
            List<char> rer = new List<char>();
            for (int i = 0; i < r.Count(); i++)
            {
                rer.Add(r[i]);
            }
            int con = rer.Count();
            int dot = a.Length + b.Length - (adot + bdot);

            //Console.WriteLine(a);
            //Console.WriteLine(b);
            //Console.WriteLine(a.Length);
            //Console.WriteLine(b.Length);
            //Console.WriteLine(adot);
            //Console.WriteLine(bdot);
            //Console.WriteLine(con);
            //Console.WriteLine(dot);
            //Console.WriteLine(con-dot);

            if (con - dot > 0)
            {
                if (con - dot != con)
                {
                    rer.Insert(con - dot + 1, '.');
                }
            }
            else if (con - dot < 0)
            {
                rer.Insert(0, '.');
                rer.Insert(0, '0');

                int co = -1 * (con - dot) - 1;
                while (co > 0)
                {
                    rer.Insert(2, '0');
                    co--;
                }
            }
            else
            {
                rer.Insert(0, '.');
                rer.Insert(0, '0');
            }

            return rer.ToArray();
        }

        private static char[] QuadUnit(string a, int b)
        {
            int r = 0;
            int jw = 0;
            char[] c = a.ToCharArray();
            List<char> items = new List<char>();
            List<char> ritems = new List<char>();
            for (int i = c.Count() - 1; i >= 0; i--)
            {
                if (a[i] == '.')
                {
                    r = '.';
                    items.Add(Convert.ToChar(r));
                }
                else
                {
                    r = ((int)c[i] - 48) * b + jw;
                    jw = 0;
                    if (r >= 10)
                    {
                        jw = r / 10;
                        r %= 10;
                    }
                    items.Add(Convert.ToChar(r.ToString()));
                }

            }
            if (jw > 0)
            {
                items.Add(Convert.ToChar(jw.ToString()));
            }

            for (int i = (items.Count() - 1); i >= 0; i--)
            {
                ritems.Add(items[i]);
            }
            return ritems.ToArray();
        }

        public static char[] Exponentiation(string a, int r)
        {
            char[] aa = new char[1];
            string b = "";
            string c = a;
            for (int i = 0; i < r - 1; i++)
            {
                aa = Quadrature(c, a);
                b = "";
                foreach (var item in aa)
                {
                    b += item;
                }
                c = b;
            }
            return aa;
        }
    }
}
