using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Designer
{
    class Program
    {
        /// <summary>
        /// Entry point of this example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (SimpleWindow example = new SimpleWindow())
            {
                example.Title = "Gwen Designer";
                example.VSync = VSyncMode.Off; // to measure performance
                example.Run(0.0, 0.0);
                //example.TargetRenderFrequency = 60;
            }
        }
    }
}
