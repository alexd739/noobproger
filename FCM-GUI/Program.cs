using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppLogic;

namespace FCM_GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logic model = new Logic();
            MainForm view = new MainForm();
            Presenter pr = new Presenter(model, view);
            Application.Run(view);
        }
    }
}
