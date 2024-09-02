using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace AppStarter
{
    public partial class MainForm : Form
    {
        private readonly string ClrProfilerDllName = "doTracer.ClrProfiler.dll";
        private readonly string ClrProfilerGuid = "{cf0d821e-299b-5307-a3d8-b283c03916dd}";

        public MainForm()
        {
            InitializeComponent();
        }

        private void pickAppButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "EXE files (*.exe)|*.exe";
            dialog.InitialDirectory = System.Environment.CurrentDirectory;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                appPathTextBox.Text = dialog.FileName;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            string exeToRun = appPathTextBox.Text;
            string arguments = appArgsTextBox.Text;
            if (System.IO.File.Exists(exeToRun))
            {
                string profilerDll = Environment.CurrentDirectory + "\\" + ClrProfilerDllName;

                ProcessStartInfo startupInfo = new ProcessStartInfo(exeToRun);
                setEnvironmentVariable(ref startupInfo, "COR_ENABLE_PROFILING", "1");
                setEnvironmentVariable(ref startupInfo, "COR_PROFILER", ClrProfilerGuid);
                setEnvironmentVariable(ref startupInfo, "COR_PROFILER_PATH", profilerDll);

                startupInfo.Arguments = arguments;
                startupInfo.UseShellExecute = false;

                Process p = Process.Start(startupInfo);
            }
        }

        private void setEnvironmentVariable(ref ProcessStartInfo startupInfo, string name, string value)
        {
            if (startupInfo.EnvironmentVariables.ContainsKey(name) == true)
                startupInfo.EnvironmentVariables[name] = value;
            else
                startupInfo.EnvironmentVariables.Add(name, value);
        }

    }
}
