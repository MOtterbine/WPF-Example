using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Web.Script.Serialization;


namespace OS.WPFJamme
{
    public class MainModelView : INotifyPropertyChanged, IDisposable
    {

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private SynchronizationContext syncContext;
        
        
        
        #endregion INotifyPropertyChanged members

        #region Async

        delegate void MethodDelegate(object obj);
        //IAsyncResult MethodDelegate.BeginInvoke(object obj, AsyncCallback cb, object AsyncState);
        //void MethodDelegate.EndInvoke(IAsyncResult ar);

        private void UpdateControls(object obj)
        {
            
            OnPropertyChanged("Processing");
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion Async

        #region Properties and Fields

        private IConnectable _DBCaller = null;
        public bool DBConnected
        {
            get
            {
                if (this._DBCaller != null && this._DBCaller.Connected)
                {
                    this.ConnectionButtonText = "Disconnect";
                    OnPropertyChanged("ConnectionButtonText");
                    return true;
                }
                this.ConnectionButtonText = "Connect";
                OnPropertyChanged("ConnectionButtonText");
                return false;
            }

        }
        DateTime labelDate = DateTime.Now;
        System.Threading.Timer _timer1 = null;
        private bool _Processing = false;
        public bool Processing
        {
            get
            {
                return this._Processing;
            }
            set
            {
                if (value == this._Processing) return;
                this._Processing = value;
                OnPropertyChanged("Processing");
            }
        }
        private void TimerCallback(object sender)
        {
            if (labelDate.Second == DateTime.Now.Second) return;
            OnPropertyChanged("TimeLabel");
        }
        private bool _UserShouldEditValueNow = false;
        public bool UserShouldEditValueNow
        {
            get
            {
                return this._UserShouldEditValueNow;
            }
            set
            {
                if (value == this._UserShouldEditValueNow) return;
                this._UserShouldEditValueNow = value;
                OnPropertyChanged("UserShouldEditValueNow");
            }
        }
        public string URLToRequest
        {
            get
            {
                return Properties.Settings.Default.URLToRequest;
            }
            set
            {
                if (value == Properties.Settings.Default.URLToRequest) return;
                Properties.Settings.Default.URLToRequest = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("URLToRequest");
            }
        }
        private int _RecordCreationCount = 100;
        public int RecordCreationCount
        {
            get
            {
                return this._RecordCreationCount;
            }
            set
            {
                if (value == this._RecordCreationCount) return;
                this._RecordCreationCount = value;
                OnPropertyChanged("RecordCreationCount");
            }
        }
        public string DBConnectionString
        {
            get
            {
                return Properties.Settings.Default.ConnectionString;
            }
            set
            {
                if (value == Properties.Settings.Default.ConnectionString) return;
                Properties.Settings.Default.ConnectionString = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("DBConnectionString");
            }
        }
        public string ConnectionButtonText { get; private set; }
        private bool _ErrorOcurred = false;
        public bool ErrorOcurred
        {
            get
            {
                return this._ErrorOcurred;
            }
            set
            {
                if (value == this._ErrorOcurred) return;
                this._ErrorOcurred = value;
                OnPropertyChanged("ErrorOcurred");
            }
        }
        public string TimeLabel
        {
            get
            {
                return DateTime.Now.ToLongTimeString();
            }
            set
            {
                OnPropertyChanged("TimeLabel");
            }
        }
        public string PolicyID
        {
            get
            {
                return Properties.Settings.Default.PolicyID;
            }
            set
            {
                if (value != Properties.Settings.Default.PolicyID)
                {
                    Properties.Settings.Default.PolicyID = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged("PolicyID");
                }
            }
        }
        private string _InfoText = "";
        public string InfoText
        {
            get
            {
                return _InfoText;
            }
            set
            {
                if (value != _InfoText)
                {
                    _InfoText = value;
                    OnPropertyChanged("InfoText");
                }
            }
        }
        public string TotalCharges
        {
            get
            {
                return "0.00";//return this._DBCaller==null?"$0":string.Format("${0}", (this._DBCaller as DBEngine).TotalCharges);
            }
        }
        public string TotalDebits
        {
            get
            {
                return "0.00";// return this._DBCaller == null ? "$0" : string.Format("${0}", (this._DBCaller as DBEngine).TotalDebits);
            }
        }
        System.Windows.Threading.Dispatcher _dispatcher = null;
        private SerializableDictionary<string, string> _HTMLAddresses = null;
        public SerializableDictionary<string, string> HTMLAddresses
        {
            get
            {
                return this._HTMLAddresses;
            }
        }
        public string HTTPProfile
        {
            get
            {
                return Properties.Settings.Default.HTTPProfile;
            }
            set
            {
                if (value == Properties.Settings.Default.HTTPProfile) return;
                Properties.Settings.Default.HTTPProfile = value;
                Properties.Settings.Default.Save();
                this.URLToRequest = this._HTMLAddresses[value];
                OnPropertyChanged("HTTPProfile");
            }
        }
        public Dictionary<string, string>.KeyCollection AddressKeys
        {
            get
            {
                return this._HTMLAddresses.Keys;
            }
        }
        public Dictionary<string, string>.ValueCollection AddressValues
        {
            get
            {
                return this._HTMLAddresses.Values;
            }
        }

        // persist only while app is running...
        private string _EmailPassword = "";
        public string EmailPassword
        {
            get
            {
                return this._EmailPassword;
            }
            set
            {
                if (value == this._EmailPassword) return;
                this._EmailPassword = value;
                OnPropertyChanged("EmailPassword");
            }
        }

        public string SMTPServer
        {
            get
            {
                return Properties.Settings.Default.SMTPServer;
            }
            set
            {
                if (value == Properties.Settings.Default.SMTPServer) return;
                Properties.Settings.Default.SMTPServer = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("SMTPServer");
            }
        }

        public string EmailUserName
        {
            get
            {
                return Properties.Settings.Default.EmailUserName;
            }
            set
            {
                if (value == Properties.Settings.Default.EmailUserName) return;
                Properties.Settings.Default.EmailUserName = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("EmailUserName");
            }
        }

        public string EmailFrom
        {
            get
            {
                return Properties.Settings.Default.EmailFrom;
            }
            set
            {
                if (value == Properties.Settings.Default.EmailFrom) return;
                Properties.Settings.Default.EmailFrom = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("EmailFrom");
            }
        }

        public string EmailTo
        {
            get
            {
                return Properties.Settings.Default.EmailTo;
            }
            set
            {
                if (value == Properties.Settings.Default.EmailTo) return;
                Properties.Settings.Default.EmailTo = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("EmailTo");
            }
        }

        public string EmailSubject
        {
            get
            {
                return Properties.Settings.Default.EmailSubject;
            }
            set
            {
                if (value == Properties.Settings.Default.EmailSubject) return;
                Properties.Settings.Default.EmailSubject = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("EmailSubject");
            }
        }

        public int SMTPPort
        {
            get
            {
                return Properties.Settings.Default.SMTPPort;
            }
            set
            {
                if (value == Properties.Settings.Default.SMTPPort) return;
                Properties.Settings.Default.SMTPPort = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("SMTPPort");
            }
        }

        private string _EmailBody = "";
        public string EmailBody
        {
            get
            {
                return this._EmailBody;
            }
            set
            {
                if (value == this._EmailBody) return;
                this._EmailBody = value;
                OnPropertyChanged("EmailBody");
            }
        }






        #endregion Properties and Fields

        #region Standard Methods

        public MainModelView(System.Windows.Threading.Dispatcher disp)
        {
            LoadHTMLStrings();
            syncContext = SynchronizationContext.Current;

            // Setup GUI clock
            this._dispatcher = disp;
            this._timer1 = new System.Threading.Timer(TimerCallback, null, 250, 250);
            this.ConnectionButtonText = "Connect";
        }


        private bool DBInit()
        {
            try
            {
                //object typeString = "";
                //Type type = null;


                //// Instantiate encryption object
                //ConfigurationManager.GetDllConfigAppSetting("IPGPEncryption", out typeString);
                //type = Type.GetType(typeString as string);
                //this._Encrypter = (IPGPEncryption)Activator.CreateInstance(type);

                //// Create the IHttpHandler object for Handler1
                //ConfigurationManager.GetDllConfigAppSetting("IProcedureCaller", out typeString);
                //type = Type.GetType(typeString as string);

                //// Create the IFieldInfo object for Handler2
                //this._DBCaller = (IProcedureCaller)Activator.CreateInstance(type, Properties.Settings.Default.ConnectionString);
                //this._DBCaller.StateChanged += _DBCaller_StateChanged;
                return true;
            }
            catch (Exception ex)
            {
                // To Do.....

                throw (ex);
            }
        }
        private void LoadHTMLStrings()
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(SerializableDictionary<string, string>));
            System.IO.TextReader reader = new System.IO.StreamReader("HTMLAddresses.xml");
            this._HTMLAddresses = (SerializableDictionary<string, string>)ser.Deserialize(reader);
            reader.Close();
        }
        private void BuildHTMLStrings()
        {
            this._HTMLAddresses = new SerializableDictionary<string, string>();
            this._HTMLAddresses.Add("Local", "http://localhost:53731/exe.ach?handler=handler1&command=export");
            this._HTMLAddresses.Add("TestServer", "https://test.maple-tech.com/hmfconsumer/mo/exe.ach?handler=handler1&command=export");
            this._HTMLAddresses.Add("Remote", "jamme");
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(this._HTMLAddresses.GetType());
            System.IO.TextWriter writer = new System.IO.StreamWriter("HTMLAddresses.xml");
            ser.Serialize(writer, this._HTMLAddresses);
            writer.Close();
        }

        #endregion Standard Methods

        #region TestCommand

        private RelayCommand testCommand;// or private ICommand testCommand;

        public ICommand TestCommand
        {
            get
            {
                return testCommand ??
                    (testCommand = new RelayCommand(param => Test(), param => CanTest));
            }
        }

        public void Test()
        {
            if (this._DBCaller != null)
            {

                // Update the user label
                this.UserShouldEditValueNow = true;
                OnPropertyChanged("ErrorOcurred");
                OnPropertyChanged("InfoText");
                OnPropertyChanged("UserShouldEditValueNow");
            }
        }
        public bool CanTest
        {

            get
            {
                return this._DBCaller == null ? false : this._DBCaller.Connected;
            }
        }

        #endregion TestCommand

        private class LogEmulator : ILogger
        {
            
            public bool Enabled { get; set; }
            public LogLevels LogLevel { get; set; }
            public string ProcessModuleName { get; private set; }
            public void WriteToLog(string logText, LogLevels level)
            {
                this.ProcessModuleName = "WPF Application";
            }
        }

        #region PopulateDBCommand

        private RelayCommand populateDBCommand;

        public ICommand PopulateDBCommand
        {
            get
            {
                return populateDBCommand ??
                    (populateDBCommand = new RelayCommand(param => PopulateDB(), param => CanPopulateDB));
            }
        }

        public void PopulateDB()
        {
            this.ErrorOcurred = false;
            this.InfoText = "Starting..";
            System.Threading.ThreadPool.QueueUserWorkItem(PopulateDB_thread);
        }

        public void PopulateDB_thread(object obj)
        {
            this.Processing = true;

            try
            {
                if (this._DBCaller == null)
                {
                    this.InfoText = "Connection Object is null";
                    this.ErrorOcurred = true;
                    return;
                }
                //// Instantiate some interface/superclass object
                //object typeString = "";
                //Type type = null;
                //ConfigurationManager.GetDllConfigAppSetting("IDBPopulator", out typeString);
                //type = Type.GetType(typeString as string);
                // this.SomeInterface = (ISomeInterface)Activator.CreateInstance(type, this.SomeInterface);
                //// Perform function
                //this.SomeInterface.SomeMethodCall();
            }
            catch (Exception ex)
            {
                this.InfoText = string.Format("Error: {0}", ex.Message);
                this.ErrorOcurred = true;
            }
            this.Processing = false;
            this.InfoText = "Finished...";
            // Tell the main UI thread to update controls...
            syncContext.Post(delegate { CommandManager.InvalidateRequerySuggested(); }, null);
        }

        public bool CanPopulateDB
        {

            get
            {
                return (this._DBCaller == null ? false : this._DBCaller.Connected) && !this.Processing;
            }
        }

        #endregion PopulateDBCommand

        #region RunURLCommand

        private RelayCommand runURLCommand;// or private ICommand testCommand;

        public ICommand RunURLCommand
        {
            get
            {
                return runURLCommand ??
                    (runURLCommand = new RelayCommand(param => RunURL(), param => CanRunURL));
            }
        }

        public void RunURL()
        {
            this.ErrorOcurred = false;
            this.InfoText = "Connecting...";
            System.Threading.ThreadPool.QueueUserWorkItem(RunURL_thread);
        }

        public void RunURL_thread(object obj)
        {
            this.Processing = true;

            // this points to (true in this case)..which substitutes for a boolean return on the callback's signature definition
            // which determines if certificate validation has succeeded...here, we just return true to get the job done..
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.URLToRequest);

            ASCIIEncoding encoding = new ASCIIEncoding();

            Dictionary<string, object> paramList = new Dictionary<string, object>();
            paramList.Add("ProgramCode", this.PolicyID);
            paramList.Add("FileIDModifier", "A");
            // paramList.Add("EntryDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            paramList.Add("BatchNumber", 1);
            paramList.Add("BatchCount", 1);
            paramList.Add("BlockCount", 1);
            // paramList.Add("BatchID", Guid.NewGuid().ToString());

            //Dictionary<string, object> jObject = new Dictionary<string, object>();
            //jObject.Add("ProgramCodes", paramList);
            //string jData = "[" + new JavaScriptSerializer().Serialize(jObject) + "]";

            //byte[] postBytes = Encoding.UTF8.GetBytes(jData);

            httpWReq.Method = "POST";
            httpWReq.ProtocolVersion = HttpVersion.Version10;
            //   httpWReq.ContentType = "application/json; charset=UTF-8"; //"application/x-www-form-urlencoded";
            httpWReq.ContentLength = 0;// postBytes.Length;
            httpWReq.Accept = "text/html; charset=utf-8";

            //try
            //{
            //    using (System.IO.Stream stream = httpWReq.GetRequestStream())
            //    {
            //        stream.Write(postBytes, 0, postBytes.Length);
            //    }
            //}
            //catch(Exception ex)
            //{
            //    this.InfoText = string.Format("Error connecting to page: {0}", ex.Message);
            //    this.ErrorOcurred = true;
            //    this.Processing = false;
            //    // Tell the main UI thread to update controls...
            //    syncContext.Post(delegate { CommandManager.InvalidateRequerySuggested(); }, null);
            //    return;
            //}
            HttpWebResponse response = null;
            System.IO.Stream resStream = null;
            try
            {
                response = (HttpWebResponse)httpWReq.GetResponse();
                resStream = response.GetResponseStream();
            }
            catch (WebException ex)
            {
                if (ex.Response != null) resStream = ex.Response.GetResponseStream();
                this.InfoText = string.Format("Error: {0}", ex.Message);
                this.ErrorOcurred = true;
            }
            catch (Exception ex)
            {
                this.InfoText = string.Format("Error: {0}", ex.Message);
                this.ErrorOcurred = true;
            }
            finally
            {
                if (resStream != null)
                {
                    using (var reader = new System.IO.StreamReader(resStream))
                    {
                        this.InfoText = reader.ReadToEnd();
                    }
                }
                ServicePointManager.ServerCertificateValidationCallback -= (sender, certificate, chain, sslPolicyErrors) => true;
            }
            CommandManager.InvalidateRequerySuggested();
            this.Processing = false;
            OnPropertyChanged("CanRunURL");
            // Tell the main UI thread to update controls...
            syncContext.Post(delegate { CommandManager.InvalidateRequerySuggested(); }, null);
        }

        public bool CanRunURL
        {

            get
            {
                return !Processing;
            }
        }

        #endregion RunURLCommand

        #region SendEmailCommand

        private RelayCommand sendEmailCommand;// or private ICommand testCommand;

        public ICommand SendEmailCommand
        {
            get
            {
                return sendEmailCommand ??
                    (sendEmailCommand = new RelayCommand(param => SendEmail(), param => CanSendEmail));
            }
        }

        public void SendEmail()
        {
            this.ErrorOcurred = false;
            this.InfoText = "Sending Email...";
            System.Threading.ThreadPool.QueueUserWorkItem(SendEmail_thread);
        }

        public void SendEmail_thread(object obj)
        {
            this.Processing = true;

            try
            {
                Emailer.SendEmail(SMTPServer, SMTPPort, EmailFrom, EmailTo, EmailSubject, EmailBody, true, EmailUserName, EmailPassword);
                this.InfoText = string.Format("Email Sent to {0}", EmailTo);
            }
            catch (WebException ex)
            {
                this.InfoText = string.Format("Error: {0}", ex.Message);
                this.ErrorOcurred = true;
            }
            catch (Exception ex)
            {
                this.InfoText = string.Format("Error: {0} {1}", ex.Message, ex.InnerException==null?"":ex.InnerException.Message);
                this.ErrorOcurred = true;
            }
            finally
            {
                ServicePointManager.ServerCertificateValidationCallback -= (sender, certificate, chain, sslPolicyErrors) => true;
            }
            CommandManager.InvalidateRequerySuggested();
            this.Processing = false;
            OnPropertyChanged("CanSendEmail");
            // Tell the main UI thread to update controls...
            syncContext.Post(delegate { CommandManager.InvalidateRequerySuggested(); }, null);
        }

        public bool CanSendEmail
        {

            get
            {
                return !Processing;
            }
        }

        #endregion SendEmailCommand

        #region DBConnectionCommand

        private RelayCommand _DBConnectionCommand;// or private ICommand testCommand;

        public ICommand DBConnectionCommand
        {


            get
            {
                return _DBConnectionCommand ??
                    (_DBConnectionCommand = new RelayCommand(param => ToggleDBConnection(), param => CanAlterConnection));
            }
        }

        public void ToggleDBConnection()
        {
            try
            {
                CommandManager.InvalidateRequerySuggested();
                // Create the connection
                if (this._DBCaller == null)
                {
                    this.DBInit();
                    this.ErrorOcurred = !this._DBCaller.Connect();
                    if(this._DBCaller is IReportInfo) this.InfoText = (this._DBCaller as IReportInfo).InfoText;
                    // Handle a failed connection here....
                    if (this.ErrorOcurred)
                    {
                        this._DBCaller.Disconnect();
                        if (this._DBCaller is IDisposable) (this._DBCaller as IDisposable).Dispose();
                        this._DBCaller = null;
                        OnPropertyChanged("DBConnected");
                        return;
                    }
                }
                // Delete the connection
                else 
                {
                    // Disconnect
                    if (this._DBCaller.Connected) this.ErrorOcurred = !this._DBCaller.Disconnect();
                    // Get any message text
                    if (this._DBCaller is IReportInfo) this.InfoText = (this._DBCaller as IReportInfo).InfoText;
                    // Dispose the object
                    if (this._DBCaller is IDisposable) (this._DBCaller as IDisposable).Dispose();
                    OnPropertyChanged("DBConnected");
                    this._DBCaller = null;
                    return;
                }
                OnPropertyChanged("DBConnected");
            }
            catch (Exception ex)
            {
                this.ErrorOcurred = true;
                this.InfoText = string.Format("DoDBConnection() - A database connection error ocurred - {0}", ex.Message);
            }
            finally
            {
                if (this._DBCaller != null)
                {
                    if (this._DBCaller is IDisposable) (this._DBCaller as IDisposable).Dispose();
                    this._DBCaller = null;
                }
            }
        }

        void _DBCaller_StateChanged(object sender, System.Data.StateChangeEventArgs e)
        {
            bool x = this.DBConnected;
        }
        public bool CanAlterConnection
        {
            get
            {
                return true;
            }
        }

        #endregion DBConnectionCommand

		#region IDisposable Members

		private bool disposed = false;
		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}
		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the
		// runtime from inside the finalizer and you should not reference
		// other objects. Only unmanaged resources can be disposed.
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{
				    // Call the appropriate methods to clean up
				    // unmanaged resources here.
				}
				// If disposing is false,
				// only the following code is executed.

                this._timer1.Dispose();
                if (this._DBCaller != null)
                {
                    if (this._DBCaller is IDisposable) (this._DBCaller as IDisposable).Dispose();
                    this._DBCaller.StateChanged -= _DBCaller_StateChanged;
                }

				// Note disposing has been done.
				disposed = true;
			}
		}
        ~MainModelView()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
		}
		#endregion

    }
}
