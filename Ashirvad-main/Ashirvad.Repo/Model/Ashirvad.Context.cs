﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ashirvad.Repo.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class AshirvadDBEntities1 : DbContext
    {
        public AshirvadDBEntities1()
            : base("name=AshirvadDBEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BANNER_MASTER> BANNER_MASTER { get; set; }
        public virtual DbSet<BANNER_TYPE_REL> BANNER_TYPE_REL { get; set; }
        public virtual DbSet<BATCH_MASTER> BATCH_MASTER { get; set; }
        public virtual DbSet<BRANCH_LICENSE> BRANCH_LICENSE { get; set; }
        public virtual DbSet<BRANCH_MAINT> BRANCH_MAINT { get; set; }
        public virtual DbSet<BRANCH_MASTER> BRANCH_MASTER { get; set; }
        public virtual DbSet<BRANCH_STAFF> BRANCH_STAFF { get; set; }
        public virtual DbSet<NOTIFICATION_MASTER> NOTIFICATION_MASTER { get; set; }
        public virtual DbSet<NOTIFICATION_TYPE_REL> NOTIFICATION_TYPE_REL { get; set; }
        public virtual DbSet<SCHOOL_MASTER> SCHOOL_MASTER { get; set; }
        public virtual DbSet<STD_MASTER> STD_MASTER { get; set; }
        public virtual DbSet<STUDENT_MAINT> STUDENT_MAINT { get; set; }
        public virtual DbSet<STUDENT_MASTER> STUDENT_MASTER { get; set; }
        public virtual DbSet<SUBJECT_MASTER> SUBJECT_MASTER { get; set; }
        public virtual DbSet<TRANSACTION_MASTER> TRANSACTION_MASTER { get; set; }
        public virtual DbSet<TYPE_DESC> TYPE_DESC { get; set; }
        public virtual DbSet<USER_DEF> USER_DEF { get; set; }
        public virtual DbSet<USER_ROLE> USER_ROLE { get; set; }
        public virtual DbSet<GALLERY_MASTER> GALLERY_MASTER { get; set; }
        public virtual DbSet<LINK_MASTER> LINK_MASTER { get; set; }
        public virtual DbSet<LIBRARY_DATA> LIBRARY_DATA { get; set; }
        public virtual DbSet<LIBRARY_MASTER> LIBRARY_MASTER { get; set; }
        public virtual DbSet<PRACTICE_PAPER> PRACTICE_PAPER { get; set; }
        public virtual DbSet<PRACTICE_PAPER_REL> PRACTICE_PAPER_REL { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<ATTENDANCE_DTL> ATTENDANCE_DTL { get; set; }
        public virtual DbSet<ATTENDANCE_HDR> ATTENDANCE_HDR { get; set; }
        public virtual DbSet<TEST_MASTER> TEST_MASTER { get; set; }
        public virtual DbSet<TEST_PAPER_REL> TEST_PAPER_REL { get; set; }
        public virtual DbSet<REMINDER_MASTER> REMINDER_MASTER { get; set; }
        public virtual DbSet<STUDENT_ANS_SHEET> STUDENT_ANS_SHEET { get; set; }
        public virtual DbSet<HOMEWORK_MASTER> HOMEWORK_MASTER { get; set; }
        public virtual DbSet<TODO_MASTER> TODO_MASTER { get; set; }
        public virtual DbSet<ABOUTUS_DETAIL_REL> ABOUTUS_DETAIL_REL { get; set; }
        public virtual DbSet<ABOUTUS_MASTER> ABOUTUS_MASTER { get; set; }
        public virtual DbSet<ANNOUNCE_MASTER> ANNOUNCE_MASTER { get; set; }
        public virtual DbSet<FEE_STRUCTURE_DTL> FEE_STRUCTURE_DTL { get; set; }
        public virtual DbSet<FEE_STRUCTURE_MASTER> FEE_STRUCTURE_MASTER { get; set; }
        public virtual DbSet<STUDENT_PAYMENT_MASTER> STUDENT_PAYMENT_MASTER { get; set; }
        public virtual DbSet<STUDENT_PAYMENT_REL> STUDENT_PAYMENT_REL { get; set; }
        public virtual DbSet<UPI_MASTER> UPI_MASTER { get; set; }
        public virtual DbSet<BRANCH_AGREEMENT> BRANCH_AGREEMENT { get; set; }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual ObjectResult<usp_get_usage_Result> usp_get_usage()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_get_usage_Result>("usp_get_usage");
        }
    }
}
