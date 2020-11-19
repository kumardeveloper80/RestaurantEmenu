﻿// <auto-generated />
using System;
using EMenuApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EMenuApplication.Migrations
{
    [DbContext(typeof(EMenuDBContext))]
    [Migration("20200626075203_Emenu_MenuItems_26062020_2")]
    partial class Emenu_MenuItems_26062020_2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EMenuApplication.Models.CRM_CQuestions", b =>
                {
                    b.Property<int>("q_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modifiedon")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<int?>("cqg_id")
                        .HasColumnType("int");

                    b.Property<string>("q_managerperformanace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("q_question")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("q_shortdescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short?>("q_status")
                        .HasColumnType("smallint");

                    b.HasKey("q_id");

                    b.ToTable("CRM_CQuestions");
                });

            modelBuilder.Entity("EMenuApplication.Models.CRM_CQuestionsGroups", b =>
                {
                    b.Property<int>("cqg_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("ServerPerformance")
                        .HasColumnType("bit");

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<string>("cqg_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cqg_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short?>("cqg_status")
                        .HasColumnType("smallint");

                    b.HasKey("cqg_id");

                    b.ToTable("CRM_CQuestionsGroups");
                });

            modelBuilder.Entity("EMenuApplication.Models.CRM_CommentCard", b =>
                {
                    b.Property<int>("cc_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("AvgScore")
                        .HasColumnType("bit");

                    b.Property<string>("CCBackground")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCBorderWidth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCControlsBorderColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("CCCustomFont")
                        .HasColumnType("bit");

                    b.Property<string>("CCCustomFontFamily")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCCustomFontFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCCustomFontFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCCustomFontLocal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCFontColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCFontFamily")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCFontSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCFontStyle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCFontWeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCLeftImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCRightImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCfooterHeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCfooterIMG")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCheaderHeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCheaderIMG")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("ColorCoded")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Display")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedBy")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Notify_Bymail")
                        .HasColumnType("bit");

                    b.Property<bool?>("Notify_Bysms")
                        .HasColumnType("bit");

                    b.Property<string>("PRBorderWidth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRControlsBorderColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRFontColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRFontFamily")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRFontSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRFontStyle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRFontWeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRfooterHeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRfooterIMG")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRheaderHeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PRheaderIMG")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileLeftImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileRightImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileTitle1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubmitbtnBackColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubmitbtnForeColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThankYouImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<string>("cc_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("cc_minvalue")
                        .HasColumnType("int");

                    b.Property<string>("cc_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short?>("ct_status")
                        .HasColumnType("smallint");

                    b.Property<bool?>("linebased")
                        .HasColumnType("bit");

                    b.HasKey("cc_id");

                    b.ToTable("CRM_CommentCard");
                });

            modelBuilder.Entity("EMenuApplication.Models.CRM_CommentCardQuestions", b =>
                {
                    b.Property<int>("ccq_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("AnswerFullSpace")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Modifiedby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Relatedquestion")
                        .HasColumnType("int");

                    b.Property<bool?>("ShowQuestionText")
                        .HasColumnType("bit");

                    b.Property<int>("cc_id")
                        .HasColumnType("int");

                    b.Property<int?>("cqg_order")
                        .HasColumnType("int");

                    b.Property<bool?>("cqg_visible")
                        .HasColumnType("bit");

                    b.Property<string>("q_answerDisplay")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("q_bootstrapSkin")
                        .HasColumnType("bit");

                    b.Property<bool?>("q_calculated")
                        .HasColumnType("bit");

                    b.Property<int>("q_id")
                        .HasColumnType("int");

                    b.Property<bool?>("q_linebased")
                        .HasColumnType("bit");

                    b.Property<int?>("q_minval")
                        .HasColumnType("int");

                    b.Property<int?>("q_ordre")
                        .HasColumnType("int");

                    b.Property<bool?>("q_required")
                        .HasColumnType("bit");

                    b.Property<string>("q_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("q_typevalue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("q_weight")
                        .HasColumnType("int");

                    b.Property<short?>("status")
                        .HasColumnType("smallint");

                    b.Property<string>("textPlaceholder")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ccq_id");

                    b.ToTable("CRM_CommentCardQuestions");
                });

            modelBuilder.Entity("EMenuApplication.Models.CRM_CommentCardStores", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("StartTime")
                        .HasColumnType("time");

                    b.Property<byte?>("Status")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("Store_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<int>("cc_id")
                        .HasColumnType("int");

                    b.Property<bool?>("selected")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.ToTable("CRM_CommentCardStores");
                });

            modelBuilder.Entity("EMenuApplication.Models.Emenu_Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConceptId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DetailsAR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DetailsEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LabelAR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabelEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Emenu_Category");
                });

            modelBuilder.Entity("EMenuApplication.Models.Emenu_ItemTags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConceptId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("IconTagName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LabelAR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabelEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Emenu_ItemTags");
                });

            modelBuilder.Entity("EMenuApplication.Models.Emenu_MenuItemTags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConceptId")
                        .HasColumnType("int");

                    b.Property<int>("EMenuItemId")
                        .HasColumnType("int");

                    b.Property<int>("ItemTagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Emenu_MenuItemTags");
                });

            modelBuilder.Entity("EMenuApplication.Models.Emenu_MenuItems", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("CommentsAR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommentsEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConceptId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DeletedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DetailsAR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DetailsEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LabelAR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabelEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LargeDetailsAR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LargeDetailsEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LargeImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OverLayImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OverlayDetailsAR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OverlayDetailsEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PLU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("ThumbnailImageName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Emenu_MenuItems");
                });

            modelBuilder.Entity("EMenuApplication.Models.Emenu_MenuMItems", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConceptId")
                        .HasColumnType("int");

                    b.Property<int>("EMenuItemsId")
                        .HasColumnType("int");

                    b.Property<int>("EMenusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Emenu_MenuMItems");
                });

            modelBuilder.Entity("EMenuApplication.Models.Emenu_MenuSchedules", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConceptId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<Guid>("Store_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UniqueCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Emenu_MenuSchedules");
                });

            modelBuilder.Entity("EMenuApplication.Models.Emenu_Menus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConceptId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Emenu_Menus");
                });

            modelBuilder.Entity("EMenuApplication.Models.Sec_UserStores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<byte?>("Status")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("Store_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sec_UserStores");
                });

            modelBuilder.Entity("EMenuApplication.Models.Sec_Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<bool?>("CanExportProfiles")
                        .HasColumnType("bit");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DefaultModule")
                        .HasColumnType("int");

                    b.Property<int?>("DefaultPage")
                        .HasColumnType("int");

                    b.Property<int?>("DeletedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoggedIn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Lock")
                        .HasColumnType("bit");

                    b.Property<bool?>("LockProfile")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte?>("Status")
                        .HasColumnType("tinyint");

                    b.Property<bool?>("UnlockProfile")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sec_Users");
                });

            modelBuilder.Entity("EMenuApplication.Models.Set_Concepts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("ConceptName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<byte?>("Status")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Set_Concepts");
                });

            modelBuilder.Entity("EMenuApplication.Models.Set_Stores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CCEmailRecipient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCSMSReceipient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("CommentCardAutoFill")
                        .HasColumnType("bit");

                    b.Property<int>("Conceptid")
                        .HasColumnType("int");

                    b.Property<int>("CountryCode")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Currency_ID")
                        .HasColumnType("int");

                    b.Property<string>("IP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ListOrder")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Regionid")
                        .HasColumnType("int");

                    b.Property<int>("SquirrelCurrency")
                        .HasColumnType("int");

                    b.Property<int>("SquirrelID")
                        .HasColumnType("int");

                    b.Property<byte?>("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("StoreCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Store_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Store_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeZone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Set_Stores");
                });
#pragma warning restore 612, 618
        }
    }
}
