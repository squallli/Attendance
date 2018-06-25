using System;
using System.Linq;
using System.Web.Mvc;
using RegalHR.ModelFactory;
using RegalHRModel;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace RegalHR.Controllers
{
    public class LoginController : Controller
    {

        LoginModelFactory ModelFactory = new LoginModelFactory();


        public ActionResult Index()
        {
            //string ip = Request.ServerVariables["REMOTE_ADDR"];

            //if (ip == "::1" || ip.IndexOf("192.168.") != -1 || ip.IndexOf("127.0.0.1") != -1)
            //{
            //    return View();
            //}
            //else
            //{
            //    Response.Write("目前IP:" + ip +" , 必須使用公司內部網路才能進行登入!");
            //    return new EmptyResult();
            //}

           return View();
        }



        public ActionResult LoginOut()
        {
            Session["LoginUserInfo"] = null;
            Response.Redirect("../Login/", true);
            return View();
        }



        /// <summary>
        /// 判定Session存不存在
        /// </summary>
        /// <returns></returns>
        public JsonResult ExistsSession()
        {
            if (Session["LoginUserInfo"] == null)
            {
                return Json("0");
            }
            else
            {
                return Json("1");
            }
        }

        [HttpGet]
        public void VerificationCode()
        {
            
            Response.ContentType = "image/gif";
            //建立 Bitmap 物件和繪製
            Bitmap basemap = new Bitmap(200, 60);
            Graphics graph = Graphics.FromImage(basemap);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, 200, 60);
            Font font = new Font(FontFamily.GenericSerif, 48, FontStyle.Bold, GraphicsUnit.Pixel);
            Random random = new Random();
            // 英數
            string letters = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz023456789";
           
            string letter;
            StringBuilder sb = new StringBuilder();

            for (int word = 0; word < 4; word++)
            {
                letter = letters.Substring(random.Next(0, letters.Length - 1), 1);
                sb.Append(letter);

                // 繪製字串 
                graph.DrawString(letter, font, new SolidBrush(Color.Black), word * 38, random.Next(0, 5));
            }


            // 混亂背景
            //Pen linePen = new Pen(new SolidBrush(Color.Black), 2);
            //for (int x = 0; x < 10; x++)
            //{
            //    graph.DrawLine(linePen, new Point(random.Next(0, 199), random.Next(0, 59)), new Point(random.Next(0, 199), random.Next(0, 59)));
            //}

            // 儲存圖片並輸出至stream      
            basemap.Save(Response.OutputStream, ImageFormat.Gif);
            // 將產生字串儲存至 Sesssion
            Session["ValidateCode"] = sb.ToString();
            //Session.Timeout = 20;
            Response.End();
        }

  
        public bool ValidateCode(string InputCode)
        {
            if (InputCode == null)
                return false;

            if (InputCode.Trim().ToLower().Equals(Session["ValidateCode"].ToString().ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        [HttpPost]
        public JsonResult LoginSys(string LoginId,string LoginPwd,string ValidationCode)
        {
            try
            {
                if(!ValidateCode(ValidationCode))
                {
                    return Json("驗證碼錯誤");
                }

                UserModel LoginUserInfo = ModelFactory.Login(LoginId, LoginPwd);

                if (LoginUserInfo == null)
                {
                    return Json("系統發生錯誤");
                }

                Session["LoginUserInfo"] = JsonConvert.SerializeObject(LoginUserInfo);

                return Json("1");//代表已經處裡完
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            catch
            {
                return Json("系統發生錯誤");
            }
        }




        [HttpGet]
        public EmptyResult AutoLogin()
        {

            Response.Redirect("../", true);
            return new EmptyResult();


            //清除Session
            if (Session["LoginUserInfo"] != null)
                {


                    Session["LoginUserInfo"] = null;
                }


                string EmpInfo = RegalEIPLoginKey.EIPLogin.GetDecryptString("1234", Request["q"].ToString());

                try
                {

                    string[] EmpInfoAry = EmpInfo.Split(',');

                    if (EmpInfoAry.Count() < 2)
                    {
                        
                        throw new Exception("解碼失敗" );
                    }
                    else
                    {

                        //EmpInfoAry[0] 代表 公號
                        //EmpInfoAry[1] 代表 LDAP帳號

                        UserModel LoginUserInfo = ModelFactory.Login(EmpInfoAry[1], "", true);

                        Session["LoginUserInfo"] = JsonConvert.SerializeObject(LoginUserInfo);

                        Response.Redirect("../", true);
                        return new EmptyResult();
                    }

                }
                catch(Exception ex)
                {
                    Response.Write(EmpInfo);

                    Response.Write(ex.Message);
                    //Response.Redirect("/Login/", true);
                    return new EmptyResult();

                }



        }


    }
}
