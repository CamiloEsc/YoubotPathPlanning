using System;

namespace VRepClient
{
    public class NO_KukaPotField
    {/*
        public float[] LaserDataKuka; //matriz 683x2 x, y, z de ledar
        public float[] RobLocDataKuka;// matriz 1x3 x,y,z ubicación del robot
        public float RV;
        public float FBV;
        public float Fx;

        public void LedDataKuka(string var)// complete la matriz LaserData, datos del lidar del robot // envíe datos del lidar aquí cookies
        {
            string g = var;

            if (g != "")
            {
                string someString = var;
                string[] words = someString.Split(new char[] { ';' });// words
                int h = 0;//variable auxiliar para convertir cadena en matriz

                LaserDataKuka = new float[words.Length];
                for (int i = 0; i < words.Length; i++)//escribimos datos del ledar en una matriz, en términos x y z
                {
                    LaserDataKuka[i] = float.Parse(words[h], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                    h++;
                }
            }
        }

        public void RodLocReceivingKuka(string RobPos) //llene la matriz RobLocData[3] con datos de ubicación de órbita
        {
            RobLocDataKuka = new float[3];

            if (RobPos != "")
            {
                string[] words = RobPos.Split(new char[] { ';' });//analizar la cadena en la matriz de palabras

                for (int i = 0; i < 3; i++)
                {
                    RobLocDataKuka[i] = float.Parse(words[i], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                }

                RobLocDataKuka[2] = RobLocDataKuka[2] * -1;
            }
        }

        double[] DistData = new double[171];
        float left = 0;
        float right = 0;
        double Phi; //rumbo al objetivo
        double RobDirect;//dirección del robot
        double TargetDirection;
        double DistToTarget;//distancia al objetivo     
        float Xold = 0;
        float Yold = 0;

        public bool ObstDistKuka(float[] M, float[] RobLoc)
        {
            if (M == null || RobLoc == null) return false;
            //Aquí determinamos el obstáculo más cercano al robot.
            //M son datos láser
            int h = 0;
            //int k = 0;
            double MinDist = 3;
            for (int i = 0; i < LaserDataKuka.Length - 1; i++)
            {

                if (LaserDataKuka[i] < MinDist)
                {
                    MinDist = LaserDataKuka[i]; h = i;
                }
            }

            Fx = Math.Abs(1 / (1 + (float)MinDist * (float)MinDist));//forma Fx=k/r^2
            if (h < 85) { Fx = Math.Abs(Fx); }//coeficientes para ruedas derecha e izquierda
            if (h > 85) { Fx = -Fx; }
            float Xrob = RobLoc[0];
            float Yrob = RobLoc[1];
            float Yfin = 0;// coordenadas del punto de rumbo
            float Xfin = 3;
            float Xpel = Xfin - Xrob;
            float Ypel = Yfin - Yrob;
            TargetDirection = Math.Atan2(Ypel, Xpel);
            DistToTarget = Math.Sqrt(Xpel * Xpel + Ypel * Ypel);

            //calcular la dirección de movimiento del robot
            if (Xold != 0)
            {
                float RobDirectX = Xrob - Xold;
                float RobDirectY = Yrob - Yold;
                RobDirect = Math.Atan2(RobDirectY, RobDirectX);
                if (TargetDirection - RobDirect < Math.PI)
                {
                    Phi = TargetDirection - RobDirect;
                }
                else
                {
                    Phi = (-1) * (Math.PI * 2 - TargetDirection + RobDirect);
                }
            }

            Xold = Xrob;
            Yold = Yrob;

            if (DistToTarget > 0.4)//va al objetivo con una ligera desviación //regulador diferencial
            {
                float Vdes = -1;
                float omdes = 2f * (float)Phi;
                float d = 0.4f;//distancia entre ruedas
                float Vr = (Vdes + d * omdes);
                float Vl = (Vdes - d * omdes);
                float rw = 0.1f;//radio de la rueda
                float R = Vr / rw;
                float L = Vl / rw;
                RV = (L - R); //RotateVelocity
                FBV = (R + L) / 2; //FrontBackVelocity
                //right = (float)Math.Round(Vr, 2);//multiplica por 0,1 para obtener una galleta real
                //left = (float)Math.Round(Vl, 2);//multiplica por 0,1 para obtener una galleta real
                right = R;//multiplica por 0,1 para obtener una galleta real
                left = L;//multiplica por 0,1 para obtener una galleta real
            }

            if (DistToTarget < 0.4)
            {
                FBV = 0;
                RV = 0;
            }
            var speed = 0.1f;
            var k_slow = 0.1f;
            var arg1 = -1 * FBV * 0.1 * k_slow;
            arg1 = Math.Max(-speed, Math.Min(arg1, speed));//es necesario rehacer estas conclusiones para una conclusión adecuada
            var arg3 = -1 * RV * 0.2 * k_slow;
            arg3 = Math.Max(-speed, Math.Min(arg3, speed));//Tal vez(left-right)
                                                           //    control_str = string.Format(CultureInfo.InvariantCulture, "LUA_Base({0}, {1}, {2})", arg1, arg2, arg3);
            return true;
        }

        public string control_str;
    */}
}

