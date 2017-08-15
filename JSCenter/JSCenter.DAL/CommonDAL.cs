using JSCenter.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCenter.DAL
{
    public class CommonDAL:BaseDAL
    {
        private static string GetNowStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private static int CurrentID(string table)
        {
            int  ID = Db.Sql(@"select count(ID) from "+table).QuerySingle<int>();
            return ID + 1;
        }

       /// <summary>
       /// 添加
       /// </summary>
       /// <param name="model"></param>
        public static void AddProject(DrugProject model)
        {
            model.ID = CurrentID("DrugProject");
            model.AddDate = GetNowStr();
            model.LastDate = GetNowStr();
            Db.Sql(@"insert into DrugProject values(@0,@1,@2,@3);")
               .Parameters(model.ID, model.DurgName, model.AddDate, model.LastDate)
               .Execute();
        }

        public static DrugProject GetProject(string ID)
        {
            DrugProject product = Db.Sql(@"select * from DrugProject where ID = @0").Parameters(ID)
         .QuerySingle<DrugProject>();
            return product;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        public static void DeleteProject(string ID)
        {
            Db.Sql(@"delete from DrugProject where ID = @0").Parameters(ID).Execute();
            Db.Sql(@"delete from DrugProjectItem where DrugProjectID = @0").Parameters(ID).Execute();
            Db.Sql(@"delete from ProjectLog where DrugProjectID = @0").Parameters(ID).Execute();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        public static void DeleteProjectItem(string ID)
        {
            var model = GetProjectItem(Convert.ToInt32(ID));
            if (model != null)
            {
                Db.Sql(@"delete from DrugProjectItem where ID = @0").Parameters(model.ID).Execute();
                LogProjectItem(model.DrugProjectID, ProjectLogType.删除, model, null);
            }
        }


        /// <summary>
        /// 获取药材检验列表
        /// </summary>
        /// <returns></returns>
        public static List<DrugProject> GetProjectList()
        {
            var list = Db.Sql(@"select * from DrugProject").QueryMany<DrugProject>();
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ProjectID"></param>
        /// <returns></returns>
        public static List<DrugProjectItem> GetProjectItemList(string _ProjectID)
        {
            var list = Db.Sql(@"select * from DrugProjectItem where DrugProjectID=@0").Parameters(_ProjectID).QueryMany<DrugProjectItem>();
            return list;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static void AddProjectItem(DrugProjectItem model)
        {
          model.ID = CurrentID("DrugProjectItem");
          int row=  Db.Sql(@"insert into DrugProjectItem values(@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13);")
               .Parameters(
               model.ID,
               model.DrugProjectID,
               model.CodeNum1,
               model.CodeNum2,
               model.PJSFMJ,
               model.GSCYL,
               model.XSBS,
               model.DZLD,
               model.DZFMJ,
               model.HL,
               model.PJHL,
               model.FC,
               model.IsFuCe,
               model.type
               ).Execute();
            //记录日志
          model = GetProjectItem(model.ID);
          LogProjectItem(model.ID, ProjectLogType.添加, null, model);
        }

        public static DrugProjectItem GetProjectItem(int ID)
        {
            DrugProjectItem product = Db.Sql(@"select * from DrugProjectItem where ID = @0").Parameters(ID)
            .QuerySingle<DrugProjectItem>();
            return product;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        public static void UpdateProjectItem(DrugProjectItem model,bool islog=false)
        {
            DrugProjectItem product = GetProjectItem(model.ID);
            //记录日志
            if (islog)
            {
                LogProjectItem(product.DrugProjectID, ProjectLogType.修改, product, model);
            }
            product.DZFMJ = model.DZFMJ;
            product.DZLD = model.DZLD;
            product.FC = model.FC;
            product.GSCYL = model.GSCYL;
            product.HL = model.HL;
            product.PJHL = model.PJHL;
            product.PJSFMJ = model.PJSFMJ;
            product.XSBS = model.XSBS;

            
            try
            {
                int rowsAffected = Db.Update("DrugProjectItem")
               .Column("DZFMJ", product.DZFMJ)
               .Column("DZLD", product.DZLD)
               .Column("FC", product.FC)
               .Column("GSCYL", product.GSCYL)
               .Column("HL", product.HL)
               .Column("PJHL", product.PJHL)
               .Column("PJSFMJ", product.PJSFMJ)
               .Column("XSBS", product.XSBS)
               .Where("ID", product.ID)
               .Execute();
            }
            catch (Exception ex)
            {

            }
            
        }


        public static DrugProjectItem GetDrugProjectItem(string ID)
        {
            DrugProjectItem product = Db.Sql(@"select * from DrugProjectItem where ID = @0").Parameters(ID)
           .QuerySingle<DrugProjectItem>();
            return product;
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="projectID">项目ID</param>
        /// <param name="type"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        private static void LogProjectItem(int projectID,Model.ProjectLogType type,Model.DrugProjectItem before, Model.DrugProjectItem after)
        {
            ProjectLog log = new ProjectLog();
            log.ID=CurrentID("ProjectLog");
            log.DrugProjectID = projectID;
            if (before != null)
            {
                log.BeforeValue = JsonConvert.SerializeObject(before);
            }
            else
            {
                log.BeforeValue = string.Empty;
            }
            if (after != null)
            {
                log.AfterValue = JsonConvert.SerializeObject(after);
            }
            else
            {
                log.AfterValue = string.Empty;
            }
           
            log.OpType = type.ToString();
            log.ChangeDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            int row = Db.Sql(@"insert into ProjectLog values(@0,@1,@2,@3,@4,@5);").Parameters
                   (
                 log.ID,
                 log.DrugProjectID,
                 log.OpType,
                 log.BeforeValue,
                 log.AfterValue,
                 log.ChangeDate
                   ).Execute();
            //log
        }


        public static Model.ProjectLog GetLogItem(string ID)
        {
            ProjectLog product = Db.Sql(@"select * from ProjectLog where ID = @0").Parameters(ID)
         .QuerySingle<ProjectLog>();
            return product;
        }

        public static List<Model.ProjectLog> GetLogList(string ProjectID)
        {
            List<ProjectLog> product = Db.Sql(@"select * from ProjectLog where DrugProjectID = @0").Parameters(ProjectID)
         .QueryMany<ProjectLog>();
            return product;
        }
        /// <summary>
        /// 添加统计数据，每次先删除在插入
        /// </summary>
        /// <param name="list"></param>
        public static void AddProjectTongji(List<Tongji> list,string projectID)
        {
            //delete
            Db.Sql(@"delete from Tongji where ProjectID = @0").Parameters(projectID).Execute();

            foreach( var item in list)
            {
                item.ID = CurrentID("Tongji");
                int row = Db.Sql(@"insert into Tongji values(@0,@1,@2,@3,@4,@5);")
                     .Parameters(
                    item.ID,
                    projectID,
                    item.GroupName,
                    item.TJHL,
                    item.YPHL,
                    item.ZYLv
                     ).Execute();
            }
        }
        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static List<Tongji> GetTongjiList(string projectID)
        {
            var list = Db.Sql(@"select * from Tongji where ProjectID=@0").Parameters(projectID).QueryMany<Tongji>();
            return list;
        }

    }
}
