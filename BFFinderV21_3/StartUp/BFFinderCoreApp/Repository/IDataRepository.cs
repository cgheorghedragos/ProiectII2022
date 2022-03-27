using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFFinderCoreApp.Models;

namespace BFFinderCoreApp.Repository
{
    internal interface IDataRepository<T>
    {
        User FindUserByUsername(String username);
        User FindUserByEmail(String email);
        bool CheckPassowrd(String password);
        void Insert(T data);
        void Update(T data);
        void Delete(T data);
        void Save();

    }
}
