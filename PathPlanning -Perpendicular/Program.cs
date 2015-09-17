using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace multiServerTestV01
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new DStarForm());
            Application.Run(new A_Star_Form());
            //Application.Run(new DStarLiteForm());
        }
    }
}
