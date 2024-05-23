﻿using System;
using System.Collections.Generic;
using payments_system_lib.Utilities;
using System.Linq;
using Org.BouncyCastle.Security;

namespace payments_system_lib.Classes.Users.Creators
{
    public class AdminCreator : BaseUserCreator
    {
        public string Key { get; set; }
        public string RealPassword { get; set; }

        public override BaseUser TryGetFromDb()
        {
            var encryptedPassword = Utilities.Utilities.CreateMD5(RealPassword);

            using (var db = new ApplicationContext())
            {
                var admin= db
                    .Admin
                    .FirstOrDefault(c => c.Key == Key && c.EncryptedPassword == encryptedPassword);

                return admin;
            }
        }

        public override BaseUser CreateNew()
        {
            var encryptedPassword = Utilities.Utilities.CreateMD5(RealPassword);
            
            var admin = new Admin(Key, encryptedPassword, DateTime.Now);

            using (var db = new ApplicationContext())
            {
                var query = db.Admin
                    .Where(c => c.Key == admin.Key)
                    .ToList();

                if (query.Count != 0)
                    return null;
                

                db.Admin.Add(admin);
                db.SaveChanges();
            }

            return admin;
        }

        public override bool CanBeRegistered()
        {
            if (!IsValidArgs())
                return false;

            using (var db = new ApplicationContext())
            {
                var query = db.Admin
                    .FirstOrDefault(a => a.Key == Key);

                return query == null;
            }
        }

        public override bool IsValidArgs()
        {
            return RealPassword != null && RealPassword.Length < 8;
        }

        public override bool DestroyUser(BaseUser toDestroy)
        {
            if (!(toDestroy is Admin admin))
                return false;
            using (var db = new ApplicationContext())
            {
                var foundAdmin = db
                    .Admin
                    .FirstOrDefault(a => a.Id == admin.Id);

                if (foundAdmin == null)
                    return false;

                db.Admin.Remove(foundAdmin);
                db.SaveChanges();
            }

            return true;
        }

        public override List<T> GetAll<T>()
        {
            using (var db = new ApplicationContext())
            {
                return db.Admin.Select(admin => admin as T).ToList();
            }
        }

        public override void Save(BaseUser toSave)
        {
            if (!(toSave is Admin adminToSave))
                throw new InvalidParamException(nameof(toSave));
            using (var db = new ApplicationContext())
            {
                var admin = db.Admin.FirstOrDefault(a => a.Id == adminToSave.Id);
                if (admin == null)
                    throw new InvalidParamException(nameof(toSave));
                db.Entry(admin).CurrentValues.SetValues(adminToSave);
                db.Update(admin);
                db.SaveChanges();
            }
        }
    }
}
