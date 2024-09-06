using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRepClient
{
    public class Drive
    {
        public float right, left, Phi;
        public float TargetDirection;
        public float RobotDirection;//variable para salida al formulario a través del formulario
        public float DistToTarget;


        public void GetDrive(float RobX, float RobY, float RobA, float GoalPointX, float GoalPointY, float Xmax, float Ymax)
        {
            GoalPointX = GoalPointX * 0.1f;
            GoalPointY = GoalPointY * 0.1f;
            Xmax = Xmax * 0.1f;
            Ymax = Ymax * 0.1f;
            RobX = RobX + Xmax / 2;
            RobY = RobY + Ymax / 2;
            RobotDirection = RobA;
            //determinar la dirección relativa del objetivo 
            float Xpel = GoalPointX - RobX;
            float Ypel = GoalPointY - RobY;
            TargetDirection = (float)Math.Atan2(Xpel, Ypel);  //solo necesitas a RobA-
            DistToTarget = (float)Math.Sqrt(Xpel * Xpel + Ypel * Ypel);


            if (TargetDirection - RobA < Math.PI && TargetDirection - RobA > -Math.PI)
            {
                Phi = TargetDirection - RobA;
            }
            else
            {
                if ((Math.PI * 2) > Math.Abs((float)(Math.PI * 2 + TargetDirection - RobA)))//si el ángulo entre los puntos es mayor que dos pi
                {
                    Phi = (float)(Math.PI * 2 + TargetDirection - RobA);
                }
                else
                {
                    Phi = (TargetDirection - RobA - (float)(Math.PI * 2));
                }
            }

            //Determinamos si el robot está muy desviado del objetivo y lo dirigimos hacia él.
            if (Phi > 0.4 || Phi < -0.4)
            {
                if (Phi > 0.4) { right = -1; left = 1; }               
                if (Phi < -0.4) { right = 1; left = -1; }
            }

            if (Phi < 0.4 || Phi > -0.4)
            {
                if (Phi < 0.4 && Phi > 0) { right = 1 - Phi * 1.4f; left = 1; }
                if (Phi > -0.4 && Phi < 0) { right = 1; left = 1 - Phi * 1.4f * -1; }
            }

            if (Phi == 0)
            {
                right = 1; left = 1;
            }

            if (DistToTarget < 0.03)
            {
                right = 0;
                left = 0;
            }
            //   right = 0; left = 0;///
            //   right = 2f; left = -2f;///
            //   right = 3; left = 3;///
        }
    }
}
