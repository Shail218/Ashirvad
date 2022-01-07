using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.ToDo
{
    public interface IToDoService
    {
        Task<ToDoEntity> ToDoMaintenance(ToDoEntity homework);
        Task<List<ToDoEntity>> GetAllToDoByBranch(long branchID, long userID = 0);
        Task<List<ToDoEntity>> GetAllToDoWithoutContentByBranch(long branchID, long userID = 0);
        Task<ToDoEntity> GetToDoByToDoID(long toID);
        bool RemoveToDo(long toID, string lastupdatedby);
        Task<List<ToDoEntity>> GetAllCustomToDo(DataTableAjaxPostModel model, long branchID);
        Task<List<ToDoEntity>> GetAllToDoList(long branchID);
    }
}
