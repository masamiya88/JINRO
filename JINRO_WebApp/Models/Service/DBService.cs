using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using JINRO_WebApp.Models.Entity;

namespace JINRO_WebApp.Models.Service
{
    public class DBService
    {
        private string getConnStr()
        {
            return ConfigurationManager.ConnectionStrings["MyConnectionStr"].ConnectionString;
        }

        /// <summary>
        /// 役職取得
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetRole(String userName)
        {
            var con = new MySqlConnection(getConnStr());
            con.Open();
            var command = new MySqlCommand("select role_name from jinro.user_info inner join jinro.role  on user_info.role_id = jinro.role.role_id where user_info.user_name = @name;", con);
            command.Parameters.Add(new MySqlParameter("name", userName));
            var reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            reader.Read();
            var role = reader["role_name"].ToString();
            //}
            con.Close();

            return role;
        }

        /// <summary>
        /// 対象ユーザーへのアクション区分を更新する
        /// kbn→
        /// 1:疑う(市民)
        /// 2:襲撃(人狼)
        /// 3:守る(騎士団、ボディーガード)
        /// 4:占う(占い師、霊媒師)
        /// 5:妨害(スパイ)
        /// 6:
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="kbn"></param>
        public void NightAction(String targetName,int kbn)
        {
            //更新処理
            var con = new MySqlConnection(getConnStr());
            con.Open();
            var command = new MySqlCommand("update jinro.night_status set status = @kbn,vote_count=vote_count+1 where user_name = @name;", con);
            command.Parameters.Add(new MySqlParameter("name", targetName));
            command.Parameters.Add(new MySqlParameter("kbn", kbn));
            var reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            //var role = reader["role_name"].ToString();
            //}
            con.Close();

            //return role;
        }
    }
}