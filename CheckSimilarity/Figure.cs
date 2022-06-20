using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Numerics;

namespace CheckSimilarity
{
    internal class Figure
    {
        public Vector3 centroid;
        public Vector3[] vertices;
        public Vector3[] edges;
        public int index;
        public Figure(int length)
        {
            this.vertices = new Vector3[length];
            this.edges = new Vector3[length];
            this.centroid = Vector3.Zero;
        }
        public static Figure AddFigure(string file)
        {
            string[] lines;
            lines = System.IO.File.ReadAllLines(file);
            Figure figureLoad = new Figure(lines.Length);
            for (int iteration = 0; iteration < lines.Length; iteration++)
            {
                string line = lines[iteration];
                string lineMod;
                int index1;
                int index2;
                index1 = line.IndexOf('(');
                index2 = line.IndexOf(' ');
                lineMod = line.Substring(index1 + 1, index2 - index1 - 1);
                figureLoad.vertices[iteration].X = float.Parse(lineMod, CultureInfo.InvariantCulture.NumberFormat);
                figureLoad.centroid.X += figureLoad.vertices[iteration].X;
                index1 = line.IndexOf(' ');
                index2 = line.LastIndexOf(' ');
                lineMod = line.Substring(index1 + 1, index2 - index1 - 1);
                figureLoad.vertices[iteration].Y = float.Parse(lineMod, CultureInfo.InvariantCulture.NumberFormat);
                figureLoad.centroid.Y += figureLoad.vertices[iteration].Y;
                index1 = line.LastIndexOf(' ');
                index2 = line.IndexOf(')');
                lineMod = line.Substring(index1 + 1, index2 - index1 - 1);
                figureLoad.vertices[iteration].Z = float.Parse(lineMod, CultureInfo.InvariantCulture.NumberFormat);
                figureLoad.centroid.Z += figureLoad.vertices[iteration].Z;  
            }

            figureLoad.centroid.X /= (float)lines.Length;
            figureLoad.centroid.Y /= (float)lines.Length;
            figureLoad.centroid.Z /= (float)lines.Length;

            return figureLoad;
        }
        public int FarVertex()
        {
            int farVertexIndex = 0;
            Vector3 distance = Vector3.Zero;
            Vector3 distanceNew = Vector3.Zero;
            for (int i = 0; i < this.vertices.Length; i++)
            {
                distanceNew = this.vertices[i] - this.centroid;
                if (distanceNew.Length() > distance.Length())
                {
                    distance = distanceNew;
                    farVertexIndex = i;
                }
            }
            return farVertexIndex;
        }
        public void GenerateEdges()
        {
            float[] dot = new float[this.edges.Length];
            for (int iteration = 0; iteration < this.edges.Length; iteration++)
            {   
                if (iteration < this.vertices.Length - 1)
                {
                    this.edges[iteration] = this.vertices[iteration + 1] - this.vertices[iteration];
                }
                else
                {
                    this.edges[iteration] = this.vertices[0] - this.vertices[iteration];
                }
            }
            /*
            for (int iteration = 0; iteration < this.edges.Length; iteration++)
            {
                if (iteration == 0)
                {
                    dot[iteration] = Vector3.Dot(this.edges[iteration], this.edges[this.edges.Length-1]);
                }
                else
                {
                    dot[iteration] = Vector3.Dot(this.edges[iteration], this.edges[iteration - 1]);
                }
            */
        }
        public void Sort(int index)
        {
            Vector3[] verticesNew = new Vector3[this.vertices.Length];
            for (int i = 0; i < this.vertices.Length; i++)
            {
                if (i + index < this.vertices.Length)
                {
                    verticesNew[i] = this.vertices[i + index];
                }
                else
                {
                    verticesNew[i] = this.vertices[i + index - this.vertices.Length];
                }
                
            }
            this.vertices = verticesNew;
        }
        public void UnifyNormals()
        {
            Vector3 normal;
            Vector3[] verticesNew = new Vector3[this.vertices.Length];
            normal = Vector3.Cross((this.vertices[1] - this.vertices[0]), (this.vertices[2] - this.vertices[1]));
            if (normal.Z < 0)
            {
                verticesNew[0] = this.vertices[0];
                for (int i = 1; i < this.vertices.Length; i++)
                {
                    verticesNew[i] = this.vertices[this.vertices.Length - i];
                }
                this.vertices = verticesNew;
                Console.WriteLine("The normal of figure " + this.index +  " was flipped");
                normal = Vector3.Cross((this.vertices[1] - this.vertices[0]), (this.vertices[this.vertices.Length - 1] - this.vertices[0]));
            } 
        }
        public void NormalizeSize(float maxSize)
        {
            Vector3 maxLength = this.vertices[this.FarVertex()];
            float scaleFactor = maxSize / maxLength.Length();
            for(int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i] *= scaleFactor;
            }
        }
        public void Flatten()
        {
            for (int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i].Z = 0;
            }
            this.centroid.Z = 0;
        }
        public void CenterToOrigin()
        {
            for (int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i] = this.vertices[i] - this.centroid;
            }
            this.centroid = Vector3.Zero;
        }
        public float NearPoint(int index, Figure figure)
        {
            float distance;
            float distanceNew;
            Vector3 distanceVector;
            distanceVector = this.vertices[0] - figure.vertices[0];
            distance = distanceVector.Length();
            for (int i = 0; i < figure.vertices.Length; i++)
            {
                distanceVector = this.vertices[index] - figure.vertices[i];
                distanceNew = distanceVector.Length();
                if (distanceNew < distance)
                {
                    distance = distanceNew;
                }
            }
            return distance;
        }
        public void AlignToOrigin(int index)
        {
            double angleOfRotation;
            Vector3 vectorOfRotateion;
            Matrix4x4 rotationMatrix;
            vectorOfRotateion = this.vertices[index] - this.centroid;
            vectorOfRotateion = Vector3.Normalize(vectorOfRotateion);
            angleOfRotation = Math.Atan2(vectorOfRotateion.Y, vectorOfRotateion.X);
            angleOfRotation = - angleOfRotation;
            rotationMatrix = Matrix4x4.CreateFromAxisAngle(Vector3.UnitZ, (float) angleOfRotation);
            for (int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i] = Vector3.Transform(this.vertices[i], rotationMatrix);
                //Console.WriteLine(this.vertices[i]);
            }
            Console.WriteLine(" ");
        }
    }
}
