using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LProject
    {
        #region Create

        public static void Create(CProject project)
        {
            using (var db=Connection.GetEntityContext())
            {
                if (db.Projects.FirstOrDefault(a => a.Code == project.Code.Trim().ToUpper()) != null)
                    throw new Exception("Project code is already registered.");
                if (db.Projects.FirstOrDefault(a => a.Name == project.Name.Trim()) != null)
                    throw new Exception("Project is already registered.");
                var obj = new Project()
                {
                    Code = project.Code.Trim().ToUpper(),
                    Name = project.Name,
                    ContractAmount = project.ContractAmount,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    DateCreated = DateTime.Now,
                    IsActive = true
                };
                db.Projects.Add(obj);
                db.SaveChanges();
            }
        }

        #endregion

        #region Read

        public static List<CProject> GetProjects()
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Projects
                    .Select(a => new CProject()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        ContractAmount = a.ContractAmount,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        DateCreated = a.DateCreated,
                        IsActive = a.IsActive
                    })
                    .OrderBy(a => a.Name)
                    .ToList();
            }
        }

        public static CProject GetProject(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Projects
                    .Where(a => a.ID == id)
                    .Select(a => new CProject()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        ContractAmount = a.ContractAmount,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        DateCreated = a.DateCreated,
                        IsActive = a.IsActive
                    })
                    .FirstOrDefault();
            }
        }

        #endregion

        #region Update

        public static void Update(CProject project)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Projects.FirstOrDefault(a => a.ID == project.ID);
                if (db.Projects.FirstOrDefault(a => a.Code == project.Code.Trim().ToUpper() && a.ID != project.ID) != null)
                    throw new Exception("Project code is already registered.");
                if (db.Projects.FirstOrDefault(a => a.Name == project.Name.Trim() && a.ID != project.ID) != null)
                    throw new Exception("Project is already registered.");

                obj.Name = project.Name;
                obj.Code = project.Code;
                obj.ContractAmount = project.ContractAmount;
                obj.StartDate = project.StartDate;
                obj.EndDate = project.EndDate;
                obj.DateCreated = project.DateCreated;
                db.SaveChanges();
            }
        }

        #endregion

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Projects.FirstOrDefault(a => a.ID == id);
                // should check if bank is used in Payables
                db.Projects.Remove(obj);
                db.SaveChanges();
            }

        }

        #endregion
    }
}