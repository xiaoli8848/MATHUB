using Flee.PublicTypes;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MATHUB
{
    public abstract class CalculatorRealize
    {
        public abstract string Compute(string exp);
    }
    public static class CalculatorRealizeHelper
    {
        public static bool isPythonCalculatorAvailabel = _isPythonCalculatorAvailabel();
        private static CalculatorRealize _BasicCalculatorRealize = new BasicCalculatorRealize();
        private static CalculatorRealize _PythonCalculatorRealize = isPythonCalculatorAvailabel ? new PythonCalculatorRealize() : null;
        private static bool _isPythonCalculatorAvailabel()
        {
            Process pythonTester = new();
            pythonTester.StartInfo.FileName = "cmd.exe";
            pythonTester.StartInfo.UseShellExecute = false;
            pythonTester.StartInfo.RedirectStandardError = true;
            pythonTester.StartInfo.RedirectStandardInput = true;
            pythonTester.StartInfo.RedirectStandardOutput = true;
            pythonTester.StartInfo.CreateNoWindow = true;
            pythonTester.Start();
            pythonTester.StandardInput.AutoFlush = true;
            pythonTester.StandardInput.WriteLine("python -h&exit");
            _ = pythonTester.StandardOutput.ReadToEnd();
            if (pythonTester.StandardError.ReadToEnd().Length == 0)
            {
                pythonTester.Kill();
                return true;
            }
            else
            {
                pythonTester.Kill();
                return false;
            }
        }
        public static ref CalculatorRealize getCurrentCalculatorRealize(bool preferPythonCalculatorRealize=true)
        {
            if(preferPythonCalculatorRealize && _PythonCalculatorRealize!=null)
            {
                return ref _PythonCalculatorRealize;
            }
            else
            {
                return ref _BasicCalculatorRealize;
            }
        }
    }
    public class PythonCalculatorRealize : CalculatorRealize
    {
        protected Process ComputeProcess;
        private string TempFile;
        public PythonCalculatorRealize()
        {
            ComputeProcess = new();
            //computeProcess.StartInfo.FileName = Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("MATHUB.exe").AsTask().Result.Name;
            TempFile= Path.GetTempFileName();
            var writer = new StreamWriter(TempFile, false);
            writer.Write(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MATHUB.MATHUB.py")).ReadToEnd());
            writer.Close();
            ComputeProcess.StartInfo.FileName = "cmd.exe";
            ComputeProcess.StartInfo.RedirectStandardOutput = true;
            ComputeProcess.StartInfo.RedirectStandardError = true;
            ComputeProcess.StartInfo.RedirectStandardInput = true;
            ComputeProcess.StartInfo.CreateNoWindow = true;
            if (!ComputeProcess.Start())
            {
                throw new Exception("计算进程创建失败");
            }
            ComputeProcess.StandardInput.AutoFlush = true;
            ComputeProcess.StandardInput.WriteLine("python " + TempFile + "&exit");
        }
        public override string Compute(string exp)
        {
            while (ComputeProcess.StandardOutput.Peek() != -1)
            {
                _ = ComputeProcess.StandardOutput.Read();
            }
            ComputeProcess.StandardInput.WriteLine(exp);
            var temp = ComputeProcess.StandardOutput.ReadLine();
            return temp.StartsWith("python") || (exp != "" && temp == "") ? Compute(exp) : temp;
        }
        ~PythonCalculatorRealize()
        {
            File.Delete(TempFile);
            ComputeProcess.Kill();
        }
    }
    public class BasicCalculatorRealize : CalculatorRealize
    {
        private ExpressionContext calculator;
        public BasicCalculatorRealize()
        {
            calculator = new ExpressionContext();
            calculator.Imports.AddType(typeof(System.Math));
        }
        public override string Compute(string exp)
        {
            return calculator.CompileDynamic(exp).Evaluate() as string;
        }
    }
}
