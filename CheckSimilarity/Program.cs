using System;
using System.Numerics;
using System.Globalization;
using System.IO;

namespace CheckSimilarity
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "";
            string name = "";
            string file = "";
            bool inputBad = true;
            Figure[] figures = new Figure[2];
            while (inputBad)
            {
                try
                {
                    Console.Write("Path to figures:");
                    path = Console.ReadLine(); //"F:/WORK/NextIT/INFO/OneDrive_2022-05-13";//Console.ReadLine();
                    if (Directory.Exists(path))
                    {
                        Directory.SetCurrentDirectory(path);
                        Console.WriteLine(" ");
                        inputBad = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Directory");
                        Console.WriteLine(" ");
                    }
                    
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Missing Directory");
                    Console.WriteLine(" ");
                }
            }

            
            for (int n = 0; n < 2; n++)
            {
                inputBad = true;
                while (inputBad)
                {
                    try
                    {
                        Console.WriteLine(" ");
                        Console.Write("Figure " + (n + 1) + " file name: ");
                        name = Console.ReadLine();
                        file = name + ".txt";
                        figures[n] = Figure.AddFigure(file);
                        figures[n].index = n + 1;
                        figures[n].CenterToOrigin();
                        if (Validate(figures[n]))
                        {
                            figures[n].Flatten();
                            figures[n].UnifyNormals();
                            Console.WriteLine("Figure " + figures[n].index + " is loaded");
                            inputBad = false;
                        }
                        else
                        {
                            Console.Write("Figure " + (n + 1) + " has invalid geometry");
                            figures[n].NormalizeSize(240.0F);
                            Form1 form1 = new Form1();
                            form1.Figure1Data = figures[n].vertices;
                            form1.Figure2Data = new Vector3[1];
                            Application.Run(form1);
                            Console.WriteLine(" ");
                        }

                    }
                    catch (Exception ex)
                    {
                        if (ex is FileNotFoundException)
                        {
                            Console.WriteLine("File not found");
                            Console.WriteLine(" ");
                        }
                        if (ex is ArgumentOutOfRangeException)
                        {
                            Console.WriteLine("Input data is incorrectly structured");
                            Console.WriteLine(" ");
                        }
                    }
                }

            }
            Console.WriteLine(" ");
            Console.WriteLine("Checking...");
            Console.WriteLine("___________________________________");
            Console.WriteLine(" ");
            if (Check(figures))
            {
                Console.WriteLine(" ");
                Console.WriteLine("The figures are the same!");
            }
            else
            {
                Console.WriteLine(" ");
                Console.WriteLine("The figures are not the same!");
            }
            Console.WriteLine("___________________________________");
            figures[0].NormalizeSize(240.0F);
            figures[1].NormalizeSize(240.0F);
            Console.WriteLine("Check if the figures are similar with eachother? (Y/N)");
            if (Console.ReadLine().ToUpper() == "Y")
            {
                if (Check(figures))
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("The figures are simmilar!");
                }
                else
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("The figures are not simmilar!");
                }
            }

            Form1 form = new Form1();
            form.Figure1Data = figures[0].vertices;
            form.Figure2Data = figures[1].vertices;
            Application.Run(form);
            Application.Exit();
        }
        private static bool Validate(Figure figure)
        {
            bool validation = true;
            int nextVertex1;
            int nextVertex2;
            float[] xRounded = new float[2];
            float[] yRounded = new float[2];
            float zValue;

            zValue = figure.vertices[0].Z;

            Vector3 vector1;
            Vector3 vector2;
            Vector3 vector3;
            Vector3 cross1;
            Vector3 cross2;

            for (int i1 = 0; i1 < figure.vertices.Length; i1++)
            {
                if (i1 == figure.vertices.Length - 1)
                {
                    nextVertex1 = 0;
                }
                else
                {
                    nextVertex1 = i1 + 1;
                }
                if (validation)
                {
                    xRounded[0] = MathF.Round(figure.vertices[i1].X, 0);
                    yRounded[0] = MathF.Round(figure.vertices[i1].Y, 0);
                    if (figure.vertices[i1].Z != zValue)
                    {
                        Console.WriteLine("Figure is not planar. It will be flattened.");
                    }
                    else
                    {
                        for (int i2 = 0; i2 < figure.vertices.Length; i2++)
                        {
                            if (i2 == figure.vertices.Length - 1)
                            {
                                nextVertex2 = 0;
                            }
                            else
                            {
                                nextVertex2 = i2 + 1;
                            }
                            if (validation)
                            {
                                xRounded[1] = MathF.Round(figure.vertices[i2].X, 0);
                                yRounded[1] = MathF.Round(figure.vertices[i2].Y, 0);
                                if (xRounded[0] == xRounded[1] && yRounded[0] == yRounded[1] && i1 != i2)
                                {
                                    validation = false;
                                    Console.WriteLine("Figure " + figure.index + " Has overlapping vertices");
                                }
                                vector1 = figure.vertices[nextVertex2] - figure.vertices[i2];
                                vector2 = figure.vertices[nextVertex1] - figure.vertices[i2];
                                vector3 = figure.vertices[i1] - figure.vertices[i2];
                                cross1 = Vector3.Normalize(Vector3.Cross(vector1, vector2));
                                cross2 = Vector3.Normalize(Vector3.Cross(vector1, vector3));
                                if ((cross1.Z + cross2.Z) == 0)
                                {
                                    vector1 = figure.vertices[nextVertex1] - figure.vertices[i1];
                                    vector2 = figure.vertices[i2] - figure.vertices[nextVertex1];
                                    vector3 = figure.vertices[nextVertex2] - figure.vertices[nextVertex1];
                                    cross1 = Vector3.Normalize(Vector3.Cross(vector1, vector2));
                                    cross2 = Vector3.Normalize(Vector3.Cross(vector1, vector3));
                                    if ((cross1.Z + cross2.Z) == 0)
                                    {
                                        validation = false;
                                        Console.WriteLine("Figure " + figure.index + " Has crossing edges");
                                    }
                                }

                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                }
                else
                {
                    break;
                }
            }
            return validation;
        }
        private static bool Check(Figure[] figures)
        {
            bool similarity = false;
            int matches = 0;
            figures[0].NormalizeSize(240.0F);
            figures[1].NormalizeSize(240.0F);
            /*
             Проверка с ротация на всяка точка от всяка фигура до оста Х
             */
            if (figures[0].vertices.Length == figures[1].vertices.Length)
            {
                int numPoints = figures[0].vertices.Length;
                figures[0].AlignToOrigin(0);
                for (int i = 0; i < numPoints; i++)
                {
                    if (!similarity)
                    {
                        figures[1].AlignToOrigin(i);
                        for (int j = 0; j < numPoints; j++)
                        {
                            if (figures[1].NearPoint(j, figures[0]) < 0.5F)
                            {
                                matches++;
                                if (matches == numPoints)
                                {
                                    similarity = true;
                                    figures[1].Sort(j);
                                    Console.WriteLine("Match!");
                                }
                            }
                        }
                        /*
                        Form1 form1 = new Form1();
                        form1.Figure1Data = figures[0].vertices;
                        form1.Figure2Data = figures[1].vertices;
                        Application.Run(form1);
                        
                        Form1 form = new Form1();
                        form.Figure1Data = figure1.vertices;
                        form.Figure2Data = figure2.vertices;
                        Application.Run(form);

                        for (int i3 = 0; i3 < numPoints; i3++)
                        {
                            Console.WriteLine(figure1.vertices[i3]);

                        }
                        Console.WriteLine(" ");
                        for (int i3 = 0; i3 < numPoints; i3++)
                        {
                            Console.WriteLine(figure2.vertices[i3]);

                        }
                        */
                    }
                }
            }
            else
            {
                similarity = false;
            }
            return similarity;
        }

    }
}