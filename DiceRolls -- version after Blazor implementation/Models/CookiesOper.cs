using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiceRolls.Models
{
    public class CookiesOper
    {
        public static void Login(string login, int sessionId, HttpResponse response)
        {
            CookieOptions opts = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(14)
            };

            response.Cookies.Append(sessionId.ToString(), login, opts);

            return;
        }

        public static string IsLogged(int sessionId, HttpRequest request)
        {
            string res = request.Cookies[sessionId.ToString()];

            if (res == null)
                return string.Empty;
            else
                return res;
        }

        public static void LastThrow(int diceCount, int diceType, HttpResponse response)
        {
            CookieOptions opts = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(2)
            };

            response.Cookies.Append("diceCount", diceCount.ToString(), opts);
            response.Cookies.Append("diceType", diceType.ToString(), opts);

            return;
        }

        public static int GetCount(HttpRequest request)
        {
            string res = request.Cookies["diceCount"];

            if (res == null)
                return 1;
            else
                try { return Convert.ToInt32(res); }
                catch (Exception) { return 1; }
        }

        public static int GetType(HttpRequest request)
        {
            string res = request.Cookies["diceType"];

            if (res == null)
                return 2;
            else
                try { return Convert.ToInt32(res); }
                catch (Exception) { return 2; }
        }
    }
}
