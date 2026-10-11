// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Services
{
    using Democrite.Toolbox.Abstractions.Services;
    using Democrite.Toolbox.Disposables;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Proxy pilot about a external execution process
    /// </summary>
    /// <seealso cref="IExternalProcess" />
    internal sealed class ExternalProcess : ExternalBaseProcess, IExternalProcess
    {
        #region Fields

        private readonly ProcessStartInfo _info;
        
        private Process? _process;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalProcess"/> class.
        /// </summary>
        public ExternalProcess(ProcessStartInfo info,
                               IReadOnlyCollection<string> arguments,
                               CancellationToken cancellationToken)
            : base(arguments, cancellationToken)
        {
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = false;

            this._info = info;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public override string Executable
        {
            get { return this._info.FileName; }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override Task KillAsync(CancellationToken cancellationToken)
        {
            Process? process;
            lock (this._info)
            {
                process = this._process;
            }

            if (process == null && this.ExitCode == null)
                throw new InvalidOperationException("Run process first");

            if (process == null)
                return Task.CompletedTask;

            return Task.Run(() => process.Kill(true), cancellationToken);
        }

        /// <inheritdoc />
        public override string ToDebugDisplayName()
        {
            return this._info?.ToString() ?? this.Executable;
        }

        /// <inheritdoc />
        protected override void OnDisposeThreadSafeBegin()
        {
            this._process?.Kill(true);
        }

        /// <summary>
        /// Start configured process
        /// </summary>
        protected override Task OnRunAsync(CancellationToken cancellationToken)
        {
            try
            {
                lock (this._info)
                {
                    if (this._process != null)
                        throw new InvalidOperationException("Could not start a running process");

                    this._process = Process.Start(this._info);

                    if (this._process == null)
                    {
                        throw new InvalidOperationException("Process information are invalid");
                    }

                    //this._process.Exited += Process_Exited;
                    this._process.OutputDataReceived += Process_OutputDataReceived;
                    this._process.ErrorDataReceived += Process_ErrorDataReceived;

                    this._process.EnableRaisingEvents = true;

                    if (!this._process.Start())
                    {
                        throw new InvalidOperationException("Process couldn't start");
                    }

                    this._process.BeginOutputReadLine();
                    this._process.BeginErrorReadLine();

                    var processTask = this._process.WaitForExitAsync(cancellationToken);

                    processTask = processTask.ContinueWith(t =>
                    {
                        this.ExitCode = this._process.ExitCode;
                        this._process = null;
                    });

                    base.SetProcessWaitingTask(processTask);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(nameof(ProcessStartInfo), this._info);
                throw;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Handles the OutputDataReceived event of the Process control.
        /// </summary>
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            base.ProcessOutputDataReceived(e?.Data);
        }

        /// <summary>
        /// Handles the ErrorDataReceived event of the Process control.
        /// </summary>
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            base.ProcessErrorDataReceived(e?.Data);
        }

        #endregion
    }
}
