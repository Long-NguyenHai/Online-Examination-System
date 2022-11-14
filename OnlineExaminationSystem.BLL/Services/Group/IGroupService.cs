using OnlineExaminationSystem.DataAccess;
using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.Group
{
    public interface IGroupService
    {
        PagedResult<GroupViewModel> GetAllGroups(int pageNumber, int pageSize);
        Task<GroupViewModel> AddGroupAsync(GroupViewModel groupViewModel);
        IEnumerable<Groups> GetAllGroups();
        GroupViewModel GetById(int groupId);
    }
}
