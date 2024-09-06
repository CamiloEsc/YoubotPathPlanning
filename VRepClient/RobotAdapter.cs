using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using VRepAdapter;

namespace VRepClient
{
    
    public class RobotAdapter
    {
        public virtual void Init() { }
        public virtual void Deactivate() { }
        public virtual void Send(Drive RobDrive) { }//enviando comandos de control
        public virtual void ReceiveLedData(string LedarData) { } /*recibir datos ledar y lanzarlos en una matriz*/
        public virtual void ReceiveOdomData(string OdometryData) { }//recibir datos de odometría y lanzarlos en una matriz
        public float[] RobotLedData;//aquí se ingresan los datos del ledar Copppelia
        public float[] RobotOdomData;//Aquí se ingresan los datos de odometría de Coppelia.
        public float right;
        public float left;
    }

    public class VrepAdapter : RobotAdapter //clase heredada para trabajar con Coppelia
    {
        public int clientID = -1;
        int leftMotorHandle, rightMotorHandle, leftMotorHandleA, rightMotorHandleA;
        float driveBackStartTime = -99000;
        float[] motorSpeeds = new float[4];


        public override void Init()
        {
            clientID = VRepFunctions.Start("127.0.0.1", 7777);
            if (clientID != -1)
            {
                VRepFunctions.GetObjectHandle(clientID, "rollingJoint_fl", out leftMotorHandle);
                VRepFunctions.GetObjectHandle(clientID, "rollingJoint_rl", out leftMotorHandleA);
                VRepFunctions.GetObjectHandle(clientID, "rollingJoint_rr", out rightMotorHandle);
                VRepFunctions.GetObjectHandle(clientID, "rollingJoint_fr", out rightMotorHandleA);
                //VRepFunctions.GetObjectHandle(clientID, "Proximity_sensor", out sensorHandle);
                //VRepFunctions.GetObjectHandle(clientID, "Disc", out Disc);
            }
        }



        public override void Send(Drive RobDrive)
        { /*youbot_connection.send(ToString(data));*/
            if (RobDrive != null)
            {
                right = RobDrive.right * (-5f);
                left = RobDrive.left * (-5f);


                               
            }

            if (VRepFunctions.GetConnectionId(clientID) == -1) return;
            int simulationTime = VRepFunctions.GetLastCmdTime(clientID);

            if (simulationTime - driveBackStartTime < 3000)
                driveBackStartTime = simulationTime;
            {
                VRepFunctions.SetJointTargetVelocity(clientID, leftMotorHandle, left);
                VRepFunctions.SetJointTargetVelocity(clientID, leftMotorHandleA, left);
                VRepFunctions.SetJointTargetVelocity(clientID, rightMotorHandle, right);
                VRepFunctions.SetJointTargetVelocity(clientID, rightMotorHandleA, right);
                
            }
        }

        public override void ReceiveLedData(string LedarData)
        {
            RobotLedData = new float[518];//más de 1412 es infinito
            float[,] LaserDatatemporaryVrep;//matriz temporal con coordenadas de obstáculos visibles
            string g = LedarData;
            LaserDatatemporaryVrep = new float[684, 3];

            if (g != "")
            {
                string someString = LedarData;
                string[] words = someString.Split(new char[] { ';' });// words
                int h = 0;//variable auxiliar para convertir cadena en matriz

                for (int i = 0; i < 684; i++)//escribimos datos del lidar en una matriz bidimensional de 683 por 3, en términos de x y z
                {
                    for (int j = 0; j < 3; j++)
                    {
                        LaserDatatemporaryVrep[i, j] = float.Parse(words[h], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                        if (LaserDatatemporaryVrep[i, j] == 0) { LaserDatatemporaryVrep[i, j] = 500; }
                        h++;
                    }
                }

                int d = 0;

                for (int i = 83; i < 601; i++) //en la matriz LaserDataVrep, longitud 516, calcule y agregue distancias a los objetos
                {
                    RobotLedData[d] = (float)(Math.Sqrt(LaserDatatemporaryVrep[i, 0] * LaserDatatemporaryVrep[i, 0] + LaserDatatemporaryVrep[i, 1] * LaserDatatemporaryVrep[i, 1]));
                    d++;
                }
            }
        }

        public override void ReceiveOdomData(string OdometryData)
        {
            RobotOdomData = new float[3];

            if (OdometryData != "")
            {               
                string[] words = OdometryData.Split(new char[] { ';' });//analizar la cadena en la matriz de palabras

                for (int i = 0; i < 3; i++)
                {
                    RobotOdomData[i] = float.Parse(words[i], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                }
            }
        }

        public override void Deactivate()
        {
            VRepFunctions.Finish(clientID);
        }
    }

}
