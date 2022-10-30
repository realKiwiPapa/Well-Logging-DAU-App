using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Logging_App.WebService
{
    public static class BigAntSDK
    {
        /// <summary>
        /// BigAnt消息发送
        /// </summary>
        /// <param name="receivers">接收者帐号  多个用 ; 分隔</param>
        /// <param name="subject">消息标题</param>
        /// <param name="content">消息内容，请加 "\n"换行</param>
        public static void SendMessage(string receivers, string subject, string content)
        {
            ANTCOMLib.AntMsg antMsg = new ANTCOMLib.AntMsg();
            ANTCOMLib.AntLoginInfo antLogin = new ANTCOMLib.AntLoginInfo();
            ANTCOMLib.AntSyncSession antSession = new ANTCOMLib.AntSyncSession();

            //设置消息信息
            antMsg.Subject = subject; //消息标题
            antMsg.Content = content ;//消息内容，请加 "\n"换行
            antMsg.ContentType = "Text/Text"; // 消息内容类型  Text/Text 文本  Text/URL  网址
            antMsg.AddReceivers(receivers); //接收者帐号  多个用 ; 分隔
            antMsg.MsgType = 0; //消息类型 0消息 1群发 2系统命令 3公告 4会议
            //antMsg.AddAttach(@"c:\1.txt","") ;  //添加消息附件

            //设置登录信息
            antLogin.Server = ConfigurationManager.AppSettings["BigAntServer"]; // BigAnt服务器
            antLogin.ServerPort = int.Parse(ConfigurationManager.AppSettings["BigAntServerPort"]);// BigAnt 端口
            antLogin.LoginName = ConfigurationManager.AppSettings["BigAntLoginName"]; //发送者帐号
            antLogin.Password = ConfigurationManager.AppSettings["BigAntPassword"]; //发送者密码

            //发送消息
            antSession.Login(antLogin); //登录服务器
            antSession.SendMsg(antMsg, 0);//发送消息
        }
    }
}