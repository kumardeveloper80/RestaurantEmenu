using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class EMenuDBContext : DbContext
    {
        public DbSet<Sec_Users> Sec_Users { get; set; }
        public DbSet<Emenu_Category> Emenu_Category { get; set; }
        public DbSet<Emenu_ItemTags> Emenu_ItemTags { get; set; }
        public DbSet<Emenu_MenuItems> Emenu_MenuItems { get; set; }
        public DbSet<Emenu_MenuItemTags> Emenu_MenuItemTags { get; set; }
        public DbSet<Emenu_Menus> Emenu_Menus { get; set; }
        public DbSet<Emenu_MenuMItems> Emenu_MenuMItems { get; set; }
        public DbSet<Emenu_MenuSchedules> Emenu_MenuSchedules { get; set; }
        public DbSet<Sec_UserStores> Sec_UserStores { get; set; }
        public DbSet<Set_Stores> Set_Stores { get; set; }
        public DbSet<Set_Concepts> Set_Concepts { get; set; }
        public DbSet<CRM_CommentCard> CRM_CommentCard { get; set; }
        public DbSet<CRM_CommentCardQuestions> CRM_CommentCardQuestions { get; set; }
        public DbSet<CRM_CommentCardStores> CRM_CommentCardStores { get; set; }
        public DbSet<CRM_CQuestions> CRM_CQuestions { get; set; }
        public DbSet<CRM_CQuestionsGroups> CRM_CQuestionsGroups { get; set; }
        public DbSet<Sys_Fields> Sys_Fields { get; set; }
        public DbSet<set_DropDown> set_DropDown { get; set; }
        public DbSet<Sys_FieldValues> Sys_FieldValues { get; set; }
        public DbSet<Set_CountryCodes> Set_CountryCodes { get; set; }
        public DbSet<CRM_CommentCardResultsHeader> CRM_CommentCardResultsHeader { get; set; }
        public DbSet<CRM_CommentCardResults> CRM_CommentCardResults { get; set; }
        public DbSet<Emenu_Currency> Emenu_Currency { get; set; }
        public DbSet<Concept_Theme> Concept_Theme { get; set; }
        public DbSet<Category_Sequence> Category_Sequence { get; set; }
        public DbSet<Item_Sequence> Item_Sequence { get; set; }
        public DbSet<Set_Region> Set_Region { get; set; }
        public DbSet<Emenu_ItemTagsConcepts> Emenu_ItemTagsConcepts { get; set; }
        public DbSet<Emenu_CategoryConcepts> Emenu_CategoryConcepts { get; set; }
        public DbSet<Emenu_MenuItemsConcepts> Emenu_MenuItemsConcepts { get; set; }
        public DbSet<Sec_Client> Sec_Client { get; set; }
        public DbSet<Sec_SuperUser> Sec_SuperUser { get; set; }
        public DbSet<Set_StoresConcepts> Set_StoresConcepts { get; set; }
        public DbSet<VoucherSetup> VoucherSetup { get; set; }
        public DbSet<VoucherItemType> VoucherItemType { get; set; }
        public DbSet<Set_VoucherSurvey> Set_VoucherSurvey { get; set; }
        public DbSet<Set_Customers> Set_Customers { get; set; }
        public DbSet<VoucherIssuance> VoucherIssuance { get; set; }
        public DbSet<Set_VoucherStore> Set_VoucherStore { get; set; }
        public DbSet<VoucherReasonCategoryMaster> VoucherReasonCategoryMaster { get; set; }
        public DbSet<VoucherReasonSubCategoryMaster> VoucherReasonSubCategoryMaster { get; set; }

        public EMenuDBContext(DbContextOptions<EMenuDBContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
