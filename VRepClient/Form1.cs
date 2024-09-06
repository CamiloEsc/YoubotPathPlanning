using System;
using System.Drawing;
using System.Windows.Forms;
using VRepAdapter;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
namespace VRepClient
{
    public partial class Form1 : Form

    {
        //public MapForm f2 = new MapForm();
        public Form1()
        {
            InitializeComponent();
            f1 = this;
            //f2.Show();
        }

        public RobotAdapter ra; //instancia de clase ra - robot adapter
        public Drive RobDrive;
        public SequencePoints SQ;//objeto de clase sequencePoints
        public Map map;//objeto de clase Map
        public SearchInGraph SiG;//objeto de clase de búsqueda gráfica        
        //public KukaPotField KukaPotField = new KukaPotField();
        //public int PotfieldButtonA = 0;//si se presiona el botón entonces el método PotField está disponible
        public int KukaPotButtonB = 0;//si se presiona el botón, entonces el método de cookies funciona.
        public List<Point> ListPoints = new List<Point>();
        public int Enviar, Detener;


        private void button1_Click(object sender, EventArgs e)
        {
            ra.Init();
        }

        Graphics g;


        private void timer1_Tick(object sender, EventArgs e)
        {
                    


            PaintEventArgs p = new PaintEventArgs(pictureBox1.CreateGraphics(), pictureBox1.Bounds); //El componente sobre el que dibujar y el área sobre la que dibujar
            pictureBox1_Paint(sender, p);        
            //f2.Invalidate();
            if (ra is VrepAdapter)
            {
                var vrep = ra as VrepAdapter;
                string Lidar = VRepFunctions.GetStringSignal(vrep.clientID, "Lidar");//datos str de ledar
                string RobPos = VRepFunctions.GetStringSignal(vrep.clientID, "RobPos");//str-data de ledar obteniendo coordenadas del robot en el escenario Vrep
                vrep.ReceiveLedData(Lidar);
                vrep.ReceiveOdomData(RobPos);
            }

            if (ra != null)
            {
                ra.Send(RobDrive);
            }

            if (RobDrive != null & ra != null && SQ != null)//enviar odometría a una instancia de la clase de unidad
            {
                if (Enviar == 1 & Detener == 0)
                {

                    map.GlobListToGraph(map.GlobalMapList, map.RobOdData);
                    float GoalPointX = Convert.ToSingle(textBox8.Text);
                    //label18.Text = GoalPointX.ToString();
                    float GoalPointY = Convert.ToSingle(textBox9.Text);
                    //label19.Text = GoalPointY.ToString();
                    Point start = new Point((int)(ra.RobotOdomData[0] * 10 + map.Xmax / 2), (int)(ra.RobotOdomData[1] * 10 + map.Ymax / 2));
                    Point goal = new Point((int)GoalPointX * 10 + map.Xmax / 2, (int)GoalPointY * 10 + map.Ymax / 2);
                    ListPoints = SiG.FindPath(map.graph, start, goal);
                   

                    if (ListPoints != null)
                    {
                        SQ.GetNextPoint(ListPoints, ra.RobotOdomData[0], ra.RobotOdomData[1], ra.RobotOdomData[2], map.Xmax, map.Ymax);
                        RobDrive.GetDrive(ra.RobotOdomData[0], ra.RobotOdomData[1], ra.RobotOdomData[2], SQ.CurrentPointX, SQ.CurrentPointY, map.Xmax, map.Ymax);
                        ra.Send(RobDrive);
                    }

                    map.LedDataToList(ra.RobotLedData, ra.RobotOdomData);
                    
                }
            }

            if (ra != null & RobDrive != null)//enviar variables desde el adaptador de robot al formulario
            {
                string OutLedData = "";
                string OutOdomData = "";

                for (int i = 0; i < ra.RobotLedData.Length; i++)
                {
                    OutLedData = OutLedData + ra.RobotLedData[i] + "; ";
                }

                for (int i = 0; i < ra.RobotOdomData.Length; i++)
                {
                    OutOdomData = OutOdomData + ra.RobotOdomData[i] + "; ";
                }
                
                

                if (RobDrive != null)
                {
                    double RobotOriGrados = Convert.ToDouble(RobDrive.RobotDirection) * (180/Math.PI );
                    //double GoalOriGrados = Convert.ToDouble(RobDrive.TargetDirection) * (180 / Math.PI);
                    //double DistanciaCm = RobDrive.DistToTarget * 100;

                    
                    textBox3.Text = RobotOriGrados.ToString();
                    textBox3.Invalidate();

                    textBox5.Text = RobDrive.DistToTarget.ToString();
                    textBox5.Invalidate();

                }
            }           

        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ra != null)
            {
                ra.Deactivate();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void invoke(Action a) { Invoke(a); }
        public static Form1 f1;

  
        private void VrepAdapter_Click(object sender, EventArgs e)
        {
            ra = new VrepAdapter();

        }

         private void Drive_Click(object sender, EventArgs e)
        {
            SQ = new SequencePoints();
            RobDrive = new Drive();
            map = new Map();
            SiG = new SearchInGraph();
            int mapWidthPxls = map.Xmax * (CellSize + 1) + 1, mapHeightPxls = map.Ymax * (CellSize + 1) + 1;
            pictureBox1.Image = new Bitmap(mapWidthPxls, mapHeightPxls);
            g = Graphics.FromImage(pictureBox1.Image);
            //});

        }



        private void textBox8_TextChanged(object sender, EventArgs e)//
        {

        }

 

        int CellSize = 4;
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (Enviar == 1 & Detener == 0)
            {
                if (ra != null && SQ != null && Drive != null && map != null && map.graph != null)
                {



                    int MapWidth = map.Xmax;
                    int MapHeight = map.Ymax;
                    // Tamaño del mapa en píxeles.
                    int mapWidthPxls = MapWidth * (CellSize + 1) + 1,
                        mapHeightPxls = MapHeight * (CellSize + 1) + 1;


                    // Llenar todo el mapa de bits:
                    g.Clear(Color.White);

                    // GRID:
                    for (int x = 0; x <= MapWidth; x++)
                        g.DrawLine(Pens.LightGray, x * (CellSize + 1), 0, x * (CellSize + 1), mapHeightPxls);
                    for (int y = 0; y <= MapHeight; y++)
                        g.DrawLine(Pens.LightGray, 0, y * (CellSize + 1), mapWidthPxls, y * (CellSize + 1));



                    for (int i = 0; i < map.Xmax; i++) //pintar una celda con obstáculos
                    {
                        for (int k = 0; k < map.Ymax; k++)
                        {
                            if (map.graph[i, k] == 2)
                            {
                                int H = CellSize + 1;//(int)(pictureBox1.Height / map.Ymax);
                                int W = CellSize + 1;//(int)(pictureBox1.Width / map.Xmax);

                                SolidBrush blueBrush = new SolidBrush(Color.Blue);
                                Rectangle rect = new Rectangle((i) * W, pictureBox1.Height + ((-1) * k * H), W, H);
                                e.Graphics.FillRectangle(blueBrush, rect);
                            }

                        }
                    }
                    if (ListPoints != null)//dibuja la ruta resultante
                    {
                        for (int i = 0; i < ListPoints.Count; i++)
                        {
                            int H = CellSize + 1; //(int)(pictureBox1.Height / map.Ymax);
                            int W = CellSize + 1;//(int)(pictureBox1.Width / map.Xmax);

                            SolidBrush blueBrush = new SolidBrush(Color.Red);
                            SolidBrush greenBrush = new SolidBrush(Color.Green);
                            Rectangle rect = new Rectangle((ListPoints[i].X) * W, pictureBox1.Height + ((-1) * ListPoints[i].Y * H), W, H);
                            Rectangle rectCurrentPoint = new Rectangle(((int)SQ.CurrentPointX) * W, pictureBox1.Height + ((-1) * (int)SQ.CurrentPointY * H), W, H);

                            e.Graphics.FillRectangle(blueBrush, rect);
                            e.Graphics.FillRectangle(greenBrush, rectCurrentPoint);
                        }

                    }
                    int H2 = CellSize + 1;// (int)(pictureBox1.Height / map.Ymax);
                    int W2 = CellSize + 1; //(int)(pictureBox1.Width / map.Xmax);
                    Point start = new Point((int)(ra.RobotOdomData[0] * 10 + map.Xmax / 2), (int)(ra.RobotOdomData[1] * 10 + map.Ymax / 2));
                    e.Graphics.DrawEllipse(Pens.Chocolate, (int)start.X * W2 - 2 * W2, pictureBox1.Height + ((-1) * (int)start.Y) * H2 - 2 * W2, 20, 20);

                }

                if (map != null && map.invalidateform == true)//actualizar el formulario
                {
                    pictureBox1.Invalidate();//mover la llamada de dibujo en el cuadro de imagen a un método más lógico
                }
                pictureBox1.Invalidate();

            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public Image Rob { get; set; }



        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

 
        private void button4_Click(object sender, EventArgs e)
        {
            Enviar = 1;
            Detener = 0;
            //label16.Text = Convert.ToString(Enviar);
            //label17.Text = Convert.ToString(Detener);
        }

        private void label16_Click(object sender, EventArgs e)
        {
           // label16.Text = "";
        }

        private void label17_Click(object sender, EventArgs e)
        {
         //   label17.Text = "";
        }

        private void label18_Click(object sender, EventArgs e)
        {
          //  label18.Text = "";
        }

        private void label19_Click(object sender, EventArgs e)
        {
            //label19.Text = "";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Enviar = 0;
            Detener = 1;
           // label16.Text = Convert.ToString(Enviar);
          //  label17.Text = Convert.ToString(Detener);
        }
    }
}
