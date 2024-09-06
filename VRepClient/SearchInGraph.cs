//búsqueda de gráficos (A*)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Collections.ObjectModel;

namespace VRepClient
{
    public class PathNode
    {
        public Point Position { get; set; }//Coordenadas de un punto en el mapa.
        public float PathLengthFromStart { get; set; }//Longitud del camino desde el inicio hasta el punto (G)
        public PathNode CameFrom { get; set; }//El punto desde donde llegaste a este punto.
        public float HeuristicEstimatePathLength { get; set; }//Distancia aproximada del punto al objetivo (H)

        public float EstimateFullPathLength
        {
            get
            {
                return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
            }
        }
    }

    public class SearchInGraph
    {
        public List<Point> FindPath(float[,] field, Point start, Point goal) //eliminado estático
        {   //PASO 1
            var closedSet = new Collection<PathNode>();
            var openSet = new Collection<PathNode>();
            //PASO 2
            PathNode startNode = new PathNode()
            {
                Position = start,
                CameFrom = null,
                PathLengthFromStart = 0,
                HeuristicEstimatePathLength = GetHeuristicPathLenght(start, goal)
            };

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                //PASO 3 
                var currentNode = openSet.OrderBy(node => node.EstimateFullPathLength).First();
                //PASO 4
                if (currentNode.Position == goal) return GetPathForNode(currentNode);
                //PASO 5
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                //PASO 6
                foreach (var neighbourNode in GetNeighbours(currentNode, goal, field))
                {
                    //PASO 7
                    if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                        continue;
                    var openNode = openSet.FirstOrDefault(node => node.Position == neighbourNode.Position);
                    //PASO 8
                    if (openNode == null)
                        openSet.Add(neighbourNode);
                    else
                        if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                    {
                        //PASO 9
                        openNode.CameFrom = currentNode;
                        openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                    }
                }
            }
            //PASO 10
            return null;
        }

        private static float GetDistanceBetweenNeighbours(float weight)//Función de distancia de X a Y (peso int)
        {
            return weight;//aquí necesitamos sumar la permeabilidad de la celda, en este momento la distancia siempre es igual a 1
        }

        private static int GetHeuristicPathLenght(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);// función para la estimación de la distancia aproximada al objetivo
        }

        private static Collection<PathNode> GetNeighbours(PathNode pathNode, Point goal, float[,] field)
        {
            var result = new Collection<PathNode>();
            //Los puntos vecinos son celdas adyacentes por un lado.
            Point[] neighbourPoints = new Point[8];
            neighbourPoints[0] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
            neighbourPoints[1] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);
            neighbourPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
            neighbourPoints[3] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);
            neighbourPoints[4] = new Point(pathNode.Position.X + 1, pathNode.Position.Y + 1);
            neighbourPoints[5] = new Point(pathNode.Position.X - 1, pathNode.Position.Y - 1);
            neighbourPoints[6] = new Point(pathNode.Position.X - 1, pathNode.Position.Y + 1);
            neighbourPoints[7] = new Point(pathNode.Position.X + 1, pathNode.Position.Y - 1);

            foreach (var point in neighbourPoints) //comprobar si el mapa ha ido más allá de los límites
            {
                if (point.X < 0 || point.X >= field.GetLength(0))
                    continue;
                if (point.Y < 0 || point.Y >= field.GetLength(1))
                    continue;
                //Comprueba que puedes caminar alrededor de la jaula.
                //revisa las cinco celdas más cercanas
                int freeNode = 0;

                for (int i = -3; i < 4; i++)
                {
                    for (int k = -3; k < 4; k++)
                    {
                        if (point.X + i > 0 && point.X + i < field.GetLength(0))
                        {
                            if (point.Y + k > 0 && point.Y + k < field.GetLength(1))
                            {
                                if (field[point.X + i, point.Y + k] == 1)
                                {
                                    freeNode++;
                                }
                            }
                        }
                    }
                }

                float weight;

                if (pathNode.Position.X != point.X && pathNode.Position.Y != point.Y)//los desplazamientos diagonales cuestan 1,4 y los rectos cuestan 1
                    weight = 1.4f;
                else
                    weight = 1;

                if ((field[point.X, point.Y] < 2) && freeNode == 49)
                {
                    //Complete los datos para el punto de ruta.
                    var neighbourNode = new PathNode()
                    {
                        Position = point,
                        CameFrom = pathNode,
                        PathLengthFromStart = pathNode.PathLengthFromStart + GetDistanceBetweenNeighbours(weight),
                        HeuristicEstimatePathLength = GetHeuristicPathLenght(point, goal)
                    };

                    result.Add(neighbourNode);
                }
            }
            return result;
        }

        private static List<Point> GetPathForNode(PathNode pathNode)
        {
            var result = new List<Point>();
            var currentNode = pathNode;

            while (currentNode != null)
            {
                result.Add(currentNode.Position);
                currentNode = currentNode.CameFrom;
            }

            result.Reverse();
            return result;
        }
    }
}
