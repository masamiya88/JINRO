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
        public string Divine(String target)
        {
            var con = new MySqlConnection(getConnStr());
            con.Open();
            var command = new MySqlCommand("select divination from jinro.user_info inner join jinro.role  on user_info.role_id = jinro.role.role_id where user_info.user_name = @name;", con);
            command.Parameters.Add(new MySqlParameter("name", target));
            var reader = command.ExecuteReader();
            reader.Read();
            var result = reader["divination"].ToString();
            con.Close();
            return result;
        }
        public string[] SuspectedMost()
        {
            string[] result={ };
            var con = new MySqlConnection(getConnStr());
            con.Open();
            var command = new MySqlCommand("select user_name from jinro.night_status where vote_count= (select max(vote_count) from jinro.night_status)", con);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Array.Resize(ref result, result.Length + 1);
                result[result.Length - 1] = reader["user_name"].ToString();
            }
            con.Close();
            return result;
        }
        public string DeathJudge()
        {
            var con = new MySqlConnection(getConnStr());
            con.Open();
            var command = new MySqlCommand(@"SELECT 
                                                *,
                                                CASE
                                                    WHEN forced_attack > 0 and(select sum(role.divination) from jinro.user_info info inner join jinro.role role on info.role_id = role.role_id where role.divination = '1' group by role.divination) > 1 THEN '強制襲撃により死亡'
                                                    WHEN trapped > 0 THEN 'トラップ死'
                                                    WHEN
                                                        protected > 0
                                                            OR protected_knights > (SELECT
                                                                COUNT(1)
                                                            FROM
                                                                jinro.user_info
                                                            WHERE
                                                                role_id = '7' AND dead_or_alive = '0')
                                                    THEN
                                                        '襲撃失敗'
                                                    ELSE '襲撃成功'
                                                END result
                                            FROM
                                                (SELECT
                                                    a.user_name,
                                                        a.protected,
                                                        a.trapped,
                                                        a.protected_knights,
                                                        a.forced_attack
                                                FROM
                                                    jinro.night_status a
                                                WHERE
                                                    forced_attack > 0
                                                        OR(attacked = (SELECT
                                                            MAX(b.attacked)
                                                        FROM
                                                            jinro.night_status b)
                                                        AND forced_attack< 1) 
                                                ORDER BY RAND()
                                                LIMIT 1) c;", con);
            var reader = command.ExecuteReader();
            reader.Read();
            var result = reader["result"].ToString();
            con.Close();
            return result;
        }
    }
}