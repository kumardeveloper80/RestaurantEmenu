using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IConceptsRepository
    {
        public List<Concepts_VM> GetConceptsPermissionList(int userId);
        public List<Concepts_VM> GetList(int loginUserId);
        public Concepts_VM Get(int id, int loginUserId);
        public int Add(Concepts_VM concepts_VM, int loginUserId);
        public int Update(Concepts_VM concepts_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public bool IsUniqueConceptName(int id, string conceptName, int clientId, int loginUserId);
    }
}
