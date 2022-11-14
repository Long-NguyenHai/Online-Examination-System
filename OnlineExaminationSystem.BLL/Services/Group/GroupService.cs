using Microsoft.Extensions.Logging;
using OnlineExaminationSystem.BusinessLogic.Services.Group;
using OnlineExaminationSystem.DataAccess;
using OnlineExaminationSystem.DataAccess.UnitOfWork;
using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.Group
{
    public class GroupService : IGroupService
    {
        IUnitOfWork _unitOfWork;
        ILogger<GroupService> _ilogger;

        public GroupService(IUnitOfWork unitOfWork, ILogger<GroupService> ilogger)
        {
            _unitOfWork = unitOfWork;
            _ilogger = ilogger;
        }

        public async Task<GroupViewModel> AddGroupAsync(GroupViewModel groupViewModel)
        {
            try
            {
                Groups obj = groupViewModel.ConvertViewModel(groupViewModel);
                await _unitOfWork.GenericRepository<Groups>().AddAsync(obj);
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return groupViewModel;
        }

        public PagedResult<GroupViewModel> GetAllGroups(int pageNumber, int pageSize)
        {
            var model = new GroupViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<GroupViewModel> detailList = new List<GroupViewModel>();
                var modelList = _unitOfWork.GenericRepository<Groups>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Groups>().GetAll().ToList();

                detailList = GroupListInfo(modelList);
                if (detailList != null)
                {
                    model.GroupList = detailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            var result = new PagedResult<GroupViewModel>
            {
                Data = model.GroupList,
                TotalItems = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return result;
        }

        private List<GroupViewModel> GroupListInfo(List<Groups> modelList)
        {
            return modelList.Select(o => new GroupViewModel(o)).ToList();
        }

        public IEnumerable<Groups> GetAllGroups()
        {
            try
            {
                var groups = _unitOfWork.GenericRepository<Groups>().GetAll();
                return groups;
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            return Enumerable.Empty<Groups>();
        }

        public GroupViewModel GetById(int groupId)
        {
            try
            {
                var group = _unitOfWork.GenericRepository<Groups>().GetByID(groupId);
                return new GroupViewModel(group);
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            return null;
            
        }
    }
}
