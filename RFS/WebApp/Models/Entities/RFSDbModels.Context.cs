﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RFS.Models.Entities
{
    using RoomManagement.Entities;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class rfsEntities : DbContext, IRoomManagementDatabasseEntities
    {
        public rfsEntities()
            : base("name=rfsEntities")
        {
          
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomPicture> RoomPictures { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<UserProfilePic> UserProfilePicture { get; set; }
        public virtual DbSet<RoomProfilePic> RoomProfilePicture { get; set; }
    }
}
