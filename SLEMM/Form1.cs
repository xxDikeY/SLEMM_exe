using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLEMM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {



                string str1 = textBox1.Text;
                string str2 = textBox2.Text;
                string str3 = textBox3.Text;

                Frac det = new Frac(), detX = new Frac(), detY = new Frac(), detZ = new Frac(), x = new Frac(), y = new Frac(), z = new Frac();
                Frac[,] chMatrix = new Frac[3, 1];
                Frac[,] matrix = new Frac[3, 3];

                if (StrToNum(str1).Length != 4 || StrToNum(str2).Length != 4 || StrToNum(str3).Length != 4)
                {
                    MessageBox.Show("Введены больше или меньше 4 коэффициента");
                    return;
                }

                matrix[0, 0] = new Frac(StrToNum(str1)[0]);
                matrix[0, 1] = new Frac(StrToNum(str1)[1]);
                matrix[0, 2] = new Frac(StrToNum(str1)[2]);

                matrix[1, 0] = new Frac(StrToNum(str2)[0]);
                matrix[1, 1] = new Frac(StrToNum(str2)[1]);
                matrix[1, 2] = new Frac(StrToNum(str2)[2]);

                matrix[2, 0] = new Frac(StrToNum(str3)[0]);
                matrix[2, 1] = new Frac(StrToNum(str3)[1]);
                matrix[2, 2] = new Frac(StrToNum(str3)[2]);



                chMatrix[0, 0] = new Frac(StrToNum(str1)[3]);
                chMatrix[1, 0] = new Frac(StrToNum(str2)[3]);
                chMatrix[2, 0] = new Frac(StrToNum(str3)[3]);

                det = Frac.DeterMatrix(matrix);

                if (det.Get() == "0")
                {
                    textBox4.Text = "Определитель равен 0";
                    return;
                }

                detX = Frac.DeterMatrix(Frac.ChMatrix(Frac.NewMatrix(matrix), chMatrix, 0));
                detY = Frac.DeterMatrix(Frac.ChMatrix(Frac.NewMatrix(matrix), chMatrix, 1));
                detZ = Frac.DeterMatrix(Frac.ChMatrix(Frac.NewMatrix(matrix), chMatrix, 2));

                x = Frac.Div(detX, det);
                y = Frac.Div(detY, det);
                z = Frac.Div(detZ, det);

                string st = x.Get() + "; " + y.Get() + "; " + z.Get();

                textBox4.Text = st;
            }
            catch
            {
                MessageBox.Show("Неправильный ввод");
                return;
            }
        }

        static string[] StrToNum(string strNum)
        {
            if (strNum.IndexOf(' ') == -1)
            {

                string[] arr = new string[strNum.Length];

                for (int i = 0; i < strNum.Length; i++)
                    arr[i] = strNum[i].ToString();

                return arr;
            }

            else
            {
                if (strNum[0] == ' ')
                {
                    strNum = strNum.Remove(0, 1);
                }

                if (strNum[strNum.Length - 1] == ' ')
                {
                    strNum = strNum.Remove(strNum.Length - 1, 1);
                }

                int indx = 0, indSpace = 0;

                foreach (char c in strNum)
                {
                    if (c == ' ')
                        indx++;
                }

                string[] arr = new string[indx + 1];

                for (int i = 0; i < indx; i++)
                {
                    indSpace = strNum.IndexOf(' ');

                    arr[i] = strNum.Substring(0, indSpace);
                    strNum = strNum.Remove(0, indSpace + 1);
                }

                arr[arr.Length - 1] = strNum.Substring(0, strNum.Length);

                return arr;
            }
        }

        public sealed class Frac
        {
            internal double up, lw = 0;

            public Frac()
            {

            }
            public Frac(string num)
            {
                Set(num);
            }

            public void Set(string num)
            {
                bool chZn = false;
                int znIndex;

                znIndex = num.IndexOf("/");

                if (znIndex > 0)
                    chZn = true;

                if (chZn)
                {
                    up = Convert.ToDouble(num.Substring(0, znIndex));
                    lw = Convert.ToDouble(num.Substring(znIndex + 1, num.Length - 1 - znIndex));

                    while (true)
                    {
                        if (up % lw == 0)
                        {
                            up /= lw;
                            lw = 0;
                            return;
                        }

                        else if (up % 2 == 0 && lw % 2 == 0)
                        {
                            up /= 2;
                            lw /= 2;
                        }

                        else if (up % 3 == 0 && lw % 3 == 0)
                        {
                            up /= 3;
                            lw /= 3;
                        }

                        else if (up % 5 == 0 && lw % 5 == 0)
                        {
                            up /= 5;
                            lw /= 5;
                        }

                        else if (up % 7 == 0 && lw % 7 == 0)
                        {
                            up /= 7;
                            lw /= 7;
                        }

                        else if (up % 11 == 0 && lw % 11 == 0)
                        {
                            up /= 11;
                            lw /= 11;
                        }

                        else if (up % 13 == 0 && lw % 13 == 0)
                        {
                            up /= 13;
                            lw /= 13;
                        }

                        else
                        {
                            return;
                        }
                    }
                }

                else
                {
                    up = Convert.ToDouble(num);
                }
            }
            public string Get()
            {
                if (lw != 0)
                {
                    return $"{up}/{lw}";
                }
                else
                {
                    return up.ToString();
                }
            }

            public static Frac Multi(Frac fr1, Frac fr2)
            {
                double up1 = fr1.up, up2 = fr2.up, lw1 = fr1.lw, lw2 = fr2.lw;
                Frac fr = new Frac();

                if (lw1 != 0 && lw2 != 0)
                {
                    up1 *= up2;
                    lw1 *= lw2;

                    fr.Set($"{up1}/{lw1}");
                    return fr;
                }

                else if (lw1 != 0 && lw2 == 0)
                {
                    up1 *= up2;

                    fr.Set($"{up1}/{lw1}");
                    return fr;
                }

                else if (lw1 == 0 && lw2 != 0)
                {
                    up1 *= up2;

                    fr.Set($"{up1}/{lw2}");
                    return fr;
                }

                else
                {
                    up1 *= up2;

                    fr.Set(up1.ToString());
                    return fr;
                }
            }

            public static Frac Div(Frac fr1, Frac fr2)
            {
                double up1 = fr1.up, up2 = fr2.up, lw1 = fr1.lw, lw2 = fr2.lw;
                Frac fr = new Frac();

                if (lw1 == 0 && lw2 == 0)
                {
                    if (lw1 % lw2 == 0)
                    {
                        up1 /= up2;

                        fr.Set(up1.ToString());
                        return fr;
                    }

                    else
                    {
                        fr.Set($"{up1}/{up2}");
                        return fr;
                    }
                }

                else if (lw1 != 0 && lw2 == 0)
                {
                    lw1 *= up2;

                    fr.Set($"{up1}/{lw1}");
                    return fr;
                }

                else if (lw1 == 0 && lw2 != 0)
                {
                    lw2 *= up1;


                    fr.Set($"{lw2}/{up2}");
                    return fr;
                }

                else
                {
                    up1 *= lw2;
                    lw1 *= up2;

                    fr.Set($"{up1}/{lw1}");
                    return fr;
                }
            }

            public static Frac Sum(Frac fr1, Frac fr2)
            {
                double up1 = fr1.up, up2 = fr2.up, lw1 = fr1.lw, lw2 = fr2.lw;
                Frac fr = new Frac();

                if (lw1 == 0 && lw2 == 0)
                {
                    up1 += up2;

                    fr.Set(up1.ToString());
                    return fr;
                }

                else if (lw1 == 0 && lw2 != 0)
                {
                    up1 *= lw2;
                    up1 += up2;

                    fr.Set($"{up1}/{lw2}");
                    return fr;
                }

                else if (lw1 != 0 && lw2 == 0)
                {
                    up2 *= lw1;
                    up1 += up2;

                    fr.Set($"{up1}/{lw1}");
                    return fr;
                }

                else
                {
                    if (lw1 == lw2)
                    {
                        up1 += up2;


                        fr.Set($"{up1}/{lw1}");
                        return fr;
                    }

                    else
                    {
                        up1 *= lw2;
                        up2 *= lw1;
                        lw1 *= lw2;

                        up1 += up2;

                        fr.Set($"{up1}/{lw1}");
                        return fr;

                    }
                }
            }

            public static Frac Sub(Frac fr1, Frac fr2)
            {

                double up1 = fr1.up, up2 = fr2.up, lw1 = fr1.lw, lw2 = fr2.lw;
                Frac fr = new Frac();

                if (lw1 == 0 && lw2 == 0)
                {
                    up1 -= up2;

                    fr.Set(up1.ToString());
                    return fr;
                }

                else if (lw1 == 0 && lw2 != 0)
                {
                    up1 *= lw2;
                    up1 -= up2;

                    fr.Set($"{up1}/{lw2}");
                    return fr;
                }

                else if (lw1 != 0 && lw2 == 0)
                {
                    up2 *= lw1;
                    up1 -= up2;

                    fr.Set($"{up1}/{lw1}");
                    return fr;
                }

                else
                {
                    if (lw1 == lw2)
                    {
                        up1 -= up2;

                        fr.Set($"{up1}/{lw1}");
                        return fr;
                    }

                    else
                    {
                        up1 *= lw2;
                        up2 *= lw1;
                        lw1 *= lw2;

                        up1 -= up2;

                        fr.Set($"{up1}/{lw1}");
                        return fr;

                    }
                }

            }

            public static Frac DeterMatrix(Frac[,] matrix)
            {
                Frac fr = new Frac();

                fr = Frac.Sub(Frac.Sub(Frac.Sub(Frac.Sum(Frac.Sum(

                    Frac.Multi(Frac.Multi(matrix[0, 0], matrix[1, 1]), matrix[2, 2]),
                    Frac.Multi(Frac.Multi(matrix[0, 1], matrix[1, 2]), matrix[2, 0])),
                    Frac.Multi(Frac.Multi(matrix[0, 2], matrix[1, 0]), matrix[2, 1])),

                    Frac.Multi(Frac.Multi(matrix[0, 2], matrix[1, 1]), matrix[2, 0])),
                    Frac.Multi(Frac.Multi(matrix[0, 0], matrix[1, 2]), matrix[2, 1])),
                    Frac.Multi(Frac.Multi(matrix[0, 1], matrix[1, 0]), matrix[2, 2]));

                return fr;
            }

            public static Frac[,] NewMatrix(Frac[,] matrix)
            {
                Frac[,] newMatrix = new Frac[3, 3];

                int rows = matrix.GetUpperBound(0) + 1;
                int columns = matrix.Length / rows;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        newMatrix[i, j] = matrix[i, j];
                    }
                }

                return newMatrix;
            }

            public static Frac[,] ChMatrix(Frac[,] matrix, Frac[,] chMatrix, int column)
            {
                if (column >= 0 && column <= 2)
                {
                    matrix[0, column] = chMatrix[0, 0];
                    matrix[1, column] = chMatrix[1, 0];
                    matrix[2, column] = chMatrix[2, 0];

                    return matrix;
                }

                else
                {
                    return matrix;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сделал Качурин Ярослав");
        }
    }
}
