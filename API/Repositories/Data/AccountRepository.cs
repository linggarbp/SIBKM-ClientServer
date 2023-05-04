using API.Context;
using API.Models;
using API.Repositories.Interface;
using API.ViewModels;

namespace API.Repositories.Data;

public class AccountRepository : GeneralRepository<Account, string, MyContext>, IAccountRepository
{
    public AccountRepository(MyContext context) : base(context)
    {
    }

    public int Register(RegisterVM registerVM)
    {
        int result = 0;
        // Insert to University Table
        var university = new University
        {
            Name = registerVM.UniversityName
        };

        //Before Insert, Check University Id
        _context.Set<University>().Find(university.Id);
        if (university.Id != null)
        {
            
        }
        if (university.Id == null)
        {
            _context.Set<University>().Add(university);
            result = _context.SaveChanges();
        }

        // Insert to Education Table
        var education = new Education
        {
            Major = registerVM.Major,
            Degree = registerVM.Degree,
            GPA = registerVM.GPA,
            UniversityID = university.Id
        };
        _context.Set<Education>().Add(education);
        result += _context.SaveChanges();

        // Insert to Employee Table
        var employee = new Employee
        {
            NIK = registerVM.NIK,
            FirstName = registerVM.FirstName,
            LastName = registerVM.LastName,
            Gender = registerVM.Gender,
            BirthDate = registerVM.BirthDate,
            Email = registerVM.Email,
            HiringDate = DateTime.Now,
            PhoneNumber = registerVM.PhoneNumber,
        };
        _context.Set<Employee>().Add(employee);
        result += _context.SaveChanges();

        // Insert to Account Table
        var account = new Account
        {
            EmployeeNIK = registerVM.NIK,
            Password = registerVM.Password,
        };
        _context.Set<Account>().Add(account);
        result += _context.SaveChanges();

        // Insert to Profiling Table
        var profiling = new Profiling
        {
            EmployeeNIK = registerVM.NIK,
            EducationID = education.ID
        };
        _context.Set<Profiling>().Add(profiling);
        result += _context.SaveChanges();

        // Insert to AccountRole Table
        var accountRole = new AccountRole
        {
            AccountNIK = registerVM.NIK,
            RoleID = 1
        };
        _context.Set<AccountRole>().Add(accountRole);
        result += _context.SaveChanges();

        return result;
    }

    public bool Login(LoginVM loginVM)
    {
        var employee = new Employee
        {
            Email = loginVM.Email
        };
        _context.Employees.Where(e => e.Email.Contains(loginVM.Email));

        var account = new Account
        {
            EmployeeNIK = employee.NIK,
            Password = loginVM.Password,
        };
        _context.Accounts.Where(p => p.Password.Contains(loginVM.Password));

        if (employee != null && account != null)
        {
            return true;
        }
        return false;
    }
}
