using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.ToDo
{
    public interface IToDoAPI
    {
        Task<long> ToDoMaintenance(ToDoEntity todoInfo);
        Task<List<ToDoEntity>> GetAllToDoByBranch(long branchID, long userID);
        Task<List<ToDoEntity>> GetAllToDoWithoutContentByBranch(long branchID, long userID);
        Task<ToDoEntity> GetToDoByToDoID(long todoID);
        bool RemoveToDo(long todoID, string lastupdatedby);
        Task<List<ToDoEntity>> GetAllCustomToDo(DataTableAjaxPostModel model, long branchID);
        Task<List<ToDoEntity>> GetAllToDoList(long branchid);
    }
}
