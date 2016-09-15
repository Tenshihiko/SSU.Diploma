using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SSU.Prediploma.GraphDrawer
{
    public class DrawMyGraph
    {
        public Bitmap picture;
        private int[] DegreeSet;
        private Graphics fig;
        private int GraphType;

        static private int m = 10;
        private int R = 50 * m;
        private int h;
        private int hR;
        private int hd;
        private int r = 10 * m;
        private int d = 20 * m;
        private int ddd = 5 * m;
        private int CountX;
        private int CountY;

        private Pen VertexPen = new Pen(Color.Black, m);
        private Pen ListPen = new Pen(Color.DarkKhaki, m);
        private Pen EdgePen = new Pen(Color.DarkGray, m - 2);
        private Brush TextBrush = Brushes.Red;

        private void DrawVertex(int x, int y, string title)
        {
            Rectangle rect = new Rectangle(x - r, y - r, 2 * r, 2 * r);
            fig.FillEllipse(Brushes.WhiteSmoke, rect);

            fig.DrawEllipse((title.Equals(DegreeSet[0].ToString()) ? ListPen : VertexPen), rect);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Font font = new Font("Arial", 90);
            fig.DrawString(title, font, TextBrush, rect, format);
            fig.Flush();
        }

        private void DrawOuterPlanarEvenStar(int x, int y, int n)
        {
            if (n % 2 != 0)
                throw new Exception("Попытка построить четную звезду с нечетным количеством листов");

            double angle = 2 * Math.PI / n;
            for (int i = 0; i < n / 2; i++)
            {
                int x1 = x + (int)(R * Math.Cos(angle * 2 * i));
                int y1 = y + (int)(R * Math.Sin(angle * 2 * i));
                int x2 = x + (int)(R * Math.Cos(angle * (2 * i + 1)));
                int y2 = y + (int)(R * Math.Sin(angle * (2 * i + 1)));
                fig.DrawLine(EdgePen, x1, y1, x2, y2);
            }
            DrawStarType0(x, y, n);
        }

        private void DrawOuterPlanarOddStar(int x, int y, int n, int direction)
        {
            if (n % 2 != 1)
                throw new Exception("Попытка построить нечетную звезду с четным количеством листов");
            double dangle = 2 * Math.PI / n;
            double angle = direction * (0.5 * Math.PI);
            int dd = d + 2 * R + 2 * r;
            for (int i = 1; i <= n / 2; i++)
            {
                int dx1 = (int)(R * Math.Cos(angle + dangle * 2 * i));
                int dy1 = - (int)(R * Math.Sin(angle + dangle * 2 * i));
                int dx2 = (int)(R*Math.Cos(angle+dangle * (2 * i - 1)));
                int dy2 = - (int)(R*Math.Sin(angle+dangle*(2 * i - 1)));
                fig.DrawLine(EdgePen, x+dx1, y + dy1, x + dx2, y + dy2);                
            }
            DrawStarType1(x, y, n, direction);            
        }

        private void DrawStarType0(int x, int y, int n)
        {
            double angle = 2 * Math.PI / n;
            for (int i = 0; i < n; i++)
            {
                int newX = x + (int)(R * Math.Cos(angle * i));
                int newY = y - (int)(R * Math.Sin(angle * i));
                fig.DrawLine(EdgePen, x, y, newX, newY);
                DrawVertex(newX, newY, GraphType == 3 ? "2" : "1");
            }
            DrawVertex(x, y, n.ToString());
        }

        private void DrawStarType1(int x, int y, int n, int direction)
        {
            /* direction: 0 = right; 1 = up; 2 = left; 3 = down; */
            double start = Math.PI * 0.5 * direction;
            double dangle = Math.PI * 2 / n;

            for (int i = 1; i < n; i++)
            {
                int x1 = x + (int)(R * Math.Cos(start + dangle * i));
                int y1 = y - (int)(R * Math.Sin(start + dangle * i));
                fig.DrawLine(EdgePen, x, y, x1, y1);
                DrawVertex(x1, y1, GraphType == 3 ? "2" : "1");
            }
            DrawVertex(x, y, n.ToString());
        }

        private void DrawStarType2(int x, int y, int n, int direction)
        {
            /* direction:
             * 0 = right and up
             * 1 = up and left
             * 2 = left and down
             * 3 = down and right
             * 4 = left and right
             */

            double startA = direction == 4 ? 0 : (Math.PI * 0.5 * direction);
            double startB = direction == 4 ? Math.PI : (Math.PI * 0.5 * (direction + 1));

            int countA = direction == 4 ? ((n - 2) / 2) : ((n - 1) / 4 + ((n - 1) % 4 >= 2 ? 1 : 0));        //inside
            int countB = direction == 4 ? (countA + (n - 2) % 2) : (n - 1 - countA);

            double dA =(direction == 4 ? 1 : 0.5)*Math.PI/(countA + 1);
            double dB =(direction == 4 ? 1 : 1.5)*Math.PI/(countB + 1);

            for (int i = 1; i <= countA; i++)
            {
                int x1 = x + (int)(R * Math.Cos(startA + dA * i));
                int y1 = y - (int)(R * Math.Sin(startA + dA * i));
                fig.DrawLine(EdgePen, x, y, x1, y1);
                DrawVertex(x1, y1, GraphType == 3 ? "2" : "1");
            }

            for (int i = 1; i <= countB; i++)
            {
                int x1 = x + (int)(R * Math.Cos(startB + dB * i));
                int y1 = y - (int)(R * Math.Sin(startB + dB * i));
                fig.DrawLine(EdgePen, x, y, x1, y1);
                DrawVertex(x1, y1, GraphType == 3 ? "2" : "1");
            }

            DrawVertex(x, y, n.ToString());
        }

        private void DrawTree()
        {           
            CreateBitmapForTree();
            int x = r + R + d; int y = r + R + d;
            int dd = 2 * r + 2 * R + d;

            if (DegreeSet.Length == 2)
            {
                DrawStarType0(x, y, DegreeSet[1]);
                return;
            }

            if (DegreeSet.Length == 1)
            {
                fig.DrawLine(EdgePen, x, y, x + dd, y);
                DrawVertex(x, y, "1");
                DrawVertex(x + dd, y, "1");
                return;
            }

            bool toRight = true;
            int lineLength=0; int currentType=1; int directionType=0;

            for (int i = 1; i < DegreeSet.Length; i++)
            {
                lineLength++;

                switch (directionType)
                {
                    case 0 :
                        currentType = 1;
                        break;
                    case 1 :
                        if (i == DegreeSet.Length - 1)
                        {
                            currentType = 3;
                        }
                        else
                        {
                            if (lineLength == CountX)
                            {
                                lineLength = 0;
                                toRight = !toRight;
                                currentType = 7;
                            }
                            else
                            {
                                currentType = 9;
                            }
                        }
                        break;
                    case 2 :
                        if (i == DegreeSet.Length - 1)
                        {
                            currentType = 2;
                        }
                        else
                        {
                            if (toRight)
                            {
                                currentType = 5;
                            }
                            else
                            {
                                currentType = 6;
                            }
                        }
                        break;
                    case 3 :
                        if (i == DegreeSet.Length - 1)
                        {
                            currentType = 1;
                        }
                        else
                        {
                            if (lineLength == CountX)
                            {
                                lineLength = 0;
                                currentType = 8;
                                toRight = !toRight;
                            }
                            else
                            {
                                currentType = 9;
                            }
                        }
                        break;
                    default:
                        throw new Exception("Некорректный код направления в DrawMyGraph.DrawTree()");
                }

                switch (currentType)
                {
                    case 0 :
                        break;
                    case 1 :
                        if (directionType == 0)
                        {
                            fig.DrawLine(EdgePen, x, y, x + dd, y);
                        }
                        DrawStarType1(x, y, DegreeSet[i], 0);
                        directionType = 1;
                        x += dd;
                        break;
                    case 2:                        
                        DrawStarType1(x, y, DegreeSet[i], 1);
                        directionType = 0;
                        break;
                    case 3:
                        DrawStarType1(x, y, DegreeSet[i], 2);
                        directionType = 0;
                        break;
                    case 5:                        
                        fig.DrawLine(EdgePen, x, y, x + dd, y);
                        DrawStarType2(x, y, DegreeSet[i], 0);
                        directionType = 1;
                        x += dd;
                        break;
                    case 6:
                        fig.DrawLine(EdgePen, x, y, x - dd, y);
                        DrawStarType2(x, y, DegreeSet[i], 1);
                        directionType = 3;
                        x -= dd;
                        break;
                    case 7:
                        fig.DrawLine(EdgePen, x, y, x, y + dd);
                        DrawStarType2(x, y, DegreeSet[i], 2);
                        directionType = 2;
                        y += dd;
                        break;
                    case 8:
                        fig.DrawLine(EdgePen, x, y, x, y + dd);
                        DrawStarType2(x, y, DegreeSet[i], 3);
                        directionType = 2;
                        y += dd;
                        break;
                    case 9:
                        fig.DrawLine(EdgePen, x, y, toRight ? x + dd : x - dd, y);
                        DrawStarType2(x, y, DegreeSet[i], 4);
                        x = toRight ? x + dd : x - dd;
                        break;
                    default :
                        throw new Exception("Некорректный тип звезды в DrawMyGraph.DrawTree():" + currentType);
                }
            }
        }

        private int CheckSet(ref int[] set)
        {
            int fl = 0;
            Array.Sort(set);
            SortedSet<int> tempset = new SortedSet<int>(set);
            bool flfl = false;
            if (tempset.Count != set.Length)
                fl += 2;
            while (tempset.Min <= 0)
            {
                tempset.Remove(tempset.Min);
                flfl = true;
            }
            if (flfl)
                fl += 1;
           
            if (GraphType == 1){
                if (!tempset.Contains(1))
                {
                    tempset.Add(1);
                    fl += 4;
                }
            }
            if (GraphType == 2){
                if (tempset.Min() > 5)
                {
                    tempset.Add(3);
                    fl += 8;
                }
            }
            if (GraphType == 3){
                if (tempset.Min() > 2)
                {
                    tempset.Add(2);
                    fl += 16;
                }
            }
            set = tempset.ToArray();
            
            return fl;
        }

        private void CreateBitmapForTree()
        {
            if (DegreeSet.Length - 1 <= 5)
            {
                CountX = DegreeSet.Length - 1;
                CountY = 1;
            }
            else
            {
                CountX = (int)Math.Ceiling(Math.Sqrt(3 * (DegreeSet.Length - 1) / 2));
                CountY = (int)Math.Ceiling((DegreeSet.Length - 1) * 1.0 / CountX);
            }
            if (DegreeSet.Length == 1)
                CountX = 2;
            int X = CountX * 2 * (R + r) + (CountX + 1) * d;
            int Y = CountY * 2 * (R + r) + (CountY + 1) * d;
            picture = new Bitmap(X, Y);
            fig = Graphics.FromImage(picture);
            fig.FillRectangle(Brushes.White, 0, 0, picture.Width, picture.Height);
        }

        public DrawMyGraph(int[] set, int type, out int fl)
        {
            GraphType = type;            
            fl = CheckSet(ref set);
            DegreeSet = new int[set.Length];
            Array.Copy(set, DegreeSet, set.Length);

            if ((type == 1 || type == 3) && DegreeSet[DegreeSet.Length - 1] > 14)
            {
                this.R = (int)((2 * r + ddd) * 1.0 / Math.Tan(2 * Math.PI / DegreeSet[DegreeSet.Length - 1]));
            }

            if (type == 2)
            {                
                switch (DegreeSet[0])
                {
                    case 2 : hR = R; break;
                    case 3 : hR = 2 * R; break;
                    case 4 : hR = 3 * R; break;
                    case 5 : hR = 4 * R; break;
                    default : break;
                }
                this.hd = (DegreeSet[0] == 2) ? d + r + R : (int)(hR * Math.Sin(Math.PI / 3));
                this.hd += (DegreeSet[0] == 5) ? d : 0;
                h = (int)(hR * 1.0 * Math.Sin(Math.PI / 3.0));
            }

            switch (GraphType)
            {
                case 1: DrawTree(); break;
                case 3: DrawOuterplanar(); break;
                case 2: DrawPlanar(); break;
                default: throw new Exception("Incorrect graph type.");
            }
        }

        private void DrawPlanar()
        {
            if (DegreeSet[0] == 1)
            {
                GraphType = 1; DrawTree(); return;
            }
            CreateBitmapForPlanar();

            if (DegreeSet.Length == 1 && DegreeSet[0] == 2)
            {
                int h = (int)(hR * Math.Sin(Math.PI / 3));
                int x1 = picture.Width / 2 - hR / 2;
                int y1 = picture.Height / 2 - h / 2;

                int x2 = picture.Width / 2 + hR / 2;
                int y2 = picture.Height / 2 - h / 2;

                int x3 = picture.Width / 2;
                int y3 = picture.Height / 2 + h / 2;

                fig.DrawLine(EdgePen, x1, y1, x2, y2);
                fig.DrawLine(EdgePen, x3, y3, x2, y2);
                fig.DrawLine(EdgePen, x1, y1, x3, y3);

                DrawVertex(x1, y1, "2");
                DrawVertex(x2, y2, "2");
                DrawVertex(x3, y3, "2");
                return;
            }

            if (DegreeSet.Length == 1 && DegreeSet[0] == 3)
            {
                int h = (int)(hR * Math.Sin(Math.PI / 3));
                int x = picture.Width / 2 + h / 2;
                int y = picture.Height / 2 - hR / 2;
                fig.DrawLine(EdgePen, x, y, x, y + hR);
                DrawTetrahedron(x, y);
                return;
            }

            if (DegreeSet.Length == 1 && DegreeSet[0] == 4)
            {
                int h = (int)(hR * Math.Sin(Math.PI / 3));
                int x = picture.Width / 2 + h / 2;
                int y = picture.Height / 2 - hR / 2;
                fig.DrawLine(EdgePen, x, y, x, y + hR);
                DrawOctahedron(x, y);
                return;
            }

            if (DegreeSet.Length == 1 && DegreeSet[0] == 5)
            {
                int h = (int)(hR * Math.Sin(Math.PI / 3));
                int x = picture.Width / 2 + h / 2;
                int y = picture.Height / 2 - hR / 2;
                fig.DrawLine(EdgePen, x, y, x, y + hR);
                DrawIcosahedron(x, y);
                return;
            }


            int xl = d + r + hd;
            int yl = d + r + hR;
            for (int i = 1; i < DegreeSet.Length; i++)
            {
                int l; int x; int y; int n = DegreeSet[i];


                if (i == 1 && i != DegreeSet.Length - 1)
                {
                    n -= 1;
                    l = (n - 1) * hd;
                    x = (DegreeSet[0] == 2) ? (xl - hd / 2 + l / 2) : (xl + l / 2);
                    y = d + r;
                    l += hd;
                    fig.DrawLine(EdgePen, x, y, x + l / 2 + hd, y);
                    fig.DrawLine(EdgePen, x, y + 3 * hR, x + l / 2 + hd, y + 3 * hR);
                    goto LIST;
                }
                if (i != 1 && i != DegreeSet.Length - 1)
                {
                    n -= 2;
                    l = (n - 1) * hd;
                    x = (DegreeSet[0] == 2) ? (xl - hd / 2 + l / 2) : (xl + l / 2);
                    y = d + r;
                    fig.DrawLine(EdgePen, x - l / 2 - hd, y, x + l / 2 + hd, y);
                    fig.DrawLine(EdgePen, x - l / 2 - hd, y + 3 * hR, x + l / 2 + hd, y + 3 * hR);
                    goto LIST;
                }
                if (i != 1 && i == DegreeSet.Length - 1)
                {
                    n -= 1;
                    l = (n - 1) * hd;
                    x = (DegreeSet[0] == 2) ? (xl - hd / 2 + l / 2) : (xl + l / 2);
                    y = d + r;
                    fig.DrawLine(EdgePen, x - l / 2 - hd, y, x, y);
                    fig.DrawLine(EdgePen, x - l / 2 - hd, y + 3 * hR, x, y + 3 * hR);
                    goto LIST;
                }
                {
                    l = (n - 1) * hd;
                    x = (DegreeSet[0] == 2) ? (xl - hd / 2 + l / 2) : (xl + l / 2);
                    y = d + r;
                }
            LIST:
                for (int j = 0; j < n; j++)
                {
                    switch (DegreeSet[0])
                    {
                        case 2:
                            fig.DrawLine(EdgePen, xl - hd / 2, yl, x, y);
                            fig.DrawLine(EdgePen, xl - hd / 2, yl + hR, x, y + 3 * hR);
                            fig.DrawLine(EdgePen, xl - hd / 2, yl, xl - hd / 2, yl + hR);
                            DrawVertex(xl - hd / 2, yl, "2");
                            DrawVertex(xl - hd / 2, yl + hR, "2");
                            break;
                        case 3:
                            fig.DrawLine(EdgePen, xl, yl, x, y);
                            fig.DrawLine(EdgePen, xl, yl + hR, x, y + 3 * hR);
                            DrawTetrahedron(xl, yl);
                            break;
                        case 4:
                            fig.DrawLine(EdgePen, xl, yl, x, y);
                            fig.DrawLine(EdgePen, xl, yl + hR, x, y + 3 * hR);
                            DrawOctahedron(xl, yl);
                            break;
                        case 5:
                            fig.DrawLine(EdgePen, xl, yl, x, y);
                            fig.DrawLine(EdgePen, xl, yl + hR, x, y + 3 * hR);
                            DrawIcosahedron(xl, yl);
                            break;
                        default:
                            throw new Exception("Некорректный тип полиэдра в DrawMyGraph.DrawPlanar()");
                    }
                    xl += hd;
                }
                DrawVertex(x, y, DegreeSet[i].ToString());
                DrawVertex(x, y + 3 * hR, DegreeSet[i].ToString());
            }
        }        

        private void DrawIcosahedron(int x0, int y0)
        {   
            int x1 = x0;
            int y1 = y0 + hR;

            int x2 = x0 - h;
            int y2 = y0 + hR / 2;

            double lambda1 = 9.0 / 11; //2.0 / 3;

            int x12 = (int)((x2 + x0 * lambda1) / (lambda1 + 1));
            int y12 = y2;

            int x10 = (int)((x0 + (x1 + x2) * 1.0 / 2 * lambda1) / (lambda1 + 1));
            int y10 = (int)((y0 + (y1 + y2) * 1.0 / 2 * lambda1) / (lambda1 + 1));

            int x11 = (int)((x1 + (x0 + x2) * 1.0 / 2 * lambda1) / (lambda1 + 1));
            int y11 = (int)((y1 + (y0 + y2) * 1.0 / 2 * lambda1) / (lambda1 + 1));

            int x32 = (int)((x11 + x10) / 2);
            int y32 = (int)((y11 + y10) / 2);

            int x30 = (int)((x11 + x12) / 2);
            int y30 = (int)((y11 + y12) / 2);

            int x31 = (int)((x12 + x10) / 2);
            int y31 = (int)((y12 + y10) / 2);

            double lambda2 = 12.0 / 1; //2.0 / 3;

            int x22 = (int)((x2 + x0 * lambda2) / (lambda2 + 1));
            int y22 = y2;

            int x20 = (int)((x0 + (x1 + x2) * 1.0 / 2 * lambda2) / (lambda2 + 1));
            int y20 = (int)((y0 + (y1 + y2) * 1.0 / 2 * lambda2) / (lambda2 + 1));

            int x21 = (int)((x1 + (x0 + x2) * 1.0 / 2 * lambda2) / (lambda2 + 1));
            int y21 = (int)((y1 + (y0 + y2) * 1.0 / 2 * lambda2) / (lambda2 + 1));

            fig.DrawLine(EdgePen, x0, y0, x2, y2);
            fig.DrawLine(EdgePen, x2, y2, x1, y1);

            fig.DrawLine(EdgePen, x0, y0, x10, y10);
            fig.DrawLine(EdgePen, x1, y1, x11, y11);
            fig.DrawLine(EdgePen, x2, y2, x12, y12);

            fig.DrawLine(EdgePen, x10, y10, x32, y32);
            fig.DrawLine(EdgePen, x12, y12, x31, y31);
            fig.DrawLine(EdgePen, x10, y10, x31, y31);
            fig.DrawLine(EdgePen, x11, y11, x30, y30);
            fig.DrawLine(EdgePen, x12, y12, x30, y30);
            fig.DrawLine(EdgePen, x11, y11, x32, y32);

            fig.DrawLine(EdgePen, x20, y20, x30, y30);
            fig.DrawLine(EdgePen, x21, y21, x31, y31);
            fig.DrawLine(EdgePen, x22, y22, x32, y32);

            fig.DrawLine(EdgePen, x30, y30, x31, y31);
            fig.DrawLine(EdgePen, x32, y32, x31, y31);
            fig.DrawLine(EdgePen, x30, y30, x32, y32);

            fig.DrawLine(EdgePen, x0, y0, x22, y22);
            fig.DrawLine(EdgePen, x1, y1, x22, y22);
            fig.DrawLine(EdgePen, x2, y2, x20, y20);
            fig.DrawLine(EdgePen, x1, y1, x20, y20);
            fig.DrawLine(EdgePen, x0, y0, x21, y21);
            fig.DrawLine(EdgePen, x2, y2, x21, y21);

            DrawVertex(x0, y0, "5");
            DrawVertex(x1, y1, "5");
            DrawVertex(x2, y2, "5");
            DrawVertex(x10, y10, "5");
            DrawVertex(x11, y11, "5");
            DrawVertex(x12, y12, "5");
            DrawVertex(x30, y30, "5");
            DrawVertex(x31, y31, "5");
            DrawVertex(x32, y32, "5");
            DrawVertex(x20, y20, "5");
            DrawVertex(x21, y21, "5");
            DrawVertex(x22, y22, "5");
        }

        private void DrawOctahedron(int x0, int y0)
        {
            int x1 = x0; int y1 = y0 + hR;
            int x2 = x0 - h; int y2 = y0 + hR / 2;

            double lambda = 2.0 / 3;

            int x12 = (int)((x2 + x0 * lambda) / (lambda + 1)); int y12 = y2;

            int x10 = (int)((x0 + (x1 + x2) * 1.0 / 2 * lambda) / (lambda + 1));
            int y10 = (int)((y0 + (y1 + y2) * 1.0 / 2 * lambda) / (lambda + 1));

            int x11 = (int)((x1 + (x0 + x2) * 1.0 / 2 * lambda) / (lambda + 1));
            int y11 = (int)((y1 + (y0 + y2) * 1.0 / 2 * lambda) / (lambda + 1));

            fig.DrawLine(EdgePen, x0, y0, x2, y2);
            fig.DrawLine(EdgePen, x2, y2, x1, y1);

            fig.DrawLine(EdgePen, x0, y0, x10, y10);
            fig.DrawLine(EdgePen, x1, y1, x11, y11);
            fig.DrawLine(EdgePen, x2, y2, x12, y12);

            fig.DrawLine(EdgePen, x10, y10, x11, y11);
            fig.DrawLine(EdgePen, x12, y12, x11, y11);
            fig.DrawLine(EdgePen, x10, y10, x12, y12);

            DrawVertex(x0, y0, "4");
            DrawVertex(x1, y1, "4");
            DrawVertex(x2, y2, "4");
            DrawVertex(x10, y10, "4");
            DrawVertex(x11, y11, "4");
            DrawVertex(x12, y12, "4");
        }

        private void DrawTetrahedron(int x, int y)
        {
            int x1 = x; int y1 = y + hR;
            int x2 = x - h; int y2 = y + hR / 2;
            int x3 = x - h / 3; int y3 = y + hR / 2;

            fig.DrawLine(EdgePen, x, y, x2, y2);
            fig.DrawLine(EdgePen, x2, y2, x1, y1);

            fig.DrawLine(EdgePen, x, y, x3, y3);
            fig.DrawLine(EdgePen, x1, y1, x3, y3);
            fig.DrawLine(EdgePen, x2, y2, x3, y3);

            DrawVertex(x, y, "3");
            DrawVertex(x1, y1, "3");
            DrawVertex(x2, y2, "3");
            DrawVertex(x3, y3, "3");
        }

        private void CreateBitmapForPlanar()
        {
            int count = 0;
            for (int i = 1; i < DegreeSet.Length; i++)
            {
                int k = DegreeSet[i];
                if (i == 1 && i != DegreeSet.Length - 1)
                    k--;
                if (i != 1 && i != DegreeSet.Length - 1)
                    k -= 2;
                if (i != 1 && i == DegreeSet.Length - 1)
                    k--;
                count += k;
            }
            int x = count == 0 ? (2 * d + hR) : (2 * d + 2 * r + count * hd);
            int y = count == 0 ? (2 * d + hR) : (2 * d + 2 * r + 3 * hR);
            picture = new Bitmap(x, y);
            fig = Graphics.FromImage(picture);
            fig.FillRectangle(Brushes.White, 0, 0, picture.Width, picture.Height);
        }

        private void DrawOuterplanar()
        {            
            if (DegreeSet[0] == 1)
            {
                GraphType = 1;
                DrawTree();
                return;
            }

            CreateBitmapForOuterplanar();

            if (DegreeSet.Length == 1)
            {
                int h = (int)(2 * R * Math.Sin(Math.PI / 3));
                int x1 = picture.Width / 2 - R;
                int y1 = picture.Height / 2 - h / 2;

                int x2 = picture.Width / 2 + R;
                int y2 = picture.Height / 2 - h / 2;

                int x3 = picture.Width / 2;
                int y3 = picture.Height / 2 + h / 2;

                fig.DrawLine(EdgePen, x1, y1, x2, y2);
                fig.DrawLine(EdgePen, x3, y3, x2, y2);
                fig.DrawLine(EdgePen, x1, y1, x3, y3);

                DrawVertex(x1, y1, "2");
                DrawVertex(x2, y2, "2");
                DrawVertex(x3, y3, "2");
            }

            int x = r + R + d;
            int y = r + R + d;
            int dd = 2 * r + 2 * R + d;

            bool toRight = true;
            int lineLength = 0;

            for (int i = 1; i < DegreeSet.Length; i++)
            {
                if (DegreeSet[i] % 2 == 0)
                {
                    lineLength++;
                    DrawOuterPlanarEvenStar(x, y, DegreeSet[i]);
                    if (lineLength < CountX)
                    {
                        x += toRight ? dd : -dd;
                    }
                    else
                    {
                        lineLength = 0;
                        toRight = !toRight;
                        y += dd;
                    }
                }
                else
                {
                    if (lineLength + 2 <= CountX)//горизонтальная звезда
                    {
                        fig.DrawLine(EdgePen, x, y, toRight ? (x + dd) : (x - dd), y);
                        lineLength += 2;
                        DrawOuterPlanarOddStar(x, y, DegreeSet[i], toRight ? 0 : 2);
                        x = toRight ? x + dd : x - dd;
                        DrawOuterPlanarOddStar(x, y, DegreeSet[i], toRight ? 2 : 0);

                        if (lineLength == CountX)
                        {
                            lineLength = 0;
                            toRight = !toRight;
                            y += dd;
                        }
                        else
                        {
                            x = toRight ? x + dd : x - dd;
                        }
                    }
                    else
                    {
                        fig.DrawLine(EdgePen, x, y, x, y + dd);
                        lineLength = 1;
                        DrawOuterPlanarOddStar(x, y, DegreeSet[i], 3);
                        y += dd;
                        DrawOuterPlanarOddStar(x, y, DegreeSet[i], 1);
                        x = toRight ? (x - dd) : (x + dd);                       
                        toRight = !toRight;
                    }
                }
            }
        }

        private void CreateBitmapForOuterplanar()
        {
            int Count = 0;

            for (int i = 1; i < DegreeSet.Length; i++)
            {
                Count += DegreeSet[i] % 2 + 1;
            }

            if (Count <= 5)
            {
                CountX = Count; CountY = 1;
            }
            else
            {
                CountX = (int)Math.Ceiling(Math.Sqrt(3.0 * (Count) / 2.0));
                CountY = (int)Math.Ceiling((Count) * 1.0 / CountX);
            }

            if (Count == 0)
                CountX = (DegreeSet[0] == 2) ? 1 : 2;
            int X = CountX * 2 * (R + r) + (CountX + 1) * d;
            int Y = CountY * 2 * (R + r) + (CountY + 1) * d;
            picture = new Bitmap(X, Y);
            fig = Graphics.FromImage(picture);
            fig.FillRectangle(Brushes.White, 0, 0, picture.Width, picture.Height);
        }
    }
}
