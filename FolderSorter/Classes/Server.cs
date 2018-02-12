using FolderSorter.Classes.DataClasses;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace FolderSorter.Classes
{
    public static class Server
    {
        public static IEnumerable<MoveFileData> ReadLogs()
        {
            using(var db = new LiteDatabase(@".\Data.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var logs = db.GetCollection<MoveFileData>(nameof(MoveFileData));
                return logs.FindAll();
            }
        }

        public static IEnumerable<Rule> GetRules()
        {
            using(var db = new LiteDatabase(@".\Data.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var rules = db.GetCollection<Rule>(nameof(Rule)).FindAll();
                return from rule in rules 
                    where rule.FallBack != true 
                    select rule;
            }
        }

        public static Rule GetFallBack()
        {
            using(var db = new LiteDatabase(@".\Data.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var rules = db.GetCollection<Rule>(nameof(Rule));
                return rules.FindOne(x => x.FallBack);
            }
        }

        public static void UpdateRule(List<Rule> rulesList, Rule fallBack)
        {
            using(var db = new LiteDatabase(@".\Data.db"))
            {
                // Drops original rules collection
                db.DropCollection(nameof(Rule));
                // Get a collection (or create, if doesn't exist)
                var rulesDb = db.GetCollection<Rule>(nameof(Rule));
                rulesDb.InsertBulk(rulesList);
                fallBack._id = rulesList.Count + 1;
                rulesDb.Insert(fallBack);
            }
        }

        public static void DeleteLog(MoveFileData data)
        {
            using (var db = new LiteDatabase(@".\Data.db"))
            {
                db.GetCollection<MoveFileData>(nameof(MoveFileData)).Delete(log => log.Equals(data));
            }
        }

        public static void ClearLog()
        {
            using (var db = new LiteDatabase(@".\Data.db"))
            {
                db.DropCollection(nameof(MoveFileData));
            }
        }

        public static void AddLog(MoveFileData data)
        {
            using (var db = new LiteDatabase(@".\Data.db"))
            {
                db.GetCollection<MoveFileData>(nameof(MoveFileData)).Insert(data);
            }
        }
    }
}
