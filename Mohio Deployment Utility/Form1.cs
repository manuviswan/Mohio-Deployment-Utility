using System;
// CR: Clean up unused namespace (Ctrl+.)
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Mohio_Deployment_Utility
{
    public partial class MDU : Form
    {
        // TODO: Move connection string to app.config. That would make it easier to change it without recompiling code
        //SQL
        string connString = @"Data Source=LAPTOP-DEV01\SQLEXPRESS01;Initial Catalog=MohioDeployment;User ID=sa;Password=sa_manu33";

        // TODO : Overall, the naming convension doesn't seem strictly followed. Some places "m_" is used in method variable,
        // while in other places, it is not followed in method variables.
        // Standardize something, and follow it. A suggestion would be
        // use "_camelCase" for any class level member variable
        // use "PascalCase" for Class Names, Property Names and Method Names
        // use "camelCase" for any method variable
        //VARIABLES
        string pho = ""; // TODO: Better initialiize with string.Empty, more verbose
        string practice = "";
        string pid = "";
        string mispid = "";
        string pms = "";
        string server = "";
        string database = "";
        string dbUser = "";
        string dbPass = "";
        string mohioUser = "";
        string mohioPass = "";
        string rmServer = "";

        string mePmsConnQuery = "";
        string meWssConnQuery = "";
        string rmPmsConnQuery = "";

        string mohioDB = "";
 

        public MDU()
        {
            InitializeComponent();
        }

        //FORM CONTROL EVENTS
        // TODO: Remove empty methods/events
        private void MDU_Load(object sender, EventArgs e)
        {

        }

                
        //GROUP: PRACTICE
        private void cbPHO_SelectedValueChanged(object sender, EventArgs e)
        {
            ResetPHO();
            pho = cbPHO.Text; // TODO:  cbPHO is a combobox, SHOULD use cbPHO.SelectedText instead of cbPHO.Text
            mohioDB = setMohioDB(pho);
            FillPracticeList(pho);
        }

        private void cbPractice_SelectedValueChanged(object sender, EventArgs e)
        {
            ResetPractice();
            practice = cbPractice.Text;

            FillPracticeID(practice);
            FillMohioLogin(pid);
        }

        private void btnGenerateMID_Click(object sender, EventArgs e)
        {
            FillMID(pho);
        }


        //GROUP: PMS
        private void cbPMS_SelectedValueChanged(object sender, EventArgs e)
        {
            ResetPMS();
            // TODO : Again, SelectedText should be used.
            // Do SelectedText instead of Text for every combo, not commenting any further on same topic
            pms = cbPMS.Text;
        }




        //GROUP: MOHIO
        private void btnGeneratePassword_Click(object sender, EventArgs e)
        {
            GenerateMohioPassword(pid);
        }


        //GROUP: QUERIES
        private void btnGenerateMEPMSConnQuery_Click(object sender, EventArgs e)
        {
            string table = "ME_Configuration";
            string field = PMSField(pms);
            string value = setQueryValue(pms);
            mePmsConnQuery = GenerateQuery(chbMEPMSConn.Checked, table, field, value);
            tbMEPMSConn.Text = mePmsConnQuery;

        }

        private void btnGenerateMEWSSConnQuery_Click(object sender, EventArgs e)
        {
            rmServer = tbRMServer.Text;
            string table = "ME_Configuration";
            string field = "WSSConn";
            string value = $"wss://{rmServer}:64466/MohioExpress";
            meWssConnQuery = GenerateQuery(chbMEWSSConn.Checked, table, field, value);
            tbMEWSSConn.Text = meWssConnQuery;
        }

        private void btnGenerateRMPMSConnQuery_Click(object sender, EventArgs e)
        {
            string table = "RM_Configuration";
            string field = PMSField(pms);
            string value = setQueryValue(pms);
            rmPmsConnQuery = GenerateQuery(chbRMPMSConn.Checked, table, field, value);
            tbRMPMSConn.Text = rmPmsConnQuery;
        }


        //APP OVERALL CONTROL BUTTONS
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopyQueries_Click(object sender, EventArgs e)
        {
            CopyQueriesToClipboard();
        }

        //FUNCTIONS
        private String setMohioDB(string pho)
        {
            String _mohioDB = "";  //TODO: More of an optional comment, use var instead of explicit types like string in methods. makes it consistent to read.

            // TODO: Could optimize this switch as
            //switch (pho)
            //{
            //    case "AHPLUS":
            //    case "AKPHO":
            //    case "EBPHA":
            //        _mohioDB = $"Mohio_{pho}";
            //        break;
            //    case "NHC":
            //    case "WKO":
            //    case "WPHO":
            //        _mohioDB = "MohioTehononga";
            //        break;
            //}


            switch (pho)
            {
                case "AHPLUS":
                    _mohioDB = "Mohio_AHPLUS";
                    break;

                case "AKPHO":
                    _mohioDB = "Mohio_AKPHO";
                    break;

                case "EBPHA":
                    _mohioDB = "Mohio_EBPHA";
                    break;

                case "NHC":
                    _mohioDB = "MohioTehononga";
                    break;

                case "WKO":
                    _mohioDB = "MohioTehononga";
                    break;

                case "WPHO":
                    _mohioDB = "MohioTehononga";
                    break;

            }

            return _mohioDB;


        }

        private void ResetPHO()
        {
            // TODO: Use string.Empty instead of ""
            pho = "";
            practice = "";
            pid = "";
            mispid = "";

            cbPHO.Text = "";
            cbPractice.Items.Clear();
            cbPractice.Text = "";
            tbID.Clear();
            tbMISID.Clear();
            
            tbMohioUser.Clear();
            tbMohioPassword.Clear();
            
            tbRMServer.Clear();

            chbMEPMSConn.Checked = false;
            chbMEWSSConn.Checked = false;
            chbRMPMSConn.Checked = false;
            tbMEPMSConn.Clear();
            tbMEWSSConn.Clear();
            tbRMPMSConn.Clear();

        }

        private void ResetPractice()
        {
            practice = "";
            pid = "";
            mispid = "";

            tbID.Clear();
            tbMISID.Clear();
            tbMohioUser.Clear();
            tbMohioPassword.Clear();

        }

        private void ResetPMS()
        {

        }

        private void ResetAll()
        {
            cbPractice.Items.Clear();
            cbPractice.Text = "";
            tbID.Clear();
            tbMISID.Clear();
            tbMohioUser.Clear();
            tbMohioPassword.Clear();
        }

        private void FillPracticeList(string pho)
        {
            cbPractice.Items.Clear();

            // Since this access Database, it would be good idea to use Aschronous programming here. Read more about async-await
            SqlConnection conn;
            conn = new SqlConnection(connString);

            conn.Open();

            SqlCommand cmd;
            SqlDataReader dataReader;
            // TODO: This is strictly "NEVER DO APPROACH". This can lead to Sql Injection. Opt for Parameterized Queries instead
            String query = $"select Practice from [MohioDeployment].dbo.PracticeData where PHO='{pho}';";

            cmd = new SqlCommand(query, conn);

            dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                cbPractice.Items.Add(dataReader.GetValue(0));
            }

            // TODO: A better way to do this would be to use the 'using' syntax. It is much more cleaner and does the dispose for you.
            dataReader.Close();
            cmd.Dispose();
            conn.Close();
        }

        private void FillPracticeID(string practice)
        {
            tbID.Clear();

            SqlConnection conn;
            conn = new SqlConnection(connString);

            // TODO: All the comments mentioned in above method applies here as well.
            conn.Open();

            SqlCommand cmd;
            SqlDataReader dataReader;
            String query = $"select Practice_ID from [MohioDeployment].dbo.PracticeData where Practice='{practice}'";

            cmd = new SqlCommand(query, conn);

            dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                pid = dataReader.GetValue(0).ToString();
                tbID.Text = pid;
            }

            dataReader.Close();
            cmd.Dispose();
            conn.Close();
        }

        private void FillMohioLogin(string pid)
        {
            tbMohioUser.Clear();
            tbMohioPassword.Clear();

            // TODO: and here too
            SqlConnection conn;
            conn = new SqlConnection(connString);

            conn.Open();

            SqlCommand cmd;
            SqlDataReader dataReader;

            String query = $"select RM_Username, RM_Password from[MohioDeployment].dbo.PracticeData where Practice_ID = '{pid}'";

            cmd = new SqlCommand(query, conn);

            dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                mohioUser = dataReader.GetValue(0).ToString();
                mohioPass = dataReader.GetValue(1).ToString();
            }

            tbMohioUser.Text = mohioUser;

            // TODO: Could make the following if condition one liner with terinary operator
            // tbMohioPassword.Text = mohioPass == null ? string.Empty : mohioPass;
            if (mohioPass != null)
            {
                tbMohioPassword.Text = mohioPass;
            }
            else
            {
                tbMohioPassword.Text = "";
            }

            dataReader.Close();
            cmd.Dispose();
            conn.Close();
        }

        private void FillMID(string pho)
        {
            tbMISID.Clear();

            // TODO: May be a better approach for this kind of mapping would be to use a Dictionary where
            // Key is AHPLUS etc, and Value is that number.
            // For Example, you could declare a dictionary as private variable to class
            //
            // private Dictionary<string,string> _midDictionary= new
            //{
            //  ["AHPLUS"] = "681316",
            //  ["AKPHO"]  = "585702"
            //}
            // and then in this method,
            // mispid =  $"{_midDictionary[pho]}{pid}";
            //

            switch (pho)
            {
                case "AHPLUS":
                    mispid = "681316" + pid;
                    break;

                case "AKPHO":
                    mispid = "585702" + pid;
                    break;

                case "EBPHA":
                    mispid = "685146" + pid;
                    break;

                case "NHC":
                    mispid = "657833" + pid;
                    break;

                case "WKO":
                    mispid = "888888" + pid;
                    break;

                case "WPHO":
                    mispid = "657999" + pid;
                    break;

            }

            tbMISID.Text = mispid;


            
        }

        private void GenerateMohioPassword(string pid)
        {
            // TODO: Previous comments in method with Sql is applicable here too
            SqlConnection conn;
            conn = new SqlConnection(connString);

            conn.Open();

            SqlCommand cmd;
            SqlDataReader dataReader;

            String query = $"select Concat('rm_', Code, Practice_ID) as newPassword from[MohioDeployment].dbo.PracticeData where PHO = '{pho}' and Practice_ID = '{pid}'";

            cmd = new SqlCommand(query, conn);

            dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                mohioPass = dataReader.GetValue(0).ToString();
            }

            tbMohioPassword.Text = mohioPass;

        }

        private string PMSField(string pms)
        {
            string _field = "";
            switch (pms)
            {
                case "MedTech-32":
                    _field = "MT32Conn";
                    break;

                case "MedTech Evolution":
                    _field = "MTEvolutionConn";
                    break;

                case "MyPractice":
                    _field = "MyPracticeConn";
                    break;
            }
            return _field;
        }

        private string setQueryValue(string pms)
        {
            server = tbServer.Text;
            database = tbDatabase.Text;
            dbUser = tbUser.Text;
            dbPass = tbPassword.Text;

            string _value = "";
            // TODO: Connection string should be stored in app.config and read from config here
            if (pms == "MedTech-32")
            {
                _value = $"Provider = LCPI.IBProvider.5.Free;User ID={dbUser};Password={dbPass}; Location={server}; auto_commit=true;";
            }
            else
            {
                _value = $"data source={server}; initial catalog={database}; persist security info=False; user id={dbUser}; password ={dbPass}; Connect Timeout = 30;";
            }

            return _value;
        }

        private string GenerateQuery(bool @checked, string table, string field, string value)
        {
            // TODO : Sql Comments applicable here too
            string _mepmsconnQuery = "";
            if (@checked == false)
            {
                _mepmsconnQuery = $"Insert into [{mohioDB}].dbo.[{table}] \r\nValues \r\n('{field}','{value}','{pid}')";
            }
            else
            {
                _mepmsconnQuery = $"Update [{mohioDB}].dbo.[{table}] \r\nset \r\nvalue='{value}' \r\nwhere field='{field}' and practiceid='{pid}' ";
            }

            return _mepmsconnQuery;
        }

        private void CopyQueriesToClipboard()
        {
            // TODO: Could use Environment.NewLine instead of '\r\n'. And Of course, use $"{}" concatnation instead of +.
            Clipboard.SetText(mePmsConnQuery + "\r\n" + meWssConnQuery + "\r\n" + rmPmsConnQuery);
        }
    }
}
