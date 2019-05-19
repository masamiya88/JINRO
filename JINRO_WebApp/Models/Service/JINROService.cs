using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JINRO_WebApp.Models.Entity;
using JINRO_WebApp.Models.Service;

namespace JINRO_WebApp.Controllers
{
    public class JINROService
    {
        /// <summary>
        /// 初期化(役職設定)
        /// </summary>
        public void Initialize(List<GameInfo> gameinfo)
        {
            //一旦省略
        }
        
        /// <summary>
        /// 昼
        /// </summary>
        public void Daytime()
        {
            //現在の進行状況取得(昨晩の犠牲者など)
            GetProgressStatus();
        }
    
        /// <summary>
        /// 夜
        /// </summary>
        public String Night(GameInfo gameInfo)
        {
            switch (GetRole(gameInfo))
            {
                case "市民":
                    //処理
                    Suspect(gameInfo);
                    return null;
                case "預言者":
                    //処理
                    var role= Divine(gameInfo);
                    return role;
                case "人狼":
                    //処理
                    Bite(gameInfo);
                    return null;
                default:
                    //処理
                    return null;
            }
        }

        //夜のアクション集計
        public void AggregateNightAction()
        {
            //疑いを集計
            var res = GetSusupectedMost();   
            //死者判定

        }
        /// <summary>
        /// 死亡
        /// </summary>
        void Death()
        {

        }
        /// <summary>
        /// 最新の進行状況
        /// </summary>
        /// <returns></returns>
        private string GetProgressStatus()
        {
            return null;
        }

        /// <summary>
        /// 占い師アクション(占い)
        /// </summary>
        /// <param name="gameInfo"></param>
        private String Divine(GameInfo gameInfo)
        {
            DBService dBService = new DBService();
            string result = dBService.GetRole(gameInfo.actionTarget);
            return result;
        }

        /// <summary>
        /// 役職取得
        /// </summary>
        /// <param name="gameInfo"></param>
        /// <returns></returns>
        private string GetRole(GameInfo gameInfo)
        {
            DBService dBService = new DBService();
            string role = dBService.GetRole(gameInfo.userName);
            return role;
        }

        /// <summary>
        /// 市民アクション(疑わしい人への投票)
        /// </summary>
        /// <param name="gameInfo"></param>
        private void Suspect(GameInfo gameInfo)
        {
            DBService dBService = new DBService();
            dBService.NightAction(gameInfo.actionTarget,1);
        }

        /// <summary>
        /// 人狼アクション(襲撃)
        /// </summary>
        /// <param name="gameInfo"></param>
        private void Bite(GameInfo gameInfo)
        {
            DBService dBService = new DBService();
            dBService.NightAction(gameInfo.actionTarget,2);
                    }
        /// <summary>
        /// 霊媒師アクション(死者の役職確認)
        /// </summary>
        /// <param name="gameInfo"></param>
        /// <returns></returns>
        private string SeeCemetery(GameInfo gameInfo)
        {
            //死者のみを対処にする処理が必要
            DBService dBService = new DBService();
            string role = dBService.GetRole(gameInfo.actionTarget);
            return role;
        }
        /// <summary>
        /// 騎士アクション(守る)
        /// </summary>
        /// <param name="gameInfo"></param>
        private void Guard(GameInfo gameInfo)
        {
            DBService dBService = new DBService();
            dBService.NightAction(gameInfo.actionTarget, 3);
        }
        /// <summary>
        /// 最初の夜
        /// </summary>
        public String FirstNight(GameInfo gameInfo)
        {
            switch (GetRole(gameInfo))
            {
                case "占い師":
                    //処理
                    var result = Divine(gameInfo);
                    return result;
                default:
                    //処理
                    Suspect(gameInfo);
                    return null;
            }
        }
        /// <summary>
        /// 一番疑われている人を返す
        /// </summary>
        /// <returns></returns>
        private string GetSusupectedMost()
        {
            DBService dBService = new DBService();
            var result = dBService.SuspectedMost();
            return result;
        }
        private string DeathJudge()
        {
            DBService dBService = new DBService();
            var result = dBService.DeathJudge();
            return result;
        }
    }
}