﻿using System.Diagnostics;
using Microsoft.Extensions.Logging;
using RapidRabbitMQ.Services.Abstracts;

namespace RapidRabbitMQ.Services;

public sealed class RabbitMqBootstrapService(
    ILogger<RabbitMqBootstrapService> logger) : IRabbitMqBootstrapService, IDisposable
{
    public async Task Run()
    {
        StartEpmd();
        StartRmq();

        await Task.CompletedTask;
    }

    public void Stop()
    {
        if (_epmdProcess != null && !_epmdProcess.HasExited)
            _epmdProcess.Kill();
        if (_process != null && !_process.HasExited)
            _process.Kill();

        Process[] allProcesses = Process.GetProcesses();
        foreach (Process p in allProcesses)
        {
            try
            {
                string fullPath = p.MainModule.FileName;
                if (fullPath != null && fullPath.StartsWith(Directories.ErlangDirectory))
                {
                    System.Diagnostics.Debug.WriteLine("Force to kill : " + fullPath);
                    p.Kill();
                }
                if (fullPath != null && fullPath.StartsWith(Directories.RabbitMqDirectory))
                {
                    System.Diagnostics.Debug.WriteLine("Force to kill : " + fullPath);
                    p.Kill();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    private void StartRmq()
    {
        _logger.LogDebug($"SET ERLANG_HOME=\"{Directories.ErlangDirectory.Replace("\\", "\\\\")}\"");
        _logger.LogDebug($"SET RABBITMQ_BASE=\"{Directories.DataDirectory.Replace("\\", "\\\\")}\"");
        _logger.LogDebug($"SET RABBITMQ_CONFIG_FILE=\"{Directories.RabbitMqConfigFile.Replace("\\", "\\\\")}\"");
       
        _process = new Process();
        _process.StartInfo.EnvironmentVariables["ERLANG_HOME"] = Directories.ErlangDirectory; //+ @"\"; // Where is erlang ? 
        _process.StartInfo.EnvironmentVariables["RABBITMQ_BASE"] = Directories.DataDirectory;  // Where to put RabbitMQ logs and database
        _process.StartInfo.EnvironmentVariables["RABBITMQ_CONFIG_FILE"] = Directories.RabbitMqConfigFile; // Where is config file
        _process.StartInfo.EnvironmentVariables["RABBITMQ_LOG_BASE"] = Directories.DataDirectory + @"\log";  // Where are log files
        _process.StartInfo.EnvironmentVariables.Remove("LOGS");

        _process.StartInfo.EnvironmentVariables["HOMEDRIVE"] = "c:";     // Erlang need this for cookie file
        _process.StartInfo.EnvironmentVariables["HOMEPATH"] = Directories.DataDirectory.Substring(2); // "data";  // Erlang need this for cookie file
        _process.StartInfo.UseShellExecute = false;
        _process.StartInfo.CreateNoWindow = true;
        _process.StartInfo.RedirectStandardOutput = true;
        _process.StartInfo.RedirectStandardError = true;
        _process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
        _process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
        _process.StartInfo.FileName = "cmd.exe";
        _process.StartInfo.Arguments = "/c \"" + Path.Combine(Directories.RabbitMqSbinDirectory, "rabbitmq-server.bat") + "\"";

        //  WriteLineToOutput(" Server started ... ", Color.White);

        _process.Start();
        _process.BeginOutputReadLine();
    }

    private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        Console.WriteLine("ERROR] " + e.Data);
    }

    private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data is not null && e.Data.StartsWith("  Starting broker... completed", StringComparison.CurrentCultureIgnoreCase))
        {
            ColorConsole.WriteSuccess("RabbitMQ started successfully. Press ENTER to stop.");
        }
        else 
        { 
            Console.WriteLine(e.Data);
        }
    }
    private Process _epmdProcess;
    private Process _process;
    private readonly ILogger<RabbitMqBootstrapService> _logger = logger;

    private void StartEpmd()
    {
        _epmdProcess = new Process();
        _epmdProcess.StartInfo.UseShellExecute = false;
        _epmdProcess.StartInfo.CreateNoWindow = true;
        _epmdProcess.StartInfo.FileName = "cmd.exe";
        _epmdProcess.StartInfo.Arguments = "/c \"" + Path.Combine(Directories.ErlangDirectory, "erts-15.1.2", "bin", "epmd"); // + "\" -daemon";
        _epmdProcess.StartInfo.RedirectStandardOutput = true;
        _epmdProcess.StartInfo.RedirectStandardError = true;
        _epmdProcess.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
        _epmdProcess.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);

        _epmdProcess.Start();
        _epmdProcess.BeginOutputReadLine();
    }

    public void Dispose()
    {
        Stop();
    }
}
