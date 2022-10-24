using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;

namespace AMS_Engine
{
    public partial class frmStopCall : DevExpress.XtraEditors.XtraForm
    {
        classMain clMain = new classMain();
        private readonly frmLogEngine _formLog;

        public frmStopCall(frmLogEngine formLog)
        {
            InitializeComponent();
            _formLog = formLog;
        }

        public string strButtonNumber()
        {
            string str = "";
            try
            {
                classMain.constop = this.clMain.GetDBConnection();
                classMain.constop.Open();
                classMain.cmd = new SqlCommand(classQuery.strQueryButtonNumber + "WHERE LOWER(master_instruction.task) = 'stop'", classMain.constop);
                classMain.rd = classMain.cmd.ExecuteReader();
                if (classMain.rd.Read())
                    str = classMain.rd.GetString(0);
                classMain.cmd.Dispose();
                classMain.rd.Close();
                return str;
            }
            finally
            {
                if (classMain.constop != null && classMain.constop.State == ConnectionState.Open)
                    classMain.constop.Close();
            }
        }

        public void getCheckingCallActive()
        {
            string str = this.strButtonNumber();
            char[] chArray = new char[1] { '#' };
            try
            {
                classMain.constop = this.clMain.GetDBConnection();
                classMain.constop.Open();
                classMain.rdCheckActive = new SqlCommand(
                    @"SELECT andon_call_head.transmitter_code,
                             andon_call_head.receiver_code
                      FROM(((andon_call_head
                         INNER JOIN((master_zone_head
                            INNER JOIN master_zone_detail
                               ON master_zone_head.id = master_zone_detail.id)
                               INNER JOIN((master_line
                                  INNER JOIN master_plant
                                     ON master_line.plant_id = master_plant.id)
                                  INNER JOIN master_operation
                                     ON master_line.operation_id = master_operation.id)
                                  ON master_zone_head.line_id = master_line.id)
                               ON andon_call_head.zone_id = master_zone_head.id)
                         INNER JOIN(master_type_head
                            INNER JOIN ((master_type_detail
                               INNER JOIN master_instruction
                                  ON master_type_detail.instruction_id = master_instruction.id)
                               LEFT JOIN master_sound
                                  ON master_type_detail.sound_id = master_sound.id)
                               ON master_type_head.id = master_type_detail.id)
                            ON andon_call_head.type_id = master_type_head.id
                            AND andon_call_head.button_number = master_type_detail.button_number)
                         LEFT JOIN(master_light_head
                            INNER JOIN master_light_detail
                               ON master_light_head.id = master_light_detail.id)
                            ON master_zone_head.id = master_light_head.zone_id
                            AND master_zone_detail.code = master_light_head.zone_code)
                      WHERE master_plant.description = '" + _formLog.plantID + "-" + _formLog.plantName + @"'
                        AND andon_call_head.status = '1'
                        AND FORMAT(GETDATE(), 'HH:mm:ss') >= DATEADD(minute, 5, andon_call_head.create_time)
                      GROUP BY andon_call_head.transmitter_code, 
                               andon_call_head.receiver_code", classMain.constop).ExecuteReader();
                while (classMain.rdCheckActive.Read())
                {
                    int num = (int)this._formLog.client.Publish("rcv_engine", Encoding.UTF8.GetBytes(classMain.rdCheckActive.GetString(0) + "#DAT#" + classMain.rdCheckActive.GetString(1) + "#" + str), (byte)this._formLog.QosComboBox.SelectedIndex, (this._formLog.RetainCheckBox.Checked ? 1 : 0) != 0);
                }
                classMain.rdCheckActive.Close();
            }
            finally
            {
                if (classMain.constop != null && classMain.constop.State == ConnectionState.Open)
                    classMain.constop.Close();
            }
        }

        private void tmStopCall_Tick(object sender, EventArgs e)
        {
            getCheckingCallActive();
        }
    }
}