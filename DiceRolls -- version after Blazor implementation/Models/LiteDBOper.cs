using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using DiceRolls.Models;

namespace DiceRolls.Models
{
    public class LiteDBOper
    {
        private static readonly string baseFile = @"Data/base.ldb";
        private static readonly string basePass = "abcd1234";

        public static LiteDatabase OpenLDB()
        {
            ConnectionString connStr = new ConnectionString
            {
                Filename = baseFile,
                Password = basePass,
                Connection = ConnectionType.Shared
            };

            LiteDatabase ldb;
            try
            {
                ldb = new LiteDatabase(connStr);
            }
            catch (LiteException)
            {
                return null;
            }

            return ldb;
        }

        public static int AddNewUser()
        {
            UserMdl mdl = new UserMdl()
            {
                Dice = string.Empty,
                CreationDate = DateTime.Now
            };

            LiteDatabase ldb = OpenLDB();

            if (ldb == null)
                return -13;

            var col = ldb.GetCollection<UserMdl>("dice");
            int result = col.Insert(mdl);

            ldb.Dispose();

            return result;
        }

        public static bool AddRoll(int userId, string roll)
        {
            LiteDatabase ldb = OpenLDB();

            if (ldb == null)
                return false;

            var col = ldb.GetCollection<UserMdl>("dice");
            UserMdl mdl = col.FindOne(x => x.Id == userId);

            if (mdl.Dice == null || mdl.Dice == string.Empty)
                mdl.Dice = roll;
            else
                mdl.Dice += "|" + roll;

            col.Update(mdl);
            ldb.Dispose();

            return true;
        }

        public static UserMdl GetUserById(int userId)
        {
            LiteDatabase ldb = OpenLDB();

            if (ldb == null)
                return null;

            var col = ldb.GetCollection<UserMdl>("dice");
            UserMdl mdl = col.FindOne(x => x.Id == userId);
            ldb.Dispose();
            return mdl;
        }

        public async static Task<string> GetUserByIdAsync(int userId)
        {
            LiteDatabase ldb = OpenLDB();

            if (ldb == null)
                return null;
            
            var col = await Task.Run(() => ldb.GetCollection<UserMdl>("dice"));
            UserMdl mdl = col.FindOne(x => x.Id == userId);

            if (mdl == null)
                return null;

            ldb.Dispose();
            return mdl.Dice;
        }

        public static void Clean()
        {
            LiteDatabase ldb = OpenLDB();

            if (ldb == null)
                return;

            var col1 = ldb.GetCollection<UpdateMdl>("lastUpdate");
            UpdateMdl mdl1 = col1.FindById(1);

            if (mdl1 == null)
            {
                mdl1 = new UpdateMdl
                {
                    LastUpdate = DateTime.Now
                };

                col1.Insert(mdl1);
            }
            else
            {
                if (mdl1.LastUpdate.AddDays(7) < DateTime.Now)
                {
                    var col2 = ldb.GetCollection<UserMdl>("dice");
                    col2.DeleteMany(x => x.CreationDate.AddDays(7) < DateTime.Now);
                    mdl1.LastUpdate = DateTime.Now;
                    col1.Update(mdl1);

                    LiteDB.Engine.RebuildOptions rebuildOpts = new LiteDB.Engine.RebuildOptions()
                    {
                        Password = basePass
                    };

                    ldb.Rebuild(rebuildOpts);
                }
            }

            ldb.Dispose();
        }
    }
}
