using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DiceRolls.Models;

namespace DiceRolls.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //private string GetStatString(string dice, int type)
        //{
        //    if (dice == null || dice == string.Empty)
        //        return string.Empty;
        //    else
        //    {
        //        int[] stats = new int[type];
        //        int count = 0;
        //        string result = string.Empty;

        //        for (int i = 0; i < type; i++)
        //            stats[i] = 0;

        //        foreach (string line in dice.Split('|'))
        //            if (line.Split('^')[0].Equals(type.ToString()))
        //                foreach (string l in line.Split('^')[1].Split(','))
        //                {
        //                    count++;
        //                    stats[Convert.ToInt32(l) - 1]++;
        //                }

        //        if (count != 0)
        //            for (int i = 0; i < type; i++)
        //                result += (i + 1) + "=" + Convert.ToInt32((stats[i] * 100.0 / count)) + "%, ";

        //        result = result.TrimEnd(' ').TrimEnd(',');

        //        return result;
        //    }
        //}

        [HttpGet]
        public IActionResult Index(int userId)
        {
            if (userId > 0)
            {
                //ViewBag.komunikat = Request.Scheme + " || " +
                //        Request.Host.ToString() + " || " +
                //        Request.PathBase.ToString() + " || " +
                //        Request.Path.ToString() + " || " +
                //        Request.QueryString.ToString();

                UserMdl mdl = LiteDBOper.GetUserById(userId);
                if (mdl != null)
                {
                    ViewBag.UserId = mdl.Id.ToString();
                    ViewBag.SessionId = userId;
                    //ViewBag.Dice = mdl.Dice;
                    //ViewBag.Stats = GetStatString(mdl.Dice, 10);
                    string login = CookiesOper.IsLogged(userId, Request);
                    if (login != string.Empty)
                        ViewBag.Login = login;
                }
                else
                {
                    Response.Redirect(Request.Scheme + "://" + Request.Host);
                }
            }
            else
            {
                int sessionId = LiteDBOper.AddNewUser();

                if (sessionId == -13)
                {
                    ViewBag.Error = "Nie udało się utworzyć nowej sesji.";
                    return View();
                }

                Response.Redirect(Request.Scheme + "://" + Request.Host + "?userId=" + sessionId);
            }

            int diceCount = CookiesOper.GetCount(Request);
            if (diceCount < 1 || diceCount > 30) diceCount = 1;
            ViewBag.diceCount = diceCount;

            int diceType = CookiesOper.GetType(Request);
            if (!(new int[] { 2, 4, 6, 8, 10, 12, 16, 20, 100 }).Contains(diceType)) diceType = 2;
            ViewBag.diceType = diceType;


            return View();
        }

        private int GetRand(int max)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(1, max + 1);
        }

        [HttpPost]
        public IActionResult Index()
        {
            int sessionId = Convert.ToInt32(Request.Form["rollSend"]);
            string rollString;
            int diceCount;
            int diceType;

            try
            {
                diceCount = Convert.ToInt32(Request.Form["diceCount"]);
            }
            catch (Exception)
            {
                diceCount = 1;
            }

            try
            {
                diceType = Convert.ToInt32(Request.Form["diceType"]);
            }
            catch (Exception)
            {
                diceType = 2;
            }

            if (diceCount < 1 || diceCount > 30)
                diceCount = 1;

            if (!(new int[] { 2, 4, 6, 8, 10, 12, 16, 20, 100 }).Contains(diceType))
                diceType = 2;

            CookiesOper.LastThrow(diceCount, diceType, Response);

            int[] diceRoll = new int[diceCount];
            //Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < diceCount; i++)
            {
                //diceRoll[i] = rand.Next(1, diceType + 1);
                diceRoll[i] = GetRand(diceType);
            }

            rollString = diceType + "^";

            foreach (var roll in diceRoll)
                rollString += roll + ",";
            rollString = rollString.TrimEnd(',');

            DateTime now = DateTime.Now;
            rollString += "^" + now.Hour.ToString("D2") + ":" + now.Minute.ToString("D2") + ":" + now.Second.ToString("D2");

            string login = Request.Form["login"];
            if (login != null && login != string.Empty)
            {
                login = login.Replace("^", "").Replace("|", "");
                CookiesOper.Login(login, sessionId, Response);
            }
            else
                login = CookiesOper.IsLogged(sessionId, Request);
            rollString += "^" + login;

            if(!LiteDBOper.AddRoll(sessionId, rollString))
            {
                ViewBag.Error = "Nie udało się wykonać rzutu.";
                return View();
            }

            LiteDBOper.Clean();
            
            Response.Redirect(Request.Scheme + "://" + Request.Host + "?userId=" + sessionId);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
