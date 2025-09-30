using IKEA.DAL.Models;

using IKEA.DAL.Presistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Repositories._Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private readonly ApplicationDBContext _dBContext;

        public GenericRepository(ApplicationDBContext dBContext)
        //ask clr for object from appdbcontext implicitly
        {
            _dBContext = dBContext;
        }
        public async  Task<T?> GetByIdAsync(int id)
        {
            //   var T= _dBContext.Ts.Local.FirstOrDefault(x => x.Id == id); //بقوله روح اعمل سيرش 
            return await _dBContext.Set<T>().FindAsync(id);
              //if not found it will return null, if found it will return the object

        }
       
        public void Add(T entity)
        {
            _dBContext.Set<T>().Add(entity); //add the entity to the dbcontext
           //save changes to the database, it will return the number of affected rows

        }


        public void Update(T entity)
        {
            _dBContext.Set<T>().Update(entity); //update the entity in the dbcontext
            //save changes to the database, it will return the number of affected rows
        }
        public void Delete(T entity)
        {
          //  entity.IsDeleted=true;  //soft delete
            _dBContext.Set<T>().Remove(entity); //remove the entity from the dbcontext اشقي
            //save changes to the database, it will return the number of affected rows

        }

        public async Task<IEnumerable<T>> GetAllAsync(bool WithAsNoTracking = true)
        {
            if (WithAsNoTracking) //
            {
             return await   _dBContext.Set<T>().Where(X=>X.IsDeleted).AsNoTracking().ToListAsync();
            }

            return await _dBContext.Set<T>().Where(X => X.IsDeleted).ToListAsync();





        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return _dBContext.Set<T>();
        }

       
    }
}
