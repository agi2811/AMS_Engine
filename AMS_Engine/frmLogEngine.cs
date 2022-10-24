using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AMS_Engine
{
    public partial class frmLogEngine : DevExpress.XtraEditors.XtraForm
    {
        private delegate void myUICallBack(string myStr, RichTextBox ctl);
        private string ClientID = "KMK_ENGINE_";
        public string plantID = "K1";
        public string plantName = "D1";
        public string LampID = "KD1_LAMP";
        public MqttClient client;
        classMain clMain = new classMain();
        CancellationToken token;
        ServiceController service;

        string[] arrDataReceive;
        string varzoneid;
        string varreceiverid;
        string varmonitorid;
        string varzonecode;
        string varreceivercode;
        string varempid;
        string varempname;
        string varempphone;

        string varregister;
        string vardesc;
        string vartypeid;
        string varinstruction;
        string varstatus;
        string varcallflag;
        string varactivetrans;

        string stridcall;
        string strtask;
        string strinst;
        string strplant;
        string stroper;
        string strline;
        string strzone;
        string strzoneid;
        string strzonenum;

        string strempid;
        string strempname;
        string strphone;
        string strserialmonitor;
        string strurl;
        string strcountcall;
        string strCreateDate;
        string strCreateTime;

        string flagOnOff = "";
        string pubLightCode = "";
        string iCountCall = "";
        string strtmpmsg = "";
        string myIP = "";

        int iCol;
        int iColTmp;
        int intMessage = 0;
        //int intcallcount2 = 0;

        int flagExit = 0;
        int flagConnect = 0;
        int flagNotConnect = 0;
        int flagDisconnect = 0;
        int flagLoadCallActive = 0;
        int intcountstop = 0;

        public frmLogEngine()
        {
            InitializeComponent();
            getAutoStartService();
            getIPAddress();
            client_MqttConnectPublishReceived();
        }

        public void getIPAddress()
        {
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            myIP = Dns.GetHostByName(strHostName).AddressList[0].ToString();
        }

        public static void closeform()
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "frmLogEngine")
                    Application.OpenForms[i].Close();
            }
        }

        private void actionChildForm(Form strChild)
        {
            //strChild.MdiParent = this;
            closeform();
            strChild.BringToFront();
        }

        protected override void OnClosed(EventArgs e)
        {
            flagExit = 1;

            if (flagDisconnect == 0)
            {
                client.Disconnect();
            }

            base.OnClosed(e);
            Application.Exit();
        }

        private void EventClosed(Object sender, EventArgs e)
        {
            if (flagExit == 0)
            {
                Task.Run((Func<Task>)(() => TryReconnectAsync(token)));
            }         
        }

        private async Task TryReconnectAsync(CancellationToken cancellationToken)
        {
            var connected = client.IsConnected;
            while (!connected && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    client.Connect(ClientID + plantID + "_" + plantName); //Guid.NewGuid().ToString()
                    string[] topics = new string[] { "rcv_engine" };
                    byte[] qosLevels = new byte[] { 2 };
                    client.Subscribe(topics, qosLevels);

                    if (flagConnect == 0)
                    {
                        getMessageMyUIConnect();
                        flagConnect = 1;
                        flagNotConnect = 0;
                    }

                    flagDisconnect = 0;
                }
                catch
                {
                    if (flagNotConnect == 0)
                    {
                        getMessageMyUIDisconnect();
                        flagConnect = 0;
                        flagNotConnect = 1;
                    }

                    flagDisconnect = 1;
                }

                connected = client.IsConnected;
                await Task.Delay(1000, cancellationToken);
                getAutoStartService();
            }
        }

        private void myUI(string myStr, RichTextBox ctl)
        {
            if (this.InvokeRequired)
            {
                myUICallBack myUpdate = new myUICallBack(myUI);
                this.Invoke(myUpdate, myStr, ctl);
            }
            else
            {
                if (iCol == 0) { ctl.SelectionColor = Color.Lime; }
                if (iCol == 1) { ctl.SelectionColor = Color.White; }
                if (iCol == 2) { ctl.SelectionColor = Color.Yellow; }
                if (iCol == 3) { ctl.SelectionColor = Color.OrangeRed; }
                if (iCol == 4) { ctl.SelectionColor = Color.Tomato; }
                if (iCol == 5) { ctl.SelectionColor = Color.BlueViolet; }
                if (iCol == 6) { ctl.SelectionColor = Color.LightGreen; }
                ctl.AppendText(myStr + Environment.NewLine);
            }
        }

        public void client_MqttConnectPublishReceived()
        {
            try
            {
                client = new MqttClient(classMain.ipMqtt, 1883, false, null);
                client.Connect(ClientID + plantID + "_" + plantName); //Guid.NewGuid().ToString()
                client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);
                client.ConnectionClosed += new MqttClient.ConnectionClosedEventHandler(EventClosed);
                getMessageMyUIConnect();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Error Description", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flagDisconnect = 1;
                return;
            }
        }

        private void getAutoStartService()
        {
            service = new ServiceController("mosquitto");

            if ((service.Status.Equals(ServiceControllerStatus.Stopped)) ||
                (service.Status.Equals(ServiceControllerStatus.StopPending)))
            {
                service.Start();
            }

            service.Dispose();
            service.Close();
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            char[] separator = new char[] { '#' };
            arrDataReceive = Encoding.UTF8.GetString(e.Message).Split(separator);

            if (Encoding.UTF8.GetString(e.Message) != "")
            {
                if (arrDataReceive.Count() == 4) getValdatePublishReceived();
            }
        }

        private void getValdatePublishReceived()
        {
            intMessage = 1;
            strtask = strTask();
            getValidateTransmitterCount();
            if (varcallflag != "2") getValidateReceiverData();

            if (varcallflag == "0")
            {
                iCol = 1;
                myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Receive " +
                    strCodeInstruction() + " call data to display on the andon monitoring system with receiver code " +
                    arrDataReceive[2] + " and transmitter code " + arrDataReceive[0], MessageTextBox);
            }
            else if (varcallflag == "1")
            {
                client.Subscribe(new string[] { "rcv_engine" }, new byte[] { 2 });

                if (strtask == "Calling")
                {
                    if (varactivetrans == "0")
                    {
                        getMessageMyUIWarningLog();
                    }
                    else if (varactivetrans == "1")
                    {
                        getMessageMyUICallRunning();
                    }
                }
                else if (strtask == "Stop")
                {
                    if (varactivetrans == "0")
                    {
                        getMessageMyUIWarningLog();
                    }
                    else if (varactivetrans == "1")
                    {
                        getMessageMyUICallActive();
                    }
                }
                else
                {
                    if (varactivetrans == "0" || varactivetrans == "2")
                    {
                        getMessageMyUIWarningLog();
                    }
                }
            }
            else
            {
                if (varactivetrans == "0")
                {
                    getMessageMyUITransCount();
                }
            }

            getLabelMsissingReceiver();
            getNewLineLog();
        }

        private void getLabelMsissingReceiver()
        {
            if (varactivetrans == "2")
            {
                lbMissReceiver.Invoke(new Action(() => lbMissReceiver.Text = "* Please check if the transmitter code " + arrDataReceive[0] + "" +
                    " is registered in receiver code " + arrDataReceive[2] + ""));
            }
            else
            {
                lbMissReceiver.Invoke(new Action(() => lbMissReceiver.Text = ""));
            }
        }

        private void getMessageMyUICallRunning()
        {
            iCol = 3;
            myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") +
                "] The call process is running with the receiver code " + arrDataReceive[2] +
                " transmitter code " + arrDataReceive[0] + ", please wait for the process to complete ...", MessageTextBox);
        }

        private void getMessageMyUICallActive()
        {
            iCol = 4;
            myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] There is no active call process ...", MessageTextBox);
            getCheckPublishLightMessage();
        }

        private void getMessageMyUIWarningLog()
        {
            if (varactivetrans == "0" || varactivetrans == "2")
            {
                iCol = 5;
                myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Please check whether the receiver code is " + arrDataReceive[2] +
                     " and the transmitter code " + arrDataReceive[0] + " used is registered and in accordance with the data specified ...", MessageTextBox);
                //myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Please check if the transmitter you are using is registered and is in accordance with the data specified ...", MessageTextBox);
            }
        }

        private void getMessageMyUITransCount()
        {
            if (varactivetrans == "0")
            {
                iCol = 6;
                myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Data couldn't be sent because transmitter " +
                    "data is over than 55, with the receiver code is " + arrDataReceive[2] + " ...", MessageTextBox);
            }
        }

        private void getMessageMyUIConnect()
        {
            iCol = 0;
            iColTmp = 0;
            myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Engine has successfully connected with Mqtt ...", MessageTextBox);
            tConnectionTmp.Text = "[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Engine has successfully connected with Mqtt ...";
        }

        private void getMessageMyUIDisconnect()
        {
            iCol = 1;
            iColTmp = 1;
            myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") +  "] Engine cannot connect with MQTT ...", MessageTextBox);
            tConnectionTmp.Text = "[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Engine cannot connect with MQTT ...";
        }

        private void frmLogEngine_Load(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string version = assembly.GetName().Version.ToString();
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            AssemblyDescriptionAttribute descriptionAttribute = (AssemblyDescriptionAttribute)attributes[0];
            AssemblyCopyrightAttribute copyright = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(
                AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute;

            this.Text = descriptionAttribute.Description + " " + version.Substring(0, 3);
            this.Text = this.Text + " ~ " + plantID + "-" + plantName + " ~ IP Address : " + myIP;
            lbCopyright.Text = "* " + copyright.Copyright;
            QosComboBox.SelectedIndex = 0;
            client.Subscribe(new string[] { "rcv_engine" }, new byte[] { 2 });
            if (flagLoadCallActive == 0) { getCheckingCallActive(); }
        }

        public void getCheckingCallActive()
        {
            string strselect;
            string varDataReceived;
            string varButtonNumber= strButtonNumber();
            char[] separator = new char[] { '#' };

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strselect = classQuery.strQueryDataCallActive;
                strselect = strselect + @"WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                     AND andon_call_head.status = '1'
                   GROUP BY andon_call_head.transmitter_code
                          , andon_call_head.receiver_code";
                classMain.rdCheckActive = new SqlCommand(strselect,
                    classMain.con).ExecuteReader();

                while (classMain.rdCheckActive.Read())
                {
                    varDataReceived = classMain.rdCheckActive.GetString(0) + "#DAT#" + classMain.rdCheckActive.GetString(1) + "#" + varButtonNumber;
                    arrDataReceive = varDataReceived.Split(separator);
                    getValdatePublishReceived();
                }

                flagLoadCallActive = 1;
                classMain.rdCheckActive.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public string strCodeInstruction()
        {
            string strselect;
            string varValue = "";

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strselect = classQuery.strQueryCodeInstruction;
                strselect = strselect + @"WHERE master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                    AND master_transmitter_detail.code = '" + arrDataReceive[0] + @"'
                    AND master_type_detail.button_number = '" + arrDataReceive[3] + @"'";

                classMain.cmd = new SqlCommand(strselect, classMain.con);
                classMain.rd = classMain.cmd.ExecuteReader();

                if (classMain.rd.Read())
                {
                    varValue = classMain.rd.GetString(0);
                }

                classMain.cmd.Dispose();
                classMain.rd.Close();
                return varValue;
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public string strButtonNumber()
        {
            string strselect;
            string varValue = "";

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strselect = classQuery.strQueryButtonNumber;
                strselect = strselect + @"WHERE LOWER(master_instruction.task) = 'stop'";

                classMain.cmd = new SqlCommand(strselect, classMain.con);
                classMain.rd = classMain.cmd.ExecuteReader();

                if (classMain.rd.Read())
                {
                    varValue = classMain.rd.GetString(0);
                }

                classMain.cmd.Dispose();
                classMain.rd.Close();
                return varValue;
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public string strTask()
        {
            string strselect;
            string varValue = "";

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strselect = classQuery.strQueryTask;
                strselect = strselect + @"WHERE master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                    AND master_transmitter_detail.code = '" + arrDataReceive[0] + @"'
                    AND master_type_detail.button_number = '" + arrDataReceive[3] + @"'";

                classMain.cmd = new SqlCommand(strselect, classMain.con);
                classMain.rd = classMain.cmd.ExecuteReader();

                if (classMain.rd.Read())
                {
                    varValue = classMain.rd.GetString(0);
                }

                classMain.cmd.Dispose();
                classMain.rd.Close();
                return varValue;
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public string strCountCall()
        {
            string strcallcount;
            string varValue = "";

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strcallcount = @"SELECT COUNT(id) AS count_code FROM andon_call_head WHERE status <> '5'";
                classMain.cmd = new SqlCommand(strcallcount, classMain.con);
                classMain.rd = classMain.cmd.ExecuteReader();

                if (classMain.rd.Read())
                {
                    varValue = classMain.rd.GetValue(0).ToString();
                }

                classMain.cmd.Dispose();
                classMain.rd.Close();
                return varValue;
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        private double getRegisterCode(string buttonid)
        {
            double v_code_reg = 1;

            for (int i = 0; i < 5; i++)
            {
                string character = buttonid.Substring(i, 1);
                char c = character[0];
                int n;
                bool isNumeric = int.TryParse(character, out n);
                if (isNumeric)
                {
                    v_code_reg = v_code_reg * (n + 1);
                }
                else
                {
                    int index = char.ToUpper(c) - 64 + 10;
                    v_code_reg = v_code_reg * index;
                }
            }

            v_code_reg = ((v_code_reg * 154258) + 55889977) * 87;
            return v_code_reg = Convert.ToDouble(v_code_reg.ToString().Substring(0, 8));
        }

        public void getValidateTransmitterCount()
        {
            string strtranscount;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strtranscount = @"SELECT COUNT(master_transmitter_detail.code) AS count_code
                    FROM (master_transmitter_head
                       INNER JOIN master_transmitter_detail
                          ON master_transmitter_head.id = master_transmitter_detail.id
					   INNER JOIN ((master_zone_head
					      INNER JOIN master_zone_detail
						     ON master_zone_head.id = master_zone_detail.id)
					      INNER JOIN ((master_line
						     INNER JOIN master_plant
							    ON master_line.plant_id = master_plant.id)
						     INNER JOIN master_operation
							    ON master_line.operation_id = master_operation.id)
						     ON master_zone_head.line_id = master_line.id)
					      ON master_transmitter_head.zone_id = master_zone_head.id
						  and master_transmitter_head.zone_code = master_zone_detail.code)
                    WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                      AND master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                      AND master_transmitter_detail.status = 1";

                classMain.cmd = new SqlCommand(strtranscount, classMain.con);
                classMain.rd = classMain.cmd.ExecuteReader();

                if (classMain.rd.Read())
                {
                    if (int.Parse(classMain.rd.GetValue(0).ToString()) > 55)
                    {
                        varcallflag = "2";
                        varactivetrans = "0";
                    }
                    else
                    {
                        getValidateCallHead();
                    }
                }

                classMain.cmd.Dispose();
                classMain.rd.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getValidateCallHead()
        {
            string strcallhead;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strcallhead = classQuery.strQueryCallHead;
                strcallhead = strcallhead + @"WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                      AND master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                      AND CASE WHEN andon_call_head.transmitter_code is null THEN master_transmitter_detail.code 
                          ELSE andon_call_head.transmitter_code END = '" + arrDataReceive[0] + @"'
                      AND andon_call_head.status = 1
                    ORDER BY 1 ASC";

                classMain.cmd = new SqlCommand(strcallhead, classMain.con);
                classMain.rd = classMain.cmd.ExecuteReader();

                if (classMain.rd.Read())
                {
                    varcallflag = strtask == "Calling" ? classMain.rd.GetString(0) : "0";
                }
                else
                {
                    varcallflag = strtask == "Calling" ? "0" : "1";
                }

                classMain.cmd.Dispose();
                classMain.rd.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getValidateReceiverData()
        {
            string strcallhead;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strcallhead = @"SELECT CASE WHEN master_transmitter_detail.register = '" + getRegisterCode(arrDataReceive[0]) + @"' THEN '0' ELSE '1' END AS call_flag
                                     , master_transmitter_detail.status
                                FROM ((master_transmitter_head
                                   INNER JOIN master_transmitter_detail
                                      ON master_transmitter_head.id = master_transmitter_detail.id
                                      INNER JOIN (master_type_head
                                         INNER JOIN (master_type_detail
                                            INNER JOIN master_instruction
                                               ON master_type_detail.instruction_id = master_instruction.id)
                                            ON master_type_head.id = master_type_detail.id)
                                         ON master_transmitter_detail.type_id = master_type_head.id)
								   INNER JOIN ((master_zone_head
								      INNER JOIN master_zone_detail
									     ON master_zone_head.id = master_zone_detail.id)
								      INNER JOIN ((master_line
									     INNER JOIN master_plant
										    ON master_line.plant_id = master_plant.id)
									     INNER JOIN master_operation
										    ON master_line.operation_id = master_operation.id)
									     ON master_zone_head.line_id = master_line.id)
									  ON master_transmitter_head.zone_id = master_zone_head.id
									  AND master_transmitter_head.zone_code = master_zone_detail.code)
                                WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                                  AND master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                                  AND master_transmitter_detail.code = '" + arrDataReceive[0] + @"'
                                GROUP BY master_transmitter_detail.register
                                    , master_transmitter_detail.status
                                ORDER BY 1 ASC";

                classMain.cmdCheck1 = new SqlCommand(strcallhead, classMain.con);
                classMain.rdCheck1 = classMain.cmdCheck1.ExecuteReader();

                if (classMain.rdCheck1.Read())
                {
                    //varcallflag = rdcallhead.GetValue(1).ToString() == "1" ? rdcallhead.GetString(0) : "1";
                    //varactivetrans = rdcallhead.GetValue(1).ToString() == "1" ? "1" : "0";
                    getValidateRegisterCode();
                }
                else
                {
                    varcallflag = "1";
                    varactivetrans = "2";
                }

                classMain.cmdCheck1.Dispose();
                classMain.rdCheck1.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getValidateRegisterCode()
        {
            string strcallhead;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strcallhead = @"SELECT CASE WHEN master_transmitter_detail.register = '" + getRegisterCode(arrDataReceive[0]) + @"' THEN '0' ELSE '1' END AS call_flag
                                FROM ((((master_transmitter_head
                                    INNER JOIN master_transmitter_detail
                                        ON master_transmitter_head.id = master_transmitter_detail.id
                                    INNER JOIN (master_type_head
                                       INNER JOIN (master_type_detail
                                          INNER JOIN master_instruction
                                             ON master_type_detail.instruction_id = master_instruction.id)
                                          ON master_type_head.id = master_type_detail.id)
                                       ON master_transmitter_detail.type_id = master_type_head.id)
                                    INNER JOIN ((master_zone_head
                                       INNER JOIN master_zone_detail
                                          ON master_zone_head.id = master_zone_detail.id)
									   INNER JOIN ((master_line
									      INNER JOIN master_plant
										     ON master_line.plant_id = master_plant.id)
									      INNER JOIN master_operation
										     ON master_line.operation_id = master_operation.id)
									      ON master_zone_head.line_id = master_line.id)
                                       ON master_transmitter_head.zone_id = master_zone_head.id
                                       AND master_transmitter_head.zone_code = master_zone_detail.code)
                                    INNER JOIN (master_receiver_head
                                       INNER JOIN master_receiver_detail
                                          ON master_receiver_head.id = master_receiver_detail.id)
                                       ON master_transmitter_head.receiver_id = master_receiver_head.id
                                       AND master_transmitter_head.receiver_code = master_receiver_detail.code)
                                    INNER JOIN master_monitor
                                       ON master_transmitter_head.monitor_id = master_monitor.id)
                                WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                                  AND master_transmitter_detail.code = '" + arrDataReceive[0] + @"'
                                  AND master_transmitter_detail.status = '1'
                                GROUP BY master_transmitter_detail.register
                                ORDER BY 1 ASC";

                classMain.cmdCheck2 = new SqlCommand(strcallhead, classMain.con);
                classMain.rdCheck2 = classMain.cmdCheck2.ExecuteReader();

                if (classMain.rdCheck2.Read())
                {
                    if (varcallflag != "0")
                    {
                        varactivetrans = classMain.rdCheck2.GetString(0) == "0" ? "1" : "0";
                        return;
                    }

                    varcallflag = classMain.rdCheck2.GetString(0);
                    varactivetrans = varcallflag == "0" ? "1" : "0";
                }
                else
                {
                    varcallflag = "1";
                    varactivetrans = "0";
                }

                classMain.cmdCheck2.Dispose();
                classMain.rdCheck2.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getPublishIndicator()
        {
            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                if (varcallflag != "0") { return; }

                getCheckSavingHead();
                getPublishDataTransmitter();
                getUpdateData();
                strcountcall = strCountCall();

                if (intMessage == 1)
                {
                    if (intcountstop != 0)
                    {
                        intMessage = 2;
                        intcountstop = 0;

                        getSavingEnginLog();
                        getPublishWebMessage();
                        getCheckPublishLightMessage();
                        getSendLogMessage();
                    }
                }
            }
            catch
            {
                return;
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getPublishDataTransmitter()
        {
            string strselect;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                if (strtask == "Calling")
                {
                    strselect = classQuery.strQueryDataTransmitter;
                    strselect = strselect + @"WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                        AND andon_call_head.receiver_code = '" + arrDataReceive[2] + @"'
                        AND andon_call_head.transmitter_code = '" + arrDataReceive[0] + @"'
                        AND andon_call_head.button_number = '" + arrDataReceive[3] + @"'
                        AND andon_call_head.status = '1'";
                }
                else
                {
                    strselect = classQuery.strQueryDataStop;
                    strselect = strselect + @"WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                        AND master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                        AND master_transmitter_detail.code = '" + arrDataReceive[0] + @"'
                        AND master_instruction.task = '" + strtask + @"'
                        AND CASE WHEN sqlCount.countCall is null THEN 0 ELSE sqlCount.countCall END <> 0";
                }

                classMain.cmdCheck1 = new SqlCommand(strselect, classMain.con);
                classMain.rdCheck1 = classMain.cmdCheck1.ExecuteReader();

                if (classMain.rdCheck1.Read())
                {
                    if (intMessage == 1)
                    {
                        intcountstop = 1;
                        stridcall = classMain.rdCheck1.GetValue(0).ToString();
                        strinst = classMain.rdCheck1.GetString(2);
                        strplant = classMain.rdCheck1.GetString(3);
                        stroper = classMain.rdCheck1.GetString(4);
                        strline = classMain.rdCheck1.GetString(5);
                        strzonenum = classMain.rdCheck1.GetString(6);
                        strurl = classMain.rdCheck1.GetString(7);
                        strCreateDate = classMain.rdCheck1.GetDateTime(9).ToString("dd-MM-yyyy");
                        strCreateTime = classMain.rdCheck1.GetDateTime(9).ToString("H:mm:ss");
                        strzone = classMain.rdCheck1.GetString(11);
                        strzoneid = classMain.rdCheck1.GetValue(12).ToString();
                        strserialmonitor = classMain.rdCheck1.GetString(13);
                        getPublishDataPerson();
                    }
                    else
                    {
                        intcountstop = 0;
                    }
                }
                else
                {
                    intcountstop = 0;
                }

                classMain.cmdCheck1.Dispose();
                classMain.rdCheck1.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getPublishDataPerson()
        {
            string strselect;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strselect = classQuery.strQueryDataPerson + @"
                    WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
				      AND master_line.code = '" + strline + @"'
				      AND job_desc = '" + strinst + @"'
					  AND master_person_detail.status = '1'
                    GROUP BY master_person_detail.employee_id
                       , master_person_detail.employee_name
                       , master_person_detail.phone_number";

                classMain.cmdDet = new SqlCommand(strselect, classMain.con);
                classMain.rdDet = classMain.cmdDet.ExecuteReader();

                if (classMain.rdDet.Read())
                {
                    strempid = classMain.rdDet.GetString(0);
                    strempname = classMain.rdDet.GetString(1);
                    strphone = classMain.rdDet.GetString(2);
                }
                else
                {
                    strempid = "";
                    strempname = "";
                    strphone = "";
                }

                classMain.cmdDet.Dispose();
                classMain.rdDet.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getSavingEnginLog()
        {
            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                string[] textArray1 = new string[35];
                textArray1[0] = arrDataReceive[0];
                textArray1[1] = "#"; textArray1[2] = arrDataReceive[2];
                textArray1[3] = "#"; textArray1[4] = arrDataReceive[3];
                textArray1[5] = "#"; textArray1[6] = stridcall;
                textArray1[7] = "#"; textArray1[8] = strtask;
                textArray1[9] = "#"; textArray1[10] = strinst;
                textArray1[11] = "#"; textArray1[12] = strplant;
                textArray1[13] = "#"; textArray1[14] = stroper;
                textArray1[15] = "#"; textArray1[16] = strline;
                textArray1[17] = "#"; textArray1[18] = strzonenum;
                textArray1[19] = "#"; textArray1[20] = strurl;
                textArray1[21] = "#"; textArray1[22] = strcountcall;
                textArray1[23] = "#"; textArray1[24] = strCreateDate;
                textArray1[25] = "#"; textArray1[26] = strCreateTime;
                textArray1[27] = "#"; textArray1[28] = strempid;
                textArray1[29] = "#"; textArray1[30] = strempname;
                textArray1[31] = "#"; textArray1[32] = strphone;
                textArray1[33] = "#"; textArray1[34] = strserialmonitor;

                string str2 = string.Concat(textArray1);
                classMain.cmd = new SqlCommand(" INSERT INTO andon_engine_log (log_date, log_description) VALUES (GETDATE(),  '" + str2 + "')", classMain.con);
                classMain.cmd.ExecuteNonQuery();
                classMain.cmd.Dispose();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getSendLogMessage()
        {
            iCol = (strTask() == "Stop") ? 2 : 1;
            myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] " + "Sending " + strCodeInstruction() +
                " call data to display on the andon monitoring system from the receiver code " + arrDataReceive[2] +
                " and transmitter code " + arrDataReceive[0], MessageTextBox);
        }

        public void getPublishWebMessage()
        {
            client.Publish("egn_web", Encoding.UTF8.GetBytes(arrDataReceive[0] + "#" + arrDataReceive[2] + "#" +
                arrDataReceive[3] + "#" + stridcall + "#" + strtask + "#" + strinst + "#" + strplant + "#" +
                stroper + "#" + strline + "#" + strzonenum + "#" + strurl + "#" + strcountcall + "#" + 
                strCreateDate + "#" + strCreateTime + "#" + strempid + "#" + strempname + "#" + strphone + "#" + strserialmonitor),
                (byte)QosComboBox.SelectedIndex, RetainCheckBox.Checked);
        }

        public void getCheckPublishLightMessage()
        {
            string strselect;          

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strselect = classQuery.strQueryDataCallLine1 + @"
                    WHERE andon_call_head.receiver_code = '" + arrDataReceive[2] + @"'
                      AND master_plant.description = '" + strplant + @"'
                      AND master_operation.code = '" + stroper + @"'
                      AND master_line.code = '" + strline + @"'
                      AND master_light_head.zone_code = '" + strzone + @"'
                      AND andon_call_head.status = '1'
                    GROUP BY master_light_detail.code
                        , andon_call_head.id) view_light
                    GROUP BY view_light.code"; //AND andon_call_head.transmitter_code = '" + arrDataReceive[0] + @"'

                classMain.cmdCheck1 = new SqlCommand(strselect, classMain.con);
                classMain.rdCheck1 = classMain.cmdCheck1.ExecuteReader();

                if (classMain.rdCheck1.HasRows)
                {
                    while (classMain.rdCheck1.Read())
                    {
                        pubLightCode = classMain.rdCheck1.GetString(0);
                        iCountCall = classMain.rdCheck1.GetValue(1).ToString();
                        flagOnOff = iCountCall == "0" ? "0" : "1";
                        client.Publish(LampID, Encoding.UTF8.GetBytes(pubLightCode + "#" + flagOnOff), 
                            (byte)QosComboBox.SelectedIndex, RetainCheckBox.Checked);
                    }
                }
                else
                {
                    getPublishLightMessage();
                }

                classMain.cmdCheck1.Dispose();
                classMain.rdCheck1.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getPublishLightMessage()
        {
            string strselect;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strselect = classQuery.strQueryDataCallLine2;
                strselect = strselect + @"WHERE master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                    AND master_plant.description = '" + strplant + @"'
                    AND master_operation.code = '" + stroper + @"'
                    AND master_line.code = '" + strline + @"'
                    AND master_light_head.zone_code = '" + strzone + @"') view_light
                    GROUP BY view_light.code"; //AND master_transmitter_detail.code = '" + arrDataReceive[0] + @"'

                classMain.cmdCheck2 = new SqlCommand(strselect, classMain.con);
                classMain.rdCheck2 = classMain.cmdCheck2.ExecuteReader();

                if (classMain.rdCheck2.HasRows)
                {
                    while (classMain.rdCheck2.Read())
                    {
                        pubLightCode = classMain.rdCheck2.GetString(0);
                        iCountCall = classMain.rdCheck2.GetValue(1).ToString();
                        flagOnOff = "0";
                        client.Publish(LampID, Encoding.UTF8.GetBytes(pubLightCode + "#" + flagOnOff), 
                            (byte)QosComboBox.SelectedIndex, RetainCheckBox.Checked);
                    }
                }
                else
                {
                    pubLightCode = "";
                    iCountCall = "0";
                    flagOnOff = "0";
                }

                classMain.cmdCheck2.Dispose();
                classMain.rdCheck2.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getCheckSavingHead()
        {
            string strbuttonid;
            string strsqlselect;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strbuttonid = classQuery.strQueryTransmitterData;
                strbuttonid = strbuttonid + @"WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                      AND master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                      AND master_transmitter_detail.code = '" + arrDataReceive[0] + @"'
                      AND master_type_detail.button_number = '" + arrDataReceive[3] + @"'
                    ORDER BY master_person_detail.status DESC";

                classMain.cmdCheck1 = new SqlCommand(strbuttonid, classMain.con);
                classMain.rdCheck1 = classMain.cmdCheck1.ExecuteReader();

                if (classMain.rdCheck1.Read())
                {
                    varzoneid = classMain.rdCheck1.GetValue(0).ToString();
                    varzonecode = classMain.rdCheck1.GetString(1);
                    varreceiverid = classMain.rdCheck1.GetValue(2).ToString();
                    varreceivercode = classMain.rdCheck1.GetString(3);
                    varmonitorid = classMain.rdCheck1.GetValue(4).ToString();
                    varempid = classMain.rdCheck1.GetString(5);
                    varempname = classMain.rdCheck1.GetString(6);
                    varempphone = classMain.rdCheck1.GetString(7);
                    varregister = classMain.rdCheck1.GetString(8);
                    vardesc = classMain.rdCheck1.GetString(9);
                    vartypeid = classMain.rdCheck1.GetValue(10).ToString();
                    varinstruction = classMain.rdCheck1.GetString(11);
                    varstatus = classMain.rdCheck1.GetString(12);
                }

                classMain.cmdCheck1.Dispose();
                classMain.rdCheck1.Close();

                if (varcallflag == "0")
                {
                    if (varstatus == "1")
                    {
                        strsqlselect = @"SELECT id FROM andon_call_head
                                         WHERE zone_id = '" + varzoneid + @"'
                                           AND zone_code = '" + varzonecode + @"'
                                           AND receiver_id = '" + varreceiverid + @"'
                                           AND receiver_code = '" + varreceivercode + @"'
                                           AND monitor_id = '" + varmonitorid + @"'
                                           AND transmitter_code = '" + arrDataReceive[0] + @"'
                                           AND button_number = '" + arrDataReceive[3] + @"'
                                           AND status = '" + varstatus + @"'
                                         ORDER BY 1 ASC";

                        classMain.cmdCheck2 = new SqlCommand(strsqlselect, classMain.con);
                        classMain.rdCheck2 = classMain.cmdCheck2.ExecuteReader();

                        if (!classMain.rdCheck2.Read())
                        {
                            getSavingHead();
                            getCheckSavingDetail();
                        }

                        classMain.cmdCheck2.Dispose();
                        classMain.rdCheck2.Close();
                    }
                }
            }
            catch
            {
                //iCol = 5;
                //myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Lose Connection 2", MessageTextBox);
                //MessageBox.Show(ex.ToString(), "Error Description", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getSavingHead()
        {
            string strsqlhead;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strsqlhead = " INSERT INTO andon_call_head (" +
                    " zone_id, zone_code, receiver_id, receiver_code, monitor_id, employee_id, employee_name, phone_number," +
                    " transmitter_code, register_code, description, instruction, type_id, button_number, status, create_by," +
                    " create_date, create_time) VALUES (" +
                    " '" + varzoneid + "', " +
                    " '" + varzonecode + "', " +
                    " '" + varreceiverid + "', " +
                    " '" + varreceivercode + "', " +
                    " '" + varmonitorid + "', " +
                    " '" + varempid + "', " +
                    " '" + varempname + "', " +
                    " '" + varempphone + "', " +
                    " '" + arrDataReceive[0] + "', " +
                    " '" + varregister + "', " +
                    " '" + vardesc + "', " +
                    " '" + varinstruction + "', " +
                    " '" + vartypeid + "', " +
                    " '" + arrDataReceive[3] + "', " +
                    " '" + varstatus + "', " +
                    " 'Engine', GETDATE(), CONVERT(varchar, GETDATE(), 108))";

                classMain.cmdDet = new SqlCommand(strsqlhead, classMain.con);
                classMain.cmdDet.ExecuteNonQuery();
                classMain.cmdDet.Dispose();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getUpdateData()
        {
            string strdetail;
            string strbuttonid;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strbuttonid = classQuery.strQueryTransmitterData;
                strbuttonid = strbuttonid + @"WHERE master_plant.description = '" + plantID + "-" + plantName + @"'
                      AND master_transmitter_head.receiver_code = '" + arrDataReceive[2] + @"'
                      AND master_transmitter_detail.code = '" + arrDataReceive[0] + @"'
                      AND master_type_detail.button_number = '" + arrDataReceive[3] + @"'
                    ORDER BY 1 ASC";

                classMain.cmdCheck1 = new SqlCommand(strbuttonid, classMain.con);
                classMain.rdCheck1 = classMain.cmdCheck1.ExecuteReader();

                if (classMain.rdCheck1.Read())
                {
                    varzoneid = classMain.rdCheck1.GetValue(0).ToString();
                    varzonecode = classMain.rdCheck1.GetString(1);
                    varreceiverid = classMain.rdCheck1.GetValue(2).ToString();
                    varreceivercode = classMain.rdCheck1.GetString(3);
                    varmonitorid = classMain.rdCheck1.GetValue(4).ToString();
                    varempid = classMain.rdCheck1.GetString(5);
                    varempname = classMain.rdCheck1.GetString(6);
                    varempphone = classMain.rdCheck1.GetString(7);
                    varregister = classMain.rdCheck1.GetString(8);
                    vardesc = classMain.rdCheck1.GetString(9);
                    vartypeid = classMain.rdCheck1.GetValue(10).ToString();
                    varinstruction = classMain.rdCheck1.GetString(11);
                    varstatus = classMain.rdCheck1.GetString(12);
                }

                classMain.cmdCheck1.Dispose();
                classMain.rdCheck1.Close();

                if (varcallflag == "0")
                {
                    if (varstatus == "5")
                    {
                        strdetail = @"SELECT id FROM andon_call_head
                                      WHERE receiver_id = '" + varreceiverid + @"'
                                        AND receiver_code = '" + varreceivercode + @"'
                                        AND transmitter_code = '" + arrDataReceive[0] + @"'
                                        AND status = '1'
                                      ORDER BY 1 ASC";

                        /*WHERE zone_id = '" + varzoneid + @"'
                        AND zone_code = '" + varzonecode + @"'
                        AND monitor_id = '" + varmonitorid + @"'*/

                        classMain.cmdCheck2 = new SqlCommand(strdetail, classMain.con);
                        classMain.rdCheck2 = classMain.cmdCheck2.ExecuteReader();

                        while (classMain.rdCheck2.Read())
                        {
                            getCheckSavingDetail(classMain.rdCheck2.GetValue(0).ToString(), varstatus);
                        }

                        classMain.cmdCheck2.Dispose();
                        classMain.rdCheck2.Close();
                        getUpdateDataHead();
                    }
                }
            }
            catch
            {
                //iCol = 5;
                //myUI("[" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "] Lose Connection 3", MessageTextBox);
                //MessageBox.Show(ex.ToString(), "Error Description", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getUpdateDataHead()
        {
            string strsqlhead;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strsqlhead = " UPDATE andon_call_head SET " +
                    " status = '" + varstatus + "', " +
                    " update_date = GETDATE(), " +
                    " update_time = CONVERT(varchar, GETDATE(), 108) " +
                    " WHERE receiver_id = '" + varreceiverid + "' " +
                    " AND receiver_code = '" + varreceivercode + "' " +
                    " AND transmitter_code = '" + arrDataReceive[0] + "'";

                /*" WHERE zone_id = '" + varzoneid + "' " +
                " AND zone_code = '" + varzonecode + "' " +*/

                classMain.cmd = new SqlCommand(strsqlhead, classMain.con);
                classMain.cmd.ExecuteNonQuery();
                classMain.cmd.Dispose();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        public void getCheckSavingDetail()
        {
            string strdetail;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strdetail = @"SELECT TOP 1 id FROM andon_call_head
                              WHERE  zone_id = '" + varzoneid + @"'
                                 AND zone_code = '" + varzonecode + @"'
                                 AND receiver_id = '" + varreceiverid + @"'
                                 AND receiver_code = '" + varreceivercode + @"'
                                 AND monitor_id = '" + varmonitorid + @"'
                                 AND transmitter_code = '" + arrDataReceive[0] + @"'
                                 AND button_number = '" + arrDataReceive[3] + @"'
                                 AND status = '" + varstatus + @"'
                              ORDER BY create_date DESC";

                classMain.cmdCheckDet1 = new SqlCommand(strdetail, classMain.con);
                classMain.rdCheckDet1 = classMain.cmdCheckDet1.ExecuteReader();

                if (classMain.rdCheckDet1.Read())
                {
                    getCheckSavingDetail(classMain.rdCheckDet1.GetValue(0).ToString(), varstatus);
                }

                classMain.cmdCheckDet1.Dispose();
                classMain.rdCheckDet1.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        private void getCheckSavingDetail(string tmpid, string tmpstatus)
        {
            string strsqlselect;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strsqlselect = @"SELECT id FROM andon_call_detail
                                 WHERE id = '" + tmpid + @"'
                                   AND call_status = '" + tmpstatus + @"'
                                 ORDER BY 1 ASC";

                classMain.cmdCheckDet2 = new SqlCommand(strsqlselect, classMain.con);
                classMain.rdCheckDet2 = classMain.cmdCheckDet2.ExecuteReader();

                if (!classMain.rdCheckDet2.Read())
                {
                    getSavingDetail(tmpid, tmpstatus);
                }

                classMain.cmdCheckDet2.Dispose();
                classMain.rdCheckDet2.Close();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        private void getSavingDetail(string tmpid, string tmpstatus)
        {
            string strsqldetail;

            try
            {
                classMain.con = clMain.GetDBConnection();
                classMain.con.Open();

                strsqldetail = " INSERT INTO andon_call_detail (" +
                               " id, call_status, create_by, create_date, create_time) VALUES (" +
                               " '" + tmpid + "', " +
                               " '" + tmpstatus + "', " +
                               " 'Engine', GETDATE(), CONVERT(varchar, GETDATE(), 108))";

                classMain.cmdDet = new SqlCommand(strsqldetail, classMain.con);
                classMain.cmdDet.ExecuteNonQuery();
                classMain.cmdDet.Dispose();
            }
            finally
            {
                if (classMain.con != null)
                {
                    if (classMain.con.State == ConnectionState.Open)
                        classMain.con.Close();
                }
            }
        }

        private void MessageTextBox_TextChanged(object sender, EventArgs e)
        {
            if (varcallflag == "0") getPublishIndicator();
            MessageTextBox.ScrollToCaret();
            if (MessageTextBox.Lines.Count() - 1 <= 1)
                lbLine.Text = "* Number Of Lines : " + (MessageTextBox.Lines.Count() - 1);
        }

        private void getNewLineLog()
        {
            int count = 0;
            MessageTextBox.Invoke(new Action(() => count = MessageTextBox.Lines.Count() - 1));
            if (count > 100)
            {
                MessageTextBox.Invoke(new Action(() => strtmpmsg = MessageTextBox.Lines[100].ToString()));
                MessageTextBox.Invoke(new Action(() => MessageTextBox.Text = ""));
                if (strtmpmsg != "") myUI(strtmpmsg, MessageTextBox);
            }
            getCountLineLog();
        }

        private void getCountLineLog()
        {
            lbLine.Invoke(new Action(() => lbLine.Text = ""));
            lbLine.Invoke(new Action(() => lbLine.Text = "* Number Of Lines : " + (MessageTextBox.Lines.Count() - 1)));
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            iCol = iColTmp;
            MessageTextBox.Text = "";
            lbMissReceiver.Text = "";
            Thread.Sleep(80);
            myUI(tConnectionTmp.Text, MessageTextBox);
            lbLine.Focus();
        }

        private void ckStopCall_CheckedChanged(object sender, EventArgs e)
        {
            frmStopCall fmchild = new frmStopCall(this);
            actionChildForm(fmchild);
            panel1.Focus();

            if (ckStopCall.Checked == true)
            {             
                fmchild.Show();
            }
            else
            {
                fmchild.Close();
            }
        }
    }
}
 
 