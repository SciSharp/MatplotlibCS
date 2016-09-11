﻿using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace MatplotlibCS
{
    /// <summary>
    /// Обёртка над питоновским скриптом построения графиков
    /// </summary>
    public class DasPlot
    {
        #region Fields

        /// <summary>
        /// Пусть к интерпрететору питона
        /// </summary>
        private readonly string _pythonExePath;

        /// <summary>
        /// Путь к скрипту dasPlot.py
        /// </summary>
        private readonly string _dasPlotPyPath;

        #endregion

        #region .ctor

        public DasPlot(string pythonExePath, string dasPlotPyPath)
        {
            _pythonExePath = pythonExePath;
            _dasPlotPyPath = dasPlotPyPath;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Выполняет задачу построения графиков
        /// </summary>
        /// <param name="task">Описание задачи</param>
        public void DoTask(Figure task)
        {
            var args = "";
            var serializer = new JsonSerializer() {StringEscapeHandling = StringEscapeHandling.EscapeHtml};
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, task);
                args = writer.ToString();
                args = args.Replace("\"", "\\\"");
            }

            var psi = new ProcessStartInfo(_pythonExePath, $"{_dasPlotPyPath} \"{args}\"");
            var process = Process.Start(psi);
            process.WaitForExit();
        } 

        #endregion
    }
}