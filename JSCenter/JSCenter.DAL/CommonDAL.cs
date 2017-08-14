using JSCenter.Model;
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        public static void DeleteProject(string ID)
        {
            Db.Sql(@"delete from DrugProject where ID = @0").Parameters(ID).Execute();
            Db.Sql(@"delete from DrugProjectItem where DrugProjectID = @0").Parameters(ID).Execute();
        }

        public static void DeleteProjectItem(string ID)
        {
            Db.Sql(@"delete from DrugProjectItem where ID = @0").Parameters(ID).Execute();
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
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        public static void UpdateProjectItem(DrugProjectItem model)
        {
            DrugProjectItem product = Db.Sql(@"select * from DrugProjectItem where ID = @0").Parameters(model.ID)
            .QuerySingle<DrugProjectItem>();
            
           // product.CodeNum1 = model.CodeNum1;
          //  product.CodeNum2 = model.CodeNum2;
           // product.DrugProjectID = model.DrugProjectID;
            product.DZFMJ = model.DZFMJ;
            product.DZLD = model.DZLD;
            product.FC = model.FC;
            product.GSCYL = model.GSCYL;
            product.HL = model.HL;
           // product.IsFuCe = model.IsFuCe;
            product.PJHL = model.PJHL;
            product.PJSFMJ = model.PJSFMJ;
            product.XSBS = model.XSBS;
            //product.type = model.type;

            //记录日志
            LogProjectItem(product, model);
            try
            {
                int rowsAffected = Db.Update("DrugProjectItem")
               .Column("DZFMJ", product.DZFMJ)
               .Column("DZLD", product.DZLD)
               .Column("FC", product.FC)
               .Column("GSCYL", product.GSCYL)
               .Column("HL", product.HL)
               //.Column("IsFuCe", product.IsFuCe)
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

        public static void LogProjectItem(Model.DrugProjectItem before, Model.DrugProjectItem after)
        {
            //log
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

    }
}
