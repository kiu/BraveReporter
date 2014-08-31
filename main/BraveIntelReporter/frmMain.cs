using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml;

namespace BraveIntelReporter
{
    public partial class frmMain : Form
    {
        long reported = 0;
        long failed = 0;

        private enum STATE
        {
            INIT, RUNNING, STOPPED
        };

        private STATE state = STATE.INIT;
        private System.Timers.Timer timerConfigCheck = new System.Timers.Timer();
        private System.Timers.Timer timerFileDiscover = new System.Timers.Timer();
        private System.Timers.Timer timerFileReader = new System.Timers.Timer();

        private Dictionary<String, FileInfo> roomToFile = new Dictionary<String, FileInfo>();
        private Dictionary<String, DateTime> roomToLastUpdate = new Dictionary<String, DateTime>();
        private Dictionary<String, long> fileToOffset = new Dictionary<String, long>();

        private static Object readerLock = new Object(); // Ensures that only one thread can read files at a time.

        #region SetEveToBackground
        /// <summary>
        /// For use with the Set EVE to Background option
        /// </summary>

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up or top-level window.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order. (HWND value)</param>
        /// <param name="X">The new position of the left side of the window, in client coordinates.</param>
        /// <param name="Y">The new position of the top of the window, in client coordinates.</param>
        /// <param name="W">The new width of the window, in pixels.</param>
        /// <param name="H">The new height of the window, in pixels.</param>
        /// <param name="uFlags">The window sizing and positioning flags. (SWP value)</param>
        /// <returns>Nonzero if function succeeds, zero if function fails.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
        private static IntPtr myhandle;
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private static string processname = "exefile";


        private void OnFocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            if (!mnuSetEveToBackground.Checked) return;
            AutomationElement element = src as AutomationElement;

            var processes = Process.GetProcesses().Where(p => p.ProcessName.ToLower() == processname.ToLower()).ToList();

            if (processes.Count > 0)
            {
                Process process = processes[0];

                int id = process.Id;
                SetWindowPos(process.MainWindowHandle, HWND_BOTTOM, 0, 0, 0, 0, SetWindowPosFlags.DoNotReposition | SetWindowPosFlags.IgnoreMove | SetWindowPosFlags.DoNotActivate | SetWindowPosFlags.IgnoreResize);
            }
        }


        [Flags()]
        enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsynchronousWindowPosition = 0x4000,
            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,
            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,
            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,
            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,
            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,
            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
            /// contents of the client area are saved and copied back into the client area after the window is sized or 
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,
            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,
            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,
            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,
            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,
            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,
            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,
            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,
            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }
        #endregion

        // ------------------------------------------------------------------------------

        private void init()
        {
            timerConfigCheck.Elapsed += new ElapsedEventHandler(execConfigCheckTimer);
            timerConfigCheck.Interval = Configuration.ConfigCheckFrequency * 1000 * 60;

            myhandle = this.Handle; // for SetEveToBackground
            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);

            mnuSetEveToBackground.Checked = Configuration.SetEveToBackground;
            mnuOutputVerbose.Checked = Configuration.Verbose;
            mnuOutputMinimal.Checked = !Configuration.Verbose;

            timerFileDiscover.Elapsed += new ElapsedEventHandler(tickFileDiscover);
            timerFileDiscover.Interval = 1000 * 60 * 1;
            timerFileDiscover.Start();

            timerFileReader.Elapsed += new ElapsedEventHandler(tickFileReader);
            timerFileReader.Interval = 1000 * 2;
            timerFileReader.Start();

            tickFileDiscover(null, null);
        }

        // ------------------------------------------------------------------------------

        private void tickFileReader(object sender, EventArgs e)
        {
            if (!Monitor.TryEnter(readerLock))
            {
                Debug.WriteLine("File Reader Thread: Locked");
                return; // Ensures that only one thread can read files at a time.
            }

            foreach (String room in Configuration.RoomsToMonitor)
            {
                parseLatest(room);
            }

            Monitor.Exit(readerLock);
        }

        private void tickFileDiscover(object sender, EventArgs e)
        {
            findLatestIntelFiles();

            DateTime last = DateTime.MinValue;
            foreach (DateTime dt in roomToLastUpdate.Values)
            {
                if (dt > last)
                {
                    last = dt;
                }
            }

            if (state != STATE.STOPPED && roomToFile.Count() == 0)
            {
                state = STATE.STOPPED;
                appendText(Color.DarkRed, "No active intel file found, sleeping... ");
                sendStatus("stop");
            }

            if (state == STATE.RUNNING && last < DateTime.Now.AddMinutes(-1))
            {
                appendVerbose(Color.DarkBlue, "Nothing happened, sending ping!");
                sendStatus("ping");
            }

            if (state != STATE.RUNNING && roomToFile.Count() > 0)
            {
                state = STATE.RUNNING;
                appendText(Color.DarkGreen, "Active intel file found, ready to upload intel...");
                sendStatus("start");
            }

            string monitoring = string.Empty;
            foreach (FileInfo fi in roomToFile.Values)
            {
                monitoring += fi.Name + ", ";
            }
            if (monitoring.Length > 2)
            {
                monitoring = monitoring.Substring(0, monitoring.Length - 2); // trim the last comma and space
            }
            lblMonitoringFiles.Invoke(new MethodInvoker(() => lblMonitoringFiles.Text = monitoring));
        }

        // ------------------------------------------------------------------------------

        private void findLatestIntelFiles()
        {
            Debug.WriteLine("Checking for new logfiles.");
            foreach (String room in Configuration.RoomsToMonitor)
            {
                DateTime last = DateTime.MinValue;
                roomToLastUpdate.TryGetValue(room, out last);
                if (last > DateTime.Now.AddMinutes(-1))
                {
                    Debug.WriteLine("Room " + room + " is up-to-date, skipping.");
                    continue;
                }

                FileInfo fiLast = null;
                roomToFile.TryGetValue(room, out fiLast);
                String fnLast = "";
                if (fiLast != null)
                {
                    fnLast = fiLast.FullName;
                }

                FileInfo fiNew = findLatestIntelFile(room);
                String fnNew = "";
                if (fiNew != null)
                {
                    fnNew = fiNew.FullName;
                }

                if (fnLast != fnNew)
                {
                    roomToFile[room] = fiNew;
                    fileToOffset.Remove(fnLast);
                }
            }
        }

        private FileInfo findLatestIntelFile(String room)
        {
            Debug.WriteLine("findLatestIntelFile: " + room);

            FileInfo[] files = new DirectoryInfo(Configuration.LogDirectory)
                    .GetFiles(room + "_*.txt", SearchOption.TopDirectoryOnly);
            FileInfo fi = files.OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

            if (fi == null)
            {
                Debug.WriteLine("No logfile found, skipping room.");
                return null;
            }

            Debug.WriteLine("Found logfile: " + fi);

            // Check if eve has opened this file -> Eve is running and user has joined channel
            try
            {
                FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                fs.Close();
                Debug.WriteLine("Logfile is not used, skipping room.");
                return null;
            }
            catch
            {
            }

            Debug.WriteLine("Found active logfile: " + fi);
            return fi;
        }

        private void parseLatest(string room)
        {

            FileInfo logfile = null;
            roomToFile.TryGetValue(room, out logfile);
            if (logfile == null)
            {
                return;
            }

            logfile.Refresh();

            long offset = 0;
            fileToOffset.TryGetValue(logfile.FullName, out offset);
            Debug.WriteLine("Room " + room + " - length: " + logfile.Length.ToString() + " offset: " + offset.ToString());

            if (offset == 0)
            {
                fileToOffset[logfile.FullName] = logfile.Length;
                return;
            }

            if (offset == logfile.Length)
            {
                return;
            }

            StreamReader logFileReader = new StreamReader(new FileStream(logfile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            logFileReader.BaseStream.Seek(offset, SeekOrigin.Begin);

            String line;
            while (!logFileReader.EndOfStream)
            {
                line = logFileReader.ReadLine();
                if (line.Trim().Length == 0)
                {
                    continue;
                }

                if (line.Length < 23)
                {
                    continue;
                }

                sendIntel(line);
                roomToLastUpdate[room] = DateTime.Now;
            }

            fileToOffset[logfile.FullName] = logfile.Length;
            logFileReader.Close();
        }

        // ------------------------------------------------------------------------------

        private void sendIntel(string line)
        {
            line = line.Replace('"', '\'');
            appendText(Color.Black, line);
            send(new ReportLine(line, "running"));
        }

        private void sendStatus(string status)
        {
            send(new ReportLine("", status));
        }

        private void send(ReportLine rl)
        {
            Encoding myEncoding = System.Text.UTF8Encoding.UTF8;
            WebClient client = new WebClient();
            try
            {
                client.UploadData(Configuration.ReportServer, "PUT", myEncoding.GetBytes(rl.ToJson()));
                reported++;
            }
            catch (WebException ex)
            {
                HttpWebResponse hwr = (HttpWebResponse)ex.Response;
                failed++;

                if (hwr.StatusCode == HttpStatusCode.Unauthorized)
                {
                    appendText(Color.Red, "Authorization Token Invalid.  Try refreshing your auth token in settings.\r\n");
                }
                else if (hwr.StatusCode == HttpStatusCode.UpgradeRequired)
                {
                    appendText(Color.Red, "Client version not supported.  Please close and restart application to update. (May require two restarts.)\r\n");
                }
                else if (hwr.StatusCode == HttpStatusCode.InternalServerError)
                {
                    appendText(Color.Red, "The server is confused right now, should be resolved soon.\r\n");
                }
                else
                {
                    appendText(Color.Red, string.Format("Unknown error ocured: {0} {1}\r\n", hwr.StatusCode, ex.Message));
                }
            }
            catch (Exception ex)
            {
                appendText(Color.Red, string.Format("Unknown error ocured: {0}\r\n", ex.Message));
            }

            lblReported.Invoke(new MethodInvoker(() => lblReported.Text = reported.ToString()));
            lblFailed.Invoke(new MethodInvoker(() => lblFailed.Text = failed.ToString()));
        }

        // ------------------------------------------------------------------------------

        private void appendText(Color c, String line)
        {
            Debug.WriteLine("I: " + line);
            this.rtbIntel.Invoke(new MethodInvoker(() => this.rtbIntel.SelectionColor = c));
            this.rtbIntel.Invoke(new MethodInvoker(() => this.rtbIntel.AppendText(line + "\r\n")));
        }

        private void appendVerbose(Color c, string line)
        {
            Debug.WriteLine("V: " + line);
            if (Configuration.Verbose)
            {
                this.rtbIntel.Invoke(new MethodInvoker(() => this.rtbIntel.SelectionColor = c));
                this.rtbIntel.Invoke(new MethodInvoker(() => this.rtbIntel.AppendText(line + "\r\n")));
            }
        }

        // ------------------------------------------------------------------------------

        private void execConfigCheckTimer(object sender, EventArgs e)
        {
            string report = string.Empty;
            Configuration.GetConfig(out report);
            appendVerbose(Color.DarkGreen, report);
        }

        // ------------------------------------------------------------------------------

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            string report = string.Empty;
            bool haveglobalsettings = false;
            while (!haveglobalsettings)
            {
                haveglobalsettings = Configuration.GetConfig(out report);
                if (!haveglobalsettings)
                {
                    appendText(Color.DarkOrange, report);
                    appendText(Color.DarkOrange, "Waiting 30 seconds and retrying.");
                    for (int i = 1; i < 300; i++) // A lazy way of waiting 30 seconds but keeping the UI responsive without multithreading. 
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }

            if (Configuration.FirstRun)
            {
                new frmSettings().ShowDialog();
            }
            init();

        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipText = "Minimized to system tray.";
                notifyIcon1.ShowBalloonTip(500);
                this.ShowInTaskbar = false;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmAbout().ShowDialog();
        }

        private void mnuViewMap_Click(object sender, EventArgs e)
        {
            if (Configuration.MapURL != string.Empty)
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.MapURL);
                Process.Start(sInfo);
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            appendText(Color.Red, "Shutting down...");
            sendStatus("stop");
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
        }

        private void mnuOutputMinimal_Click(object sender, EventArgs e)
        {
            Configuration.Verbose = false;
            Configuration.Save();
            mnuOutputMinimal.Checked = true;
            mnuOutputVerbose.Checked = false;
        }

        private void mnuOutputVerbose_Click(object sender, EventArgs e)
        {
            Configuration.Verbose = true;
            Configuration.Save();
            mnuOutputMinimal.Checked = false;
            mnuOutputVerbose.Checked = true;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mnuSetEveToBackground_Click(object sender, EventArgs e)
        {
            mnuSetEveToBackground.Checked = !mnuSetEveToBackground.Checked;
            Configuration.SetEveToBackground = mnuSetEveToBackground.Checked;
            Configuration.Save();
        }


    }
}
