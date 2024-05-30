using System;
using System.Collections.Generic;
using payments_system_lib.Utilities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace payments_system_lib.Classes.Users.Creators
{
    public class AdminCreator : BaseUserCreator
    {
        public string Key { get; set; } = null;
        public string RealPassword { get; set; } = null;

        /// <summary>
        /// + RealPassword <br/>
        /// + Key
        /// </summary>
        public override async Task<BaseUser> TryGetFromDb()
        {
            if (RealPassword == null)
                throw new InvalidParamException(nameof(RealPassword));
            if (Key == null)
                throw new InvalidParamException(nameof(Key));

            var encryptedPassword = Utilities.Utilities.CreateMD5(RealPassword);

            using (var db = new ApplicationContext())
            {
                var admin= await db
                    .Admin
                    .FirstOrDefaultAsync(c => c.Key == Key && c.EncryptedPassword == encryptedPassword);

                return admin;
            }
        }

        /// <summary>
        /// + RealPassword <br/>
        /// + Key
        /// </summary>
        public override async Task<BaseUser> CreateNew()
        {
            if (RealPassword == null)
                throw new InvalidParamException(nameof(RealPassword));
            if (Key == null)
                throw new InvalidParamException(nameof(Key));

            var encryptedPassword = Utilities.Utilities.CreateMD5(RealPassword);
            
            var admin = new Admin(Key, encryptedPassword, DateTime.Now);

            using (var db = new ApplicationContext())
            {
                var query = await db.Admin
                    .Where(c => c.Key == admin.Key)
                    .ToListAsync();

                if (query.Count != 0)
                    return null;
                
                await db.Admin.AddAsync(admin);
                await db.SaveChangesAsync();
            }

            return admin;
        }

        /// <summary>
        /// + RealPassword(optional) <br/>
        /// + Key
        /// </summary>
        public override async Task<bool> CanBeRegistered()
        {
            if (!await IsValidArgs())
                return false;

            using (var db = new ApplicationContext())
            {
                var query = await db.Admin
                    .FirstOrDefaultAsync(a => a.Key == Key);

                return query == null;
            }
        }

        /// <summary>
        /// + RealPassword(optional)
        /// </summary>
        public override async Task<bool> IsValidArgs()
        {
            return RealPassword != null && RealPassword.Length < 8;
        }

        public override async Task<List<T>> GetAll<T>()
        {
            using (var db = new ApplicationContext())
            {
                return await db.Admin.Select(admin => admin as T).ToListAsync();
            }
        }

        public override async Task Save(BaseUser toSave)
        {
            if (!(toSave is Admin adminToSave))
                throw new InvalidParamException(nameof(toSave));

            using (var db = new ApplicationContext())
            {
                var admin = await db.Admin.FirstOrDefaultAsync(a => a.Id == adminToSave.Id);
                if (admin == null)
                    throw new InvalidParamException(nameof(toSave));
                db.Entry(admin).CurrentValues.SetValues(adminToSave);
                db.Update(admin);
                await db.SaveChangesAsync();
            }
        }

        public override async Task Destroy(BaseUser toDestroy)
        {
            if (!(toDestroy is Admin admin))
                throw new InvalidParamException(nameof(toDestroy));

            using (var db = new ApplicationContext())
            {
                var foundAdmin = await db
                    .Admin
                    .FirstOrDefaultAsync(a => a.Id == admin.Id);

                if (foundAdmin == null)
                    throw new InvalidParamException(nameof(toDestroy));

                db.Admin.Remove(foundAdmin);
                await db.SaveChangesAsync();
            }
        }
    }
}
