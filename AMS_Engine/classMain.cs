using System;
using System.Data.SqlClient;

namespace AMS_Engine
{
    class classMain
    {
        private string ip;
        private string database;
        private string uid;
        private string password;
        private SqlConnection connection;

        public static SqlConnection con;
        public static SqlConnection constop;
        public static SqlCommand cmd;
        public static SqlCommand cmdDet;
        public static SqlCommand cmdCheck1;
        public static SqlCommand cmdCheck2;
        public static SqlCommand cmdCheckDet1;
        public static SqlCommand cmdCheckDet2;

        public static SqlDataReader rd;
        public static SqlDataReader rdDet;
        public static SqlDataReader rdCheck1;
        public static SqlDataReader rdCheck2;
        public static SqlDataReader rdCheckDet1;
        public static SqlDataReader rdCheckDet2;
        public static SqlDataReader rdCheckActive;

        //public static string ipMqtt = "172.19.152.152";
        //public static string ipMqtt = "172.19.152.122";
        public static string ipMqtt = "127.0.0.1";
        string connectionString;

        public SqlConnection GetDBConnection()
        {
            ip = @"DESKTOP-74V7V5B"; //127.0.0.1,49170 SERVER\SQLEXPRESS 172.19.152.152
            database = "andonkmk";
            uid = "KmkConnection"; //KmkConnection sa 
            password = "kmk222222"; //N7c19N11d10a28la8 kmk111111

            //ip = @"172.19.152.152";
            //database = "andonkmk";
            //uid = "andon";
            //password = "andon1";

            connectionString = @"Data Source=" + ip + ";user id =" + uid + "; " +
                "password =" + password + "; initial catalog=" + database + ";" +
                "persist security info=True;packet size=4096;Connection Lifetime=202130;Pooling=false;Connect Timeout=202545";
            //"Integrated Security=SSPI;Connection Timeout=200;pooling=true;Max Pool Size=200";

            connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
