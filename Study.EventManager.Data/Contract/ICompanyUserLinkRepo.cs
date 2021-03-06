using Study.EventManager.Data.Repositiry;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{  
    public interface ICompanyUserLinkRepo : IRepository<CompanyUserLink>
    {
        CompanyUserLink GetRecordByCompanyAndUser(int UserId, int CompanyId);

        List<Company> GetCompaniesByUser(int UserId, int page, int pageSize, string companyName = "", int del = 0);

        int GetCompaniesByUserCount(int UserId, int del = 0);

        int GetUserRole(int userId, int companyId);

        List<CompanyUserLink> GetCompanyUserLinkListForUser(int userId, List<int> companyIdList);

        List<User> GetAllUsers(int CompanyId, int page, int pageSize, string filter, string lastName);

        int GetAllUsersCount(int CompanyId);
    }
}
